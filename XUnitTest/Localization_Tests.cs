using System;
using System.ComponentModel;
using System.Globalization;
using NewLife;
using Xunit;

namespace XUnitTest;

/// <summary>本地化。修改全局语言状态，必须串行执行</summary>
[Collection("GlobalState")]
[DisplayName("多语言本地化")]
public class Localization_Tests
{
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
        Assert.True(Localization.IsSupported(new CultureInfo("zh-CN")));
        Assert.True(Localization.IsSupported(new CultureInfo("zh-TW")));

        foreach (var code in new[] { "es", "de", "ja", "fr", "pt", "ru", "ko", "ar" })
        {
            Assert.True(Localization.IsSupported(new CultureInfo(code)), code);
        }
    }
}
