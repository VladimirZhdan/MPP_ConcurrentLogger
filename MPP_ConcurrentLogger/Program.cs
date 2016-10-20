using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MPP_ConcurrentLogger
{
    class Program
    {        
        static void Main(string[] args)
        {
            ILoggerTarget[] loggerTarget = new ILoggerTarget[1];
            loggerTarget[0] = new FileTarget("data.txt");
            Logger logger = new Logger(2, loggerTarget);
            CreateAndStartThreads(50, 2, logger);            
            Console.ReadLine();
        }

        private static void CreateAndStartThreads(int countThreads, int countMessage, ILogger logger)
        {
            LogThreadPool logThreadPool = new LogThreadPool(countMessage, LogLevel.Info, logger);
            Thread[] thread = new Thread[countThreads];
            for(int i = 0; i < thread.Length; i++)
            {
                thread[i] = new Thread(logThreadPool.FuncLog);
            } 
            for(int i = 0; i < thread.Length; i++)
            {
                thread[i].Start();
            }
        }        
    }
}
