using log4net;
using MonitoringBatchService.Data;
using System.Diagnostics;

namespace MonitoringBatchService.Services
{
    public interface IServiceOutbound
    {
        Task RunExe();
    }

    public class ServiceOutbound : IServiceOutbound
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ServiceOutbound));
        private readonly IDatabaseService _databaseService;
        private readonly IValidatejson _validjson;
        private readonly IFileService _fileService;
        private readonly IProcessService _processService;
        private readonly string? _destinationFolder;
        private readonly string? _destinationFolderout;
        private readonly string? _serviceoutbound;

        public ServiceOutbound(
            IConfiguration configuration,
            IDatabaseService databaseService,
            IValidatejson validjson,
            IFileService fileService,
            IProcessService processService)
        {
            _databaseService = databaseService;
            _validjson = validjson;
            _fileService = fileService;
            _processService = processService;
            _serviceoutbound = configuration["ExeSettings:serviceoutbound"];
            _destinationFolder = configuration["ExeSettings:ExePathdestinationFolder"];
            _destinationFolderout = configuration["ExeSettings:ExePathdestinationFolderout"];
        }

        public async Task RunExe()
        {
            _log.Info($"-- Proses Jalan Outbound ---");
            _log.Info($"ini Exenya {_serviceoutbound}");
            var open = UpdateStatus(3, "IN PROGRESS");
            //var proses = ProcessOkFiles();
            //var prosesgpg = ProcessGpgFiles();
           
            var running =  RunExternalProcess();

            var sendemail = SendEmail();
            var done = UpdateStatus(3, "DONE");
            _log.Info($"-- Proses Selesai Outbound ---");
        }

        private async Task UpdateStatus(int id, string status)
        {
            await _databaseService.UpdateRecordsAsyncruls(id, status);
        }

        public async Task ProcessOkFiles()
        {
            var okFiles = _fileService.GetFilesa(_destinationFolder ?? string.Empty, "*.ok");
            if (okFiles.Length == 0)
            {
                _log.Info("No .ok files found.");
                return;
            }

            foreach (var file in okFiles)
            {
                string filenameok = file.Name.Replace(".ok", "");
                await _databaseService.UpdateFileMonitoringtriger(filenameok);
                string sourcePath = Path.Combine(_destinationFolder ?? "", filenameok);
                string targetPath = Path.Combine(_destinationFolderout ?? "", filenameok);

                if (_fileService.FileExists(sourcePath))
                {
                    _fileService.CopyFile(sourcePath, targetPath, overwrite: true);
                }
            }
            _log.Info("Processed .ok files.");
        }

        public async Task ProcessGpgFiles()
        {
            var gpgFiles = _fileService.GetFilesa(_destinationFolder ?? string.Empty, "*.json.gpg");
            if (gpgFiles.Length == 0)
            {
                _log.Info("No .json.gpg files found.");
                return;
            }

            foreach (var file in gpgFiles)
            {
                string filename = file.Name.Replace(".gpg", "");
                await _databaseService.UpdateFileMonitoringtriger(filename);
                string sourcePath = Path.Combine(_destinationFolder ?? "", filename);
                string targetPath = Path.Combine(_destinationFolderout ?? "", filename);

                if (_fileService.FileExists(sourcePath))
                {
                    _fileService.CopyFile(sourcePath, targetPath, overwrite: true);
                }
            }
            _log.Info("Processed .json.gpg files.");
        }

        public async Task SendEmail()
        {
            await _validjson.SendEmailWithConsume();
        }

        public async Task RunExternalProcess()
        {
            await _processService.RunProcessAsync(_serviceoutbound ?? string.Empty);
            _log.Info($"Informasi Service Outbound {_serviceoutbound}");
            _log.Info($"Proses berjalann");
        }
    }
}