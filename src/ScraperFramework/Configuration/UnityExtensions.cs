using System;
using System.Linq;
using System.Reflection;
using MediatR;
using Unity;
using Unity.Lifetime;

namespace ScraperFramework.Configuration
{
    static class UnityExtensions
    {
        public static IUnityContainer RegisterMediator(this IUnityContainer container, LifetimeManager lifetimeManager)
        {
            container
                .RegisterType<IMediator, Mediator>(lifetimeManager)
                .RegisterInstance<SingleInstanceFactory>(t => container.IsRegistered(t) ? container.Resolve(t) : null)
                .RegisterInstance<MultiInstanceFactory>(t => container.ResolveAll(t));

            return container;
        }

        public static IUnityContainer RegisterMediatorHandlers(this IUnityContainer container, Assembly assembly)
        {
            container
                .RegisterTypesImplementingType(assembly, typeof(IRequestHandler<>))
                .RegisterTypesImplementingType(assembly, typeof(IRequestHandler<,>));

            return container;
        }

        public static IUnityContainer RegisterTypesImplementingType(this IUnityContainer container, Assembly assembly, Type type)
        {
            Type[] implementations = assembly.GetTypes().Where(
                t => t.GetInterfaces().Any(i => IsSubClassOf(type, i))).ToArray();

            foreach (Type implementation in implementations)
            {
                Type[] interfaces = implementation.GetInterfaces();
                foreach (var @interface in interfaces)
                {
                    container.RegisterType(@interface, implementation);
                }
            }

            return container;
        }

        private static bool IsSubClassOf(Type parent, Type child)
        {
            while (child != null && child != typeof(object))
            {
                Type type = child.IsGenericType ? child.GetGenericTypeDefinition() : child;
                if (type == parent)
                {
                    return true;
                }

                child = child.BaseType;
            }

            return false;
        }
    }
}
