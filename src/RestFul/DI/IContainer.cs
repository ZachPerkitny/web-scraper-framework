using System;

namespace RestFul.DI
{
    public interface IContainer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="creator"></param>
        /// <returns></returns>
        IContainer Register<TService>(Func<IContainer, TService> creator);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TConcrete"></typeparam>
        /// <returns></returns>
        IContainer Register<TService, TConcrete>()
            where TService : class
            where TConcrete : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        TService Resolve<TService>() where TService : class;
    }

}
