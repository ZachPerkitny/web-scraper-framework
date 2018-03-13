using System;
using System.Net;
using System.Threading.Tasks;

namespace RestFul.Http
{
    public interface IHttpListener
    {
        /// <summary>
        /// 
        /// </summary>
        bool IsListening { get; }

        /// <summary>
        /// 
        /// </summary>
        HttpListenerPrefixCollection Prefixes { get; }

        /// <summary>
        /// 
        /// </summary>
        void Abort();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="obj"></param>
        void BeginGetContext(AsyncCallback callback, object obj);

        /// <summary>
        /// 
        /// </summary>
        void Close();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asyncResult"></param>
        HttpContext EndGetContext(IAsyncResult asyncResult);

        /// <summary>
        /// 
        /// </summary>
        HttpContext GetContext();

        /// <summary>
        /// 
        /// </summary>
        Task<HttpContext> GetContextAsync();

        /// <summary>
        /// 
        /// </summary>
        void Start();

        /// <summary>
        /// 
        /// </summary>
        void Stop();
    }
}
