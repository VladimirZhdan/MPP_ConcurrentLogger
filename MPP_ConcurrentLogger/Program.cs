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
            ILoggerTarget[] loggerTarger = new ILoggerTarget[1];
            loggerTarger[0] = new FileTarget("data.txt");
            Logger logger = new Logger(2, loggerTarger);
            CreateAndStartThreads(50, 2, logger);            
            Console.ReadLine();
        }

        private static void CreateAndStartThreads(int countThreads, int countMessage, ILogger logger)
        {
            LogThread logThread = new LogThread(countMessage, LogLevel.Info, logger);
            Thread[] thread = new Thread[countThreads];
            for(int i = 0; i < thread.Length; i++)
            {
                thread[i] = new Thread(logThread.FuncLog);
            } 
            for(int i = 0; i < thread.Length; i++)
            {
                thread[i].Start();
            }
        }        
    }
}
