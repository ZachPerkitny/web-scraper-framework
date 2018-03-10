using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using ScraperFramework.Attributes;
using ScraperFramework.Exceptions;

namespace ScraperFramework
{
    abstract class HttpRequestHandler : IHttpRequestHandler
    {
        public HttpRequestHandler() { }

        public async Task<object> Execute(HttpListenerRequest listenerRequest)
        {
            string template = listenerRequest.Url.Segments[1].Replace("/", "");

            MethodInfo method = GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(m => (m.ReturnType == typeof(Task) || (m.IsGenericMethod && m.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))) 
                && m.GetCustomAttributes().Any(attr =>
            {
                return attr is HttpRequestAttribute &&
                    ((HttpRequestAttribute)attr).HttpMethod == listenerRequest.HttpMethod &&
                    ((HttpRequestAttribute)attr).Template == template;
            }))
            .FirstOrDefault();

            if (method != null)
            {
                string[] strParameters = listenerRequest.Url.Segments
                    .Skip(2).Select(p => p.Replace("/", "")).ToArray();

                if (strParameters.Length != method.GetParameters().Length)
                {
                    throw new BadRequest("Expected {0} Parameters, Got {1}.",
                        method.GetParameters().Length, strParameters.Length);
                }

                object[] parameters = method.GetParameters()
                    .Select((p, i) => Convert.ChangeType(strParameters[i], p.ParameterType))
                    .ToArray();

                string body = string.Empty;
                if (listenerRequest.HasEntityBody)
                {
                    using (StreamReader reader = new StreamReader(
                        listenerRequest.InputStream, listenerRequest.ContentEncoding))
                    { 
                        body = await reader.ReadToEndAsync();
                    }
                }

                try
                {
                    Task task = (Task)method.Invoke(this, parameters);
                    await task;
                    return (object)((dynamic)task).Result;
                }
                catch (Exception)
                {
                    throw new InternalServerError();
                }
            }
            else
            {
                throw new NotFound();
            }
        }
    }
}
