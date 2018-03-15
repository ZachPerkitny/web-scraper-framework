using System;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ScraperFramework.Pocos;

namespace ScraperFramework
{
    class Scraper : IScraper
    {
        public Guid ScraperID { get; private set; }

        private readonly IScraperQueue _queue;
        private readonly CancellationToken _cancellationToken;

        public Scraper(CancellationToken cancellationToken)
        {
            ScraperID = Guid.NewGuid();
            _cancellationToken = cancellationToken;
        }

        public async Task Start()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                CrawlDescription crawlDescription = await _queue.Dequeue();
                byte[] serializedCrawlDescription = Encoding.UTF8.GetBytes(
                    JsonConvert.SerializeObject(crawlDescription));

                // Non-persisted files are memory-mapped files 
                // that are not associated with a file on a disk.
                // When the last process has finished working with
                // the file, the data is lost.
                using (var mmf = MemoryMappedFile.CreateNew("testmap", 4096)) // TODO(zvp): Random Capacity 4kb, fix this
                {
                    string mutexName = "testmapmutex";
                    Mutex mutex = new Mutex(true, mutexName);

                    Process process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            Arguments = $"WebScraper.dll --mapName={ScraperID} --mutex={mutexName}", // for mutex, and mmf
                            FileName = "dotnet",
                            CreateNoWindow = true,
                            UseShellExecute = true,
                            // TODO (zvp): Don't Hardcode this
                            WorkingDirectory = @"C:\Users\zaperkitny\Projects\web-scraper-framework\src\WebScraper\bin\Release\WebScraper\"
                        }
                    };

                    process.Start();

                    using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                    {
                        BinaryWriter binaryWriter = new BinaryWriter(stream);
                        binaryWriter.Write(serializedCrawlDescription);
                    }

                    mutex.ReleaseMutex();

                    // Wait for scraper to finish
                    // TODO(zvp): Add Timeout ?
                    mutex.WaitOne();

                    using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        stream.CopyTo(memoryStream);
                        byte[] crawlResult = memoryStream.ToArray();
                    }

                    //wait for scraper to shut down
                    // TODO(zvp): Add Timeout ?
                    process.WaitForExit();
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
