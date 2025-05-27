using System;
using System.IO;
using System.Threading.Tasks;
using log4net;
using Microsoft.Extensions.Configuration;
using Moq;
using MonitoringBatchService.Data;
using MonitoringBatchService.Services;
using Xunit;

namespace MonitoringBatchServiceTest.Services
{
    public class ServiceOutboundTests
    {
        private readonly Mock<IDatabaseService> _mockDatabaseService;
        private readonly Mock<IValidatejson> _mockValidJson;
        private readonly Mock<IFileService> _mockFileService;
        private readonly Mock<IProcessService> _mockProcessService;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly ServiceOutbound _serviceOutbound;

        public ServiceOutboundTests()
        {
            _mockDatabaseService = new Mock<IDatabaseService>();
            _mockValidJson = new Mock<IValidatejson>();
            _mockFileService = new Mock<IFileService>();
            _mockProcessService = new Mock<IProcessService>();
            _mockConfig = new Mock<IConfiguration>();

            _mockConfig.SetupGet(x => x["ExeSettings:serviceinbound"]).Returns("mockServiceInbound");
            _mockConfig.SetupGet(x => x["ExeSettings:ExePathdestinationFolder"]).Returns("mockDestinationFolder");
            _mockConfig.SetupGet(x => x["ExeSettings:ExePathdestinationFolderout"]).Returns("mockDestinationFolderOut");

            _serviceOutbound = new ServiceOutbound(
                _mockConfig.Object,
                _mockDatabaseService.Object,
                _mockValidJson.Object,
                _mockFileService.Object,
                _mockProcessService.Object);
        }

        [Fact]
        public async Task RunExe_UpdatesStatusAndProcessesFiles()
        {
            // Arrange
            _mockFileService.Setup(x => x.GetFilesa(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new[] { new FileInfo("mockFile.ok") });

            // Act
            await _serviceOutbound.RunExe();

            // Assert
            _mockDatabaseService.Verify(x => x.UpdateRecordsAsyncruls(3, "IN PROGRESS"), Times.Once);
            _mockDatabaseService.Verify(x => x.UpdateRecordsAsyncruls(3, "DONE"), Times.Once);
            _mockFileService.Verify(x => x.GetFilesa(It.IsAny<string>(), "*.ok"), Times.Once);
            _mockFileService.Verify(x => x.GetFilesa(It.IsAny<string>(), "*.json.gpg"), Times.Once);
            _mockValidJson.Verify(x => x.SendEmailWithConsume(), Times.Once);
            _mockProcessService.Verify(x => x.RunProcessAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task ProcessOkFiles_WhenNoFilesFound_LogsMessage()
        {
            // Arrange
            _mockFileService.Setup(x => x.GetFilesa(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Array.Empty<FileInfo>());

            // Act
            await _serviceOutbound.ProcessOkFiles();

            // Assert
            _mockFileService.Verify(x => x.GetFilesa(It.IsAny<string>(), "*.ok"), Times.Once);
        }

        [Fact]
        public async Task ProcessGpgFiles_WhenNoFilesFound_LogsMessage()
        {
            // Arrange
            _mockFileService.Setup(x => x.GetFilesa(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Array.Empty<FileInfo>());

            // Act
            await _serviceOutbound.ProcessGpgFiles();

            // Assert
            _mockFileService.Verify(x => x.GetFilesa(It.IsAny<string>(), "*.json.gpg"), Times.Once);
        }
    }
}