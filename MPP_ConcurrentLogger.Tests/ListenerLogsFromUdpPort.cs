using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPP_ConcurrentLogger.Tests
{
    public class ListenerLogsFromUdpPort
    {        
        private UdpClient receiver;
        private Thread listenerThread;
        private static int countLogs = 0;

        private volatile bool isStopThread;

        public bool IsRunningListenerThread
        {
            get
            {
                return (listenerThread.IsAlive);                
            }
        }


        public ListenerLogsFromUdpPort(int listeningPort)
        {
            receiver = new UdpClient(listeningPort);
            receiver.Client.ReceiveTimeout = 300;
            Initialization();
        }

        private void Initialization()
        {
            isStopThread = false;
            listenerThread = new Thread(new ThreadStart(ReceiveLogs));
        }

        public void StartListen()
        {
            if(listenerThread != null)
            {
                listenerThread.Start();                
            }
        }

        public void StopListen()
        {
            isStopThread = true;
        }

        private void ReceiveLogs()
        {            
            DateTime prevLogTime = default(DateTime);
            while (!isStopThread)
            {                
                try
                {
                    IPEndPoint senderIPEndPoint = null;
                    byte[] receivedData = receiver.Receive(ref senderIPEndPoint);
                    LogInfo[] logsInfo = ObjectConverter<LogInfo[]>.ByteConverter.BytesToObject(receivedData);
                    DateTime currentLogTime = logsInfo[0].Time;
                    bool expectedResult = true;
                    bool actualResult = (prevLogTime <= currentLogTime);
                    Assert.AreEqual(expectedResult, actualResult);

                    countLogs++;              
                }
                catch(SocketException e)
                {
                    if(e.ErrorCode == (int)SocketError.TimedOut)
                    {
                        continue;                        
                    }                    
                }
                catch(Exception ex)
                {
                    return;
                }
                
            }
        }

    }
}
