# NewLife.JT809 - JT/T 809 プロトコル

![GitHub License](https://img.shields.io/github/license/NewLifeX/NewLife.JT809?logo=github)
![NuGet Downloads](https://img.shields.io/nuget/dt/NewLife.JT809?logo=nuget)
![NuGet](https://img.shields.io/nuget/v/NewLife.JT809?logo=nuget)
![NuGet prerelease](https://img.shields.io/nuget/vpre/NewLife.JT809?label=dev%20nuget&logo=nuget)
[![test](https://github.com/NewLifeX/NewLife.JT809/actions/workflows/test.yml/badge.svg?branch=master)](https://github.com/NewLifeX/NewLife.JT809/actions/workflows/test.yml)
![platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux%20%7C%20macOS-blue)
[![contributors](https://img.shields.io/github/contributors/NewLifeX/NewLife.JT809?logo=github)](https://github.com/NewLifeX/NewLife.JT809/graphs/contributors)

<!-- lang:start -->
🌐 [English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [Español](README.es.md) | [Deutsch](README.de.md) | [Français](README.fr.md) | [Português](README.pt.md) | [Русский](README.ru.md) | [한국어](README.ko.md) | [العربية](README.ar.md)
<!-- lang:end -->

<!-- metrics:start -->
[![TFM](https://img.shields.io/badge/TFM-3%20%28netstandard2.0%20%7C%20net8.0%20%7C%20net10.0%29-512BD4?logo=dotnet)](https://www.nuget.org/packages/NewLife.JT809)
[![locales](https://img.shields.io/badge/locales-10-2ea44f?logo=dotnet)](Doc/多语言本地化.md)
<!-- metrics:end -->

> この翻訳は遅れている場合があります。最新情報は [English README](README.md) を参照してください。

道路交通（中国）におけるプラットフォーム間接続のための JT/T 809 プロトコルライブラリです。フレームコーデック、2011/2013/2019 各バージョンのメッセージ、主リンク・従リンクのクライアント／サーバーセッションを提供します。

ソース: https://github.com/NewLifeX/NewLife.JT809  
NuGet: https://www.nuget.org/packages/NewLife.JT809  

## 特長

- フレームコーデック：0x5B/0x5D による区切り、エスケープ処理、CRC-16、リンク暗号化
- プロトコルバージョン：JT/T 809-2011／2013／2019 に対応し、ログイン時に自動判別
- コマンド網羅：30 以上のトップレベル命令と 50 以上のサブメッセージ（車両位置・アラーム・静的情報モデルを含む）
- 統合：`[MessageKind]` を付けたハンドラーメソッド、要求／応答の対応付け、再接続
- エンジニアリング：Native AOT 対応、実行時メッセージは 11 言語（翻訳表 10 種＋英語原文）

## 動作環境

- ターゲットフレームワーク `netstandard2.0`／`net8.0`／`net10.0`、Windows／Linux／macOS で動作
- NewLife.Core に依存（パッケージ復元時に自動取得）。ソースからのビルドには .NET 8 SDK 以上が必要
- プロトコル仕様：[Doc](Doc)（JT/T 809-2011 と JT/T 809-2019）

## インストール

```bash
dotnet add package NewLife.JT809
```

## クイックスタート

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

## ドキュメント

- ランタイムローカライズ：[Doc/多语言本地化.md](Doc/多语言本地化.md)（中国語）
- エコシステムマップ：[README.zh-CN.md](README.zh-CN.md)（中国語の正本）

## NewLife Team

Founded in 2002, the NewLife team has focused on the .NET and IoT ecosystem for more than twenty years and maintains nearly one hundred open source projects. Website: https://newlifex.com · Organization: https://github.com/NewLifeX

## ライセンス

MIT License © 2002-2026 NewLife Team

