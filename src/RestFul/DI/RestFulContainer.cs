using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RestFul.Exceptions;

namespace RestFul.DI
{
    class RestFulContainer : IContainer
    {
        private readonly Dictionary<Type, Delegate> _creators;
        private readonly Dictionary<Type, Type> _registrations;
        private readonly Dictionary<Type, object> _instances;
        private readonly object _locker = new object();

        public RestFulContainer()
        {
            _creators = new Dictionary<Type, Delegate>();
            _registrations = new Dictionary<Type, Type>();
            _instances = new Dictionary<Type, object>();
        }

        public IContainer Register<TService>(Func<IContainer, TService> creator)
        {
            lock (_locker)
            {
                Type serviceType = typeof(TService);
                if (_creators.ContainsKey(serviceType))
                {
                    return this;
                }

                _creators.Add(serviceType, creator);
                return this;
            }
        }

        public IContainer Register<TService, TConcrete>()
            where TService : class
            where TConcrete : class
        {
            lock (_locker)
            {
                Type serviceType = typeof(TService);
                Type concreteType = typeof(TConcrete);

                if (IsRegistered(serviceType))
                {
                    return this;
                }

                if (!concreteType.GetInterfaces().Contains(serviceType))
                {
                    throw new RestFulException("{0} is not an interface of {1}",
                        serviceType.Name, concreteType.Name);
                }

                if (concreteType.GetConstructors().Length > 1)
                {
                    throw new RestFulException("Expected {0} to have a single constructor",
                        concreteType.Name);
                }

                _registrations.Add(serviceType, concreteType);

                return this;
            }
        }

        public TService Resolve<TService>() where TService : class
        {
            lock (_locker)
            {
                Type serviceType = typeof(TService);

                if (_instances.ContainsKey(serviceType))
                {
                    return (TService)_instances[serviceType];
                }
                else if (_registrations.ContainsKey(serviceType))
                {
                    ConstructorInfo constructor = _registrations[serviceType].GetConstructors()[0];

                    object[] args = constructor.GetParameters()
                        .Select(p =>
                        {
                            return typeof(RestFulContainer)
                                .GetMethod("Resolve", new Type[0])
                                .MakeGenericMethod(p.ParameterType)
                                .Invoke(this, new object[0]);
                        })
                        .ToArray();

                    TService instance = (TService)constructor.Invoke(args);
                    _instances.Add(serviceType, instance);
                    return instance;
                }
                else if(_creators.ContainsKey(serviceType))
                {
                    TService instance = ((Func<IContainer, TService>)_creators[serviceType]).Invoke(this);
                    _instances.Add(serviceType, instance);
                    return instance;
                }
                else
                {
                    throw new RestFulException("Unable to Resolve Instance of Type {0}", serviceType.Name);
                }
            }
        }

        private bool IsRegistered(Type serviceType)
        {
            return _creators.ContainsKey(serviceType) || _registrations.ContainsKey(serviceType);
        }
    }
}
