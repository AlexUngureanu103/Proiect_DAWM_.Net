namespace RestaurantAPI.Domain
{
    public interface IDataLogger
    {
        /// <summary>
        /// Log info message
        /// </summary>
        /// <param name="message">Info message to display into the logger file</param>
        void LogInfo(string message);

        /// <summary>
        /// Log debug message
        /// </summary>
        /// <param name="message">Debug message to display into the logger file</param>
        void LogDebug(string message);

        /// <summary>
        /// Log warn message
        /// </summary>
        /// <param name="message">Warning message to display into the logger file</param>
        void LogWarn(string message);

        /// <summary>
        /// Log error message
        /// </summary>
        /// <param name="message">Error message to display into the logger file</param>
        void LogError(string message);


        /// <summary>
        /// Log error message with exception
        /// </summary>
        /// <param name="message">Error message to display into the logger file</param>
        /// <param name="ex">Exception stack traces and inner exceptions to display intro the logger file</param>
        void LogError(string message, Exception ex);
    }
}