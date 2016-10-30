using System;
using System.IO;

namespace MPP_ConcurrentLogger
{
    class Program
    {        
        static void Main(string[] args)
        {
            //string fileName = "data.txt";
            //ReCreateFile(fileName);
            //ILoggerTarget[] loggerTarget = new ILoggerTarget[1];
            //loggerTarget[0] = new FileTarget(fileName);
            //Logger logger = new Logger(2, loggerTarget);
            //LogThreadPool.RunAndWaitLogingThreads(50, 2, logger);                   
            //Console.WriteLine("Press <Enter> to exit");         
            WriteCharacters();
            Console.ReadLine();            
        }   

        private static void ReCreateFile(string fileName)
        {
            FileStream fStream = File.Create(fileName);
            fStream.Close();
        }

        static async void WriteCharacters()
        {
            FileStream fStream = File.Create("newfile.txt");
            fStream.Close();
            using (StreamWriter writer = new StreamWriter("newfile.txt", true))
            {
                await writer.WriteLineAsync("First line of example1");
                await writer.WriteLineAsync("and second line");
            }
        }

    }
}
