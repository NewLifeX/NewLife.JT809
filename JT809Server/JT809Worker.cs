using JT809Server.Controllers;
using NewLife;
using NewLife.Log;

namespace JT809Server
{
    public class JT809Worker : IHostedService
    {
        private readonly ITracer _tracer;
        private readonly IServiceProvider _serviceProvider;
        private JT809Server _server;

        public JT809Worker(ITracer tracer, IServiceProvider serviceProvider)
        {
            _tracer = tracer;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var set = JT809Setting.Current;

            var server = new JT809Server
            {
                Port = set.Port,

                Tracer = _tracer,
                Log = XTrace.Log,
                SessionLog = XTrace.Log,
            };

#if DEBUG
            if (set.Debug)
            {
                server.LogSend = true;
                server.LogReceive = true;
                server.SocketLog = XTrace.Log;
            }
#endif

            foreach (var item in _serviceProvider.GetServices<IJT809Handler>())
            {
                server.AddHandler(item);
            }

            server.Start();

            _server = server;

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _server.TryDispose();
            _server = null;

            return Task.CompletedTask;
        }
    }
}