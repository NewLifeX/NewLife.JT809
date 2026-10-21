# 多语言词表校验与抽样
#
# 单一事实源：NewLife.JT809/Locales/*.yml。英文源文是键，译文是值。
# 校验每种语言的键集合一致、译文非空、占位符与格式说明符和英文一致。
#
# 用法：
#   pwsh -File tools/verify-localization.ps1                          # 校验并输出摘要
#   pwsh -File tools/verify-localization.ps1 -Check                   # 校验，失败退出码 1（CI 使用）
#   pwsh -File tools/verify-localization.ps1 -Culture zh-CN -Sample 20 # 抽样查看译文
param(
    [Switch]$Check,
    [String]$Root = (Split-Path $PSScriptRoot -Parent),
    [String]$Culture = "",
    [Int32]$Sample = 0
)

$ErrorActionPreference = "Stop"

$localeDir = Join-Path $Root "NewLife.JT809/Locales"
if (!(Test-Path -LiteralPath $localeDir)) { throw "未找到词表目录：$localeDir" }

$entryPattern = '^\s*"((?:[^"\\]|\\.)*)"\s*:\s*"((?:[^"\\]|\\.)*)"\s*(?:#.*)?$'
$placeholderPattern = '\{(\d+)(?::([^}]*))?\}'

# 解析一个词表文件。英文源文可能包含中文，但键值对本身的格式是固定的
function Get-Entries([String]$path) {
    $table = [ordered]@{}
    $lineNo = 0
    foreach ($line in [IO.File]::ReadAllLines($path)) {
        $lineNo++
        $text = $line.Trim()
        if (!$text -or $text.StartsWith("#")) { continue }

        $match = [regex]::Match($text, $entryPattern)
        if (!$match.Success) { throw "$([IO.Path]::GetFileName($path)):$lineNo 词条格式错误：$text" }

        $key = $match.Groups[1].Value
        if ($table.Contains($key)) { throw "$([IO.Path]::GetFileName($path)):$lineNo 重复键：$key" }
        $table[$key] = $match.Groups[2].Value
    }
    return $table
}

# 提取 {0} / {0:X4} 这类占位符及其格式说明符
function Get-Placeholders([String]$text) {
    $map = @{}
    foreach ($match in [regex]::Matches($text, $placeholderPattern)) {
        $index = [Int32]$match.Groups[1].Value
        $spec = if ($match.Groups[2].Success) { $match.Groups[2].Value } else { "" }
        $map[$index] = $spec
    }
    return $map
}

# 把 zh-CN / zh-TW / de-AT 等区域性名称映射到词表文件名
function Get-LocaleFile([String]$name) {
    if ($name -like "zh*") {
        $hant = $name -match "Hant" -or $name -match "-(TW|HK|MO)$"
        return $(if ($hant) { "zh-Hant.yml" } else { "zh-Hans.yml" })
    }

    $lang = ($name -split "-")[0].ToLowerInvariant()
    return "$lang.yml"
}

$files = @(Get-ChildItem -LiteralPath $localeDir -Filter "*.yml" -File | Sort-Object Name)
if ($files.Count -eq 0) { throw "词表目录为空：$localeDir" }

$tables = @{}
foreach ($file in $files) { $tables[$file.Name] = Get-Entries $file.FullName }

$keys = @()
foreach ($table in $tables.Values) { $keys += @($table.Keys) }
$keys = @($keys | Sort-Object -Unique)

$errors = 0
foreach ($item in $tables.GetEnumerator()) {
    foreach ($key in $keys) {
        if (!$item.Value.Contains($key)) {
            Write-Host "✗ $($item.Key) 缺少键：$key"
            $errors++
            continue
        }

        $value = $item.Value[$key]
        if ([String]::IsNullOrWhiteSpace($value)) {
            Write-Host "✗ $($item.Key) 译文为空：$key"
            $errors++
            continue
        }

        $source = Get-Placeholders $key
        $target = Get-Placeholders $value
        $sourceKeys = @($source.Keys | Sort-Object)
        $targetKeys = @($target.Keys | Sort-Object)
        if (($sourceKeys -join ",") -ne ($targetKeys -join ",")) {
            Write-Host "✗ $($item.Key) 占位符不一致：$key"
            $errors++
            continue
        }

        foreach ($index in $sourceKeys) {
            if ($source[$index] -ne $target[$index]) {
                Write-Host "✗ $($item.Key) 格式说明符不一致：$key（{${index}:$($source[$index])}）"
                $errors++
            }
        }
    }
}

Write-Host "语言文件：$($files.Count)，词条：$($keys.Count)，问题：$errors"

if ($Culture) {
    $file = Get-LocaleFile $Culture
    if (!$tables.ContainsKey($file)) { throw "未找到语言词表：$file" }

    $table = $tables[$file]
    $count = if ($Sample -gt 0) { [Math]::Min($Sample, $keys.Count) } else { $keys.Count }
    Write-Host ""
    Write-Host "$Culture -> $file（$count 条）"
    foreach ($key in ($keys | Select-Object -First $count)) {
        Write-Host ("  {0}`n    -> {1}" -f $key, $table[$key])
    }
}

if ($errors -gt 0) {
    if ($Check) { Write-Host "词表校验失败。" }
    exit 1
}
if ($Check) { Write-Host "词表校验通过。" }
