using NewLife.Serialization;

namespace NewLife.JT809.Serialization;

internal class MyBinaryGeneral : BinaryGeneral
{
    public override String ReadString() => base.ReadString()?.TrimEnd('\0');

    public override Boolean TryRead(Type type, ref Object value)
    {
        if (type == null) type = value?.GetType();

        if (type == typeof(DateTime))
        {
            value = ReadInt64().ToDateTime();
            return true;
        }

        return base.TryRead(type, ref value);
    }

    public override Boolean Write(Object value, Type type)
    {
        if (type == null) type = value?.GetType();

        if (type == typeof(DateTime) && value is DateTime dt)
        {
            base.Write(dt.ToLong());
            return true;
        }

        return base.Write(value, type);
    }
}