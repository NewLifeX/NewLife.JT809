using NewLife.Serialization;

namespace NewLife.JT809.Serialization;

internal class MyBinaryGeneral : BinaryGeneral
{
    public override String ReadString() => base.ReadString()?.TrimEnd('\0');
}