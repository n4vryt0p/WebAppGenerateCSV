using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MonitoringBatchService.Controllers;
using MonitoringBatchService.Data;
using MonitoringBatchService.Services;
using Moq;
using Xunit;

namespace MonitoringBatchService.Tests.Controllers
{
    public class TriggerAutomationControllerTests
    {
        private readonly Mock<IDatabaseService> _mockDatabaseService;
        private readonly TriggerAutomationController _controller;

        public TriggerAutomationControllerTests()
        {
            _mockDatabaseService = new Mock<IDatabaseService>();
            _controller = new TriggerAutomationController(_mockDatabaseService.Object);
        }

        [Fact]
        public async Task GetRecords_ShouldReturnSuccessResponse_WhenUpdateIsSuccessful()
        {
            // Arrange
            _mockDatabaseService.Setup(x => x.UpdateRecordsAsyncruls(1, "OPEN"))
                .Returns(Task.CompletedTask);

            var expectedResponse = new
            {
                Code = 200,
                Result = "Success,",
                Content = new
                {
                    Message = "Status has been updated to 1-Open."
                }
            };

            // Act
            var result = await _controller.GetRecords();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = JsonSerializer.Serialize(okResult.Value);
            var expected = JsonSerializer.Serialize(expectedResponse);
            Assert.Equal(expected, response);

            _mockDatabaseService.Verify(x => x.UpdateRecordsAsyncruls(1, "OPEN"), Times.Once);
            _mockDatabaseService.Verify(x => x.GetLogs(It.IsAny<string>()), Times.Once);
        }



        [Fact]
        public async Task GetRecords_ShouldLogTheResponse_WhenSuccessful()
        {
            // Arrange
            _mockDatabaseService.Setup(x => x.UpdateRecordsAsyncruls(1, "OPEN"))
                .Returns(Task.CompletedTask);

            string loggedMessage = null;
            _mockDatabaseService.Setup(x => x.GetLogs(It.IsAny<string>()))
                .Callback<string>(msg => loggedMessage = msg)
                .Returns(Task.CompletedTask);

            // Act
            await _controller.GetRecords();

            // Assert
            Assert.NotNull(loggedMessage);
            var loggedObject = JsonSerializer.Deserialize<dynamic>(loggedMessage);
            Assert.NotNull(loggedObject);

            // Verify the logged message contains expected properties
            var jsonDoc = JsonDocument.Parse(loggedMessage);
            var root = jsonDoc.RootElement;
            Assert.Equal(200, root.GetProperty("Code").GetInt32());
            Assert.Equal("Success,", root.GetProperty("Result").GetString());
            Assert.Equal("Status has been updated to 1-Open.",
                root.GetProperty("Content").GetProperty("Message").GetString());
        }
    }
}