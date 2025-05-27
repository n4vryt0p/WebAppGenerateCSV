using System.Data;

namespace MonitoringBatchService.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection(string connectionName);
    }
}
