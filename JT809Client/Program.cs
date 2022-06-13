using NewLife.Log;

namespace JT809Client;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        XTrace.UseWinForm();

        // 加大网络HEX日志长度
        var set = NewLife.Net.Setting.Current;
        if (set.LogDataLength <= 64)
        {
            set.LogDataLength = 1024;
            set.Save();
        }

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new FrmMain());
    }
}