
using log4net;
using MonitoringBatchService.Data;
using MonitoringBatchService.Models;
using System.Globalization;

namespace MonitoringBatchService.Services
{
    public class DatabaseMonitorService : BackgroundService
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(DatabaseMonitorService));
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly string? _delaytime;
        private readonly string? _timesendstatus;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1,1);


        public DatabaseMonitorService(IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _delaytime = configuration["ExeSettings:DelayTime"];
            _timesendstatus = configuration["ExeSettings:TimeSendStatus"];




        }
        public List<string> Getall()
        {
            return ["Data1", "Data2"];
        }

        // Metode publik untuk mengambil data
        public async Task<IEnumerable<BatchGenerateOutput>> GetRecordsAsync()
        {
            await _semaphore.WaitAsync();
            try
            {

              
                using (var scope = _scopeFactory.CreateScope())
                {
                 
                    var _call = scope.ServiceProvider.GetRequiredService<IDatabaseService>();
                    return await _call.GetRecordsAsync();
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
    
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _semaphore.WaitAsync(stoppingToken);

                   
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var _call = scope.ServiceProvider.GetRequiredService<IDatabaseService>();
                        IEnumerable<BatchGenerateOutput> records = await _call.GetRecordsAsync();
                        foreach (var record in records)
                        {
                            await ProcessRecordAsync(record);
                        }
                    }
                   
                }
                catch (Exception ex)
                {
                    _log.Error("Error occurred", ex);
                }
                finally
                {
                    _semaphore.Release();
                }

                int delay = Convert.ToInt32(_delaytime);
                await Task.Delay(delay, stoppingToken);
            }
        }

        public async Task ProcessRecordAsync(BatchGenerateOutput record)
        {
            var processActions = new Dictionary<int, Func<string, Task>>
            {
                { 1, HandleProcess1Async },
                { 2, HandleProcess2Async },
                { 3, HandleProcess3Async },
                { 4, HandleProcess4Async },
                { 0, HandleProcess0Async }
            };

            if (processActions.TryGetValue(record.ProcessId, out var action))
            {
                await action(record.ProcessRemarks ?? string.Empty);
            }
        }

        public  async  Task HandleProcess1Async(string remarks)
        {
            if (remarks == "OPEN")
            {


               await ProcessStatusGenerate();
            }
        }

        public async Task HandleProcess2Async(string remarks)
        {
            if (remarks == "OPEN")
            {
                
                await ProcessCopyFile();
               
            }
           
        }

        public async Task HandleProcess3Async(string remarks)
        {
            if (remarks == "OPEN")
            {
                await Processexeoutbond();
            }
            else if (remarks == "DONE")
            {
                await ProcessStatusDRF();
            }
        }

        public async Task HandleProcess4Async(string remarks)
        {
            if (remarks == "DONE")
            {
                await Processrespondate();
            }
        }

        public async Task HandleProcess0Async(string remarks)
        {
            if (remarks == "DONE")
            {
                await Processrespondatenol();
            }
        }

        public async Task ProcessStatusGenerate()
        {
            _log.Info($"process disinih generatejson");

            using (var scope = _scopeFactory.CreateScope())
            {

                var myScopedService = scope.ServiceProvider.GetRequiredService<IAutoGenerateJson>();

                await myScopedService.RunExe();
            }
            _log.Info($"process Selesai");
           

        }

        public async Task ProcessCopyFile()
        {
            _log.Info($"process disinih generatejson");

            using (var scope = _scopeFactory.CreateScope())
            {
                var myScopedService = scope.ServiceProvider.GetRequiredService<ICopyFileService>();

                await myScopedService.CopyExe();
            }
            _log.Info($"process Selesai");

        }
        public async Task Processexeoutbond()
        {
            _log.Info($"process disinih Kirim dengan Outbound");

            using (var scope = _scopeFactory.CreateScope())
            {
                var myScopedService = scope.ServiceProvider.GetRequiredService<IServiceOutbound>();

                await myScopedService.RunExe();
            }
            _log.Info($"process Selesai Kirim Inbound");

        }

        public async Task ProcessStatusDRF()
        {
     


            using (var scope = _scopeFactory.CreateScope())
            {
                var myScopedService = scope.ServiceProvider.GetRequiredService<IFileWatcherService>();

                await myScopedService.ExecuteAsync();
                
            }
          

        }

        //public async Task Processrespondate()
        //{


        //    using (var scope = _scopeFactory.CreateScope())
        //    {
        //        var _call = scope.ServiceProvider.GetRequiredService<IDatabaseService>();
        //        string responsedatestr = _call.CheckDataRespondate();
        //        if (string.IsNullOrEmpty(responsedatestr) || !DateTime.TryParse(responsedatestr, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime responsedate))
        //        {
        //            _log.Info("Data Respondt tidak valid atau null");
        //            return;
        //        }
        //        DateTime currentUtc = DateTime.UtcNow;
        //        TimeSpan timeDifference = currentUtc.Subtract(responsedate);
        //        //Time24 jam
        //        double timeset = Convert.ToDouble(_timesendstatus);
        //        //if (timeDifference.TotalHours >= timeset)
        //        if (timeDifference.TotalMinutes >= timeset)
        //            {
        //            _log.Info("Data sudah bisa di lanjutkan Update FlAG 0 KARNA SUDAH 24 JAM DATA DI TERIMA");
        //            await _call.UpdateRecordsAsyncruls(0, "OPEN");
        //        }
        //    }


        //}
        public async Task Processrespondate()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _call = scope.ServiceProvider.GetRequiredService<IDatabaseService>();
                string responedatestr = _call.CheckDataRespondate();

                if (string.IsNullOrEmpty(responedatestr) ||
                    !DateTime.TryParse(responedatestr, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime responseDate))
                {
                    _log.Info("Data Respondate tidak valid atau null");
                    return;
                }

                DateTime currentUtc = DateTime.Now;
                TimeSpan timeDifference = currentUtc - responseDate;
                double timeset = Convert.ToDouble(_timesendstatus);
                // Validasi lebih dari 1 menit
                if (timeDifference.TotalMinutes > timeset)
                {
                  
                    
                    
                    var record =  _call.Checkbatchmonitoring();
                
                   
                   
                        await _call.UpdateRecordsAsyncruls(1, "OPEN");
                        _log.Info("Data sudah bisa di lanjutkan Update FLAG 1 karena sudah bisa di lanjutkan Generate");
                   
                 
                  
                }
                else
                {
                    
                }
            }

           
        }

        // Proses jika sudah 0
        public async Task Processrespondatenol()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _call = scope.ServiceProvider.GetRequiredService<IDatabaseService>();
                string responedatestr = _call.CheckDataRespondate();

                if (string.IsNullOrEmpty(responedatestr) ||
                    !DateTime.TryParse(responedatestr, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime responseDate))
                {
                    _log.Info("Data Respondate tidak valid atau null");
                    return;
                }

                DateTime currentUtc = DateTime.Now;
                TimeSpan timeDifference = currentUtc - responseDate;
                double timeset = Convert.ToDouble(_timesendstatus);
                // Validasi lebih dari 1 menit
                if (timeDifference.TotalMinutes > timeset)
                {



                    var record = _call.Checkbatchmonitoring();

                
                        await _call.UpdateRecordsAsyncruls(1, "OPEN");
                        _log.Info("Data sudah bisa di lanjutkan Update FLAG 1 karena sudah bisa di lanjutkan Generate");
                   


                }
                else
                {
                   // _log.Info("Data belum lebih dari 24 Jam, proses ditunda.");
                }
            }


        }

    }
}
