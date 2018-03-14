using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;

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

                System.Console.WriteLine("Hey");

                using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    byte[] buffer = memoryStream.ToArray();;
                }

                Thread.Sleep(500);

                mutex.ReleaseMutex();
            }
        }
    }
}
