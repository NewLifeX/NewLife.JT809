namespace NewLife.JT809.Protocols;

/// <summary>协议配置</summary>
public class JTConfig
{
    /// <summary>协议版本</summary>
    public JTVersion Version { get; set; }

    /// <summary>协议版本号标识。上下级平台之间采用的标准协议版本编号，长度3字节，0x01020F表示v1.2.15</summary>
    public UInt32 VersionFlag { get; set; } = 0x01020F;

    /// <summary>加密密钥</summary>
    public UInt32 M1 { get; set; }

    /// <summary>加密密钥</summary>
    public UInt32 IA1 { get; set; }

    /// <summary>加密密钥</summary>
    public UInt32 IC1 { get; set; }
}