namespace EasyWork.Utilities
{
    public interface IELogger
    {
        void LogError(string context);
        void LogWarning(string context);
        void Log(string context);
    }
}

