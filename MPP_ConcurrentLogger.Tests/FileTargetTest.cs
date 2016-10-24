using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace MPP_ConcurrentLogger.Tests
{
    [TestClass]
    public class FileTargetTest
    {
        private FileTarget fileTarget;
        private string targetFileName;

        [TestInitialize]
        public void Initialize()
        {
            targetFileName = "testFileTarget.txt";
            ReCreateFile(targetFileName);
            fileTarget = new FileTarget(targetFileName);
        }


        [TestMethod]
        public void CheckRightFlushAllLogsInFileTarget()
        {
            LogInfo[] logsInfo = new LogInfo[10];
            for(int i = 0; i < logsInfo.Length; i++)
            {
                string message = ("message " + i);
                logsInfo[i] = new LogInfo(LogLevel.Info, message);
            }
            fileTarget.Flush(logsInfo);

            bool result = true;
            using (StreamReader reader = new StreamReader(targetFileName))
            {                
                int indexLog = 0;
                while(!reader.EndOfStream && result)
                {
                    string actualStringLine = reader.ReadLine();
                    string expectedStringLine = logsInfo[indexLog].ToString();
                    indexLog++;
                    if(actualStringLine != expectedStringLine)
                    {
                        result = false;
                    }
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
