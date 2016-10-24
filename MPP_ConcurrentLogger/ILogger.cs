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
