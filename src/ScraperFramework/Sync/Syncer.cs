using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ScraperFramework.Sync
{
    class Syncer : ISyncer
    {
        private readonly SortedSet<ISyncTask> _syncTasks = new SortedSet<ISyncTask>();
        private readonly System.Timers.Timer _timer = new System.Timers.Timer();
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public Syncer() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="syncInterval"></param>
        /// <param name="syncAutoReset"></param>
        /// <param name="autoStart"></param>
        public Syncer(int syncInterval, bool syncAutoReset = true, bool autoStart = false)
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

        public void AddSyncTask(ISyncTask syncTask)
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

        public void StartSyncTimer()
        {
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

                var batches = _syncTasks.GroupBy(st => st.Order);

                foreach (var batch in batches)
                {
                    var tasks = batch.Select(async syncTask =>
                    {
                        await syncTask.Execute();
                    });

                    await Task.WhenAll(tasks);
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
