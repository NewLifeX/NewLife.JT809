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

> 本翻譯可能滯後，以 [English README](README.md) 為準。

JT809 部標協議。面向道路運輸監管的上下級平台互聯協議庫，提供幀編解碼、2011/2013/2019 各版本訊息與主從鏈路用戶端服務端。

原始碼： https://github.com/NewLifeX/NewLife.JT809  
NuGet：https://www.nuget.org/packages/NewLife.JT809  

## 主要特性

- 幀編解碼：0x5B/0x5D 定界、轉義處理、CRC-16 校驗與鏈路加密
- 協議版本：支援 JT/T 809-2011、2013、2019，登入時自動識別版本
- 指令覆蓋：30+ 頂層業務指令、50+ 子業務訊息，含車輛定位、報警與靜態資訊模型
- 接入方式：以 `[MessageKind]` 標註的處理方法、請求回應配對、斷線重連
- 工程能力：Native AOT 相容、11 種語言執行階段訊息（10 份譯文 + 英文原文）

## 環境需求

- 目標框架 `netstandard2.0`、`net8.0`、`net10.0`，可在 Windows、Linux、macOS 執行
- 依賴 NewLife.Core（隨套件還原）；原始碼編譯需 .NET 8 SDK 以上
- 協議規範見 [Doc](Doc)：JT/T 809-2011 與 JT/T 809-2019

## 安裝

```bash
dotnet add package NewLife.JT809
```

## 快速開始

```csharp
using NewLife.Net;
using NewLife.JT809;
using NewLife.JT809.Messages;
using NewLife.JT809.Models;
using NewLife.JT809.Protocols;
using NewLife.JT809.SubMessages;

// Built-in commands register on first use; call Init() explicitly for clarity under AOT/trimming
MessageFactory.Instance.Init();

// Server: handle commands with methods marked by [MessageKind]
var server = new JTServer { Port = 809 };
server.AddHandler(new MyController());
server.Start();

// Client: log in over the master link, then push a real-time location (0x1200 / 0x1202)
var client = new JTClient
{
    Server = new NetUri("tcp://127.0.0.1:809"),
    Config = new JTConfig { Version = JTVersion.JT2019 },
};
client.Open();
var verifyCode = await client.LoginAsync(9528, "stone", 123654, "127.0.0.1", 2809);

client.Send1200("皖AT0011", 2, new JT809_0x1200_0x1202
{
    Location = new LocationMessage
    {
        Longitude = 117123456,
        Latitude = 31876543,
        Speed = 600,
        GPSTime = DateTime.Now,
    },
    PlatformId1 = "34010000001",
});

class MyController
{
    [MessageKind(MessageKinds.主链路登录请求消息)]
    public Object Login(IJTSession session, JTMessage message)
        => new JT809_0x1002 { Result = JT809_0x1002_Result.成功, VerifyCode = 0x1234 };

    [MessageKind(MessageKinds.主链路连接保持请求消息)]
    public Object KeepAlive(IJTSession session, JTMessage message) => new JT809_0x1006();
}
```

## 文件

- 執行階段多語言：[Doc/多语言本地化.md](Doc/多语言本地化.md)
- 生態版圖見 [README.zh-CN.md](README.zh-CN.md)

## NewLife Team

Founded in 2002, the NewLife team has focused on the .NET and IoT ecosystem for more than twenty years and maintains nearly one hundred open source projects. Website: https://newlifex.com · Organization: https://github.com/NewLifeX

## 授權條款

MIT License © 2002-2026 新生命開發團隊

