namespace ScraperFramework.Pipeline
{
    internal abstract class PipeLine<T>
    {
        private Pipe<T> _rootPipe;

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
