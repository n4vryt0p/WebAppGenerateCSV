
using System.Diagnostics;
using log4net;
using MonitoringBatchService.Data;
using MonitoringBatchService.Models;

namespace MonitoringBatchService.Services
{
    public interface ICopyFileService
    {
        Task CopyExe();
    }
    public class CopyFileService : ICopyFileService
    {

        private static readonly ILog _log = LogManager.GetLogger(typeof(CopyFileService));
        private readonly IDatabaseService _databaseService;
        private readonly IFileService _fileService;
        private readonly string? _sourceFolder;
        private readonly string? _destinationFolder;
        private readonly IValidatejson _validjson;
        public CopyFileService(IConfiguration configuration, IDatabaseService databaseService ,IValidatejson validjson, IFileService fileService)
        {
            _databaseService = databaseService;
            _fileService = fileService;
            _sourceFolder = configuration["ExeSettings:ExePathsourceFolder"];
            _destinationFolder = configuration["ExeSettings:ExePathdestinationFolder"];
            _validjson = validjson;

        }


        public async Task CopyExe()
        {
           
           
        

            string sourceFolder = _sourceFolder ?? string.Empty;
            string destinationFolder = _destinationFolder ?? string.Empty;


            //Tambahakn Logic Disini

            var directoriesa = _fileService.GetDirectories(sourceFolder, "*", SearchOption.TopDirectoryOnly);
            var latestFoldera = directoriesa.OrderByDescending(d => d.LastWriteTime).FirstOrDefault();
            var gpgFilesa = _fileService.GetFiles(latestFoldera.FullName, "*.ok", SearchOption.AllDirectories);

            if(gpgFilesa.Length >0 )
            {
                await _databaseService.UpdateRecordsAsyncruls(2, "IN PROGRESS");
                _log.Info($"---Start CopyFileTo Inbound Exe----");
                //bakal lanju lagi ketika dia ada ok filenya
                



                if (_fileService.DirectoryExists(sourceFolder))
                {
                    if (!_fileService.DirectoryExists(destinationFolder))
                    {
                        _fileService.CreateDirectory(destinationFolder);
                    }

                    var directories = _fileService.GetDirectories(sourceFolder, "*", SearchOption.TopDirectoryOnly);
                    var latestFolder = directories.OrderByDescending(d => d.LastWriteTime).FirstOrDefault();

                    if (latestFolder != null)
                    {
                        var gpgFiles = _fileService.GetFiles(latestFolder.FullName, "*.json.gpg", SearchOption.AllDirectories);
                        var okFiles = _fileService.GetFiles(latestFolder.FullName, "*.ok", SearchOption.AllDirectories);

                        foreach (var file in gpgFiles.Concat(okFiles))
                        {
                            var filename = file.Name.Replace(".gpg", "");
                            await _databaseService.UpdateFileMonitoring(filename);

                            string destFilePath = Path.Combine(destinationFolder, file.Name);

                            if (!_fileService.FileExists(destFilePath) || file.LastWriteTime > _fileService.GetLastWriteTime(destFilePath))
                            {
                                _fileService.CopyFile(file.FullName, destFilePath, true);
                            }
                        }
                        //Akan melakukan setelah proses copy selesai    
                        await _databaseService.UpdateRecordsAsyncruls(2, "DONE");


                        _log.Info("Copying completed.");
                        await updaterulseopen3();
                        await _validjson.SendEmailWithTrigger();
                    }
                    else
                    {
                        _log.Info("No subfolders found in the source folder.");
                    }
                }
                else
                {
                    _log.Info("Source folder does not exist.");
                }
            }
          

            _log.Info($"---End CopyFileTo Inbound Exe----");
        }

        public async Task updaterulseopen3()
        {
            await _databaseService.UpdateRecordsAsyncruls(3, "OPEN");
        }
    }
}
