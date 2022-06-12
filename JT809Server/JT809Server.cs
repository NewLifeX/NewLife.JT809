using System.Reflection;
using JT809Server.Controllers;
using NewLife;
using NewLife.JT809;
using NewLife.JT809.Protocols;
using NewLife.Log;
using NewLife.Net;
using NewLife.Reflection;

namespace JT809Server
{
    internal class JT809Server : NetServer<JT809Session>
    {
        public Dictionary<MessageKinds, MessageHandler> Handlers = new();

        public JT809Server()
        {
            Port = 809;
            Add<JTCodec>();
        }

        /// <summary>注册类型处理器</summary>
        /// <typeparam name="T"></typeparam>
        public void AddHandler<T>(T handler)
        {
            var type = handler.GetType();
            foreach (var item in type.GetMethods())
            {
                if (item.IsStatic) continue;
                if (item.ReturnType != typeof(Object)) continue;

                // 参数匹配
                var pis = item.GetParameters();
                if (pis.Length != 2) continue;

                // 获取类型
                var att = item.GetCustomAttribute<MessageKindAttribute>();
                if (att != null)
                {
                    Handlers[att.Kind] = item.As<MessageHandler>(handler);

                    WriteLog("注册[{0}]：{1}.{2}", att.Kind, item.DeclaringType.FullName, item.Name);
                }
            }
        }
    }

    internal class JT809Session : NetSession<JT809Server>
    {
        private Int32 _gid;
        private MessageKinds[] _nologs = new[] { MessageKinds.主链路连接保持请求消息 };
        /// <summary>接收指令</summary>
        /// <param name="e"></param>
        protected override void OnReceive(ReceivedEventArgs e)
        {
            var debug = XTrace.Log.Level <= NewLife.Log.LogLevel.Debug;
#if DEBUG
            debug = true;
#endif
            //Device = null;

            var msg = e.Message as JTMessage;
            if (debug && !_nologs.Contains(msg.Kind)) WriteLog("<= {0}", msg);
            if (msg != null)
            {
                //// 当前会话IP地址，影响内部所有创建IP和更新IP字段
                //ManageProvider.UserHost = Remote.Address + "";

                var set = JT809Setting.Current;
                ////写msg
                //logHelper.WriteLine($"{msg.Kind}--{ e.Packet?.ToHex(1024)}");

                Object result = null;
                //var _deviceService = Host.DeviceService;
                using var span = Host.Tracer?.NewSpan($"jt809:{msg.Kind}", e.Packet);
                try
                {
                    //// 所有指令自动注册设备，方便下面的处理
                    //if (set.AutoRegister) _deviceService.AutoRegister(this, null, msg.Mobile, true);

                    //Device = _deviceService.GetDevice(this, msg.Mobile);

                    // 找到处理器
                    if (!Host.Handlers.TryGetValue(msg.Kind, out var handler)) throw new NotSupportedException($"未支持指令[{msg.Kind}]");

                    // 执行处理器
                    result = handler(this, msg);
                }
                catch (Exception ex)
                {
                    XTrace.WriteException(ex);

                    var hex = e.Packet?.ToHex(1024);
                    span?.SetError(ex, hex);

                    XTrace.WriteLine(hex);

                    //// 写设备历史
                    //var dv = _deviceService.GetDevice(this, msg.Mobile);
                    //_deviceService.WriteHistory(dv, msg.Kind + "", false, ex.Message + " " + msg.ToJsonJT808(), Remote.Address + "");

                    //var kind = ex switch
                    //{
                    //    NotSupportedException => ResultKinds.NotSupport,
                    //    InvalidDataException => ResultKinds.Error,
                    //    _ => ResultKinds.Failure,
                    //};
                    //result = new ServerResponseMessage { Result = kind };
                }

                // 处理响应
                if (result != null)
                {
                    //if (result is ServerResponseMessage response)
                    //{
                    //    response.Kind = msg.Kind;
                    //    response.Sequence = msg.Sequence;
                    //}
                    //else if (result is ClientResponseMessage response2)
                    //{
                    //    //response2.Kind = msg.Kind;
                    //    //response2.Sequence = msg.Sequence;
                    //}
                    //else
                    {
                        if (debug) WriteLog("响应 {0}", result.ToJsonJT809());
                    }

                    var rs = msg.CreateReply() as JTMessage;
                    rs.Sequence = (UInt16)Interlocked.Increment(ref _gid);
                    rs.SetResult(result);

                    if (debug && !_nologs.Contains(msg.Kind)) WriteLog("=> {0}", rs);

                    //Send(rs.ToPacket());
                    Session.SendMessage(rs);
                }
            }

            // 父级 OnReceive 触发事件，调用 NetServer.OnReceive
            base.OnReceive(e);
        }

        protected override void OnError(Object sender, ExceptionEventArgs e) => XTrace.WriteException(e.Exception);
    }
}