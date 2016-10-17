using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPP_ConcurrentLogger
{
    public interface ILoggerTarget
    {
        bool Flush();
        Task<bool> FlushAsync();
    }
}
