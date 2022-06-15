using System;
using System.Collections.Generic;
using System.Text;

namespace NewLife.JT809.Protocols;

public interface ISubMessage
{
    UInt16 DataType { get; set; }
}