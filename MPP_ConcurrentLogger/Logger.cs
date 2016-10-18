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

        public Logger(int bufferLimit, ILoggerTarget[] targets)
        {
            this.bufferLimit = bufferLimit;
            this.targets = targets;
            logsInfo = new LogInfo[bufferLimit];
        }

        public void Log(LogLevel level, string message)
        {
            Monitor.Enter(lockObj);
            try
            {
                if (currentLogsCount < bufferLimit)
                {
                    logsInfo[currentLogsCount] = new LogInfo(level, message);
                    currentLogsCount++;
                }
                else
                {
                    Flush();
                    ResetLogsCounter();
                }
            }  
            finally
            {
                Monitor.Exit(lockObj);
            }          
        }

        private void Flush()
        {
            LogInfo[] tempLogsInfo = CopyLogInfo(logsInfo, currentLogsCount);            
            for(int i = 0; i < targets.Length; i++)
            {
                targets[i].Flush(tempLogsInfo);
            }            
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
