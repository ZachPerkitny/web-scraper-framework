using System.Diagnostics;
using System.Timers;
using Serilog;

namespace ScraperFramework.Utils
{
    /// <summary>
    /// Utility class to log CPU Usage, Memory Usage and Disk Reads, Writes and Transfers
    /// </summary>
    internal class PerformanceLogger
    {
        private readonly PerformanceCounter _cpuPerformanceCounter = new PerformanceCounter();
        private readonly PerformanceCounter _memoryPerformanceCounter = new PerformanceCounter();
        private readonly PerformanceCounter _diskReadsPerformanceCounter = new PerformanceCounter();
        private readonly PerformanceCounter _diskWritesPerformanceCounter = new PerformanceCounter();
        private readonly PerformanceCounter _diskTransfersPerformanceCounter = new PerformanceCounter();

        private readonly Timer _timer = new Timer();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="autoStart"></param>
        public PerformanceLogger(double interval = 60000, bool autoStart = false)
        {
            _cpuPerformanceCounter.CategoryName = "Processor";
            _cpuPerformanceCounter.CounterName = "% Processor Time";
            _cpuPerformanceCounter.InstanceName = "_Total";

            _memoryPerformanceCounter.CategoryName = "Memory";
            _memoryPerformanceCounter.CounterName = "Available MBytes";
            
            _diskReadsPerformanceCounter.CategoryName = "PhysicalDisk";
            _diskReadsPerformanceCounter.CounterName = "Disk Reads/sec";
            _diskReadsPerformanceCounter.InstanceName = "_Total";
            
            _diskWritesPerformanceCounter.CategoryName = "PhysicalDisk";
            _diskWritesPerformanceCounter.CounterName = "Disk Writes/sec";
            _diskWritesPerformanceCounter.InstanceName = "_Total";
            
            _diskTransfersPerformanceCounter.CategoryName = "PhysicalDisk";
            _diskTransfersPerformanceCounter.CounterName = "Disk Transfers/sec";
            _diskTransfersPerformanceCounter.InstanceName = "_Total";

            _timer.Interval = interval;
            _timer.AutoReset = true;
            _timer.Elapsed += OnTimerCallback;
            if (autoStart)
            {
                _timer.Enabled = true;
            }
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void OnTimerCallback(object sender, ElapsedEventArgs e)
        {
            Log.Information("CPU Usage: {0}%", _cpuPerformanceCounter.NextValue());
            Log.Information("Memory Usage: {0}Mb", _memoryPerformanceCounter.NextValue());
            Log.Information("Disk Reads / Sec: {0}", _diskReadsPerformanceCounter.NextValue());
            Log.Information("Disk Writes / Sec: {0}", _diskWritesPerformanceCounter.NextValue());
            Log.Information("Disk Transfers / Sec: {0}", _diskTransfersPerformanceCounter.NextValue());
        }
    }
}
