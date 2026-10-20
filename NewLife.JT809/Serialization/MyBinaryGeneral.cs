using NewLife.Serialization;

namespace NewLife.JT809.Serialization;

internal class MyBinaryGeneral : BinaryGeneral
{
    public override Boolean TryReadString(out String value)
    {
        if (!base.TryReadString(out value)) return false;

        value = value?.TrimEnd('\0');
        return true;
    }

    public override Boolean TryRead(Type type, ref Object value)
    {
        if (type == null) type = value?.GetType();

        if (type == typeof(DateTime))
        {
            // 协议中的 UTC 时间字段为 8 字节 Unix 秒；0 表示未设置
            if (TryReadInt64(out var num))
                value = num > 0 ? num.ToDateTime() : default(DateTime);
            return true;
        }

        return base.TryRead(type, ref value);
    }

    public override Boolean Write(Object value, Type type)
    {
        if (type == null) type = value?.GetType();

        if (type == typeof(DateTime) && value is DateTime dt)
        {
            // ToLong 为 Unix 毫秒，协议时间字段为 Unix 秒，必须换算，否则对端会读到数万年后
            base.Write(dt.ToLong() / 1000);
            return true;
        }

        return base.Write(value, type);
    }
}