namespace Services.Debug
{
    public interface ILogService : IService
    {
        void Log(string message);
        void Warning(string message);
        void Error(string message);
        void ErrorFormat(string message, params object[] formatValues);
    }
}