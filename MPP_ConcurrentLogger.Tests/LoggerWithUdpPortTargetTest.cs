using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace MPP_ConcurrentLogger.Tests
{
    [TestClass]
    public class LoggerWithUdpPortTargetTest
    {
        private UdpPortTarget udpPortTarget;
        private int port;
       
        [TestInitialize]
        public void Initialize()
        {
            port = 45000;
            udpPortTarget = new UdpPortTarget(IPAddress.Parse("127.0.0.1"), port);
        }        

        [TestMethod]
        public void CheckFlushOrderInUdpPortTargetByThreads()
        {
            ILoggerTarget[] loggerTargets = new ILoggerTarget[1];
            loggerTargets[0] = udpPortTarget;
            Logger logger = new Logger(2, loggerTargets);

            ListenerLogsFromUdpPort listener = new ListenerLogsFromUdpPort(port);
            listener.StartListen();

            LogThreadPool.RunAndWaitLogingThreads(10, 2, logger);
            while (!logger.IsFlushLogThreadPoolClear) ;


            listener.StopListen();
            while (listener.IsRunningListenerThread) ;            
        }      
    }
}
