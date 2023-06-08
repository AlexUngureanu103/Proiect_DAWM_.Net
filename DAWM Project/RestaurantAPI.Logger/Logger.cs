using RestaurantAPI.Domain;

namespace RestaurantAPI.Logger
{
    public class Logger : IDataLogger
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void LogDebug(string debugMessage)
        {
            log.Debug(debugMessage);
        }

        public void LogError(string errorMessage)
        {
            log.Error(errorMessage);
        }

        public void LogError(string message, Exception exception)
        {
            log.Error(message, exception);
        }

        public void LogInfo(string infoMessage)
        {
            log.Info(infoMessage);
        }

        public void LogWarn(string warnMessage)
        {
            log.Warn(warnMessage);
        }
    }
}