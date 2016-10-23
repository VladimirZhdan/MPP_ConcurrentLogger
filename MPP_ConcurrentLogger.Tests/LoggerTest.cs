using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPP_ConcurrentLogger;
using System.Threading;
using System.IO;

namespace MPP_ConcurrentLogger.Tests
{
    [TestClass]
    public class LoggerTest
    {
        private string targetFileName;

        [TestInitialize]
        public void Initialize()
        {
            targetFileName = "temp.txt";            
            ReCreateFile(targetFileName);
        }
         

        [TestMethod]
        public void CheckFlushOrderByThreads()
        {                        
            ILoggerTarget[] loggerTargets = new ILoggerTarget[1];
            loggerTargets[0] = new FileTarget(targetFileName);
            Logger logger = new Logger(2, loggerTargets);
            CreateAndStartLogingThreads(50, 2, logger);                        
            
            bool result = true;
            using (StreamReader reader = new StreamReader(targetFileName))
            {                
                DateTime prevDate = new DateTime();                                
                DateTime currentDate;
                while(!reader.EndOfStream && result)
                {
                    string currentLogLine = reader.ReadLine();
                    currentDate = Convert.ToDateTime(currentLogLine.Substring(1, currentLogLine.LastIndexOf(']') - 1));
                    if(currentDate < prevDate)
                    {
                        result = false;
                    }
                    prevDate = currentDate;
                }
            }

            bool expectedResult = true;

            Assert.AreEqual(expectedResult, result);
        }

        private void ReCreateFile(string fileName)
        {
            FileStream fStream = File.Create(fileName);
            fStream.Close();
        }

        private void CreateAndStartLogingThreads(int countThreads, int countMessage, ILogger logger)
        {
            LogThreadPool logThreadPool = new LogThreadPool(countThreads, countMessage, LogLevel.Info, logger);
            logThreadPool.StartThreads();
            while (logThreadPool.IsThreadsRunning) ;    
        }        
        
    }
}
