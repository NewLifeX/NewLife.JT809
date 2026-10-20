# NewLife.JT809 - protocolo JT/T 809

![GitHub License](https://img.shields.io/github/license/NewLifeX/NewLife.JT809?logo=github)
![NuGet Downloads](https://img.shields.io/nuget/dt/NewLife.JT809?logo=nuget)
![NuGet](https://img.shields.io/nuget/v/NewLife.JT809?logo=nuget)
![NuGet prerelease](https://img.shields.io/nuget/vpre/NewLife.JT809?label=dev%20nuget&logo=nuget)
[![test](https://github.com/NewLifeX/NewLife.JT809/actions/workflows/test.yml/badge.svg?branch=master)](https://github.com/NewLifeX/NewLife.JT809/actions/workflows/test.yml)
![platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux%20%7C%20macOS-blue)
[![contributors](https://img.shields.io/github/contributors/NewLifeX/NewLife.JT809?logo=github)](https://github.com/NewLifeX/NewLife.JT809/graphs/contributors)

<!-- lang:start -->
🌐 [English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [Deutsch](README.de.md) | [日本語](README.ja.md) | [Français](README.fr.md) | [Português](README.pt.md) | [Русский](README.ru.md) | [한국어](README.ko.md) | [العربية](README.ar.md)
<!-- lang:end -->

<!-- metrics:start -->
[![TFM](https://img.shields.io/badge/TFM-3%20%28netstandard2.0%20%7C%20net8.0%20%7C%20net10.0%29-512BD4?logo=dotnet)](https://www.nuget.org/packages/NewLife.JT809)
[![locales](https://img.shields.io/badge/locales-10-2ea44f?logo=dotnet)](Doc/多语言本地化.md)
<!-- metrics:end -->

> Esta traducción puede estar desactualizada; consulta el [README en inglés](README.md).

Biblioteca del protocolo JT/T 809 para la interconexión de plataformas en el transporte por carretera (China). Incluye códec de tramas, mensajes 2011/2013/2019 y sesiones cliente/servidor para enlaces maestro y esclavo.

Origen: https://github.com/NewLifeX/NewLife.JT809  
NuGet: https://www.nuget.org/packages/NewLife.JT809  

## Características

- Códec de tramas: delimitación 0x5B/0x5D, escape, CRC-16 y cifrado de enlace
- Versiones del protocolo: JT/T 809-2011, 2013 y 2019, detectadas automáticamente al iniciar sesión
- Cobertura: más de 30 comandos de nivel superior y 50 submensajes, con modelos de posición, alarma e información estática
- Integración: métodos de controlador con `[MessageKind]`, emparejamiento solicitud/respuesta y reconexión
- Ingeniería: compatible con Native AOT; mensajes en tiempo de ejecución en 11 idiomas (10 tablas de traducción y el inglés como origen)

## Requisitos

- Marcos objetivo `netstandard2.0`, `net8.0` y `net10.0`; funciona en Windows, Linux y macOS
- Depende de NewLife.Core (se restaura con el paquete); compilar el código requiere .NET 8 SDK o superior
- Especificaciones del protocolo: [Doc](Doc) (JT/T 809-2011 y JT/T 809-2019)

## Instalación

```bash
dotnet add package NewLife.JT809
```

## Inicio rápido

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

## Documentación

- Localización en tiempo de ejecución: [Doc/多语言本地化.md](Doc/多语言本地化.md) (chino)
- Mapa del ecosistema: [README.zh-CN.md](README.zh-CN.md) (texto chino canónico)

## NewLife Team

Founded in 2002, the NewLife team has focused on the .NET and IoT ecosystem for more than twenty years and maintains nearly one hundred open source projects. Website: https://newlifex.com · Organization: https://github.com/NewLifeX

## Licencia

MIT License © 2002-2026 NewLife Team

