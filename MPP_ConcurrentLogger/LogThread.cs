using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MPP_ConcurrentLogger
{
    public class LogThread
    {
        private int countMessage;
        private LogLevel level;
        private ILogger logger;

        public LogThread(int countMessage, LogLevel level, ILogger logger)
        {
            this.countMessage = countMessage;
            this.level = level;
            this.logger = logger;
        }

        public void FuncLog()
        {
            for (int i = 0; i < countMessage; i++)
            {
                string message = "task" + i + " thread №" + Thread.CurrentThread.ManagedThreadId;
                logger.Log(level, message);
                Thread.Sleep(500);
            }
        }
    }
}
