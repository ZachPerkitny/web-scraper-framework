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
                .RegisterType<IKeywordScrapeDetailRepo, KeywordScrapeDetailRepo>()
                .RegisterType<ILocalUULERepo, LocalUULERepo>()
                .RegisterType<IProxyMultiplierRepo, ProxyMultiplierRepo>("DBreezeProxyMultiplierRepo")
                .RegisterType<IProxyMultiplierRepo, CachedProxyMultiplierRepo>(
                    new InjectionConstructor(
                        new ResolvedParameter<IProxyMultiplierRepo>("DBreezeProxyMultiplierRepo")))
                .RegisterType<IProxyRepo, ProxyRepo>("DBreezeProxyRepo")
                .RegisterType<IProxyRepo, CachedProxyRepo>(
                    new InjectionConstructor(
                        new ResolvedParameter<IProxyRepo>("DBreezeProxyRepo")))
                .RegisterType<ISearchEngineRepo, SearchEngineRepo>()
                .RegisterType<ISearchStringRepo, SearchStringRepo>()
                .RegisterType<ISpecialKeywordRepo, SpecialKeywordRepo>()
                // start weird
                //.RegisterType<UserAgentPipe>()
                .RegisterType<ProxyPipe>()
                .RegisterType<SearchUrlPipe>()
                .RegisterType<PipeLine<PipelinedCrawlDescription>>(
                    new InjectionFactory(c => (new KeywordDrivenPipeline(
                        Container.Resolve<IKeywordManager>()))
                        .Connect(Container.Resolve<ProxyPipe>())
                        .Connect(Container.Resolve<SearchUrlPipe>())))
                .RegisterType<KeywordSyncTask>()
                .RegisterType<KeywordScrapeDetailSyncTask>(
                    new InjectionConstructor(_config.ScraperNo, 
                    typeof(IDataStore), typeof(IKeywordScrapeDetailRepo)))
                .RegisterType<ProxyMultiplierSyncTask>()
                .RegisterType<ProxySyncTask>() 
                .RegisterType<SearchEngineSyncTask>()
                .RegisterType<SearchStringSyncTask>()
                .RegisterType<SpecialKeywordSyncTask>()
                .RegisterType<ISyncer>(
                    new InjectionFactory(c => (new Syncer(_config.SyncInterval))
                        .AddSyncTask(Container.Resolve<KeywordSyncTask>())
                        .AddSyncTask(Container.Resolve<KeywordScrapeDetailSyncTask>())
                        .AddSyncTask(Container.Resolve<ProxyMultiplierSyncTask>())
                        .AddSyncTask(Container.Resolve<ProxySyncTask>())
                        .AddSyncTask(Container.Resolve<SearchEngineSyncTask>())
                        .AddSyncTask(Container.Resolve<SearchStringSyncTask>())
                        .AddSyncTask(Container.Resolve<SpecialKeywordSyncTask>())))
                // end weird
                .RegisterType<ICrawlLogger, CrawlLogger>(new PerThreadLifetimeManager())
                .RegisterType<IKeywordManager, KeywordManager>(new PerThreadLifetimeManager())
                .RegisterType<IProxyManager, ProxyManager>(new PerThreadLifetimeManager())
                .RegisterType<IScraperQueue, ScraperQueue>(new PerThreadLifetimeManager())
                .RegisterType<IScraperFactory, ScraperFactory>()
                .RegisterType<ICoordinator, Coordinator>();

            return Container.Resolve<ICoordinator>();
        }
    }
}
