using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MPP_ConcurrentLogger
{
    public class Logger : ILogger
    {        
        private object lockObj = new object();

        int bufferLimit;
        int currentLogsCount = 0;
        private ILoggerTarget[] targets;
        private LogInfo[] logsInfo;
        private FlushLogsThreadPool flushLogsThreadPool;

        public Logger(int bufferLimit, ILoggerTarget[] targets)
        {
            this.bufferLimit = bufferLimit;
            this.targets = targets;
            logsInfo = new LogInfo[bufferLimit];
            flushLogsThreadPool = new FlushLogsThreadPool(bufferLimit, targets);
        }

        public void Log(LogLevel level, string message)
        {
            Monitor.Enter(lockObj);
            try
            {
                AddLog(new LogInfo(level, message));
                if (currentLogsCount == bufferLimit)
                {
                    FlushLogsAndResetLogCounter(logsInfo);
                }                                         
            }  
            finally
            {
                Monitor.Exit(lockObj);
            }          
        }

        private void AddLog(LogInfo logInfo)
        {
            logsInfo[currentLogsCount] = logInfo;
            currentLogsCount++;
        }        

        private void FlushLogsAndResetLogCounter(LogInfo[] logsInfo)
        {
            FlushingThreadData flushingThreadData = new FlushingThreadData(logsInfo);
            flushLogsThreadPool.AddThreadIdToPool(flushingThreadData.ThreadId);
            ThreadPool.QueueUserWorkItem(FlushLogsToTargets, flushingThreadData);
            ResetLogsCounter();
        }

        private void FlushLogsToTargets(object flushingThreadData)
        {
            flushLogsThreadPool.Flush((FlushingThreadData)flushingThreadData);
        }       

        private LogInfo[] CopyLogInfo(LogInfo[] source, int countToCopy)
        {
            LogInfo[] result = new LogInfo[countToCopy];
            for(int i = 0; i < countToCopy; i++)
            {
                result[i] = source[i];
            }
            return result;
        } 

        private void ResetLogsCounter()
        {
            logsInfo = new LogInfo[bufferLimit];
            currentLogsCount = 0;
        }
    }
}
