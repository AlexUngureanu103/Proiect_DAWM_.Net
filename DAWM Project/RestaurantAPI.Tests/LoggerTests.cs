using Moq;
using RestaurantAPI.Domain;

namespace RestaurantAPI.Tests
{
    [TestClass]
    public class LoggerTests
    {
        protected Mock<IDataLogger> _mockLogger;

        /// <summary>
        /// Tests the how many times the logger methods have been used
        /// </summary>
        /// <param name="logErrorCount">LogError counter</param>
        /// <param name="logErrorExCount">LogError with Exception counter</param>
        /// <param name="logWarnCount">LogWarn counter</param>
        /// <param name="logInfoCount">LogInfo counter</param>
        /// <param name="logDebugCount">LogDebug counter</param>
        protected void TestLoggerMethods(int logErrorCount, int logErrorExCount, int logWarnCount, int logInfoCount, int logDebugCount)
        {
            _mockLogger.Verify(log => log.LogError(It.IsAny<string>()), Times.Exactly(logErrorCount));
            _mockLogger.Verify(log => log.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Exactly(logErrorExCount));
            _mockLogger.Verify(log => log.LogWarn(It.IsAny<string>()), Times.Exactly(logWarnCount));
            _mockLogger.Verify(log => log.LogInfo(It.IsAny<string>()), Times.Exactly(logInfoCount));
            _mockLogger.Verify(log => log.LogDebug(It.IsAny<string>()), Times.Exactly(logDebugCount));
        }
    }
}
