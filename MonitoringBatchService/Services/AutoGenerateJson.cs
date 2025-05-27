using System.Diagnostics;
using System.Text.RegularExpressions;
using log4net;
using MonitoringBatchService.Data;
using MonitoringBatchService.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MonitoringBatchService.Services
{
    public interface IAutoGenerateJson
    {
        Task RunExe();
    }
    public class AutoGenerateJson : IAutoGenerateJson
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(AutoGenerateJson));
        private readonly IDatabaseService _databaseService;
        private readonly IProcessService _processService;
        private readonly string? _exepath;
        private readonly IValidatejson _validjson;
        private readonly string? gfccflowa = "GfccFlow";
        private readonly string? gfccflowb = "GfccFlow~";

        public AutoGenerateJson(IConfiguration configuration, IDatabaseService databaseService, IValidatejson validjson, IProcessService processService)
        {
            _databaseService = databaseService;
            _processService = processService;
            _exepath = configuration["ExeSettings:ExePath"];
            _validjson = validjson;
        }

        public async Task RunExe()
        {
            await _databaseService.UpdateRecordsAsyncruls(1, "IN PROGRESS");
            _log.Info("---Start execution of Exe----");

            // Array untuk menyimpan hasil flow
            int[] flows = new int[9];
            flows[0] = _databaseService.FLOW1();
            flows[1] = _databaseService.FLOW2();
            flows[2] = _databaseService.FLOW3();
            flows[3] = _databaseService.FLOW4();
            flows[4] = _databaseService.FLOW5();
            flows[5] = _databaseService.FLOW6();
            flows[6] = _databaseService.FLOW7();
            flows[7] = _databaseService.FLOW8();
            flows[8] = _databaseService.FLOW9();


            //Penambahan logic operation
            bool hasOtherFlows = flows.Take(8).Any(f => f > 0);

            // Update flow berdasarkan hasil
            for (int i = 0; i < flows.Length; i++)
            {
                if (flows[i] <= 0 && i != 8)
                    continue;

                string flowName = $"Flow_{i + 1}";
                
                string group = string.Empty;
                if(i == 8)
                {
                    if (hasOtherFlows)
                    {
                        group = gfccflowa ?? string.Empty;
                    }
                    else
                    {
                        group = gfccflowb ?? string.Empty;
                    }
                
                }
                else
                {
                    group = flows[i] > 0 ? gfccflowa ?? string.Empty : gfccflowb ?? string.Empty;

                }
                await _databaseService.Updateallflow(flowName, group ?? string.Empty);


                //string group = flows[i] > 0 ? gfccflowa ?? string.Empty : gfccflowb ?? string.Empty;
                //await _databaseService.Updateallflow(flowName, group ?? string.Empty);
            }
            //Cek lqgii
           

            // Cek apakah ada flow yang > 0
            bool hasFlow = flows.Any(flow => flow > 0);

            // Proses eksekusi jika ada flow yang > 0
            if (hasFlow)
            {
                //path
                //var proses = _processService.RunProcessAsync(_exepath ?? string.Empty);

                var isDone = BatchingFileWatcher.Program.generateEncryptFiles();
                if (isDone)
                {

                    var dataupdate = _databaseService.UpdateRecordsAsyncruls(1, "DONE");
                    var validjson = _validjson.SendEmailWithJson();
                    _log.Info("Process execution completed.");
                    var updatetwo = _databaseService.UpdateRecordsAsyncruls(2, "OPEN");
                }
                else
                {
                    _log.Error("Process execution error.");
                }
            }
            else
            {
                var updateeerordone =  _databaseService.UpdateRecordsAsyncruls(1, "DONE");
                var updateeeroropen =  _databaseService.UpdateRecordsAsyncruls(0, "OPEN");
            }
        }
    }
}
