using System;
using System.Threading;
using Serilog;

namespace ScraperFramework
{
    class Controller : IController
    {
        private readonly ICommandListener _commandListener;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public Controller(ICommandListener commandListener, CancellationTokenSource cancellationTokenSource)
        {
            _commandListener = commandListener ?? throw new ArgumentNullException(nameof(commandListener));
            _cancellationTokenSource = cancellationTokenSource ?? throw new ArgumentNullException(nameof(cancellationTokenSource));
        }

        public void Start()
        {
            Log.Information("Starting Command Listener");
            _commandListener.Listen(new string[]
            {
                "http://*:8080/"
            }, _cancellationTokenSource.Token);
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
