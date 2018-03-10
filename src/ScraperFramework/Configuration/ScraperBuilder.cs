using System;
using System.Reflection;
using System.Threading;
using DBreeze;
using Unity;
using Unity.Injection;
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
                .RegisterInstance<DBreezeEngine>(new DBreezeEngine(_config.DBreezeDataFolderName), new ContainerControlledLifetimeManager())
                .RegisterMediator(new HierarchicalLifetimeManager())
                .RegisterMediatorHandlers(Assembly.GetExecutingAssembly())
                .RegisterType<ICrawlLogRepo, CrawlLogRepo>()
                .RegisterType<IKeywordRepo, KeywordRepo>()
                .RegisterType<ISearchTargetRepo, SearchTargetRepo>()
                .RegisterType<IKeywordSearchTargetService, KeywordSearchTargetService>()
                .RegisterType<IHttpRequestHandler, CommandListener>()
                .RegisterType<IHttpServer, HttpServer>()
                .RegisterType<IController, Controller>();

            return Container.Resolve<IController>();
        }
    }
}
