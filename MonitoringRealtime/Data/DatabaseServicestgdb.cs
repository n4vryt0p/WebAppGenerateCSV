using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Text.RegularExpressions;
using MonitoringRealtime.Models;

namespace MonitoringRealtime.Data
{
    public class DatabaseServicestgdb
    {
        private readonly DatabaseService _databaseService;

        public DatabaseServicestgdb(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<IEnumerable<BatchGenerateOutput>> GetBatchCustomeroutput()
        {
            string sql = "SELECT * FROM [STG_DB].[LIL].[BatchGenerateOutput]";
            return await _databaseService.QueryAsync<BatchGenerateOutput>(sql);
        }

        public async Task<IEnumerable<Flow>> GetUserById(string flowKey)
        {
            string sql = $"Select * FROM [GFCC_WEB].[WebApp].[MasterConfigs] where [Key] ='{flowKey}'";
            return await _databaseService.QueryAsync<Flow>(sql);
        }

        public async Task<int> FLOW1()
        {
       
                string sql = @"select COUNT (*) FROM [GFCC_WEB].[WebApp].SQLMNLGetCustomer";
                return await _databaseService.QuerySingleAsync<int>(sql);        
           
        }
        public async Task<int> FLOW2()
        {

            string sql = @"select COUNT (*) FROM [GFCC_WEB].[WebApp].SQLMNLGetCustomerCustomerLink";
            return await _databaseService.QuerySingleAsync<int>(sql);

        }
        public async Task<int> FLOW3()
        {

            string sql = @"select COUNT (*) FROM [GFCC_WEB].[WebApp].SQLMNLGetCustomerPolicyLink";
            return await _databaseService.QuerySingleAsync<int>(sql);

        }
        public async Task<int> FLOW4()
        {

            string sql = @"select COUNT (*) FROM [GFCC_WEB].[WebApp].SQLMNLGetIntermediaryPolicyLink";
            return await _databaseService.QuerySingleAsync<int>(sql);

        }
        public async Task<int> FLOW5()
        {

            string sql = @"select COUNT (*) FROM [GFCC_WEB].[WebApp].SQLMNLGetIntermediaries";
            return await _databaseService.QuerySingleAsync<int>(sql);
        }
        public async Task<int> FLOW6()
        {

            string sql = @"select COUNT (*) FROM [GFCC_WEB].[WebApp].SQLMNLGetPolicies";
            return await _databaseService.QuerySingleAsync<int>(sql);
        }
        public async Task<int> FLOW7()
        {

            string sql = @"select COUNT (*) FROM [GFCC_WEB].[WebApp].SQLMNLGetProductSourceType";
            return await _databaseService.QuerySingleAsync<int>(sql);

        }
        public async Task<int> FLOW8()
        {

            string sql = @"select COUNT (*) FROM [GFCC_WEB].[WebApp].SQLMNLGetProducts";
            return await _databaseService.QuerySingleAsync<int>(sql);

        }
        public async Task<int> FLOW9()
        {

            string sql = @"select COUNT (*) FROM [GFCC_WEB].[WebApp].SQLMNLGetOperation";
            return await _databaseService.QuerySingleAsync<int>(sql);

        }

        public async Task<string> Checkdatafilemonitoring(string filename)
        {

            string sql = $"SELECT  consumeDT  FROM [GFCC_WEB].[LIL].[FileMonitoring] where fileRq = '{filename}'";
            return await _databaseService.QuerySingleAsync<string>(sql);

        }

        public async Task<int> Updateallflow(string Key, string Group)
        {
            string sql = "UPDATE [GFCC_WEB].[WebApp].[MasterConfigs] set[Group] = @Group where [Key] = @Key";
            return await _databaseService.ExecuteAsync(sql, new { Group = Group, Key = Key });
        }


    }
}
