using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace MonitoringBatchService.Data.Tests
{
    public class DbConnectionFactoryTests
    {
        [Fact]
        public void CreateConnection_ValidConnectionName_ReturnsSqlConnection()
        {
            // Arrange
            var connectionName = "ValidConnection";
            var connectionString = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;";

            var configurationSectionMock = new Mock<IConfigurationSection>();
            configurationSectionMock
                .Setup(c => c[connectionName])
                .Returns(connectionString);

            var configurationMock = new Mock<IConfiguration>();
            configurationMock
                .Setup(c => c.GetSection("ConnectionStrings"))
                .Returns(configurationSectionMock.Object);

            var dbConnectionFactory = new DbConnectionFactory(configurationMock.Object);

            // Act
            var connection = dbConnectionFactory.CreateConnection(connectionName);

            // Assert
            Assert.NotNull(connection);
            Assert.IsType<SqlConnection>(connection);
            Assert.Equal(connectionString, ((SqlConnection)connection).ConnectionString);
        }

    }
}