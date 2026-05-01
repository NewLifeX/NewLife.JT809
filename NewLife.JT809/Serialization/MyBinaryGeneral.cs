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
            if (TryReadInt64(out var num))
                value = num.ToDateTime();
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