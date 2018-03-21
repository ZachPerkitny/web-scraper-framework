using System;
using System.Threading;
using DBreeze;
using RestFul;
using RestFul.Loggers;
using Restful.Serilog;
using Serilog;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using ScraperFramework.Data;
using ScraperFramework.Data.Concrete;
using ScraperFramework.Pipeline;
using ScraperFramework.Pocos;
using ScraperFramework.Sync;

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
                }))
                .RegisterType<ConnectionFactory>(new InjectionConstructor(_config.ScraperConnectionString))
                .RegisterType<IDataStore, DataStore>()
                .RegisterType<IInternationalUULERepo, InternationalUULERepo>()
                .RegisterType<IKeywordRepo, KeywordRepo>()
                .RegisterType<ILocalUULERepo, LocalUULERepo>()
                .RegisterType<IProxyMultiplierRepo, ProxyMultiplierRepo>()
                .RegisterType<IProxyRepo, ProxyRepo>()
                .RegisterType<ISearchEngineRepo, SearchEngineRepo>()
                .RegisterType<ISearchStringRepo, SearchStringRepo>()
                // start weird
                .RegisterType<UserAgentPipe>()
                .RegisterType<SearchUrlPipe>()
                .RegisterType<PipeLine<PipelinedCrawlDescription>>(
                    new InjectionFactory(c => (new CrawlDescriptionPipeline(Container.Resolve<IProxyRepo>()))
                        .Connect(Container.Resolve<SearchUrlPipe>())
                        .Connect(Container.Resolve<UserAgentPipe>())))
                .RegisterType<KeywordSyncTask>()
                .RegisterType<SearchEngineSyncTask>()
                .RegisterType<SearchStringSyncTask>()
                .RegisterType<ISyncer>(
                    new InjectionFactory(c => (new Syncer(_config.SyncInterval))
                        .AddSyncTask(Container.Resolve<KeywordSyncTask>())
                        .AddSyncTask(Container.Resolve<SearchEngineSyncTask>())
                        .AddSyncTask(Container.Resolve<SearchStringSyncTask>())))
                // end weird
                .RegisterType<IScraperQueue, ScraperQueue>()
                .RegisterType<IScraperFactory, ScraperFactory>()
                .RegisterType<ICoordinator, Coordinator>();

            return Container.Resolve<ICoordinator>();
        }
    }
}
