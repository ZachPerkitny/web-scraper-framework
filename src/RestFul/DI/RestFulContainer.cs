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
        private readonly HashSet<Type> _cRegistrations;
        private readonly Dictionary<Type, Type> _iRegistrations;
        private readonly Dictionary<Type, object> _instances;
        private readonly object _locker = new object();

        public RestFulContainer()
        {
            _creators = new Dictionary<Type, Delegate>();
            _cRegistrations = new HashSet<Type>();
            _iRegistrations = new Dictionary<Type, Type>();
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

                _iRegistrations.Add(serviceType, concreteType);

                return this;
            }
        }

        public IContainer Register<T>() where T : class
        {
            lock (_locker)
            {
                Type type = typeof(T);

                if (IsRegistered(type))
                {
                    return this;
                }

                if (type.GetConstructors().Length > 1)
                {
                    throw new RestFulException("Expected {0} to have a single constructor",
                        type.Name);
                }

                _cRegistrations.Add(type);

                return this;
            }
        }

        public IContainer Register<T>(T instance) where T : class
        {
            lock (_locker)
            {
                Type type = typeof(T);
                if (_instances.ContainsKey(type))
                {
                    return this;
                }

                _instances.Add(type, instance);
                return this;
            }
        }

        public object Resolve(Type serviceType)
        {
            lock (_locker)
            {
                if (_instances.ContainsKey(serviceType))
                {
                    return _instances[serviceType];
                }
                else if (_iRegistrations.ContainsKey(serviceType))
                {
                    ConstructorInfo constructor = _iRegistrations[serviceType].GetConstructors()[0];

                    object[] args = ResolveParameters(constructor.GetParameters());

                    object instance = constructor.Invoke(args);
                    _instances.Add(serviceType, instance);
                    return instance;
                }
                else if (_cRegistrations.Contains(serviceType))
                {
                    ConstructorInfo constructor = serviceType.GetConstructors()[0];

                    object[] args = ResolveParameters(constructor.GetParameters());

                    object instance = constructor.Invoke(args);
                    _instances.Add(serviceType, instance);
                    return instance;
                }
                else if (_creators.ContainsKey(serviceType))
                {
                    object instance = ((Func<IContainer, object>)_creators[serviceType]).Invoke(this);
                    _instances.Add(serviceType, instance);
                    return instance;
                }
                else
                {
                    throw new RestFulException("Unable to Resolve Instance of Type {0}", serviceType.Name);
                }
            }
        }

        public TService Resolve<TService>() where TService : class
        {
            return (TService)Resolve(typeof(TService));
        }

        private object[] ResolveParameters(ParameterInfo[] parameters)
        {
            return parameters
                .Select(p =>
                {
                    return typeof(RestFulContainer)
                        .GetMethod("Resolve", new Type[] { typeof(Type) })
                        .Invoke(this, new object[] { p.ParameterType });
                })
                .ToArray();
        }

        private bool IsRegistered(Type serviceType)
        {
            return _creators.ContainsKey(serviceType) ||
                _iRegistrations.ContainsKey(serviceType) ||
                _cRegistrations.Contains(serviceType);
        }
    }
}
