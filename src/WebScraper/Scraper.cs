using System;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.OffScreen;
using WebScraper.Extractors;
using WebScraper.Pocos;

namespace WebScraper
{
    internal class Scraper : IDisposable
    {
        public CrawlDescription CrawlDescription { get; private set; }
        
        private const string BING_URL = "https://www.bing.com/search?q={0}";
        private bool _disposed = false;

        public Scraper(CrawlDescription crawlDescription)
        {
            CrawlDescription = crawlDescription ?? throw new ArgumentNullException(nameof(crawlDescription));
        }

        public void Initialize()
        {
            CefSettings cefSettings = new CefSettings
            {
                LogSeverity = LogSeverity.Disable,
                //LogFile = "",
                /*
                 *  Custom flags used when initializing V8.
                 *  See https://github.com/v8/v8/blob/master/src/flag-definitions.h
                 */
                //JavascriptFlags = "",
                UserAgent = CrawlDescription.EndpointAddress
            };

            cefSettings.CefCommandLineArgs.Add("proxy-server", CrawlDescription.EndpointAddress);

            // Disable WebGL
            //cefSettings.DisableGpuAcceleration();

            // This will also diable WebGL and perform 
            // some magic to optimize performance.
            cefSettings.SetOffScreenRenderingBestPerformanceArgs();

            if (!Cef.Initialize(cefSettings, performDependencyCheck: true, browserProcessHandler: null))
            {
                throw new Exception("Unable to Start CEF");
            }
        }

        public async Task<CrawlResult> Scrape()
        {
            // TODO(zvp): Add WebsiteID to Crawl Desc
            // TODO(zvp): Add Search String to Crawl Desc
            CrawlResult crawlResult = new CrawlResult
            {
                KeywordID = CrawlDescription.KeywordID,
                SearchTargetID = CrawlDescription.SearchTargetID
            };

            using (ChromiumWebBrowser browser = new ChromiumWebBrowser())
            {
                await WaitForBrowserInit(browser);
                await LoadAsync(browser, string.Format(BING_URL, CrawlDescription.Keyword));

                crawlResult.Ads = await BingExtractor.ExtractTextAds(browser);
            }

            return crawlResult;
        }

        /// <summary>
        /// Waits for the browser initialized event to fire.
        /// </summary>
        /// <param name="browser"></param>
        /// <returns></returns>
        private Task WaitForBrowserInit(ChromiumWebBrowser browser)
        {
            var source = new TaskCompletionSource<bool>();

            browser.BrowserInitialized += (sender, args) =>
            {
                source.TrySetResult(true);
            };

            return source.Task;
        }

        /// <summary>
        /// Loads the url specified asynchronously
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private Task LoadAsync(ChromiumWebBrowser browser, string url)
        {
            var source = new TaskCompletionSource<bool>();

            /*
             * This Event is fired off twice, once when loading is initiated
             * through user action or programatically, and once when loading
             * is termianated due to completion, failure or cancellation. This event
             * is fired on UI Thread, TID_UI thread is the main thread in the browser process. 
             * Do not block.
             */
            EventHandler<LoadingStateChangedEventArgs> handler = null;
            handler = (sender, args) =>
            {
                if (!args.IsLoading)
                {
                    browser.LoadingStateChanged -= handler;

                    source.TrySetResult(true);
                }
            };

            browser.LoadingStateChanged += handler;

            browser.Load(url);

            return source.Task;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // cef shutdown
                    // must be explicitly call or application will hang
                    Cef.Shutdown();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
