using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPP_ConcurrentLogger
{
    public enum LogLevel
    {
        Dubug,
        Info,
        Warning,
        Error
    }

    public interface ILogger
    {
        void Log(LogLevel level, string message);
    }
}
