using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using NewLife;
using NewLife.Configuration;
using NewLife.JT809;
using NewLife.JT809.Protocols;
using NewLife.Log;
using NewLife.Serialization;
using Xunit;

namespace XUnitTest;

/// <summary>本地化。修改全局语言状态，必须串行执行</summary>
[Collection("GlobalState")]
[DisplayName("多语言本地化")]
public class Localization_Tests
{
    private const String _descriptionKey = "JT/T 809 protocol library for platform interconnection in road transport: frame codec, 2011/2013/2019 messages, sub-messages and client/server sessions.";
    private static readonly String[] _cultures = ["ar", "bn", "cs", "da", "de", "el", "es", "fa", "fi", "fr", "he", "hi", "hu", "id", "it", "ja", "ko", "ms", "nb", "nl", "pl", "pt", "ro", "ru", "sv", "th", "tr", "uk", "vi", "zh-CN", "zh-TW"];
    private static readonly Regex _entry = new(@"^\s*""((?:[^""\\]|\\.)*)""\s*:\s*""((?:[^""\\]|\\.)*)""\s*(?:#.*)?$", RegexOptions.Compiled);

    static Localization_Tests()
    {
        // 触发 JT809 程序集加载与模块初始化，注册本程序集词表；单独运行本类时也能命中译文
        MessageFactory.Instance.Init();
    }

    [Fact]
    [DisplayName("简体中文命中内置词表")]
    public void Translate_Chinese()
    {
        var old = Localization.Culture;
        try
        {
            Localization.Culture = new CultureInfo("zh-CN");

            Assert.Equal("登录失败！7", Localization.Translate("Login failed! {0}", 7));
            Assert.Equal("不支持的指令[0x1001]", Localization.Translate("Unsupported command [{0}]", "0x1001"));
            Assert.Equal("JT809 长度前缀字符串不能超过 255 字节。", Localization.Translate("JT809 length-prefixed string cannot exceed 255 bytes."));
            Assert.Equal("指令帧解析失败：boom", Localization.Translate("Command frame parse failed: {0}", "boom"));
            Assert.Equal("Crc16 校验错误 0012!=0034", Localization.Translate("Crc16 Error {0:X4}!={1:X4}", 0x12, 0x34));
        }
        finally
        {
            Localization.Culture = old;
        }
    }

    [Fact]
    [DisplayName("繁体中文命中内置词表")]
    public void Translate_TraditionalChinese()
    {
        var old = Localization.Culture;
        try
        {
            Localization.Culture = new CultureInfo("zh-TW");

            Assert.Equal("登錄失敗！7", Localization.Translate("Login failed! {0}", 7));
            Assert.Equal("指令訊框解析失敗：boom", Localization.Translate("Command frame parse failed: {0}", "boom"));
        }
        finally
        {
            Localization.Culture = old;
        }
    }

    [Fact]
    [DisplayName("新增语言命中词表")]
    public void Translate_NewLanguages()
    {
        var samples = new Dictionary<String, String>
        {
            ["bn"] = "লগইন ব্যর্থ হয়েছে! 7",
            ["hi"] = "लॉगिन विफल! 7",
            ["it"] = "Accesso non riuscito! 7",
            ["sv"] = "Inloggningen misslyckades! 7",
            ["vi"] = "Đăng nhập thất bại! 7",
        };

        var old = Localization.Culture;
        try
        {
            foreach (var item in samples)
            {
                Localization.Culture = new CultureInfo(item.Key);
                Assert.Equal(item.Value, Localization.Translate("Login failed! {0}", 7));
            }
        }
        finally
        {
            Localization.Culture = old;
        }
    }

    [Fact]
    [DisplayName("未登记语言回退英文源文")]
    public void Translate_FallbackEnglish()
    {
        var old = Localization.Culture;
        try
        {
            Localization.Culture = new CultureInfo("sw-KE");

            Assert.Equal("Login failed! 7", Localization.Translate("Login failed! {0}", 7));
        }
        finally
        {
            Localization.Culture = old;
        }
    }

