namespace ScraperFramework.Pipeline
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal abstract class PipeLine<T>
    {
        protected Pipe<T> _rootPipe;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pipe"></param>
        public PipeLine<T> Connect(Pipe<T> pipe)
        {
            if (_rootPipe == null)
            {
                _rootPipe = pipe;
            }
            else
            {
                _rootPipe.Connect(pipe);
            }

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract T Drain();
    }
}
