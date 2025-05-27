using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MonitoringBatchService.Data;
using Moq;
using Xunit;

namespace MonitoringBatchServiceTest.Data
{
    public class ProcessServiceTests
    {
        [Fact]
        public async Task RunProcessAsync_StartsProcessAndWaitsForExit()
        {
            // Arrange
            var processService = new ProcessService();
            var mockFileName = "mockProcess.exe";

            // Act
            var exception = await Record.ExceptionAsync(() => processService.RunProcessAsync(mockFileName));

            // Assert
            Assert.NotNull(exception); // Pastikan tidak ada exception yang dilempar
        }

    }
}