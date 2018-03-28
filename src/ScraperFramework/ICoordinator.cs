using System;

namespace ScraperFramework
{
    public interface ICoordinator
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
