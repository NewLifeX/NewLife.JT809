using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewLife.JT809.Protocols;
using NewLife.Net;

namespace JT809Server.Controllers;

/// <summary>消息处理委托</summary>
/// <param name="session"></param>
/// <param name="message"></param>
/// <returns></returns>
public delegate Object MessageHandler(INetSession session, JTMessage message);
