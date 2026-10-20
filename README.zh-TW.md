# NewLife.JT809 - JT/T 809 部標協議

![GitHub License](https://img.shields.io/github/license/NewLifeX/NewLife.JT809?logo=github)
![NuGet Downloads](https://img.shields.io/nuget/dt/NewLife.JT809?logo=nuget)
![NuGet](https://img.shields.io/nuget/v/NewLife.JT809?logo=nuget)
![NuGet prerelease](https://img.shields.io/nuget/vpre/NewLife.JT809?label=dev%20nuget&logo=nuget)
[![test](https://github.com/NewLifeX/NewLife.JT809/actions/workflows/test.yml/badge.svg?branch=master)](https://github.com/NewLifeX/NewLife.JT809/actions/workflows/test.yml)
![platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux%20%7C%20macOS-blue)
[![contributors](https://img.shields.io/github/contributors/NewLifeX/NewLife.JT809?logo=github)](https://github.com/NewLifeX/NewLife.JT809/graphs/contributors)

<!-- lang:start -->
🌐 [English](README.md) | [简体中文](README.zh-CN.md) | [Español](README.es.md) | [Deutsch](README.de.md) | [日本語](README.ja.md) | [Français](README.fr.md) | [Português](README.pt.md) | [Русский](README.ru.md) | [한국어](README.ko.md) | [العربية](README.ar.md)
<!-- lang:end -->

<!-- metrics:start -->
[![TFM](https://img.shields.io/badge/TFM-3%20%28netstandard2.0%20%7C%20net8.0%20%7C%20net10.0%29-512BD4?logo=dotnet)](https://www.nuget.org/packages/NewLife.JT809)
[![locales](https://img.shields.io/badge/locales-10-2ea44f?logo=dotnet)](Doc/多语言本地化.md)
<!-- metrics:end -->

JT809 部標協議。面向道路運輸監管的上下級平台互聯協議庫，提供幀編解碼、2011/2013/2019 各版本訊息與主從鏈路用戶端服務端。

原始碼： https://github.com/NewLifeX/NewLife.JT809  
NuGet：https://www.nuget.org/packages/NewLife.JT809  

## 主要特性

- 幀編解碼：0x5B/0x5D 定界、轉義處理、CRC-16 校驗與鏈路加密
- 協議版本：支援 JT/T 809-2011、2013、2019，登入時自動識別版本
- 指令覆蓋：30+ 頂層業務指令、50+ 子業務訊息，含車輛定位、報警與靜態資訊模型
- 執行形態：基於 Core 訊息泵的用戶端/服務端會話、請求回應配對、AOT 相容、10 種執行階段語言

## 安裝

```bash
dotnet add package NewLife.JT809
```

## 快速開始

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

## 文件

- 執行階段多語言：[Doc/多语言本地化.md](Doc/多语言本地化.md)
- 生態版圖見 [README.zh-CN.md](README.zh-CN.md)

## NewLife Team

Founded in 2002, the NewLife team has focused on the .NET and IoT ecosystem for more than twenty years and maintains nearly one hundred open source projects. Website: https://newlifex.com · Organization: https://github.com/NewLifeX

## 授權條款

MIT License © 2002-2026 新生命開發團隊

