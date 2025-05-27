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
using System.Data;
using NLog;

namespace MonitoringBatchServiceTest.Service
{
    public class FileWatcherServiceTest
    {
    

       
            private readonly Mock<IDatabaseService> _mockDatabaseService;
            private readonly Mock<IValidatejson> _mockvalidjson;
            private readonly Mock<IConfiguration> _mockconfig;
            private readonly Mock<IFileService> _mockFileService;
            private readonly FileWatcherService _filewatcherService;

            public FileWatcherServiceTest()
            {
                _mockDatabaseService = new Mock<IDatabaseService>();
                _mockvalidjson = new Mock<IValidatejson>();
                _mockconfig = new Mock<IConfiguration>();
                _mockFileService = new Mock<IFileService>();

                _mockconfig.SetupGet(x => x["Settings:WatchDirectorry"]).Returns("mockSourceFolder");
                _mockconfig.SetupGet(x => x["Settings:DestinationDirectory"]).Returns("mockDestination");

                // Mock hasil dari GetFiles
                _mockFileService.Setup(x => x.GetFiles("mockSourceFolder", "*.gpg"))
                    .Returns(new[] { "mockFile1.gpg", "mockFile2.gpg" });

                _mockFileService.Setup(x => x.GetFiles("mockSourceFolder", "*"))
                    .Returns(new[] { "mockFile1.gpg", "mockFile2.gpg", "mockFile3.txt" });

                _mockvalidjson.Setup(x => x.UpdateFileNameDRF(It.IsAny<string>())).Returns("mockRenamedFile");
                _mockDatabaseService.Setup(x => x.CheckCountDRF("mockRenamedFile")).Returns("2");

                _filewatcherService = new FileWatcherService(
                    _mockconfig.Object,
                    _mockDatabaseService.Object,
                    _mockvalidjson.Object,
                    _mockFileService.Object);
            }

            [Fact]
            public async Task Copyexetest()
            {
                // Act
                await _filewatcherService.ExecuteAsync();

                // Assert - Pastikan metode CheckCountDRF dipanggil
                _mockDatabaseService.Verify(x => x.CheckCountDRF("mockRenamedFile"), Times.Once);
            }
   
        


    }
}
