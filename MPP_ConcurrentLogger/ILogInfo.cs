using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPP_ConcurrentLogger
{
    public interface ILogInfo
    {
        LogLevel Level { get; }
        string Message { get; }
        DateTime Time { get; }
    }
}
