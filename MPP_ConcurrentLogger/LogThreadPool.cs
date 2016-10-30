using System.Threading;

namespace MPP_ConcurrentLogger
{
    public class LogThreadPool
    {
        private int countMessage;
        private LogLevel level;
        private ILogger logger;
        private Thread[] logThread;

        public bool IsThreadsRunning
        {
            get
            {
                return CheckThreadsIsRunning();
            }
        }

        public LogThreadPool(int countThreads, int countMessage, LogLevel level, ILogger logger)
        {
            logThread = new Thread[countThreads];
            initializeLogThreads();

            this.countMessage = countMessage;
            this.level = level;
            this.logger = logger;            
        }

        private void initializeLogThreads()
        {
            if(logThread != null)
            {
                for(int i = 0; i < logThread.Length; i++)
                {
                    logThread[i] = new Thread(FuncLog);
                }
            }
        }

        public void FuncLog()
        {
            for (int i = 0; i < countMessage; i++)
            {
                string startMessage = GetStartTaskMessage(i);
                logger.Log(level, startMessage);                
                string endMessage = GetEndTaskMessage(i);
                logger.Log(level, endMessage);
            }
        }

        private string GetStartTaskMessage(int indexMessage)
        {
            string result = ("task" + indexMessage + " thread №" + Thread.CurrentThread.ManagedThreadId + " start");
            return result;
        }

        private string GetEndTaskMessage(int indexMessage)
        {
            string result = ("task" + indexMessage + " thread №" + Thread.CurrentThread.ManagedThreadId + " end");
            return result;
        }

        public void StartThreads()
        {
            if(logThread != null)
            {
                for(int i = 0; i < logThread.Length; i++)
                {

                    logThread[i].Start();
                }
            }
        }

        private bool CheckThreadsIsRunning()
        {
            bool result = false;
            if (logThread != null)
            {
                for(int i = 0; i < logThread.Length; i++)
                {
                    if(logThread[i].ThreadState != ThreadState.Stopped)
                    {
                        result = true;
                    }
                }
            }            
            return result;
        }

        public static void RunAndWaitLogingThreads(int countThreads, int countMessage, ILogger logger)
        {
            LogThreadPool logThreadPool = new LogThreadPool(countThreads, countMessage, LogLevel.Info, logger);
            logThreadPool.StartThreads();
            Thread.Sleep(3000);
            while (logThreadPool.IsThreadsRunning) ;                        
        }

    }
}
