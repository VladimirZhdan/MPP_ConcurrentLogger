using System;

namespace MPP_ConcurrentLogger
{
    public interface ILogInfo
    {
        LogLevel Level { get; }
        string Message { get; }
        DateTime Time { get; }
    }
}
