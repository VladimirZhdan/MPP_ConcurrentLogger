using System;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace MPP_ConcurrentLogger
{
    public class UdpPortTarget : ILoggerTarget
    {
        private UdpClient sender;
        private IPEndPoint descPoint;

        public UdpPortTarget(IPAddress ipAddress, int descPort)
        {
            descPoint = new IPEndPoint(ipAddress, descPort);
            sender = new UdpClient();            
        }

        public bool Flush(LogInfo[] logsInfo)
        {
            try
            {
                byte[] bytesLogsInfo = ObjectConverter<LogInfo[]>.ByteConverter.ObjectToBytes(logsInfo);
                sender.Connect(descPoint);                                 
                sender.Send(bytesLogsInfo, bytesLogsInfo.Length);
            }
            catch (Exception e)
            {
                return false;
            }                            
            return true;            
        }

        public Task<bool> FlushAsync(LogInfo[] logsInfo)
        {
            return Task.Run(() =>
            {
                return Flush(logsInfo);
            });
        }
    }
}
