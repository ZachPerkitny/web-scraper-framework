using System;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.OffScreen;
using Serilog;
using WebScraper.Enum;
using WebScraper.Extractors;
using WebScraper.Pocos;

namespace WebScraper
{
    internal class Scraper : IDisposable
    {
        public CrawlDescription CrawlDescription { get; private set; }
        
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
                UserAgent = CrawlDescription.UserAgent
            };

            cefSettings.CefCommandLineArgs.Add("proxy-server", $"{CrawlDescription.IP}:{CrawlDescription.Port}");

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
            CrawlResult crawlResult = new CrawlResult();

            BrowserSettings browserSettings = new BrowserSettings
            {
                WindowlessFrameRate = 1
            };

            using (ChromiumWebBrowser browser = new ChromiumWebBrowser(browserSettings: browserSettings))
            {
                try
                {
                    await WaitForBrowserInit(browser);
                    await LoadAsync(browser, string.Format(CrawlDescription.SearchString, CrawlDescription.Keyword));

                    crawlResult.Ads = await BingExtractor.ExtractTextAds(browser);
                    crawlResult.CrawlResultID = CrawlResultID.Success;
                }
                catch (Exception ex)
                {
                    Log.Error("Scraper Exception({0}): {1}", ex.GetType(), ex.Message);
                    crawlResult.CrawlResultID = CrawlResultID.Failure;
                }
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
