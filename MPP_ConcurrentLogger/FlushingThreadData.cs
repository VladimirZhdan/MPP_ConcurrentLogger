namespace MPP_ConcurrentLogger
{
    public class FlushingThreadData
    {
        private static int freeThreadId = 0;

        private int threadId;
        private LogInfo[] logsInfo;

        public FlushingThreadData(LogInfo[] logsInfo)
        {
            this.logsInfo = logsInfo;
            threadId = FreeThreadId;
        }
        
        private int FreeThreadId
        {
            get
            {
                int result = freeThreadId;
                freeThreadId++;
                return result;
            }
        }        

        public LogInfo[] LogsInfo
        {
            get
            {
                return logsInfo;
            }
        }

        public int ThreadId
        {
            get
            {
                return threadId;
            }
        }        
    }
}
