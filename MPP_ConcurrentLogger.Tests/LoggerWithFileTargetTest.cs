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

            while (!logger.IsFlushLogThreadPoolClear) ;

            TestMethod(targetFileName);
        }

        public static void TestMethod(string targetFileName)
        {
            bool result = true;
            using (StreamReader reader = new StreamReader(targetFileName))
            {
                DateTime prevLogTime = default(DateTime);
                int prevLogMillisecond = 0;
                DateTime currentLogTime;
                int currentLogMillisecond;
                while (!reader.EndOfStream && result)
                {
                    string currentLogLine = reader.ReadLine();
                    currentLogTime = Convert.ToDateTime(currentLogLine.Substring(1, currentLogLine.LastIndexOf(']') - 5));

                    currentLogMillisecond = int.Parse(currentLogLine.Substring(currentLogLine.LastIndexOf(']') - 3, 3));

                    if (!IsSecondValueNotLessThanFirst(prevLogTime, currentLogTime, prevLogMillisecond, currentLogMillisecond))
                    {
                        result = false;
                    }
                    prevLogTime = currentLogTime;
                    prevLogMillisecond = currentLogMillisecond;
                }
            }

            bool expectedResult = true;

            Assert.AreEqual(expectedResult, result);
        }

        private static bool IsSecondValueNotLessThanFirst(DateTime firstTime, DateTime secondTime, int firstMillisecond, int secondMillisecond)
        {
            if (secondTime > firstTime)
            {
                return true;
            }
            if (secondTime < firstTime)
            {
                return false;
            }
            if(secondMillisecond >= firstMillisecond)
            {
                return true;
            }
            else
            {
                return false;
            }                           
        }

        private void ReCreateFile(string fileName)
        {
            FileStream fStream = File.Create(fileName);
            fStream.Close();
        }            
    }
}