    [Fact]
    [DisplayName("组件词表已注册到本地化中心")]
    public void Locales_Registered()
    {
        foreach (var code in _cultures)
        {
            Assert.True(Localization.IsSupported(new CultureInfo(code)), code);
        }
    }

    [Fact]
    [DisplayName("异常消息命中中文词表")]
    public void Exception_Message_Localized()
    {
        var old = Localization.Culture;
        try
        {
            Localization.Culture = new CultureInfo("zh-CN");

            var binary = new Binary();
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => binary.WriteString(new String('A', 300)));
            Assert.Contains("不能超过 255 字节", ex.Message);
        }
        finally
        {
            Localization.Culture = old;
        }
    }

    [Fact]
    [DisplayName("配置元数据注释命中中文词表")]
    public void Config_Metadata_Localized()
    {
        var old = Localization.Culture;
        try
        {
            Localization.Culture = new CultureInfo("zh-CN");

            Assert.Equal("JT809 设置", "JT809 settings".LocalizeComment());
            Assert.Equal("调试。默认启用", "Debugging. Enabled by default.".LocalizeComment());
            Assert.Equal("端口。默认 809", "Port. Default 809.".LocalizeComment());
        }
        finally
        {
            Localization.Culture = old;
        }
    }

    [Fact]
    [DisplayName("日志消息命中日志语言词表")]
    public void Log_Message_Localized()
    {
        var old = Localization.LogCulture;
        try
        {
            Localization.LogCulture = new CultureInfo("zh-CN");

            var text = Logger.FormatMessage(null, "Crc16 Error {0:X4}!={1:X4}", [0x12, 0x34]);
            Assert.Equal("Crc16 校验错误 0012!=0034", text);
        }
        finally
        {
            Localization.LogCulture = old;
        }
    }

    [Fact]
    [DisplayName("全部语言词表键集合一致且无空译文")]
    public void Locales_AllKeysTranslated()
    {
        var dir = Path.Combine(AppContext.BaseDirectory, "Locales");
        Assert.True(Directory.Exists(dir), $"未找到词表目录：{dir}");

        var files = Directory.GetFiles(dir, "*.yml");
        Assert.True(files.Length >= 31, $"语言文件数量不足：{files.Length}");

        var tables = new Dictionary<String, Dictionary<String, String>>(StringComparer.Ordinal);
        foreach (var file in files)
        {
            var table = new Dictionary<String, String>(StringComparer.Ordinal);
            foreach (var line in File.ReadAllLines(file))
            {
                var match = _entry.Match(line.Trim());
                if (!match.Success) continue;

                var key = match.Groups[1].Value;
                Assert.False(table.ContainsKey(key), $"{Path.GetFileName(file)} 存在重复键：{key}");
                table[key] = match.Groups[2].Value;
            }

            Assert.True(table.Count > 0, $"{Path.GetFileName(file)} 未解析到词条");
            tables[Path.GetFileName(file)] = table;
        }

        var keys = new HashSet<String>(StringComparer.Ordinal);
        foreach (var table in tables.Values)
        {
            foreach (var key in table.Keys) keys.Add(key);
        }

        Assert.Contains(_descriptionKey, keys);
        Assert.Contains("JT809 length-prefixed string cannot exceed 255 bytes.", keys);
        Assert.Contains("Crc16 Error {0:X4}!={1:X4}", keys);
        Assert.Contains("JT809 settings", keys);

        foreach (var item in tables)
        {
            foreach (var key in keys)
            {
                Assert.True(item.Value.TryGetValue(key, out var text), $"{item.Key} 缺少键：{key}");
                Assert.False(String.IsNullOrWhiteSpace(text), $"{item.Key} 的译文为空：{key}");
            }
        }
    }
}
