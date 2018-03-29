namespace ScraperFramework.Pipeline
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal abstract class Pipe<T>
    {
        protected Pipe<T> _connection;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pipe"></param>
        public void Connect(Pipe<T> pipe)
        {
            if (_connection == null)
            {
                _connection = pipe;
            }
            else
            {
                _connection.Connect(pipe);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract T Flow(T entity);
    }
}
