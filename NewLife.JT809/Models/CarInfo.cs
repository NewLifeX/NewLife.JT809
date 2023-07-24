using System.Text;

namespace NewLife.JT809.Models;

/// <summary>车辆运营信息。（静态信息）</summary>
public class CarInfo
{
    #region 属性
    /// <summary>车牌号</summary>
    public String Vin { get; set; }

    /// <summary>车辆颜色</summary>
    public JT809VehicleColorType Vehicle_Color { get; set; }

    /// <summary>车辆类型</summary>
    public Int32 Vehicle_Type { get; set; }

    /// <summary>运输行业编码</summary>
    public String Trans_Type { get; set; }

    /// <summary>车籍地</summary>
    public String Vehicle_Nationality { get; set; }

    /// <summary>业户ID</summary>
    public String Owners_Id { get; set; }

    /// <summary>业户名称</summary>
    public String Owners_Name { get; set; }

    /// <summary>业户联系电话</summary>
    public String Owners_Tel { get; set; }
    #endregion

    #region 方法
    public void Parse(String value)
    {
        var dic = value.SplitAsDictionary(":=", ";");
        if (dic.TryGetValue("VIN", out var str)) Vin = str;
        if (dic.TryGetValue("Vehicle_Color", out str)) Vehicle_Color = (JT809VehicleColorType)str.ToInt();
        if (dic.TryGetValue("Vehicle_Type", out str)) Vehicle_Type = str.ToInt();
        if (dic.TryGetValue("Trans_Type", out str)) Trans_Type = str;
        if (dic.TryGetValue("Vehicle_Nationality", out str)) Vehicle_Nationality = str;
        if (dic.TryGetValue("Owers_Id", out str)) Owners_Id = str;
        if (dic.TryGetValue("Owers_Name", out str)) Owners_Name = str;
        if (dic.TryGetValue("Owers_Tel", out str)) Owners_Tel = str;
    }

    public String ToStr()
    {
        var sb = new StringBuilder();

        sb.AppendFormat("VIN:={0};", Vin);
        sb.AppendFormat("VEHICLE_COLOR:={0};", (Int32)Vehicle_Color);
        sb.AppendFormat("VEHICLE_TYPE:={0};", Vehicle_Type);
        sb.AppendFormat("TRANS_TYPE:={0};", Trans_Type);
        sb.AppendFormat("VEHICLE_NATIONALITY:={0};", Vehicle_Nationality);
        sb.AppendFormat("OWERS_ID:={0};", Owners_Id);
        sb.AppendFormat("OWERS_NAME:={0};", Owners_Name);
        sb.AppendFormat("OWERS_TEL:={0}", Owners_Tel);

        return sb.ToString();
    }
    #endregion
}
