# README 指标徽章同步与校验
#
# 单一事实源：readme-metrics.json。TFM 数量、运行时语言数等会随发布变化的数字，
# 不再散落在多份 README 里手工维护。
#
# 用法：
#   pwsh -File sync-readme-metrics.ps1            # 按配置写入标记块与数字
#   pwsh -File sync-readme-metrics.ps1 -Check     # 只校验不写入，不一致退出码 1（CI 使用）
param(
    [Switch]$Check,
    [String]$Root = $PSScriptRoot,
    [String]$Config = "readme-metrics.json"
)

$ErrorActionPreference = "Stop"

$configPath = Join-Path $Root $Config
if (!(Test-Path -LiteralPath $configPath)) { throw "未找到指标配置：$configPath" }

$metrics = Get-Content -LiteralPath $configPath -Raw -Encoding UTF8 | ConvertFrom-Json
$values = @{
    "tfm-count"     = [String]$metrics.values.tfmCount
    "tfm-text"      = [String]$metrics.values.tfmText
    "locale-count"  = [String]$metrics.values.localeCount
}

$files = @(Get-ChildItem -Path $Root -File | Where-Object {
    $_.Name -match '^(README|Readme)(\.[A-Za-z-]+)?\.md$' -and $_.Name -notmatch '\.(template|nuget)\.'
} | Sort-Object Name)
if ($files.Count -eq 0) { throw "根目录未找到 README 文件" }

$StartMarker = "<!-- metrics:start -->"
$EndMarker = "<!-- metrics:end -->"
$BlockPattern = "(?s)" + [regex]::Escape($StartMarker) + ".*?" + [regex]::Escape($EndMarker)

# shields.io 的静态徽章需要转义空格、竖线与括号，否则会被当作路径分隔符
function Convert-BadgeText([String]$text) {
    $value = [Uri]::EscapeDataString($text)
    $value = $value.Replace("(", "%28").Replace(")", "%29")
    return $value
}

function Get-MetricsBlock([String]$newline) {
    $tfm = Convert-BadgeText ($values["tfm-text"])
    $lines = @(
        $StartMarker
        "[![TFM](https://img.shields.io/badge/TFM-$($values["tfm-count"])%20%28$tfm%29-512BD4?logo=dotnet)](https://www.nuget.org/packages/NewLife.JT809)"
        "[![locales](https://img.shields.io/badge/locales-$($values["locale-count"])-2ea44f?logo=dotnet)]($($metrics.doc))"
        $EndMarker
    )
    return ($lines -join $newline)
}

# 语言数量必须与 readme-languages.json 一致，防止 README 语言与指标漂移
$langPath = Join-Path $Root "readme-languages.json"
if (Test-Path -LiteralPath $langPath) {
    $langConfig = Get-Content -LiteralPath $langPath -Raw -Encoding UTF8 | ConvertFrom-Json
    $count = @($langConfig.languages).Count
    if ([String]$metrics.values.readmeLocaleCount -ne [String]$count) {
        if ($Check) {
            Write-Host "✗ readme-metrics.json 的 readmeLocaleCount=$($metrics.values.readmeLocaleCount) 与 readme-languages.json 的 $count 不一致"
            exit 1
        }
        Write-Host "✗ readme-metrics.json 的 readmeLocaleCount 与 readme-languages.json 不一致（$($metrics.values.readmeLocaleCount) != $count）"
        exit 1
    }
}

$errors = 0
$changed = 0
foreach ($file in $files) {
    $text = [IO.File]::ReadAllText($file.FullName)
    if (!$text.Contains($StartMarker) -or !$text.Contains($EndMarker)) { throw "$($file.Name) 缺少 $StartMarker / $EndMarker 标记" }

    $newline = if ($text.Contains("`r`n")) { "`r`n" } else { "`n" }
    $block = Get-MetricsBlock $newline
    $updated = [regex]::Replace($text, $BlockPattern, $block.Replace('$', '$$'))
    if ($updated -eq $text) { continue }

    $changed++
    if ($Check) {
        $errors++
        Write-Host "✗ $($file.Name) 指标徽章与 readme-metrics.json 不一致"
    }
    else {
        [IO.File]::WriteAllText($file.FullName, $updated, (New-Object System.Text.UTF8Encoding($false)))
        Write-Host "✓ $($file.Name)"
    }
}

if ($Check) {
    if ($errors -gt 0) {
        Write-Host "`nREADME 指标不一致：$errors / $($files.Count) 个文件。请运行 pwsh -File sync-readme-metrics.ps1 后提交。"
        exit 1
    }
    Write-Host "README 指标一致（$($files.Count) 个文件）"
}
elseif ($changed -eq 0) {
    Write-Host "README 指标已是最新（$($files.Count) 个文件）"
}
