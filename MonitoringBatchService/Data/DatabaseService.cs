using Dapper;
using MonitoringBatchService.Models;
using Microsoft.EntityFrameworkCore;
using log4net;
using System.Diagnostics;
using System.Text;

namespace MonitoringBatchService.Data
{


    public class DatabaseService : IDatabaseService   
    {

        private static readonly ILog _log = LogManager.GetLogger(typeof(DatabaseService));
        //baru
        private readonly IDbContextFactory<GfccstgDbContext> _ctxStg;
        private readonly IDbContextFactory<GfccWebDbContext> _ctxWeb;
        private readonly IFileService _fileService;

        public DatabaseService(IDbContextFactory<GfccWebDbContext> ctxWeb, IDbContextFactory<GfccstgDbContext> ctxStg, IFileService fileService)
        {
            _ctxWeb = ctxWeb;
            _ctxStg = ctxStg;
            _fileService = fileService;
        }


        public async Task<IEnumerable<BatchGenerateOutput>> GetRecordsAsync()
        {
            try
            {
                using var context = await _ctxStg.CreateDbContextAsync();
                return await context.BatchGenerateOutput.ToListAsync();
            }
            catch (Exception ex)
            {
                _log.Error("Kesalahan Pada Query BatchGenerateOutput",ex);
                return Enumerable.Empty<BatchGenerateOutput>();
                
            }
        }
     
        public Mastertabel GetAllMasterTabel(string Actualcd)
        {
            
            using var context = _ctxStg.CreateDbContext();
            _log.Info($"Ini koneksi {context}");
            var result = context.MasterTable
                .Where(x => x.TableName == "File Status" && x.ActualCd == Actualcd)
                .FirstOrDefault();

            return result ?? new Mastertabel();
        }
        public async Task UpdateRecordsAsync(int prosesid, int newstaus)
        {
            using var context = await _ctxStg.CreateDbContextAsync();
            await context.Database.ExecuteSqlInterpolatedAsync($"Update [STG_DB].[LIL].[BatchGenerateOutput] set ProcessId = {prosesid} ,ProcessDate = GETDATE();");
        }

        public async Task UpdateRecordsAsyncruls(int ProcessId, string ProcessRemarks)
        {

            using var context = await _ctxStg.CreateDbContextAsync();

            await context.BatchGenerateOutput

                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(b => b.ProcessId, ProcessId)
                    .SetProperty(b => b.ProcessRemarks, ProcessRemarks)
                    .SetProperty(b => b.ProcessDate, DateTime.Now)
                );
        }
        public async Task UpdateDRFsuccess(string fileRq)
        {


            using var context = await _ctxStg.CreateDbContextAsync();
            await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE [GFCC_WEB].[LIL].[FileMonitoring] set responseDT = GETDATE() ,statusCode = '200', isInbound = 1 where fileRq  = {fileRq}  ");
        }

        public async Task<IEnumerable<Flow>> GetFlow(string flowKey)
        {
            try
            {
             

                using var context = _ctxWeb.CreateDbContext();


                var result = context.MasterConfigs
               .Where(x => x.Key == flowKey);
                return result;
            }
            catch (Exception ex)
            {
                _log.Error("Kesalahan pada MasterConfigs", ex);

                return Enumerable.Empty<Flow>();
            }
        }

        public async Task SendEmailWithResults(List<FileStatusModel> fileStatusList)
        {
            foreach (var fileStatus in fileStatusList)
            {
                using var context = await _ctxWeb.CreateDbContextAsync();

                var fileMonitoring = await context.FileMonitoring
                    .FirstOrDefaultAsync(fm => fm.fileRq == fileStatus.FileRequest);

                if (fileMonitoring != null)
                {
                    fileMonitoring.responseDT = DateTime.Now;
                    fileMonitoring.fileSizeRq = GetFileSize(fileStatus.FileName ?? string.Empty);
                    fileMonitoring.fileRs = fileStatus.FileName;
                    fileMonitoring.isInbound = true;
                    context.FileMonitoring.Update(fileMonitoring);
                    await context.SaveChangesAsync();
                }
            }
        }
        public async Task UpdateFileMonitoring(string fileRq)
        {


            using var context = await _ctxWeb.CreateDbContextAsync();
            await context.Database.ExecuteSqlInterpolatedAsync($"Update [GFCC_WEB].[LIL].[FileMonitoring] set triggerDT = GETDATE()  where fileRq = {fileRq} ");
        }
        public async Task UpdateFileMonitoringRespon(string fileRq, long fileSizeRs, string fileRs, string urlPathRs)
        {

            using var context = await _ctxWeb.CreateDbContextAsync();
            await context.Database.ExecuteSqlInterpolatedAsync($" Update [GFCC_WEB].[LIL].[FileMonitoring] set responseDT = GETDATE() , fileSizeRs = {fileSizeRs} , statusCode = '200' , fileRs = {fileRs} ,urlPathRs = {urlPathRs} where fileRq = {fileRq}");

           
        }
        public async Task UpdateFileMonitoringtriger(string fileRq)
        {


   
            using var context = await _ctxWeb.CreateDbContextAsync();
            await context.Database.ExecuteSqlInterpolatedAsync($"Update [GFCC_WEB].[LIL].[FileMonitoring] set consumeDT = GETDATE()  where fileRq = {fileRq} ");
        }

