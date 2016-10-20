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
            LogThreadPool logThreadPool = new LogThreadPool(50, LogLevel.Info, logger);
            logThreadPool.FuncLog();
            Thread.Sleep(1000);
            
            bool result = true;
            using (StreamReader reader = new StreamReader(targetFileName))
            {
                int indexMessage = 0;
                while(!reader.EndOfStream && result)
                {
                    string actualStartTaskLine = reader.ReadLine();
                    string expectedContainedStartTaskStr = logThreadPool.GetStartTaskMessage(indexMessage);
                    result = (result && actualStartTaskLine.Contains(expectedContainedStartTaskStr));
                    string actualEndTaskLine = reader.ReadLine();
                    string expectedContainedEndTaskStr = logThreadPool.GetEndTaskMessage(indexMessage);
                    result = (result && actualEndTaskLine.Contains(expectedContainedEndTaskStr));
                    indexMessage++;
                }
            }

            bool expectedResult = true;

            Assert.AreEqual(expectedResult, result);
        }

        private void ReCreateFile(string fileName)
        {

        }
    }
}
