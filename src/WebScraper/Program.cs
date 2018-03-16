using System;
using System.IO;
using System.IO.Pipes;
using Newtonsoft.Json;
using Serilog;
using WebScraper.Pocos;

namespace WebScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = SetupLogger();

            if (args.Length == 2)
            {
                using (AnonymousPipeClientStream pipeClientReader =
                    new AnonymousPipeClientStream(PipeDirection.In, args[0]))
                using (PipeStream pipeClientWriter =
                    new AnonymousPipeClientStream(PipeDirection.Out, args[1]))
                {
                    CrawlDescription crawlDescription;

                    // read crawl description from pipe
                    try
                    {
                        using (StreamReader sr = new StreamReader(pipeClientReader))
                        {
                            string message;

                            do
                            {
                                // TODO(zvp) : have to exit eventually.
                                message = sr.ReadLine();
                                Log.Debug("Pipe Received Message: {0}", message);
                            } while (message == null || !message.StartsWith("SYNC"));

                            message = sr.ReadLine(); 
                            crawlDescription = JsonConvert.DeserializeObject<CrawlDescription>(message);
                            Log.Debug("Pipe Received Crawl Description: {0}", message);
                        }

                        // process the message
                        CrawlResult crawlResult = null;
                        using (Scraper scraper = new Scraper(crawlDescription))
                        {
                            scraper.Initialize();
                            crawlResult = scraper.Scrape().GetAwaiter().GetResult();
                        }

                        using (StreamWriter sw = new StreamWriter(pipeClientWriter))
                        {
                            sw.AutoFlush = true;

                            // write Sync message and wait for drain
                            sw.WriteLine("SYNC");
                            pipeClientWriter.WaitForPipeDrain();

                            // write back the crawl result
                            string serializedCrawlResult = JsonConvert.SerializeObject(crawlResult);
                            sw.WriteLine(serializedCrawlResult);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("WebScraper Exception({0}): {1}", ex.GetType(), ex.Message);
                    }
                }
            }
            else
            {
                Log.Error("Expected 2 Arguments (PipeWriteHandle and PipeReadHandle).");
            }
        }

        private static ILogger SetupLogger()
        {
            var logConfig = new LoggerConfiguration();

            logConfig
                .MinimumLevel.Debug()
                .WriteTo.Console();

            return logConfig.CreateLogger();
        }
    }
}
