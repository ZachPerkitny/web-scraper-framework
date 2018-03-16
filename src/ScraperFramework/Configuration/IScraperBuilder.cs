using Unity;

namespace ScraperFramework.Configuration
{
    public interface IScraperBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        IUnityContainer Container { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ICoordinator Build();
    }
}
