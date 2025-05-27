using System;
using System.IO;
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

namespace MonitoringBatchServiceTest.Service
{
    public class ValidatejsonTest
    {
        private readonly Mock<IDatabaseService>? _mockDatabaseService;
        private readonly Mock<IChecking> _mockvalidjson;
        private readonly Mock<IConfiguration> _mockconfig;
        private readonly Validatejson _validatejson;
        private readonly Mock<IFileService> _mockFileService;

        public ValidatejsonTest()
        {
            _mockDatabaseService = new Mock<IDatabaseService>();
            _mockvalidjson = new Mock<IChecking> { };
            _mockconfig = new Mock<IConfiguration>();

            _mockFileService = new Mock<IFileService>();


            // Mock GetSection untuk mengembalikan connection string yang diharapkan
            var mockConnectionStringSection = new Mock<IConfigurationSection>();
            mockConnectionStringSection.Setup(c => c["DefaultConnection"])
                                       .Returns("Server=your_server;Database=your_db;User Id=your_user;Password=your_password;");
            mockConnectionStringSection.Setup(c => c["DefaultConnectiongfcct"])
                                       .Returns("Server=your_server_gfcct;Database=your_db_gfcct;User Id=your_user;Password=your_password;");

            _mockconfig.Setup(c => c.GetSection("ConnectionStrings"))
                               .Returns(mockConnectionStringSection.Object);
         
            _mockFileService.Setup(x => x.GetFiles(It.IsAny<string>(), "*"))
          .Returns(new[] { "mockFile1.json", "mockFile2.json.gpg", "mockFile3._json" });
            _mockFileService.Setup(x =>x.FileExists(It.IsAny<string>()))
                .Returns(true);

            _mockFileService.Setup(x => x.ReadAllTextAsync(It.IsAny<string>()))
                .ReturnsAsync("{ \"FILE_STATUS\": \"OK\"}");
            _mockDatabaseService.Setup(x => x.GetAllMasterTabel(It.IsAny<string>()))
          .Returns(new Mastertabel { Cd = "OK" });
            _validatejson = new Validatejson(_mockconfig.Object, _mockDatabaseService.Object, _mockvalidjson.Object, _mockFileService.Object);
        }

        [Fact]
        public void Jsonlostest()
        {
            //arrage
   
            string targetFolder = "mockFolder";
            string datePart = "2025-03-06";
            //act

            var result = _validatejson.Jsonlos(targetFolder, datePart);

            //assert
            Assert.NotNull(result);

           
        }
        [Fact]
        public void Jsonlostestfalse()
        {
            //arrage

            string targetFolder = "mockFolder";
            string datePart = "2025-03-06";
            //act
            _mockFileService.Setup(x => x.FileExists(It.IsAny<string>()))
        .Returns(false);
            var result = _validatejson.Jsonlos(targetFolder, datePart);

            //assert
            Assert.NotNull(result);

        }


        [Fact]
        public void jsonlosaftertest()
        {

            string targetFolder = "mockFolder";
            string datePart = "2025-03-06";
            //act

            var result = _validatejson.jsonlosafter(targetFolder, datePart);

            //assert
            Assert.NotNull(result);

        }
        [Fact]
        public void jsonlosaftertestflase()
        {

            string targetFolder = "mockFolder";
            string datePart = "2025-03-06";
            //act
            _mockFileService.Setup(x => x.FileExists(It.IsAny<string>()))
    .Returns(false);
            var result = _validatejson.jsonlosafter(targetFolder, datePart);

            //assert
            Assert.NotNull(result);

        }


        [Fact]
        public async Task SendEmailWithConsumeTest()
        {


            //act
            var result = await Record.ExceptionAsync(() => _validatejson.SendEmailWithConsume());

            //assert
            Assert.NotNull(result);

        }

        [Fact]
        public async Task SendEmailWithJsonTest()
        {


            //act
            var result = await Record.ExceptionAsync(() => _validatejson.SendEmailWithJson());

            //assert
            Assert.NotNull(result);

        }

        [Fact]
        public async Task SendEmailWithTriggerTest()
        {

            //act
            var result = await Record.ExceptionAsync(() => _validatejson.SendEmailWithTrigger());

            //assert
            Assert.NotNull(result);

        }
        [Fact]
        public async Task SendAllMailTest()
        {
            //arrage
            string stremail = "abc";
            string strmessage = "abc";
            string strsubject = "abc";
            string strfrom = "abc";
            //act
            var result = await Record.ExceptionAsync(() => _validatejson.SendAllMail(stremail, strmessage, strsubject, strfrom));

            //assert
            Assert.NotNull(result);


        }

        [Theory]
        [InlineData("DRF_test_file_12345.json")] // Kasus 1: File dengan "DRF_" di awal
                                                 // Kasus 6: File kosong
        public void UpdateFileNameDRF_ShouldReturnCorrectFileName(string originalFileName)
        {
            // Act
            string result = _validatejson.UpdateFileNameDRF(originalFileName);

            // Assert
            Assert.Equal("test_file_12345.json", result);
        }
        [Fact]
        public async Task SendEmailWithResultsafterTest()
        {
            //arrage
            string stremail = "";

            List<FileStatusModel> model = new List<FileStatusModel>();

            //act
            var result = await Record.ExceptionAsync(() => _validatejson.SendEmailWithResultsafter(model, stremail));

            //assert
            Assert.NotNull(result);


        }
        [Fact]
        public async Task SendEmailWithResultsTest()
        {
            //arrage
            string stremail = "";

            List<FileStatusModel> model = new List<FileStatusModel>();

            //act
            var result = await Record.ExceptionAsync(() => _validatejson.SendEmailWithResults(model, stremail));

            //assert
            Assert.NotNull(result);


        }

        [Fact]
        public void UpdatedbFileMonitoringtest()
        {
            //arrage
            string date = "2025-03-06";

            List<FileStatusModel> model = new List<FileStatusModel>();


            //act
            var result = _validatejson.UpdatedbFileMonitoring(model, date);

            //assert
            Assert.NotNull(result);


        }

        [Theory]
        [InlineData("file_20240305_120000.json_20240305_123000.gpg", "120000", "123000")]
        [InlineData("data_20240228_220000.json_20240228_223000.gpg", "220000", "223000")]
        public void ExtractTimestamps_ShouldReturnCorrectTimestamps(string fileName, string expected1, string expected2)
        {
            // Act
            var result = _validatejson.ExtractTimestamps(fileName);

            // Assert
            Assert.Equal(expected1, result[0]);
            Assert.Equal(expected2, result[1]);
        }
    }
}
