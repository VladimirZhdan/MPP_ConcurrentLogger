using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MPP_ConcurrentLogger
{
    public class LogThreadPool
    {
        private int countMessage;
        private LogLevel level;
        private ILogger logger;

        public LogThreadPool(int countMessage, LogLevel level, ILogger logger)
        {
            this.countMessage = countMessage;
            this.level = level;
            this.logger = logger;
        }

        public void FuncLog()
        {
            for (int i = 0; i < countMessage; i++)
            {
                string startMessage = GetStartTaskMessage(i);
                logger.Log(level, startMessage);
                Thread.Sleep(1000);
                string endMessage = GetEndTaskMessage(i);
                logger.Log(level, endMessage);
            }
        }

        public string GetStartTaskMessage(int indexMessage)
        {
            string result = ("task" + indexMessage + " thread №" + Thread.CurrentThread.ManagedThreadId + " start");
            return result;
        }

        public string GetEndTaskMessage(int indexMessage)
        {
            string result = ("task" + indexMessage + " thread №" + Thread.CurrentThread.ManagedThreadId + " end");
            return result;
        }

    }
}
