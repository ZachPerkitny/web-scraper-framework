using System;
using System.Threading;
using DBreeze;
using RestFul;
using RestFul.Loggers;
using Restful.Serilog;
using Serilog;
using Unity;
using Unity.Lifetime;
using ScraperFramework.Controllers;
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

        public ICoordinator Build()
        {
            Container
                .RegisterInstance(_config)
                .RegisterInstance(new CancellationTokenSource())
                .RegisterInstance(new DBreezeEngine(_config.DBreezeDataFolderName), new ContainerControlledLifetimeManager())
                .RegisterInstance(RestfulServerFactory.Create(c => 
                {
                    c.Register<IRestFulLogger>((_) => new SerilogLogger(Log.Logger));
                    // TODO(zvp): add unity adaptor
                    c.Register((_) => new KeywordController(Container.Resolve<IKeywordRepo>()));
                    c.Register((_) => new SearchTargetController(Container.Resolve<ISearchTargetRepo>()));
                    c.Register((_) => new KeywordSearchTargetController(Container.Resolve<IKeywordSearchTargetRepo>()));
                }))
                .RegisterType<ICrawlLogRepo, CrawlLogRepo>()
                .RegisterType<IKeywordRepo, KeywordRepo>()
                .RegisterType<IKeywordSearchTargetRepo, KeywordSearchTargetRepo>()
                .RegisterType<ISearchTargetRepo, SearchTargetRepo>()
                .RegisterType<ICrawlService, CrawlService>()
                .RegisterType<IScraperQueue, ScraperQueue>()
                .RegisterType<ICoordinator, Coordinator>();

            return Container.Resolve<ICoordinator>();
        }
    }
}
