using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using Dapper;
using log4net;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MonitoringBatchService.Data;
using MonitoringBatchService.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MonitoringBatchService.Services
{
    public interface IFileWatcherService
    {
         Task ExecuteAsync();
    }
    public class FileWatcherService : IFileWatcherService
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(FileWatcherService));
        private readonly IValidatejson _validjson;
        private readonly IDatabaseService _databaseService;
        private readonly string? _watchDirectory;
        private readonly string? _destinationDirectory;
        private readonly string? _keydec;
        private readonly IFileService _fileService;
        public FileWatcherService(IConfiguration configuration,IDatabaseService databaseService, IValidatejson validjson, IFileService fileService)
        {
            _databaseService = databaseService;
            _watchDirectory = configuration["Settings:WatchDirectorry"];
            _destinationDirectory = configuration["Settings:DestinationDirectory"];
            _validjson = validjson;
            _keydec = configuration["Settings:KeyDec"];
            _fileService = fileService;
        }
        public async Task ExecuteAsync()
        {
            try
            {
              
                var files = _fileService.GetFiles(_watchDirectory ?? string.Empty, "*.gpg");
                var filesAll = _fileService.GetFiles(_watchDirectory ?? string.Empty, "*");
           
                var nameFile = files.FirstOrDefault();
               
                string strSongName = Path.GetFileName(nameFile ?? string.Empty);
              
                // Operasi sinkron di dalam Task.Run
                var renameDrf = _validjson.UpdateFileNameDRF(strSongName ?? string.Empty);
               
                var checkCount = _databaseService.CheckCountDRF(renameDrf);
                int hasil = files.Length;

                if (checkCount == hasil.ToString())
                {
                    _log.Info($"Hasil Copya {checkCount}");
                    await OnFileCreated2(filesAll);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error in ExecuteAsync", ex);
            }
        }

        public async Task OnFileCreated2(string[] files)
        {
            try
            {
                string targetFolder = null;

                var datePart = DateTime.Now.ToString("yyyyMMddHHmmss");
                foreach (string file in files)
                {
                    var fileName = Path.GetFileName(file);

                    targetFolder = Path.Combine(_destinationDirectory ?? string.Empty, datePart);
                    if (!Directory.Exists(targetFolder))
                    {
                        Directory.CreateDirectory(targetFolder);
                    }

                    string targetPath = Path.Combine(targetFolder, fileName);
                    if (File.Exists(file))
                    {
                        File.Move(file, targetPath, true);
                    }
                }

                // Generate decrypt
                if (targetFolder != null)
                {
                    var filesGet = Directory.GetFiles(targetFolder, "*.gpg");
                    var nameFileGet = filesGet.FirstOrDefault();
                    string strSongName = Path.GetFileName(nameFileGet) ?? string.Empty;
                    string drfRenam = _validjson.UpdateFileNameDRF(strSongName) ?? string.Empty;
                    string logMessage = $"Update File Name DRF {drfRenam}";
                    _log.Info(logMessage);

                    await _databaseService.UpdateRecordsAsyncruls(4, "OPEN");
                    await _databaseService.UpdateRecordsAsyncruls(4, "IN PROGRESS");
                    await RunBefore(targetFolder, datePart);
                    _log.Info("File diterima sebelum jam 2 atau tanggal belum besok jalankan flow 4");
                    await _databaseService.UpdateRecordsAsyncruls(4, "DONE");
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error in OnFileCreated2", ex);
            }
        }
        public async Task RunBefore(string targetFolder, string datePart)
        {
            string passphrase = _keydec ?? string.Empty;

            // Siapkan perintah untuk file batch
            //codelama
            //string command = $"gpg.exe --pinentry-mode=loopback --batch --passphrase \"{passphrase}\" --decrypt-files *.gpg";
            //code baru decypt
            string command = $"gpg.exe --batch --passphrase \"{passphrase}\" --decrypt-files *.gpg";
            //string command = $"gpg --pinentry-mode loopback --no-tty --batch --yes --passphrase \"{passphrase}\" --decrypt-files {targetFolder}/*.gpg";


            // Tentukan jalur lengkap ke Dec.bat
            string batchFilePath = System.IO.Path.Combine(targetFolder, "Dec.bat");

            // Cek dan buat Dec.bat jika belum ada
            if (!System.IO.File.Exists(batchFilePath))
            {
                using (var writer = new System.IO.StreamWriter(batchFilePath))
                {
                   await writer.WriteLineAsync("@echo off");
                   await writer.WriteLineAsync(command);
                }
            }

            // Siapkan untuk menjalankan file batch
            var processStartInfo = new ProcessStartInfo
            {
                FileName = batchFilePath, // Jalankan file batch
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = targetFolder // Set working directory
            };

            using (var process = Process.Start(processStartInfo))
            {
                if (process == null)
                {
                    _log.Error("Gagal memulai proses Dec.bat.");
                    return;
                }

                await process.WaitForExitAsync();

              

                _log.Info("Proses Dec.bat selesai.");

               await  _validjson.Jsonlos(targetFolder, datePart);
              
            }
        }
      
        

    }
}
