using System;
using System.IO;
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
            string fileName = "data.txt";
            ReCreateFile(fileName);
            ILoggerTarget[] loggerTarget = new ILoggerTarget[1];
            loggerTarget[0] = new FileTarget(fileName);
            Logger logger = new Logger(2, loggerTarget);
            RunAndWaitThreads(50, 2, logger);
            Console.WriteLine("Press <Enter> to exit");         
            Console.ReadLine();
        }

        private static void RunAndWaitThreads(int countThreads, int countMessage, ILogger logger)
        {
            LogThreadPool logThreadPool = new LogThreadPool(countThreads, countMessage, LogLevel.Info, logger);            
            logThreadPool.StartThreads();
            while (logThreadPool.IsThreadsRunning) ;
        }        

        private static void ReCreateFile(string fileName)
        {
            FileStream fStream = File.Create(fileName);
            fStream.Close();
        }

    }
}
