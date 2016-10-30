using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace MPP_ConcurrentLogger
{
    class FlushLogsThreadPool
    {
        private object lockObj = new object();
        private Queue<int> threadsQueue;
        private Task[] flushingTasks;
        private ILoggerTarget[] targets;        

        public bool IsFlushingComplete
        {
            get
            {
                return (threadsQueue.Count == 0);
            }
        }

        public FlushLogsThreadPool(int flushingLogsCount, ILoggerTarget[] targets)
        {
            this.targets = targets;
            flushingTasks = new Task[targets.Length];
            threadsQueue = new Queue<int>();            
        }        

        public void Flush(FlushingThreadData flushingThreadData)
        {
            lock(lockObj)
            {                
                while(threadsQueue.Peek() != flushingThreadData.ThreadId)
                {
                    Monitor.Wait(lockObj);
                }
                for(int i = 0; i < targets.Length; i++)
                {                   
                    flushingTasks[i] = FlushAsync(targets[i], flushingThreadData.LogsInfo);
                }
                WaitFlushingTasksIsComplete();
                threadsQueue.Dequeue();
                Monitor.PulseAll(lockObj);
            }
        }

        private async Task FlushAsync(ILoggerTarget loggerTarget, LogInfo[] logsInfo)
        {
            bool result = await loggerTarget.FlushAsync(logsInfo);            
        }

        private void WaitFlushingTasksIsComplete()
        {            
            foreach(Task task in flushingTasks)
            {
                if((task != null) && !(task.IsCompleted))
                {
                    task.Wait();
                }
            }
        }

        public void AddThreadIdToPool(int threadId)
        {
            threadsQueue.Enqueue(threadId);
        }
    }
}
