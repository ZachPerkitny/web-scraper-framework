using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Serilog;

namespace ScraperFramework.Sync
{
    class Syncer : ISyncer
    {
        private readonly List<ISyncTask> _syncTasks = new List<ISyncTask>();
        private readonly System.Timers.Timer _timer = new System.Timers.Timer();
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public Syncer() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="syncInterval"></param>
        /// <param name="syncAutoReset"></param>
        /// <param name="autoStart"></param>
        public Syncer(double syncInterval, bool syncAutoReset = true, bool autoStart = false)
        {
            _timer.Interval = syncInterval;
            _timer.AutoReset = syncAutoReset;
            _timer.Elapsed += OnTimerCallback;
            if (autoStart)
            {
                _timer.Enabled = true;
            }
        }

        public bool SyncTimerAutoReset
        {
            get { return _timer.AutoReset; }
            set { _timer.AutoReset = value; }
        }

        public bool SyncTimerEnabled
        {
            get { return _timer.Enabled; }
            set { _timer.Enabled = value; }
        }

        public double SyncInterval
        {
            get { return _timer.Interval; }
            set { _timer.Interval = value; }
        }

        public IEnumerable<ISyncTask> SyncTasks
        {
            get { return _syncTasks; }
        }

        public ISyncer AddSyncTask(ISyncTask syncTask)
        {
            try
            {
                _semaphore.Wait();
                _syncTasks.Add(syncTask);
            }
            finally
            {
                _semaphore.Release();
            }

            return this;
        }

        public void RemoveSyncTask(ISyncTask syncTask)
        {
            try
            {
                _semaphore.Wait();
                _syncTasks.Remove(syncTask);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public void StartSyncTimer(bool immediate)
        {
            if (immediate)
            {
                // immediate and blocking execution
                // of sync tasks
                ExecuteSyncTasks()
                    .GetAwaiter()
                    .GetResult();
            }

            _timer.Start();
        }

        public void StopSyncTimer()
        {
            _timer.Stop();
        }

        public async Task Execute()
        {
            await ExecuteSyncTasks();
        }

        /// <summary>
        /// 
        /// </summary>
        private async void OnTimerCallback(object source, ElapsedEventArgs e)
        {
            await ExecuteSyncTasks();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteSyncTasks()
        {
            try
            {
                await _semaphore.WaitAsync();

                Log.Information("Starting {0} Sync Task(s)", _syncTasks.Count);

                var batches = _syncTasks
                    .GroupBy(st => st.Order)
                    .OrderBy(g => g.Key);

                foreach (var batch in batches)
                {
                    var tasks = batch.Select(async syncTask =>
                    {
                        await syncTask.Execute();
                    });

                    await Task.WhenAll(tasks);
                }

                Log.Information("Completed All Sync Tasks Successfully");
            }
            catch (Exception ex)
            {
                Log.Error("Sync Task Exception ({0}): {1}", ex.GetType(), ex.Message);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
