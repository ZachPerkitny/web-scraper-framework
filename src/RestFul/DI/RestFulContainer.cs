using System;
using System.Collections.Generic;

namespace RestFul.DI
{
    class RestFulContainer : IContainer
    {
        private readonly Dictionary<Type, Delegate> _creators;
        private readonly object _locker = new object();

        public RestFulContainer()
        {
            _creators = new Dictionary<Type, Delegate>();
        }

        public IContainer Register<TService>(Func<IContainer, TService> creator)
        {
            lock (_locker)
            {
                if (_creators.ContainsKey(typeof(TService)))
                {
                    return this;
                }

                _creators.Add(typeof(TService), creator);
                return this;
            }
        }

        public IContainer Register<TService, TConcrete>()
            where TService : class
            where TConcrete : class
        {
            throw new NotImplementedException();
        }

        public TService Resolve<TService>() where TService : class
        {
            throw new NotImplementedException();
        }
    }
}
