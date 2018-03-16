using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Serilog;
using WebScraper.Pocos;
using ScraperFramework.Services;

namespace ScraperFramework
{
    class Scraper : IScraper
    {
        private readonly ILoggerService _loggerService;
        private readonly IScraperQueue _scraperQueue;
        private readonly CancellationToken _cancellationToken;

        public Scraper(ILoggerService loggerService, IScraperQueue scraperQueue, CancellationToken cancellationToken)
        {
            _loggerService = loggerService ?? throw new ArgumentNullException(nameof(loggerService));
            _scraperQueue = scraperQueue ?? throw new ArgumentNullException(nameof(scraperQueue));
            _cancellationToken = cancellationToken;
        }

        public async Task Start()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                CrawlDescription crawlDescription = await _scraperQueue.Dequeue();
                CrawlResult crawlResult = null;

                Process pipeClient = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "WebScraper.exe",
                        CreateNoWindow = false,
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory
                    }
                };

                // For In-Out Inter-Process Communication
                using (AnonymousPipeServerStream pipeServerWriter =
                    new AnonymousPipeServerStream(PipeDirection.Out,
                    HandleInheritability.Inheritable))
                using (AnonymousPipeServerStream pipeServerReader =
                    new AnonymousPipeServerStream(PipeDirection.In,
                    HandleInheritability.Inheritable))
                {
                    // Start Pipe Client (WebScraper.exe)
                    pipeClient.StartInfo.Arguments =
                        $"{pipeServerWriter.GetClientHandleAsString()} {pipeServerReader.GetClientHandleAsString()}";
                    pipeClient.Start();

                    // release object handles
                    pipeServerWriter.DisposeLocalCopyOfClientHandle();
                    pipeServerReader.DisposeLocalCopyOfClientHandle();

                    try
                    {
                        using (StreamWriter sw = new StreamWriter(pipeServerWriter))
                        {
                            // flush after every write
                            sw.AutoFlush = true;

                            // write sync message
                            await sw.WriteLineAsync("SYNC");
                            pipeServerWriter.WaitForPipeDrain();

                            // write crawl description
                            string serializedCrawlDescription = JsonConvert.SerializeObject(crawlDescription);
                            await sw.WriteLineAsync(serializedCrawlDescription);
                        }

                        using (StreamReader sr = new StreamReader(pipeServerReader))
                        {
                            string message;

                            do
                            {
                                // TODO(zvp) : have to exit eventually.
                                message = await sr.ReadLineAsync();
                                Log.Debug("Pipe Received Message: {0}", message);
                            } while (message == null || !message.StartsWith("SYNC"));

                            message = await sr.ReadLineAsync();
                            crawlResult = JsonConvert.DeserializeObject<CrawlResult>(message);
                            Log.Debug("Pipe Received Crawl Result: {0}", message);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("WebScraper Exception({0}): {1}", ex.GetType(), ex.Message);
                    }
                    finally
                    {
                        // wait for client to shutdown
                        pipeClient.WaitForExit();
                        // free resources
                        pipeClient.Close();
                    }
                }

                if (crawlResult != null)
                {
                    _loggerService.LogCrawlResult(crawlResult);
                }
            }
        }

        public void Pause()
        {
            throw new NotImplementedException();
        } 

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
