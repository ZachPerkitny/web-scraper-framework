using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading;
using WebScraper.Pocos;
using Newtonsoft.Json;

namespace WebScraper
{
    class Program
    {
        // temp, just testing
        static void Main(string[] args)
        {
            using (MemoryMappedFile mmf = MemoryMappedFile.OpenExisting("testmap"))
            {
                Mutex mutex = Mutex.OpenExisting("testmapmutex");
                mutex.WaitOne();

                CrawlDescription crawlDescription = null;

                using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                {
                    BinaryReader binaryReader = new BinaryReader(stream);
                    string value = Encoding.UTF8.GetString(binaryReader.ReadBytes((int) stream.Length));
                    crawlDescription = JsonConvert.DeserializeObject<CrawlDescription>(value);
                }

                // scrape using crawl desc
                CrawlResult crawlResult = null;
                using (Scraper scraper = new Scraper(crawlDescription))
                {
                    scraper.Initialize();
                    crawlResult = scraper.Scrape().GetAwaiter().GetResult();
                }

                byte[] serializedCrawlResult = Encoding.UTF8.GetBytes(
                    JsonConvert.SerializeObject(crawlResult));

                using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                {
                    BinaryWriter binaryWriter = new BinaryWriter(stream);
                    binaryWriter.Write(serializedCrawlResult);
                }

                mutex.ReleaseMutex();
            }
        }
    }
}
