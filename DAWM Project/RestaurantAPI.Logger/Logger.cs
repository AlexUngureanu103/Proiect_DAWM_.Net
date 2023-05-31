using RestaurantAPI.Domain;

namespace RestaurantAPI.Logger
{
    public class Logger : IDataLogger
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Log debug message
        /// </summary>
        /// <param name="debugMessage">Debug message to display into the logger file</param>
        public void LogDebug(string debugMessage)
        {
            log.Debug(debugMessage);
        }

        /// <summary>
        /// Log error message
        /// </summary>
        /// <param name="errorMessage">Error message to display into the logger file</param>
        public void LogError(string errorMessage)
        {
            log.Error(errorMessage);
        }

        /// <summary>
        /// Log error message with exception
        /// </summary>
        /// <param name="message">Error message to display into the logger file</param>
        /// <param name="exception">Exception stack traces and inner exceptions to display intro the logger file</param>
        public void LogError(string message, Exception exception)
        {
            log.Error(message, exception);
        }

        /// <summary>
        /// Log info message
        /// </summary>
        /// <param name="infoMessage">Info message to display into the logger file</param>
        public void LogInfo(string infoMessage)
        {
            log.Info(infoMessage);
        }

        /// <summary>
        /// Log warn message
        /// </summary>
        /// <param name="warnMessage">Warning message to display into the logger file</param>
        public void LogWarn(string warnMessage)
        {
            log.Warn(warnMessage);
        }
    }
}