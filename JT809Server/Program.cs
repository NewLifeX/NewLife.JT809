using JT809Server;
using JT809Server.Controllers;
using NewLife.JT809.Handlers;
using NewLife.Log;

// 启用控制台日志，拦截所有异常
XTrace.UseConsole();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        // 配置星尘。借助StarAgent，或者读取配置文件 config/star.config 中的服务器地址
        var star = services.AddStardust();

        services.AddSingleton<IJT809Handler, NormalController>();
        services.AddSingleton<IJT809Handler, InfoController>();
        services.AddSingleton<IJT809Handler, J1200Controller>();
        services.AddSingleton<IJT809Handler, J1400Controller>();

        services.AddHostedService<Worker>();
        services.AddHostedService<JT809Worker>();
    })
    .Build();

await host.RunAsync();
