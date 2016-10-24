using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Net;

namespace MPP_ConcurrentLogger.Tests
{
    [TestClass]
    public class LoggerTest
    {
        private FileTarget fileTarget;
        private string targetFileName;
        private UdpPortTarget udpPortTarget;
        private int port;

        [TestInitialize]
        public void Initialize()
        {
            targetFileName = "loggerTest.txt";
            ReCreateFile(targetFileName);
            fileTarget = new FileTarget(targetFileName);

            port = 50000;
            udpPortTarget = new UdpPortTarget(IPAddress.Parse("127.0.0.1"), port);
        }

        [TestMethod]
        public void CheckFlushOrderByThreads()
        {
            ILoggerTarget[] loggerTarget = new ILoggerTarget[2];
            loggerTarget[0] = fileTarget;
            loggerTarget[1] = udpPortTarget;

            Logger logger = new Logger(2, loggerTarget);

            ListenerLogsFromUdpPort listener = new ListenerLogsFromUdpPort(port);
            listener.StartListen();
            
            LogThreadPool.RunAndWaitLogingThreads(50, 2, logger);

            LoggerWithFileTargetTest.TestMethod(targetFileName);

            listener.StopListen();
            while (listener.IsRunningListenerThread) ;
        }

        private void ReCreateFile(string fileName)
        {
            FileStream fStream = File.Create(fileName);
            fStream.Close();
        }

    }
}
