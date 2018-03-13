using System;
using RestFul.Configuration;
using RestFul.DI;

namespace RestFul
{
    public static class RestfulServerFactory
    {
        private static IContainer _container = new RestFulContainer();

        public static void UseDIContainer(IContainer container)
        {
            _container = container;
        }

        public static IRestFulServer Create()
        {
            return Create((_) => { });
        }

        public static IRestFulServer Create(Action<IContainer> register)
        {
            return Create(new RestFulSettings(), register);
        }

        public static IRestFulServer Create(string host, int port, bool useHttps, Action<IContainer> register)
        {
            IRestFulSettings settings = new RestFulSettings
            {
                Host = host,
                Port = port,
                UseHTTPs = useHttps
            };

            return Create(settings, register);
        }

        public static IRestFulServer Create(IRestFulSettings settings, Action<IContainer> register)
        {
            register?.Invoke(_container);
            _container.Register((_) => settings);
            RestFulRegistration.RegisterComponents(_container);

            return _container.Resolve<IRestFulServer>();
        }
    }
}
