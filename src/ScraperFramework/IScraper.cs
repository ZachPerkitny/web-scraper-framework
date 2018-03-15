using System.Threading.Tasks;

namespace ScraperFramework
{
    public interface IScraper
    {
        /// <summary>
        /// 
        /// </summary>
        Task Start();
        /// <summary>
        /// 
        /// </summary>
        void Pause();
        /// <summary>
        /// 
        /// </summary>
        void Stop();
    }
}
