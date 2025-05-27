using System.Data;
using Microsoft.Data.SqlClient;

namespace MonitoringBatchService.Data
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _configuration;
        public DbConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IDbConnection CreateConnection(string connectionName)
        {
            var connectionString  = _configuration.GetConnectionString(connectionName);
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException($"Connection string ;{connectionName}'  not found");
            }
            return new SqlConnection(connectionString);
        }
    }
}
