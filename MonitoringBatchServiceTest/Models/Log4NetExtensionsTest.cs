using Xunit;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using MonitoringBatchService.Models;
using log4net;
using log4net.Config;
using log4net.Core;

namespace MonitoringBatchServiceTest.Models
{
    public class Log4NetExtensionsTest
    {
        [Fact]
        public void AddLog4Net_ShouldConfigureLog4NetAndRegisterLogger()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddLog4net();
            var provider = services.BuildServiceProvider();
            var logger = provider.GetService<ILog>();

            // Assert
            Assert.NotNull(logger);
            Assert.IsType<LogImpl>(logger); // Memastikan logger dari log4net
        }
    }
}
