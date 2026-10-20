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

> 이 번역은 [영문 README](README.md)보다 늦을 수 있습니다.

도로 운송(중국) 플랫폼 간 연계를 위한 JT/T 809 프로토콜 라이브러리입니다. 프레임 코덱, 2011/2013/2019 메시지, 주 링크 및 종속 링크의 클라이언트/서버 세션을 제공합니다.

소스: https://github.com/NewLifeX/NewLife.JT809  
NuGet: https://www.nuget.org/packages/NewLife.JT809  

## 주요 기능

- 프레임 코덱: 0x5B/0x5D 구분자, 이스케이프 처리, CRC-16 및 링크 암호화
- 프로토콜 버전: JT/T 809-2011, 2013, 2019 지원 및 로그인 시 자동 식별
- 명령 범위: 30개 이상의 상위 명령과 50개 이상의 서브 메시지(차량 위치, 경보, 정적 정보 모델 포함)
- 통합: `[MessageKind]` 핸들러 메서드, 요청/응답 매칭, 재연결
- 엔지니어링: Native AOT 호환, 런타임 메시지 11개 언어(번역 10종 + 영어 원문)

## 요구 사항

- 대상 프레임워크 `netstandard2.0`, `net8.0`, `net10.0`이며 Windows, Linux, macOS에서 동작합니다
- NewLife.Core에 의존합니다(패키지 복원 시 자동 포함). 소스 빌드에는 .NET 8 SDK 이상이 필요합니다
- 프로토콜 규격: [Doc](Doc) (JT/T 809-2011 및 JT/T 809-2019)

## 설치

```bash
dotnet add package NewLife.JT809
```

## 빠른 시작

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
    Time = DateTime.Now,
    Longitude = 117123456,
    Latitude = 31876543,
    Speed = 600,
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

## 문서

- 런타임 지역화: [Doc/多语言本地化.md](Doc/多语言本地化.md) (중국어)
- 생태계 지도: [README.zh-CN.md](README.zh-CN.md) (중국어 정본)

## NewLife Team

Founded in 2002, the NewLife team has focused on the .NET and IoT ecosystem for more than twenty years and maintains nearly one hundred open source projects. Website: https://newlifex.com · Organization: https://github.com/NewLifeX

## 라이선스

MIT License © 2002-2026 NewLife Team

