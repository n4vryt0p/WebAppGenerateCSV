using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Microsoft.Extensions.Configuration;
using Moq;
using MonitoringBatchService.Data;
using MonitoringBatchService.Models;
using MonitoringBatchService.Services;
using Xunit;

namespace MonitoringBatchServiceTest.Service
{
    public class CopyFileServiceTests
    {
        private readonly Mock<IDatabaseService> _mockDatabaseService;
        private readonly Mock<IValidatejson> _mockValidJson;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly Mock<IFileService> _mockFileService;
        private readonly CopyFileService _copyFileService;

        public CopyFileServiceTests()
        {
            _mockDatabaseService = new Mock<IDatabaseService>();
            _mockValidJson = new Mock<IValidatejson>();
            _mockConfig = new Mock<IConfiguration>();
            _mockFileService = new Mock<IFileService>();

            _mockConfig.SetupGet(x => x["ExeSettings:ExePathsourceFolder"]).Returns("mockSourceFolder");
            _mockConfig.SetupGet(x => x["ExeSettings:ExePathdestinationFolder"]).Returns("mockDestination");

            _copyFileService = new CopyFileService(_mockConfig.Object, _mockDatabaseService.Object, _mockValidJson.Object, _mockFileService.Object);
        }
        [Fact]
        public async Task CopyExe_CallsDatabaseUpdates()
        {
            // Arrange
            var mockLatestFolder = new DirectoryInfo("mockFolder");
            var mockGpgFile = new FileInfo("test.json.gpg");
            var mockOkFile = new FileInfo("test.ok");

            _mockFileService.Setup(x => x.DirectoryExists(It.IsAny<string>())).Returns(true);
            _mockFileService.Setup(x => x.GetDirectories(It.IsAny<string>(), "*", SearchOption.TopDirectoryOnly))
                .Returns(new[] { mockLatestFolder });
            _mockFileService.Setup(x => x.GetFiles(mockLatestFolder.FullName, "*.ok", SearchOption.AllDirectories))
                .Returns(new[] { mockOkFile });
            _mockFileService.Setup(x => x.GetFiles(mockLatestFolder.FullName, "*.json.gpg", SearchOption.AllDirectories))
                .Returns(new[] { mockGpgFile });
            _mockFileService.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false);

            // Act
            await _copyFileService.CopyExe();

            // Assert
            _mockDatabaseService.Verify(db => db.UpdateRecordsAsyncruls(2, "IN PROGRESS"), Times.Once);
            _mockDatabaseService.Verify(db => db.UpdateRecordsAsyncruls(2, "DONE"), Times.Once);
            _mockDatabaseService.Verify(db => db.UpdateFileMonitoring(It.IsAny<string>()), Times.Exactly(2));
            _mockFileService.Verify(fs => fs.CopyFile(It.IsAny<string>(), It.IsAny<string>(), true), Times.Exactly(2));
        }

 
        [Fact]
        public async Task CopyExe_WhenFilesAlreadyExist_DoesNotCopyIfNotModified()
        {
            // Arrange
            _mockFileService.Setup(x => x.DirectoryExists(It.IsAny<string>())).Returns(true);
            _mockFileService.Setup(x => x.GetDirectories(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>()))
                .Returns(new[] { new DirectoryInfo("mockFolder") });
            _mockFileService.Setup(x => x.GetFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>()))
                .Returns(new[] { new FileInfo("mockFile.json.gpg") });
            _mockFileService.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
            _mockFileService.Setup(x => x.GetLastWriteTime(It.IsAny<string>())).Returns(DateTime.Now);

            // Act
            await _copyFileService.CopyExe();

            // Assert
            _mockFileService.Verify(x => x.CopyFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
        }

        [Fact]
        public async Task updaterulseopen3test()
        {
            // Act
            var result = await Record.ExceptionAsync(() => _copyFileService.updaterulseopen3());

            // Assert
            Assert.Null(result);
        }
    }
}