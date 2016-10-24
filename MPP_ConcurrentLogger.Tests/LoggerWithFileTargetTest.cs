using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace MPP_ConcurrentLogger.Tests
{
    [TestClass]
    public class LoggerWithFileTargetTest
    {
        private string targetFileName;

        [TestInitialize]
        public void Initialize()
        {
            targetFileName = "loggerWithFileTargetTest.txt";            
            ReCreateFile(targetFileName);
        }        

        [TestMethod]
        public void CheckFlushOrderInFileTargetByThreads()
        {                        
            ILoggerTarget[] loggerTargets = new ILoggerTarget[1];
            loggerTargets[0] = new FileTarget(targetFileName);
            Logger logger = new Logger(2, loggerTargets);
            LogThreadPool.RunAndWaitLogingThreads(50, 2, logger);

            TestMethod(targetFileName);
        }

        public static void TestMethod(string targetFileName)
        {
            bool result = true;
            using (StreamReader reader = new StreamReader(targetFileName))
            {
                DateTime prevLogTime = default(DateTime);
                DateTime currentLogTime;
                while (!reader.EndOfStream && result)
                {
                    string currentLogLine = reader.ReadLine();
                    currentLogTime = Convert.ToDateTime(currentLogLine.Substring(1, currentLogLine.LastIndexOf(']') - 1));
                    if (currentLogTime < prevLogTime)
                    {
                        result = false;
                    }
                    prevLogTime = currentLogTime;
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
    }
}
