using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScraperFramework.Utils
{
    // based on Steven Toub's Article on msdn blog
    // https://blogs.msdn.microsoft.com/pfxteam/2012/02/11/building-async-coordination-primitives-part-2-asyncautoresetevent/
    public class AsyncAutoResetEvent
    {
        private readonly Queue<TaskCompletionSource<bool>> _taskCompletionSources;
        private readonly Task<bool> _completed;
        private bool _set = false;

        private readonly object _locker = new object();

        /// <summary>
        /// 
        /// </summary>
        public AsyncAutoResetEvent()
            : this(false) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="set"></param>
        public AsyncAutoResetEvent(bool set)
        {
            _taskCompletionSources = new Queue<TaskCompletionSource<bool>>();
            _completed = Task.FromResult(true);
            _set = set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task WaitAsync()
        {
            lock (_locker)
            {
                if (_set)
                {
                    _set = false;
                    return _completed;
                }
                else
                {
                    var tcs = new TaskCompletionSource<bool>();
                    _taskCompletionSources.Enqueue(tcs);
                    return tcs.Task;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Set()
        {
            lock (_locker)
            {
                if (_taskCompletionSources.Count > 0)
                {
                    var tcs = _taskCompletionSources.Dequeue();
                    tcs.SetResult(true);
                }
                else if (!_set)
                {
                    _set = true;
                }
            }
        }
    }
}
