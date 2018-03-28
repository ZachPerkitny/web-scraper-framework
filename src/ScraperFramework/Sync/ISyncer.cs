using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScraperFramework.Sync
{
    public interface ISyncer : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        bool SyncTimerAutoReset { get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool SyncTimerEnabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        double SyncInterval { get; set; }

        /// <summary>
        /// 
        /// </summary>
        IEnumerable<ISyncTask> SyncTasks { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="syncTask"></param>
        ISyncer AddSyncTask(ISyncTask syncTask);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="syncTask"></param>
        void RemoveSyncTask(ISyncTask syncTask);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="immediate"></param>
        void StartSyncTimer(bool immediate = false);

        /// <summary>
        /// 
        /// </summary>
        void StopSyncTimer();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task Execute();
    }
}
