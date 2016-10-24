using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace MPP_ConcurrentLogger
{
    class FlushLogsThreadPool
    {
        private object lockObj = new object();
        private Queue<int> threadsQueue;
        private Task<bool>[] flushingTasks;
        private ILoggerTarget[] targets;

        public FlushLogsThreadPool(int flushingLogsCount, ILoggerTarget[] targets)
        {
            this.targets = targets;
            flushingTasks = new Task<bool>[targets.Length];
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
                    flushingTasks[i] = targets[i].FlushAsync(flushingThreadData.LogsInfo);                    
                }
                WaitFlushingTasksIsComplete();
                threadsQueue.Dequeue();
                Monitor.Pulse(lockObj);
            }
        }

        private void WaitFlushingTasksIsComplete()
        {            
            foreach(Task<bool> task in flushingTasks)
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
