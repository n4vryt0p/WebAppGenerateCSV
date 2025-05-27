using Xunit;
using Moq;
using MonitoringBatchService.Models;
using log4net;

namespace MonitoringBatchServiceTest.Models
{
    public class LoggerTests
    {
        [Fact]
        public void logDebug()
        {
            // Arrange
            var mocklog = new Mock<ILog>();
            var LOGGER  = new Logger(mocklog.Object);

            // Act
            LOGGER.Debug("Thiss message");
            mocklog.Verify(log => log.Debug("Thiss message"), Times.Once);

            // Assert

        }
        [Fact]
        public void logInfo()
        {
            // Arrange
            var mocklog = new Mock<ILog>();
            var LOGGER = new Logger(mocklog.Object);

            // Act
            LOGGER.Info("Thiss message");
            mocklog.Verify(log => log.Info("Thiss message"), Times.Once);

            // Assert

        }
        [Fact]
        public void logerror()
        {

            // Assert

            // Arrange
            var mockLog = new Mock<ILog>();
            var logger = new Logger(mockLog.Object);
            var exception = new Exception("This is an error message");

            // Act
            logger.Error("This is an error message", exception);

            // Assert
            mockLog.Verify(log => log.Error("This is an error message", exception), Times.Once);

        }
    }
    }
