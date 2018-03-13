using System;
using Unity;

namespace ScraperFramework.Configuration
{
    public interface IScraperBuilder
    {
        IUnityContainer Container { get; }

        ICoordinator Build();
    }
}
