using System.ComponentModel;
using NewLife.Configuration;

namespace JT809Server;

/// <summary>JT809设置</summary>
[DisplayName("JT809设置")]
[Config("JT809")]
public class JT809Setting : Config<JT809Setting>
{
    #region 属性
    /// <summary>调试。默认启用</summary>
    [Description("调试。默认启用")]
    public Boolean Debug { get; set; } = true;

    /// <summary>端口。默认809</summary>
    [Description("端口。默认809")]
    public Int32 Port { get; set; } = 809;
    #endregion

    #region 方法
    /// <summary>加载完成后</summary>
    protected override void OnLoaded()
    {
        if (Port == 0) Port = 809;

        base.OnLoaded();
    }
    #endregion
}