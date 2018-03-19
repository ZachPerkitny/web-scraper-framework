using System;
using RestFul.Configuration;
using RestFul.DI;

namespace RestFul
{
    public static class RestfulServerFactory
    {
        private static Func<IContainer> _createContainer = () => new RestFulContainer();

        public static void UseDIContainer(Func<IContainer> createContainer)
        {
            _createContainer = createContainer ?? throw new ArgumentNullException(nameof(createContainer));
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
            IContainer container = _createContainer();

            register?.Invoke(container);
            container.Register((_) => settings);
            RestFulRegistration.RegisterComponents(container);

            return container.Resolve<IRestFulServer>();
        }
    }
}