        public long GetFileSize(string fileName)
        {
            try
            {
                return _fileService.GetFileSize(fileName);
            }
            catch (UnauthorizedAccessException ex)
            {
                _log.Error("Access to the file is denied.", ex);
                throw;
            }
       
        }

        public string CheckCountDRF(string fileRq)
        {
       
       
            using var context = _ctxWeb.CreateDbContext();



            var result = context.FileMonitoring
           .Where(x =>  x.fileRq == fileRq)
           .Select(x => x.CountFile)
           .FirstOrDefault();
            return result?.ToString() ?? string.Empty;

        }
        //baru
        public async Task GetLogs(string response)
        {
            var los = "http://wrdapp05/gfcc_api/api/Monitoring/v2/TriggerAutomation";
            var RequestId  = Guid.NewGuid().ToString();
           var size  = Encoding.UTF8.GetByteCount(response);
            using var context = await _ctxWeb.CreateDbContextAsync();
            await context.Database.ExecuteSqlInterpolatedAsync($@"INSERT INTO [GFCC_WEB].[dbo].[WebApi.Logs] 
                                                              (requestID,txnLogID,clientID,clientIP,nodeIP, contentType,method,urlPath,trxRq_DT,payloadSizeRq,payloadRq,trxRs_DT,processingTime,httpStatus_CD,payloadSizeRs,PayloadRs,isInbound) 
                                                                values 
                                                             ({RequestId},{RequestId},'triggerAutomation','10.55.10.21','WRDAPP05','application/json','GET',{los},GETDATE(),'0','',GETDATE(),'1','200',{size},{response},'1')");

        }

        //Check flow

        public int FLOW1()
        {
           using var context = _ctxWeb.CreateDbContext();
           return context.SQLMNLGetCustomer.Count();      
        }
        public int FLOW2()
        {
      
            using var context = _ctxWeb.CreateDbContext();
            return context.SQLMNLGetCustomerCustomerLink.Count();
        }
        public int FLOW3()
        {
        
            using var context = _ctxWeb.CreateDbContext();
            return context.SQLMNLGetCustomerPolicyLink.Count();
        }
        public int FLOW4()
        {
          
            using var context = _ctxWeb.CreateDbContext();
            return context.SQLMNLGetIntermediaryPolicyLink.Count();
        }
        public int FLOW5()
        {
         
            using var context = _ctxWeb.CreateDbContext();
            return context.SQLMNLGetIntermediaries.Count();
        }
        public int FLOW6()
        {
          
            using var context = _ctxWeb.CreateDbContext();
            return context.SQLMNLGetPolicies.Count();
        }
        public int FLOW7()
        {
     
            using var context = _ctxWeb.CreateDbContext();
            return context.SQLMNLGetProductSourceType.Count();
        }
        public int FLOW8()
        {
         
            using var context = _ctxWeb.CreateDbContext();
            return context.SQLMNLGetProducts.Count();
        }

        public int FLOW9()
        {
          
            
            using var context = _ctxWeb.CreateDbContext();
            return context.SQLMNLGetOperation.Count();
        }

        public async Task Updateallflow(string Key, string Group)
        {


            using var context = await _ctxWeb.CreateDbContextAsync();


            var config = await context.MasterConfigs
                .FirstOrDefaultAsync(mc => mc.Key == Key);

            if (config != null)
            {
                config.Group = Group;
                context.MasterConfigs.Update(config);
                await context.SaveChangesAsync();
            }



        }

        public BatchGenerateOutput Checkbatchmonitoring()
        {
            using var context = _ctxStg.CreateDbContext();

            var result = context.BatchGenerateOutput
                .Select(x => new  BatchGenerateOutput
                { 
                ProcessId = x.ProcessId,
                ProcessRemarks = x.ProcessRemarks
                })
                .FirstOrDefault();

            return result;
        }


        public string Checkdatafilemonitoring(string filename)
        {
            using var context = _ctxWeb.CreateDbContext();

            var result = context.FileMonitoring
                .Where(x => x.fileRq == filename)
                .Select(x => x.consumeDT)
                .FirstOrDefault();

            return result?.ToString() ?? string.Empty;
        }

        public string CheckDataRespondate()
        {
            using var context = _ctxWeb.CreateDbContext();

            var result = context.FileMonitoring
                .Where(x => x.contentType != "OkFile" && x.requestID == "Batch (UDM)")
                .OrderByDescending(x => x.responseDT)
                .Select(x => x.responseDT)
                .FirstOrDefault();

            return result?.ToString() ?? string.Empty;
        }
        public string GetMasterTbaeldrf(string Actualcd)
        {
            using var context = _ctxStg.CreateDbContext();

            var result = context.MasterTable
           .Where(x => x.TableName == "Impact" && x.ActualCd == Actualcd)
           .Select(x => x.Cd)
           .FirstOrDefault();
            return result ?? string.Empty;
        }
        public string GetMasterstatus(string Actualcd)
        {

            using var context = _ctxStg.CreateDbContext();
        
            var result = context.MasterTable
                .Where(x => x.TableName == "File Status" && x.ActualCd == Actualcd)
                .Select(x => x.Cd)
                .FirstOrDefault();
            return result ?? string.Empty;
        }
        public string GetMasterfilestatus(string fileStatus)
        {



            using var context = _ctxStg.CreateDbContext();

           
            var result = context.MasterTable
                .Where(x =>x.TableName == "File Status Code" && x.ActualCd == fileStatus)
                .Select(x =>x.Cd)
                .FirstOrDefault();
            return result ?? string.Empty;
        }

    }
}
