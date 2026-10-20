# NewLife.JT809 - JT/T 809 Protokoll

![GitHub License](https://img.shields.io/github/license/NewLifeX/NewLife.JT809?logo=github)
![NuGet Downloads](https://img.shields.io/nuget/dt/NewLife.JT809?logo=nuget)
![NuGet](https://img.shields.io/nuget/v/NewLife.JT809?logo=nuget)
![NuGet prerelease](https://img.shields.io/nuget/vpre/NewLife.JT809?label=dev%20nuget&logo=nuget)
[![test](https://github.com/NewLifeX/NewLife.JT809/actions/workflows/test.yml/badge.svg?branch=master)](https://github.com/NewLifeX/NewLife.JT809/actions/workflows/test.yml)
![platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux%20%7C%20macOS-blue)
[![contributors](https://img.shields.io/github/contributors/NewLifeX/NewLife.JT809?logo=github)](https://github.com/NewLifeX/NewLife.JT809/graphs/contributors)

<!-- lang:start -->
🌐 [English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [Español](README.es.md) | [日本語](README.ja.md) | [Français](README.fr.md) | [Português](README.pt.md) | [Русский](README.ru.md) | [한국어](README.ko.md) | [العربية](README.ar.md)
<!-- lang:end -->

<!-- metrics:start -->
[![TFM](https://img.shields.io/badge/TFM-3%20%28netstandard2.0%20%7C%20net8.0%20%7C%20net10.0%29-512BD4?logo=dotnet)](https://www.nuget.org/packages/NewLife.JT809)
[![locales](https://img.shields.io/badge/locales-10-2ea44f?logo=dotnet)](Doc/多语言本地化.md)
<!-- metrics:end -->

Bibliothek für das JT/T 809 Protokoll zur Plattform-Vernetzung im Straßentransport (China). Enthält Frame-Codec, Nachrichten der Versionen 2011/2013/2019 sowie Client- und Server-Sitzungen für Haupt- und Nebenstrecke.

Quelle: https://github.com/NewLifeX/NewLife.JT809  
NuGet: https://www.nuget.org/packages/NewLife.JT809  

## Funktionen

- Frame-Codec: 0x5B/0x5D-Begrenzung, Maskierung, CRC-16 und Streckenverschlüsselung
- Protokollversionen: JT/T 809-2011, 2013 und 2019, automatische Erkennung bei der Anmeldung
- Befehlsabdeckung: über 30 Hauptbefehle und 50 Unternachrichten, inklusive Positions-, Alarm- und Statikmodellen
- Laufzeit: Client-/Server-Sitzungen auf der Core-Nachrichtenpumpe, Anfrage-/Antwort-Paarung, AOT-kompatibel, 10 Laufzeitsprachen

## Installation

```bash
dotnet add package NewLife.JT809
```

## Schnellstart

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

## Dokumentation

- Laufzeitlokalisierung: [Doc/多语言本地化.md](Doc/多语言本地化.md) (Chinesisch)
- Ökosystemkarte: [README.zh-CN.md](README.zh-CN.md) (kanonischer chinesischer Text)

## NewLife Team

Founded in 2002, the NewLife team has focused on the .NET and IoT ecosystem for more than twenty years and maintains nearly one hundred open source projects. Website: https://newlifex.com · Organization: https://github.com/NewLifeX

## Lizenz

MIT License © 2002-2026 NewLife Team

