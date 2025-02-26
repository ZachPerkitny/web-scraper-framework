﻿using System;
using System.Reflection;
using System.Text.RegularExpressions;
using RestFul.DI;
using RestFul.Enum;
using RestFul.Extensions;
using RestFul.Http;
using RestFul.Result;

namespace RestFul.Routing
{
    class Route : IRoute
    {
        public HttpMethod HttpMethod { get; private set; }

        public string Path { get; private set; }

        public Regex PathPattern { get; private set; }

        public Func<HttpContext, IResult> Method { get; private set; }

        private readonly IContainer _container;

        public Route(MethodInfo method, HttpMethod httpMethod, string path, IContainer container)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            Method = CreateRouteFunc(method);
            Path = path ?? string.Empty;
            PathPattern = CreatePathPattern(Path);
            HttpMethod = httpMethod;
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public IResult Execute(HttpContext httpContext)
        {
            return Method.Invoke(httpContext);
        }

        public bool IsMatch(HttpContext httpContext)
        {
            if (httpContext.Request.HttpMethod == HttpMethod && PathPattern.IsMatch(httpContext.Request.Path))
            {
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return $"{HttpMethod} {Path}";
        }

        public override bool Equals(object obj)
        {
            Route route = obj as Route;
            if (route == null)
            {
                return false;
            }
            else
            {
                return HttpMethod.Equals(route.HttpMethod) &&
                    Path.Equals(route.Path);
            }
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Creates a delegate that takes an HttpContext object as a parameter
        /// and returns an IResult object. If the method is part of a non-static,
        /// class it will make use of the IContainer object to resolve its dependencies.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        private Func<HttpContext, IResult> CreateRouteFunc(MethodInfo method)
        {
            method.IsValidRoute(true);
            
            if (method.IsStatic || method.ReflectedType == null)
            {
                return (context) => (IResult)method.Invoke(null, new object[] { context });
            }

            return (context) =>
            {
                // anti-pattern, i know...
                object instance = _container.Resolve(method.ReflectedType);
                return (IResult)method.Invoke(instance, new object[] { context });
            };
        }

        /// <summary>
        /// Creates a Regular Expression that is used in the Match method.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private Regex CreatePathPattern(string path)
        {
            if (!path.StartsWith("^"))
            {
                path = $"^{path}";
            }

            if (!path.EndsWith("$"))
            {
                path += "$";
            }

            return new Regex(path);
        }
    }
}
