using System;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Microsoft.Extensions.Configuration;
using Moq;
using MonitoringBatchService.Data;
using MonitoringBatchService.Services;
using Xunit;

namespace MonitoringBatchServiceTest.Services
{
    public class AutoGenerateJsonTests
    {
        private readonly Mock<IDatabaseService> _mockDatabaseService;
        private readonly Mock<IValidatejson> _mockValidJson;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly Mock<IProcessService> _mockProcessService;
        private readonly AutoGenerateJson _autoGenerateJson;

        public AutoGenerateJsonTests()
        {
            _mockDatabaseService = new Mock<IDatabaseService>();
            _mockValidJson = new Mock<IValidatejson>();
            _mockConfig = new Mock<IConfiguration>();
            _mockProcessService = new Mock<IProcessService>();

            _mockConfig.SetupGet(x => x["ExeSettings:ExePath"]).Returns("mockExePath");

            _autoGenerateJson = new AutoGenerateJson(
                _mockConfig.Object,
                _mockDatabaseService.Object,
                _mockValidJson.Object,
                _mockProcessService.Object);
        }

    

        [Fact]
        public async Task RunExe_WhenNoFlow_UpdatesStatusWithoutRunningProcess()
        {
            // Arrange
            _mockDatabaseService.Setup(x => x.FLOW1()).Returns(0);
            _mockDatabaseService.Setup(x => x.FLOW2()).Returns(0);
            _mockDatabaseService.Setup(x => x.FLOW3()).Returns(0);
            _mockDatabaseService.Setup(x => x.FLOW4()).Returns(0);
            _mockDatabaseService.Setup(x => x.FLOW5()).Returns(0);
            _mockDatabaseService.Setup(x => x.FLOW6()).Returns(0);
            _mockDatabaseService.Setup(x => x.FLOW7()).Returns(0);
            _mockDatabaseService.Setup(x => x.FLOW8()).Returns(0);
            _mockDatabaseService.Setup(x => x.FLOW9()).Returns(0);

            // Act
            await _autoGenerateJson.RunExe();

            // Assert
            _mockDatabaseService.Verify(x => x.UpdateRecordsAsyncruls(1, "IN PROGRESS"), Times.Once);
            _mockDatabaseService.Verify(x => x.UpdateRecordsAsyncruls(1, "DONE"), Times.Once);
            _mockDatabaseService.Verify(x => x.UpdateRecordsAsyncruls(0, "OPEN"), Times.Once);
            _mockProcessService.Verify(x => x.RunProcessAsync(It.IsAny<string>()), Times.Never);
            _mockValidJson.Verify(x => x.SendEmailWithJson(), Times.Never);
        }
    }
}