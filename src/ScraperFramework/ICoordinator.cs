using System;

namespace ScraperFramework
{
    public interface ICoordinator : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        int ScraperCount { get; }

        /// <summary>
        /// 
        /// </summary>
        void Start();

        /// <summary>
        /// 
        /// </summary>
        void Pause();

        /// <summary>
        /// 
        /// </summary>
        void Resume();

        /// <summary>
        /// 
        /// </summary>
        void Stop();
    }
}
