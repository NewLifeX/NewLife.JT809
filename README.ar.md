# NewLife.JT809 - بروتوكول JT/T 809

![GitHub License](https://img.shields.io/github/license/NewLifeX/NewLife.JT809?logo=github)
![NuGet Downloads](https://img.shields.io/nuget/dt/NewLife.JT809?logo=nuget)
![NuGet](https://img.shields.io/nuget/v/NewLife.JT809?logo=nuget)
![NuGet prerelease](https://img.shields.io/nuget/vpre/NewLife.JT809?label=dev%20nuget&logo=nuget)
[![test](https://github.com/NewLifeX/NewLife.JT809/actions/workflows/test.yml/badge.svg?branch=master)](https://github.com/NewLifeX/NewLife.JT809/actions/workflows/test.yml)
![platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux%20%7C%20macOS-blue)
[![contributors](https://img.shields.io/github/contributors/NewLifeX/NewLife.JT809?logo=github)](https://github.com/NewLifeX/NewLife.JT809/graphs/contributors)

<!-- lang:start -->
🌐 [English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [Español](README.es.md) | [Deutsch](README.de.md) | [日本語](README.ja.md) | [Français](README.fr.md) | [Português](README.pt.md) | [Русский](README.ru.md) | [한국어](README.ko.md)
<!-- lang:end -->

<!-- metrics:start -->
[![TFM](https://img.shields.io/badge/TFM-3%20%28netstandard2.0%20%7C%20net8.0%20%7C%20net10.0%29-512BD4?logo=dotnet)](https://www.nuget.org/packages/NewLife.JT809)
[![locales](https://img.shields.io/badge/locales-10-2ea44f?logo=dotnet)](Doc/多语言本地化.md)
<!-- metrics:end -->

مكتبة بروتوكول JT/T 809 لترابط المنصات في النقل البري (الصين). توفر ترميز الإطارات ورسائل الإصدارات 2011/2013/2019 وجلسات العميل والخادم للرابط الرئيسي والتابع.

المصدر: https://github.com/NewLifeX/NewLife.JT809  
NuGet: https://www.nuget.org/packages/NewLife.JT809  

## المزايا

- ترميز الإطارات: تحديد 0x5B/0x5D ومعالجة التهريب والتحقق CRC-16 وتشفير الرابط
- إصدارات البروتوكول: JT/T 809-2011 و2013 و2019 مع اكتشاف تلقائي عند تسجيل الدخول
- تغطية الأوامر: أكثر من 30 أمرًا رئيسيًا و50 رسالة فرعية تشمل نماذج الموقع والإنذار والمعلومات الثابتة
- بيئة التشغيل: جلسات عميل/خادم على مضخة رسائل Core، ومطابقة الطلب/الرد، ومتوافقة مع AOT و10 لغات وقت التشغيل

## التثبيت

```bash
dotnet add package NewLife.JT809
```

## البدء السريع

```csharp
using NewLife.Net;
using NewLife.JT809;
using NewLife.JT809.Protocols;

// Register the built-in commands explicitly (AOT friendly)
MessageFactory.Instance.Init();

// Client: connect to the upper-level platform over the master link
var client = new JTClient
{
    Server = new NetUri("tcp://127.0.0.1:809"),
    Config = new JTConfig { Version = JTVersion.JT2019 },
};
client.Open();
var verifyCode = await client.LoginAsync(9528, "stone", 123654, "127.0.0.1", 2809);

// Server: host the lower-level platform and handle commands
var server = new JTServer { Port = 809 };
server.AddHandler(new MyController());
server.Start();
```

## التوثيق

- التوطين في وقت التشغيل: [Doc/多语言本地化.md](Doc/多语言本地化.md) (بالصينية)
- خريطة المنظومة: [README.zh-CN.md](README.zh-CN.md) (النص الصيني المرجعي)

## NewLife Team

Founded in 2002, the NewLife team has focused on the .NET and IoT ecosystem for more than twenty years and maintains nearly one hundred open source projects. Website: https://newlifex.com · Organization: https://github.com/NewLifeX

## الترخيص

MIT License © 2002-2026 NewLife Team

