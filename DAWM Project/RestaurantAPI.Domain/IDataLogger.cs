namespace RestaurantAPI.Domain
{
    public interface IDataLogger
    {
        void LogInfo(string message);

        void LogDebug(string message);

        void LogWarn(string message);

        void LogError(string message);

        void LogError(string message, Exception ex);
    }
}