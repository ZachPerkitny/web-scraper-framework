using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScraperFramework.Sync
{
    public interface ISyncer
    {
        /// <summary>
        /// 
        /// </summary>
        IEnumerable<ISyncTask> SyncTasks { get; }

        /// <summary>
        /// 
        /// </summary>
        int SyncInterval { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="syncTask"></param>
        void AddSyncTask(ISyncTask syncTask);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="syncTask"></param>
        void RemoveSyncTask(ISyncTask syncTask);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task Execute();
    }
}
