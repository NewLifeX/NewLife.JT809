# NewLife.JT809 - JT/T 809 프로토콜

![GitHub License](https://img.shields.io/github/license/NewLifeX/NewLife.JT809?logo=github)
![NuGet Downloads](https://img.shields.io/nuget/dt/NewLife.JT809?logo=nuget)
![NuGet](https://img.shields.io/nuget/v/NewLife.JT809?logo=nuget)
![NuGet prerelease](https://img.shields.io/nuget/vpre/NewLife.JT809?label=dev%20nuget&logo=nuget)
[![test](https://github.com/NewLifeX/NewLife.JT809/actions/workflows/test.yml/badge.svg?branch=master)](https://github.com/NewLifeX/NewLife.JT809/actions/workflows/test.yml)
![platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux%20%7C%20macOS-blue)
[![contributors](https://img.shields.io/github/contributors/NewLifeX/NewLife.JT809?logo=github)](https://github.com/NewLifeX/NewLife.JT809/graphs/contributors)

<!-- lang:start -->
🌐 [English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [Español](README.es.md) | [Deutsch](README.de.md) | [日本語](README.ja.md) | [Français](README.fr.md) | [Português](README.pt.md) | [Русский](README.ru.md) | [العربية](README.ar.md)
<!-- lang:end -->

<!-- metrics:start -->
[![TFM](https://img.shields.io/badge/TFM-3%20%28netstandard2.0%20%7C%20net8.0%20%7C%20net10.0%29-512BD4?logo=dotnet)](https://www.nuget.org/packages/NewLife.JT809)
[![locales](https://img.shields.io/badge/locales-10-2ea44f?logo=dotnet)](Doc/多语言本地化.md)
<!-- metrics:end -->

도로 운송(중국) 플랫폼 간 연계를 위한 JT/T 809 프로토콜 라이브러리입니다. 프레임 코덱, 2011/2013/2019 메시지, 주 링크 및 종속 링크의 클라이언트/서버 세션을 제공합니다.

소스: https://github.com/NewLifeX/NewLife.JT809  
NuGet: https://www.nuget.org/packages/NewLife.JT809  

## 주요 기능

- 프레임 코덱: 0x5B/0x5D 구분자, 이스케이프 처리, CRC-16 및 링크 암호화
- 프로토콜 버전: JT/T 809-2011, 2013, 2019 지원 및 로그인 시 자동 식별
- 명령 범위: 30개 이상의 상위 명령과 50개 이상의 서브 메시지(차량 위치, 경보, 정적 정보 모델 포함)
- 실행 환경: Core 메시지 펌프 기반 클라이언트/서버 세션, 요청/응답 매칭, AOT 호환, 10개 런타임 언어

## 설치

```bash
dotnet add package NewLife.JT809
```

## 빠른 시작

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

## 문서

- 런타임 지역화: [Doc/多语言本地化.md](Doc/多语言本地化.md) (중국어)
- 생태계 지도: [README.zh-CN.md](README.zh-CN.md) (중국어 정본)

## NewLife Team

Founded in 2002, the NewLife team has focused on the .NET and IoT ecosystem for more than twenty years and maintains nearly one hundred open source projects. Website: https://newlifex.com · Organization: https://github.com/NewLifeX

## 라이선스

MIT License © 2002-2026 NewLife Team

