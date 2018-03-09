using System;
using System.Threading;
using Unity;

namespace ScraperFramework.Configuration
{
    public class ScraperBuilder : IScraperBuilder
    {
        public IUnityContainer Container { get; } = new UnityContainer();
        private readonly ScraperConfig _config = new ScraperConfig();

        public ScraperBuilder(Action<ScraperConfig> setup)
        {
            if (setup == null)
            {
                throw new ArgumentNullException(nameof(setup));
            }

            setup.Invoke(_config);
        }

        public IController Build()
        {
            Container
                .RegisterInstance(_config)
                .RegisterInstance(new CancellationTokenSource())
                .RegisterType<ICommandListener, CommandListener>();

            return Container.Resolve<IController>();
        }
    }
}
