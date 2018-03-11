namespace RestFul
{
    public interface IRestFulServer
    {
        /// <summary>
        /// 
        /// </summary>
        bool IsListening { get; }

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
