using System.Threading.Tasks;

namespace ScraperFramework.Utils
{
    // based on Steven Toub's Article on msdn blog
    // https://blogs.msdn.microsoft.com/pfxteam/2012/02/11/building-async-coordination-primitives-part-1-asyncmanualresetevent/
    public class AsyncManualResetEvent
    {
        private TaskCompletionSource<bool> _taskCompletionSource;
        private readonly object _locker = new object();

        /// <summary>
        /// 
        /// </summary>
        public AsyncManualResetEvent()
            : this (false) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="set"></param>
        public AsyncManualResetEvent(bool set)
        {
            _taskCompletionSource = new TaskCompletionSource<bool>();
            if (set)
            {
                _taskCompletionSource.TrySetResult(true);
            }
        }

        /// <summary>
        /// Returns the TaskCompletionSource's Task
        /// </summary>
        /// <returns></returns>
        public Task WaitAsync()
        {
            lock (_locker)
            {
                return _taskCompletionSource.Task;
            }
        }

        /// <summary>
        /// Sets the TaskCompletionSource result
        /// </summary>
        public void Set()
        {
            lock (_locker)
            {
                _taskCompletionSource.TrySetResult(true);
            } 
        }

        /// <summary>
        /// Resets the Task Completion Source
        /// </summary>
        public void Reset()
        {
            lock (_locker)
            {
                if (_taskCompletionSource.Task.IsCompleted)
                {
                    _taskCompletionSource = new TaskCompletionSource<bool>();
                }
            }
        }
    }
}
