namespace ScraperFramework.Pipeline
{
    internal abstract class Sink<T>
    {
        private Pipe<T> _connection;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pipe"></param>
        public void Connect(Pipe<T> pipe)
        {
            _connection = pipe;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract T Drain();
    }
}
