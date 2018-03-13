using System;
using System.Threading;
using DBreeze;
using RestFul;
using RestFul.Loggers;
using Restful.Serilog;
using Serilog;
using Unity;
using Unity.Lifetime;
using ScraperFramework.Data;
using ScraperFramework.Data.Concrete;
using ScraperFramework.Services;
using ScraperFramework.Services.Concrete;

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
                .RegisterInstance(new DBreezeEngine(_config.DBreezeDataFolderName), new ContainerControlledLifetimeManager())
                .RegisterInstance(RestfulServerFactory.Create(c => 
                {
                    c.Register<IRestFulLogger>((_) => new SerilogLogger(Log.Logger));
                }))
                .RegisterType<ICrawlLogRepo, CrawlLogRepo>()
                .RegisterType<IKeywordRepo, KeywordRepo>()
                .RegisterType<ISearchTargetRepo, SearchTargetRepo>()
                .RegisterType<IKeywordSearchTargetService, KeywordSearchTargetService>()
                .RegisterType<IController, Controller>();

            return Container.Resolve<IController>();
        }
    }
}
