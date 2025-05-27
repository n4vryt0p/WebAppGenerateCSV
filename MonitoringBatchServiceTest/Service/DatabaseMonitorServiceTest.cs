using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using log4net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MonitoringBatchService.Data;
using MonitoringBatchService.Models;
using MonitoringBatchService.Services;
using Xunit;
using System.Globalization;

namespace MonitoringBatchService.Tests
{
    public class DatabaseMonitorServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IServiceScopeFactory> _mockScopeFactory;
        private readonly DatabaseMonitorService _databaseMonitorService;

        public DatabaseMonitorServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockScopeFactory = new Mock<IServiceScopeFactory>();
            
            // Setup mock configuration
            _mockConfiguration.Setup(c => c["ExeSettings:DelayTime"]).Returns("1000");

            // Initialize the service
            _databaseMonitorService = new DatabaseMonitorService(
                _mockConfiguration.Object,
                _mockScopeFactory.Object
             
            );
        }

        [Fact]
        public async Task ExecuteAsync_ShouldProcessRecords()
        {
            // Arrange
            var mockDatabaseService = new Mock<IDatabaseService>();
            mockDatabaseService
                .Setup(s => s.GetRecordsAsync())
                .ReturnsAsync(new List<BatchGenerateOutput>
                {
            new BatchGenerateOutput { ProcessId = 1, ProcessRemarks = "OPEN" }
                });

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider
                .Setup(p => p.GetService(typeof(IDatabaseService)))
                .Returns(mockDatabaseService.Object);

            var mockScope = new Mock<IServiceScope>();
            mockScope.Setup(s => s.ServiceProvider).Returns(mockServiceProvider.Object);

            _mockScopeFactory
                .Setup(f => f.CreateScope())
                .Returns(mockScope.Object);

            // Act
            var executeAsyncMethod = typeof(DatabaseMonitorService)
                .GetMethod("ExecuteAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var task = (Task)executeAsyncMethod.Invoke(_databaseMonitorService, new object[] { new CancellationToken() });
           

            // Assert
            mockDatabaseService.Verify(s => s.GetRecordsAsync(), Times.Once);
        }


        [Fact]
        public async Task HandleProcess1Async_ShouldCallProcessStatusGenerate()
        {
            // Arrange
            var mockAutoGenerateJson = new Mock<IAutoGenerateJson>();
            mockAutoGenerateJson
                .Setup(s => s.RunExe())
                .Returns(Task.CompletedTask);

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider
                .Setup(p => p.GetService(typeof(IAutoGenerateJson)))
                .Returns(mockAutoGenerateJson.Object);

            var mockScope = new Mock<IServiceScope>();
            mockScope.Setup(s => s.ServiceProvider).Returns(mockServiceProvider.Object);

            _mockScopeFactory
                .Setup(f => f.CreateScope())
                .Returns(mockScope.Object);

            // Act
            await _databaseMonitorService.HandleProcess1Async("OPEN");

            // Assert
            mockAutoGenerateJson.Verify(s => s.RunExe(), Times.Once);
        }

        [Fact]
        public async Task HandleProcess2Async_ShouldCallProcessCopyFile()
        {
            // Arrange
            var mockCopyFileService = new Mock<ICopyFileService>();
            mockCopyFileService
                .Setup(s => s.CopyExe())
                .Returns(Task.CompletedTask);

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider
                .Setup(p => p.GetService(typeof(ICopyFileService)))
                .Returns(mockCopyFileService.Object);

            var mockScope = new Mock<IServiceScope>();
            mockScope.Setup(s => s.ServiceProvider).Returns(mockServiceProvider.Object);

            _mockScopeFactory
                .Setup(f => f.CreateScope())
                .Returns(mockScope.Object);

            // Act
            await _databaseMonitorService.HandleProcess2Async("OPEN");

            // Assert
            mockCopyFileService.Verify(s => s.CopyExe(), Times.Once);
        }

        [Fact]
        public async Task HandleProcess3Async_ShouldCallProcessexeoutbond()
        {
            // Arrange
            var mockServiceOutbound = new Mock<IServiceOutbound>();
            mockServiceOutbound
                .Setup(s => s.RunExe())
                .Returns(Task.CompletedTask);

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider
                .Setup(p => p.GetService(typeof(IServiceOutbound)))
                .Returns(mockServiceOutbound.Object);

            var mockScope = new Mock<IServiceScope>();
            mockScope.Setup(s => s.ServiceProvider).Returns(mockServiceProvider.Object);

            _mockScopeFactory
                .Setup(f => f.CreateScope())
                .Returns(mockScope.Object);

            // Act
            await _databaseMonitorService.HandleProcess3Async("OPEN");
          
            // Assert
            mockServiceOutbound.Verify(s => s.RunExe(), Times.Once);
        }
      
        [Fact]
        public async Task HandleProcess4Async_ShouldCallProcessrespondate()
        {
            // Arrange
            var mockDatabaseService = new Mock<IDatabaseService>();
            mockDatabaseService
                .Setup(s => s.CheckDataRespondate())
                .Returns(DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider
                .Setup(p => p.GetService(typeof(IDatabaseService)))
                .Returns(mockDatabaseService.Object);

            var mockScope = new Mock<IServiceScope>();
            mockScope.Setup(s => s.ServiceProvider).Returns(mockServiceProvider.Object);

            _mockScopeFactory
                .Setup(f => f.CreateScope())
                .Returns(mockScope.Object);

            // Act
            await _databaseMonitorService.HandleProcess4Async("DONE");

            // Assert
            mockDatabaseService.Verify(s => s.CheckDataRespondate(), Times.Once);
        }
    }
}