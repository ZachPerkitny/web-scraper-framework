using System;

namespace ScraperFramework.Pipeline
{
    internal abstract class PipeLine<T>
    {
        private Pipe<T> _rootPipe;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pipe"></param>
        public void Connect(Pipe<T> pipe)
        {
            _rootPipe = pipe ?? throw new ArgumentNullException(nameof(pipe));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract T Drain();
    }
}
