namespace ScraperFramework.Pipeline
{
    internal abstract class Pipe<T>
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
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract T Flow(T entity);
    }
}
