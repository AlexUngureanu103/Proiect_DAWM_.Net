using Moq;

namespace RestaurantAPI.Tests.LoggersTests
{
    [TestClass]
    public class LoggerTests
    {
        Mock<log4net.ILog> _mockILog;
        private Logger.Logger _logger;

        [TestInitialize]
        public void Setup()
        {
            _mockILog = new();
            _logger = new();
            _logger.SetLogger(_mockILog.Object);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            _mockILog = null;
            _logger = null;
        }

        [TestMethod]
        public void SetLogger_WhenLog4NetILogIsNull_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _logger.SetLogger(null));
        }

        [TestMethod]
        public void Logger_LogDebug_Calls_LogDebug_Method_From_log4Net_ILog()
        {
            var debugMessage = "Test Debug Message";

            _logger.LogDebug(debugMessage);

            _mockILog.Verify(log => log.Debug(It.IsAny<string>()), Times.Once);
            _mockILog.Verify(log => log.Error(It.IsAny<string>()), Times.Never);
            _mockILog.Verify(log => log.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
            _mockILog.Verify(log => log.Info(It.IsAny<string>()), Times.Never);
            _mockILog.Verify(log => log.Warn(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void Logger_LogError_Calls_LogError_Method_From_log4Net_ILog()
        {
            var errorMessage = "Test Error Message";

            _logger.LogError(errorMessage);

            _mockILog.Verify(log => log.Debug(It.IsAny<string>()), Times.Never);
            _mockILog.Verify(log => log.Error(It.IsAny<string>()), Times.Once);
            _mockILog.Verify(log => log.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
            _mockILog.Verify(log => log.Info(It.IsAny<string>()), Times.Never);
            _mockILog.Verify(log => log.Warn(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void Logger_LogErrorWithException_Calls_LogErrorWithException_Method_From_log4Net_ILog()
        {
            var errorMessage = "Test Error Message";
            var exception = new Exception();

            _logger.LogError(errorMessage, exception);
            _mockILog.Verify(log => log.Debug(It.IsAny<string>()), Times.Never);
            _mockILog.Verify(log => log.Error(It.IsAny<string>()), Times.Never);
            _mockILog.Verify(log => log.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
            _mockILog.Verify(log => log.Info(It.IsAny<string>()), Times.Never);
            _mockILog.Verify(log => log.Warn(It.IsAny<string>()), Times.Never);

        }

        [TestMethod]
        public void Logger_LogInfo_Calls_LogInfo_Method_From_log4Net_ILog()
        {
            var infoMessage = "Test Information Message";

            _logger.LogInfo(infoMessage);

            _mockILog.Verify(log => log.Debug(It.IsAny<string>()), Times.Never);
            _mockILog.Verify(log => log.Error(It.IsAny<string>()), Times.Never);
            _mockILog.Verify(log => log.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
            _mockILog.Verify(log => log.Info(It.IsAny<string>()), Times.Once);
            _mockILog.Verify(log => log.Warn(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void Logger_LogWarn_Calls_LogWarn_Method_From_log4Net_ILog()
        {
            var warnMessage = "Test Warning Message";

            _logger.LogWarn(warnMessage);

            _mockILog.Verify(log => log.Debug(It.IsAny<string>()), Times.Never);
            _mockILog.Verify(log => log.Error(It.IsAny<string>()), Times.Never);
            _mockILog.Verify(log => log.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
            _mockILog.Verify(log => log.Info(It.IsAny<string>()), Times.Never);
            _mockILog.Verify(log => log.Warn(It.IsAny<string>()), Times.Once);
        }
    }
}