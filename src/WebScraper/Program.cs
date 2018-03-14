using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;

namespace WebScraper
{
    class Program
    {
        // temp, just testing
        private const string _mapPrefix = "--mapName=";
        private const string _mutexPrefix = "--mutex=";

        static void Main(string[] args)
        {
            string mapName = null;
            string mutexName = null;
            // TODO (zvp) : Clean this shit up
            foreach (string arg in args)
            {
                if (arg.StartsWith(_mapPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    mapName = arg.Split('=')[0];
                }

                if (arg.StartsWith(_mutexPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    mutexName = arg.Split('=')[0];
                }
            }

            if (string.IsNullOrEmpty(mapName))
            {
                Console.WriteLine("Missing or Invalid Map Name");
                return;
            }

            if (string.IsNullOrEmpty(mutexName))
            {
                Console.WriteLine("Missing or Invalid Map Name");
                return;
            }
            Console.WriteLine(mapName, mutexName);
            using (MemoryMappedFile mmf = MemoryMappedFile.OpenExisting(mapName))
            {
                Mutex mutex = Mutex.OpenExisting(mapName);
                mutex.WaitOne();

                using (MemoryMappedViewStream stream = mmf.CreateViewStream())
                {
                    BinaryReader binaryReader = new BinaryReader(stream);
                    byte[] buffer = binaryReader.ReadBytes(int.MaxValue);
                }

                Thread.Sleep(500);
                mutex.ReleaseMutex();
            }
        }
    }
}
