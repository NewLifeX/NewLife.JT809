using System.Text;

namespace NewLife.JT809.Models;

/// <summary>车辆运营信息。（静态信息）</summary>
public class CarInfo
{
    #region 属性
    /// <summary>车牌号</summary>
    public String Vin { get; set; }

    /// <summary>车辆颜色</summary>
    public JT809VehicleColorType VehicleColor { get; set; }

    /// <summary>车辆类型</summary>
    public Int32 VehicleType { get; set; }

    /// <summary>运输行业编码</summary>
    public String TransType { get; set; }

    /// <summary>车籍地</summary>
    public String VehicleNationality { get; set; }

    /// <summary>业户ID</summary>
    public String OwnersId { get; set; }

    /// <summary>业户名称</summary>
    public String OwnersName { get; set; }

    /// <summary>业户联系电话</summary>
    public String OwnersTel { get; set; }
    #endregion

    #region 方法
    /// <summary>解析静态信息文本。空文本不改变任何字段</summary>
    /// <param name="value">形如 VIN:=xxx;VEHICLE_COLOR:=2; 的键值文本</param>
    public void Parse(String value)
    {
        if (value.IsNullOrEmpty()) return;

        var dic = value.SplitAsDictionary(":=", ";");
        if (dic.TryGetValue("VIN", out var str)) Vin = str;
        if (dic.TryGetValue("Vehicle_Color", out str)) VehicleColor = (JT809VehicleColorType)str.ToInt();
        if (dic.TryGetValue("Vehicle_Type", out str)) VehicleType = str.ToInt();
        if (dic.TryGetValue("Trans_Type", out str)) TransType = str;
        if (dic.TryGetValue("Vehicle_Nationality", out str)) VehicleNationality = str;
        if (dic.TryGetValue("Owers_Id", out str)) OwnersId = str;
        if (dic.TryGetValue("Owers_Name", out str)) OwnersName = str;
        if (dic.TryGetValue("Owers_Tel", out str)) OwnersTel = str;
    }

    public String ToStr()
    {
        var sb = new StringBuilder();

        sb.AppendFormat("VIN:={0};", Vin);
        sb.AppendFormat("VEHICLE_COLOR:={0};", (Int32)VehicleColor);
        sb.AppendFormat("VEHICLE_TYPE:={0};", VehicleType);
        sb.AppendFormat("TRANS_TYPE:={0};", TransType);
        sb.AppendFormat("VEHICLE_NATIONALITY:={0};", VehicleNationality);
        sb.AppendFormat("OWERS_ID:={0};", OwnersId);
        sb.AppendFormat("OWERS_NAME:={0};", OwnersName);
        sb.AppendFormat("OWERS_TEL:={0}", OwnersTel);

        return sb.ToString();
    }
    #endregion
}
