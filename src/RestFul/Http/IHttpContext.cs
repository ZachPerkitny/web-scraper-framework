namespace RestFul.Http
{
    public interface IHttpContext
    {
        /// <summary>
        /// Gets an object that represents the client's request
        /// </summary>
        IHttpRequest Request { get; }

        /// <summary>
        /// Gets an object that will be sent back to the client
        /// </summary>
        IHttpResponse Response { get; }
    }
}
