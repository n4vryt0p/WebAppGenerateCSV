using System.Diagnostics;
using log4net;
using MonitoringBatchService.Models;
using MonitoringBatchService.Services;

namespace MonitoringBatchService.Data
{

    public interface IProcessService
    {
        Task RunProcessAsync(string fileName);
    }
    public class ProcessService : IProcessService
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(FileWatcherService));
        public async Task RunProcessAsync(string fileName)
        {
            _log.Info($"--Proses Bat Running --");
            var processInfo = new ProcessStartInfo
            {
                //FileName = "E:\\UDM\\bin\\Netreveal_PROD_AMFS.bat",
               FileName = fileName,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(fileName)
            };

            _log.Info($"filename {fileName}");
            _log.Info(processInfo);

            using var process = new Process { StartInfo = processInfo };
            process.Start();
            await Task.Delay(6000); // Simulasi proses yang berjalan
            process.WaitForExit();
            _log.Info($"--Proses Bat Selesai --");
        }
    }
}
