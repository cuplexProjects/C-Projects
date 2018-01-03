using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageView.Utility
{
    public sealed class SimpleScheduler : TaskScheduler, IDisposable
    {
        private readonly BlockingCollection<Task> _tasks = new BlockingCollection<Task>();
        private Thread _main = null;

        public SimpleScheduler()
        {
            _main = new Thread(new ThreadStart(this.Main));
        }

        private void Main()
        {
            Console.WriteLine("Starting Thread " + Thread.CurrentThread.ManagedThreadId.ToString());

            foreach (var t in _tasks.GetConsumingEnumerable())
            {
                TryExecuteTask(t);
            }
        }

        /// <summary>
        /// Used by the Debugger
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return _tasks.ToArray<Task>();
        }


        protected override void QueueTask(Task task)
        {
            _tasks.Add(task);

            if (!_main.IsAlive) { _main.Start(); }//Start thread if not done so already
        }


        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return false;
        }


        #region IDisposable Members

        public void Dispose()
        {
            _tasks.CompleteAdding(); //Drops you out of the thread
        }

        #endregion
    }
}
