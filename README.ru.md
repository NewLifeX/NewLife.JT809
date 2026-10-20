# NewLife.JT809 - протокол JT/T 809

![GitHub License](https://img.shields.io/github/license/NewLifeX/NewLife.JT809?logo=github)
![NuGet Downloads](https://img.shields.io/nuget/dt/NewLife.JT809?logo=nuget)
![NuGet](https://img.shields.io/nuget/v/NewLife.JT809?logo=nuget)
![NuGet prerelease](https://img.shields.io/nuget/vpre/NewLife.JT809?label=dev%20nuget&logo=nuget)
[![test](https://github.com/NewLifeX/NewLife.JT809/actions/workflows/test.yml/badge.svg?branch=master)](https://github.com/NewLifeX/NewLife.JT809/actions/workflows/test.yml)
![platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux%20%7C%20macOS-blue)
[![contributors](https://img.shields.io/github/contributors/NewLifeX/NewLife.JT809?logo=github)](https://github.com/NewLifeX/NewLife.JT809/graphs/contributors)

<!-- lang:start -->
🌐 [English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [Español](README.es.md) | [Deutsch](README.de.md) | [日本語](README.ja.md) | [Français](README.fr.md) | [Português](README.pt.md) | [한국어](README.ko.md) | [العربية](README.ar.md)
<!-- lang:end -->

<!-- metrics:start -->
[![TFM](https://img.shields.io/badge/TFM-3%20%28netstandard2.0%20%7C%20net8.0%20%7C%20net10.0%29-512BD4?logo=dotnet)](https://www.nuget.org/packages/NewLife.JT809)
[![locales](https://img.shields.io/badge/locales-10-2ea44f?logo=dotnet)](Doc/多语言本地化.md)
<!-- metrics:end -->

> Этот перевод может отставать от [английского README](README.md).

Библиотека протокола JT/T 809 для межплатформенного взаимодействия на автомобильном транспорте (Китай). Включает кодек кадров, сообщения версий 2011/2013/2019 и клиентские/серверные сессии главного и подчинённого каналов.

Исходный код: https://github.com/NewLifeX/NewLife.JT809  
NuGet: https://www.nuget.org/packages/NewLife.JT809  

## Возможности

- Кодек кадров: разделители 0x5B/0x5D, экранирование, CRC-16 и шифрование канала
- Версии протокола: JT/T 809-2011, 2013 и 2019 с автоопределением при входе
- Покрытие команд: более 30 основных команд и 50 подсообщений, включая модели местоположения, тревог и статических данных
- Интеграция: методы-обработчики с `[MessageKind]`, сопоставление запрос/ответ и переподключение
- Инженерия: поддержка Native AOT; сообщения времени выполнения на 11 языках (10 таблиц перевода и английский как источник)

## Требования

- Целевые платформы `netstandard2.0`, `net8.0`, `net10.0`; работает в Windows, Linux и macOS
- Зависит от NewLife.Core (восстанавливается вместе с пакетом); для сборки из исходников нужен .NET 8 SDK или новее
- Спецификации протокола: [Doc](Doc) (JT/T 809-2011 и JT/T 809-2019)

## Установка

```bash
dotnet add package NewLife.JT809
```

## Быстрый старт

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

## Документация

- Локализация времени выполнения: [Doc/多语言本地化.md](Doc/多语言本地化.md) (китайский)
- Карта экосистемы: [README.zh-CN.md](README.zh-CN.md) (канонический китайский текст)

## NewLife Team

Founded in 2002, the NewLife team has focused on the .NET and IoT ecosystem for more than twenty years and maintains nearly one hundred open source projects. Website: https://newlifex.com · Organization: https://github.com/NewLifeX

## Лицензия

MIT License © 2002-2026 NewLife Team

