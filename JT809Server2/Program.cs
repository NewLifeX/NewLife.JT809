using NewLife.JT809.Protocols;
using NewLife.Log;
using Stardust;

namespace JT809Server2;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        XTrace.UseWinForm();

        // 加大网络HEX日志长度
        var set = NewLife.Net.SocketSetting.Current;
        if (set.LogDataLength <= 64)
        {
            set.LogDataLength = 1024;
            set.Save();
        }

        // 配置星尘。借助StarAgent，或者读取配置文件 config/star.config 中的服务器地址
        var star = new StarFactory();

        MessageFactory.Instance.Init();

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new FrmMain());
    }
}