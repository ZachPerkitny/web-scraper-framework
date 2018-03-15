using System;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Serilog;
using WebScraper.Pocos;

namespace ScraperFramework
{
    class Scraper : IScraper
    {
        public Guid ScraperID { get; private set; }

        private readonly IScraperQueue _scraperQueue;
        private readonly CancellationToken _cancellationToken;
        private readonly Mutex _mutex;

        public Scraper(IScraperQueue scraperQueue, CancellationToken cancellationToken)
        {
            _scraperQueue = scraperQueue ?? throw new ArgumentNullException(nameof(scraperQueue));
            ScraperID = Guid.NewGuid();
            _cancellationToken = cancellationToken;
            _mutex = new Mutex(false, "testmapmutex");
        }

        public async Task Start()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                CrawlDescription crawlDescription = await _scraperQueue.Dequeue();
                byte[] serializedCrawlDescription = Encoding.UTF8.GetBytes(
                    JsonConvert.SerializeObject(crawlDescription));

                // Non-persisted files are memory-mapped files 
                // that are not associated with a file on a disk.
                // When the last process has finished working with
                // the file, the data is lost.
                using (MemoryMappedFile mmf = MemoryMappedFile.CreateNew("testmap", 1024))
                {
                    _mutex.WaitOne();

                    Process process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            //Arguments = $"--mapName={ScraperID} --mutex={mutexName}", // for mutex, and mmf
                            FileName = "WebScraper.exe",
                            CreateNoWindow = true,
                            UseShellExecute = false,
                            WindowStyle = ProcessWindowStyle.Hidden,
                            WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory
                        }
                    };

                    process.Start();

                    using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                    {
                        BinaryWriter binaryWriter = new BinaryWriter(stream);
                        binaryWriter.Write(serializedCrawlDescription);
                    }

                    _mutex.ReleaseMutex();

                    Thread.Sleep(100); // temp race condition fix

                    // Wait for scraper to finish
                    // TODO(zvp): Add Timeout ?
                    _mutex.WaitOne(30000);

                    CrawlResult crawlResult = null;

                    using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                    {
                        BinaryReader binaryReader = new BinaryReader(stream);
                        string value = Encoding.UTF8.GetString(binaryReader.ReadBytes((int) stream.Length));
                        crawlResult = JsonConvert.DeserializeObject<CrawlResult>(value);
                        if (crawlResult.Ads != null)
                        {
                            Log.Information("Finished Crawling {0}", crawlDescription.Keyword);
                            foreach (var ad in crawlResult.Ads)
                            {
                                Log.Information("{0} - {1}", ad.Title, ad.Url);
                            }
                        }
                    }

                    _mutex.ReleaseMutex();

                    //wait for scraper to shut down
                    // TODO(zvp): Add Timeout ?
                    process.WaitForExit(30000);
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
