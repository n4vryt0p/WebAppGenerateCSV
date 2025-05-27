using Microsoft.Data.SqlClient;
using MonitoringBatchService.Models;

namespace MonitoringBatchService.Data
{
    public interface IDatabaseService
    {
    
        Task<IEnumerable<BatchGenerateOutput>> GetRecordsAsync();
       
        Mastertabel GetAllMasterTabel(string Actualcd);
        Task UpdateRecordsAsync(int prosesid, int newstaus);
        Task UpdateRecordsAsyncruls(int ProcessId, string ProcessRemarks);
        Task UpdateDRFsuccess(string fileRq);
        Task<IEnumerable<Flow>> GetFlow(string flowKey);
        Task SendEmailWithResults(List<FileStatusModel> fileStatusList);
        Task UpdateFileMonitoring(string fileRq);
        Task UpdateFileMonitoringRespon(string fileRq, long fileSizeRs, string fileRs, string urlPathRs);
        Task UpdateFileMonitoringtriger(string fileRq);
        string CheckCountDRF(string fileRq);
        int FLOW1();
        int FLOW2();
        int FLOW3();
        int FLOW4();
        int FLOW5();
        int FLOW6();
        int FLOW7();
        int FLOW8();
        int FLOW9();
        Task Updateallflow(string Key, string Group);
        string Checkdatafilemonitoring(string filename);
        BatchGenerateOutput Checkbatchmonitoring();
        string CheckDataRespondate();
        string GetMasterTbaeldrf(string Actualcd);
        string GetMasterstatus(string Actualcd);
        string GetMasterfilestatus(string fileStatus);
        Task GetLogs(string response);
    }
}
