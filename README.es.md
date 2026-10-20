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

Biblioteca del protocolo JT/T 809 para la interconexión de plataformas en el transporte por carretera (China). Incluye códec de tramas, mensajes 2011/2013/2019 y sesiones cliente/servidor para enlaces maestro y esclavo.

Origen: https://github.com/NewLifeX/NewLife.JT809  
NuGet: https://www.nuget.org/packages/NewLife.JT809  

## Características

- Códec de tramas: delimitación 0x5B/0x5D, escape, CRC-16 y cifrado de enlace
- Versiones del protocolo: JT/T 809-2011, 2013 y 2019, detectadas automáticamente al iniciar sesión
- Cobertura: más de 30 comandos de nivel superior y 50 submensajes, con modelos de posición, alarma e información estática
- Ejecución: sesiones cliente/servidor sobre el bombeo de mensajes de Core, emparejamiento solicitud/respuesta, compatible con AOT y 10 idiomas en tiempo de ejecución

## Instalación

```bash
dotnet add package NewLife.JT809
```

## Inicio rápido

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

## Documentación

- Localización en tiempo de ejecución: [Doc/多语言本地化.md](Doc/多语言本地化.md) (chino)
- Mapa del ecosistema: [README.zh-CN.md](README.zh-CN.md) (texto chino canónico)

## NewLife Team

Founded in 2002, the NewLife team has focused on the .NET and IoT ecosystem for more than twenty years and maintains nearly one hundred open source projects. Website: https://newlifex.com · Organization: https://github.com/NewLifeX

## Licencia

MIT License © 2002-2026 NewLife Team

