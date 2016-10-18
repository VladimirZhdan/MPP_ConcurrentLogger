using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPP_ConcurrentLogger
{
    public class FileTarget : ILoggerTarget
    {
        private string fileName;

        public FileTarget(string fileName)
        {
            this.fileName = fileName;
        }

        public bool Flush(LogInfo[] logsInfo)
        {
            using (StreamWriter streamWriter = new StreamWriter(fileName, true))
            {
                for(int i = 0; i < logsInfo.Length; i++)
                {
                    streamWriter.WriteLine(logsInfo[i]);
                }                
            }
            return true;
        }

        public Task<bool> FlushAsync(LogInfo[] logsInfo)
        {
            throw new NotImplementedException();
        }
    }
}
