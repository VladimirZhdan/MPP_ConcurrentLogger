using System.IO;
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

        public async Task<bool> FlushAsync(LogInfo[] logsInfo)
        {                        
            using (StreamWriter streamWriter = File.AppendText(fileName))
            {
                for (int i = 0; i < logsInfo.Length; i++)
                {
                    await streamWriter.WriteLineAsync(logsInfo[i].ToString());
                }
            }
            return true;
        }
    }
}
