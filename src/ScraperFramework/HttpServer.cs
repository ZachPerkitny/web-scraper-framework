using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Serilog;
using ScraperFramework.Exceptions;

namespace ScraperFramework
{
    class HttpServer : IHttpServer
    {
        private readonly HttpListener _httpListener;
        private readonly IHttpRequestHandler _requestHandler;
        private readonly int _concurrentRequests;
        private Task _listenerTask;
        private bool _disposed = false;

        public HttpServer(IHttpRequestHandler requestHandler)
        {
            //if (concurrentRequests <= 0)
            //{
            //    throw new ArgumentException("Expected Concurrenct Requests to be greater than 0.");
            //}

            _httpListener = new HttpListener();
            _requestHandler = requestHandler;
            _concurrentRequests = 4 * Environment.ProcessorCount;
        }

        public void Listen(string[] prefixes, CancellationToken cancelToken)
        {
            if (prefixes == null || prefixes.Length == 0)
            {
                throw new ArgumentException("URL Prefixes are Required");
            }

            foreach (string prefix in prefixes)
            {
                _httpListener.Prefixes.Add(prefix);
            }

            try
            {
                _httpListener.Start();
            }
            catch (HttpListenerException ex)
            {
                Log.Error("Error Starting Http Listener: {0}", ex.Message);
            }
            
            _listenerTask = Task.Factory.StartNew(async () =>
            {
                Log.Information("Command Listener Waiting For Requests");
                HashSet<Task<HttpListenerContext>> requests = new HashSet<Task<HttpListenerContext>>();
                for (int i = 0; i < _concurrentRequests; i++)
                {
                    requests.Add(_httpListener.GetContextAsync());
                }

                while (!cancelToken.IsCancellationRequested)
                {
                    Task<HttpListenerContext> request = await Task.WhenAny(requests);
                    requests.Remove(request);

                    HttpListenerContext ctx = request.Result;

                    string responseString = string.Empty;
                    try
                    {
                        object responseObj = await _requestHandler.Execute(ctx.Request);
                        responseString = JsonConvert.SerializeObject(responseObj);
                    }
                    catch (HttpServerException ex)
                    {
                        responseString = JsonConvert.SerializeObject(ex.ExceptionDesc);
                        ctx.Response.StatusCode = Convert.ToInt32(ex.ExceptionDesc.StatusCode);
                    }

                    byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                    Stream output = ctx.Response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();
                    ctx.Response.Close();

                    requests.Add(_httpListener.GetContextAsync());
                }
            }, TaskCreationOptions.LongRunning);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _httpListener.Close();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
