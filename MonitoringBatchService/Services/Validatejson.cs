using Dapper;
using log4net;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using MonitoringBatchService.Data;
using MonitoringBatchService.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using ZstdSharp.Unsafe;


namespace MonitoringBatchService.Services
{
    public interface IValidatejson
    {
        Task Jsonlos(string targetFolder, string datePart);
        Task jsonlosafter(string targetFolder, string datePart);
        string UpdateFileNameDRF(string originalFileName);
        string[] ExtractTimestamps(string originalFileName);
        Task SendEmailWithConsume();
        Task SendEmailWithJson();
        Task SendEmailWithTrigger();
        Task UpdatedbFileMonitoring(List<FileStatusModel> fileStatusList, string datePart);

    }
    public class Validatejson : IValidatejson
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Validatejson));
        private readonly IChecking _Chek;
        private readonly IDatabaseService _databaseService;
        private readonly string? _Emailsend;
        private readonly string? _Emailsendadmin;
        private readonly string? _baseAddress;
        private readonly string? _baseapi;
        private const string _strip = "_";
        private const string _json = ".json";
        private const string _gpg = ".gpg";
        private const string _DRF = "DRF_";
        private const string _appjson = "application/json";
        public const string _from = "do-not-reply@aml.axa-mandiri.co.id";
        private const string HTMLall = @"<!DOCTYPE html>
                                        <html lang='id'>
                                        <head><meta charset='UTF-8'><title>Email Report</title></head>
                                          <body>
                                           <p>Hi AMFS - AML Team,</p>
                                           <p>Please find the result of integration below:</p>
                                            <table style='width: 100%; border-collapse: collapse;'>
                                            <tr><th style='border: 1px solid #ddd; padding: 8px; text-align: left;'>File Name</th>
                                           <th style='border: 1px solid #ddd; padding: 8px; text-align: left;'>Received Date</th>
                                            <th style='border: 1px solid #ddd; padding: 8px; text-align: left;'>File Status Code</th>
                                            <th style='border: 1px solid #ddd; padding: 8px; text-align: left;'>Status</th></tr>
                                        ";
        private const string HTMLend = @"
                                    </table>
                                   <p>If you need the detail, please check the file in the UDM directly.</p>
                                   <p>Thank you.</p>
                                   </body></html>
                                    ";

        private const string HTMLTemplateEmail = @"
                                         <!DOCTYPE html>
                                           <html lang='id'>
                                          <head><meta charset='UTF-8'><title>Email Report</title></head>
                                           <body>
                                            <p>Hi AMFS - AML Team,</p>";
        private const string HTMLTemplateEmailEnd = @"
                                            <p>Thank you.</p>
                                                  </body></html>";
     
        private readonly IFileService _fileService;
        public Validatejson(IConfiguration configuration, IDatabaseService databaseService, IChecking chek, IFileService fileService)
        {

          
            _Emailsend = configuration["Settings:Emailsend"];
            _Emailsendadmin = configuration["Settings:Emailsendadmin"];
            _baseAddress = configuration["Settings:BaseAddress"];

            _baseapi = configuration["Settings:Baseapi"];
            _Chek = chek;
            _databaseService = databaseService;
            _fileService = fileService;


        }


        public async Task Jsonlos(string targetFolder, string datePart)
        {
            var jsonFiles = _fileService.GetFiles(targetFolder, "*");

            var fileStatusList = new List<FileStatusModel>();
            foreach (var file in jsonFiles)
            {
                try
                {
                    if (file.Contains(_json))
                    {
                        var fileName = Path.GetFileName(file);
                        var fileNameSplit = fileName.Split(".");
                        var gabungan = targetFolder + "\\" + fileName;
                        var jsonString = "json";
                        var fullPath = Path.Combine(targetFolder, fileName);

                        if (!_fileService.FileExists(fullPath))
                        {
                            string logMessage = $"File not found: {fullPath}";
                            _log.Info(logMessage);
                            continue;
                        }

                        if (fileNameSplit.Length > 0 && fileNameSplit[fileNameSplit.Length - 1].StartsWith(jsonString, StringComparison.OrdinalIgnoreCase))
                        {
                            var jsonContent = await _fileService.ReadAllTextAsync(fullPath);
                            var jsonObject = JObject.Parse(jsonContent);

                            string fileStatus = jsonObject["FILE_STATUS"]?.ToString() ?? "Unknown";
                            _log.Info($"ini {fileStatus}");
                            var okrespon = _databaseService.GetAllMasterTabel(fileStatus);
                            _log.Info($" ini ok respon{okrespon}");
                            _log.Info($" ini ok respon CD{okrespon.Cd}");

                            var masterResponse = _databaseService.GetAllMasterTabel(fileStatus);
                            string logMessage = $"Get All Master get{masterResponse}";
                            _log.Info(logMessage);

                            fileStatusList.Add(new FileStatusModel
                            {
                                FileName = fileName,
                                FileStatus = fileStatus,
                                ReceivedDate = datePart,
                                Status = okrespon.Cd,
                                PatchRs = gabungan
                            });

                            // Insert
                            var isertdrf = await _Chek.InsertDrf(file);
                            if (isertdrf)
                            {
                                _log.Info($"Insert Drf Berhasil");
                            }
                            else
                            {
                                _log.Info($"Insert Drf Tidak Berhasil");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string logMessage = $"Error processing file {file}";
                    _log.Error($"{logMessage}: {ex.Message}");
                }
            }

            UpdateErrorDb(fileStatusList);
            await UpdatedbFileMonitoring(fileStatusList, datePart);
            await SendEmailWithResults(fileStatusList, datePart);
        }

        public async Task jsonlosafter(string targetFolder, string datePart)
        {

   
            var jsonFiles = _fileService.GetFiles(targetFolder, "*");


            var fileStatusList = new List<FileStatusModel>();
            foreach (var file in jsonFiles)
            {
                try
                {
                    if (file.Contains(_json))
                    {
                        var fileName = Path.GetFileName(file);
                        var fileNameSplit = fileName.Split(".");
                        var jsonString = "json";
                        if (fileNameSplit.Length > 0 && fileNameSplit[fileNameSplit.Length - 1].StartsWith(jsonString, StringComparison.OrdinalIgnoreCase))
                        {
                            string jsonContent = await _fileService.ReadAllTextAsync(file);
                          
                            var jsonObject = JObject.Parse(jsonContent);

                            string fileStatus = jsonObject["FILE_STATUS"]?.ToString() ?? "Unknown";
                            var okrespon = _databaseService.GetAllMasterTabel(fileStatus);
                            fileStatusList.Add(new FileStatusModel
                            {
                                FileName = Path.GetFileName(file),
                                FileStatus = fileStatus,
                                ReceivedDate = datePart,
                                Status = okrespon.Cd
                            });

                        }
                    }

                }
                catch (Exception ex)
                {
                    _log.Error(ex);
                }

            }


            UpdateErrorDb(fileStatusList);
            await SendEmailWithResultsafter(fileStatusList, datePart);

          
            await UpdatedbFileMonitoring(fileStatusList, datePart);



        }

        public void UpdateErrorDb(List<FileStatusModel> fileStatusList)
        {
            try
            {
              bool hasKO = false; // Flag untuk menandai apakah ada status KO

                    foreach (var fileStatus in fileStatusList)
                    {
                        // Query untuk memeriksa status dari database
                     
                        var actualCD = _databaseService.GetMasterfilestatus(fileStatus.FileStatus ?? string.Empty);
                        if (actualCD != null)
                        {
                            // Jika statusnya KO, set hasKO menjadi true dan keluar dari loop
                            if (actualCD.Equals("KO", StringComparison.OrdinalIgnoreCase))
                            {
                                hasKO = true; // Tandai jika ada KO

                                break; // Keluar dari loop jika ditemukan KO
                            }
                            else if (actualCD.Equals("OK", StringComparison.OrdinalIgnoreCase))
                            {
                                hasKO = false; // Tandai jika ada KO



                            }
                        }
                    }


                    if (hasKO)
                    {

                        return;
                    }


                    _log.Info("Update Database Berhasil: GenerateBatch Dilanjutkan.");
                
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }
     
        public async Task UpdatedbFileMonitoring(List<FileStatusModel> fileStatusList, string datePart)
        {

            foreach (var fileStatus in fileStatusList)
            {

                string originalFileName = fileStatus.FileName ?? string.Empty;


                string updatedFileName = UpdateFileNameDRF(originalFileName);
                string pathRs = fileStatus.PatchRs ?? string.Empty;
                long sizefile = GetFileSize(pathRs);

                var updatefileresponse = _databaseService.UpdateFileMonitoringRespon(updatedFileName, sizefile, originalFileName, pathRs);

                string logMessage = string.Format($"Update File Monitoring berhasil {updatefileresponse}");
                _log.Info(logMessage);



            }


        }
        public static long GetFileSize(string fileName)
        {
            // Periksa apakah file ada
            if (File.Exists(fileName))
            {
                FileInfo fileInfo = new FileInfo(fileName);
                return fileInfo.Length; // Mengembalikan ukuran file dalam byte
            }
            return 0; // Jika file tidak ada, kembalikan 0
        }


        public string UpdateFileNameDRF(string originalFileName)
        {
            // Hilangkan "DRF_" dari awal
            if (originalFileName.StartsWith(_DRF))
            {
                originalFileName = originalFileName.Substring(4); // Menghapus 4 karakter pertama
            }

            // Temukan indeks "_" sebelum ".json" dan hapus bagian setelahnya
            int jsonIndex = originalFileName.LastIndexOf(_json);
            if (jsonIndex > 0)
            {
                // Ambil substring sampai "_" sebelum ".json" dan tambahkan kembali ".json"
                originalFileName = originalFileName.Substring(0, jsonIndex + 5);

            }


            return originalFileName;
        }
        public string[] ExtractTimestamps(string originalFileName)
        {
            // Initialize an array to hold the two timestamps
            // Initialize an array to hold the timestamps
            string[] timestamps = new string[2];

            // Check if originalFileName contains required substrings
            int jsonIndex = originalFileName.IndexOf(_json);
            int gpgIndex = originalFileName.IndexOf(_gpg);

            if (jsonIndex == -1 || gpgIndex == -1)
            {
                throw new ArgumentException("Filename must contain both JSON and GPG markers.");
            }

            // Extract first timestamp
        
            int firstTimestampStart = originalFileName.LastIndexOf(_strip, jsonIndex) + 1;
            timestamps[0] = originalFileName.Substring(firstTimestampStart, jsonIndex - firstTimestampStart);


            // Extract second timestamp
            int secondTimestampStart = originalFileName.LastIndexOf(_strip, gpgIndex) + 1;
            timestamps[1] = originalFileName.Substring(secondTimestampStart, gpgIndex - secondTimestampStart);

            return timestamps;
        }

        public async Task SendEmailWithResults(List<FileStatusModel> fileStatusList, string datePart)
        {



            var EMAIL = _Emailsend;
            var strMsg = new StringBuilder();
            strMsg.Append(HTMLall);


            foreach (var fileStatus in fileStatusList)
            {
                string receivedDateString = fileStatus.ReceivedDate ?? string.Empty;
                DateTime receivedDate = DateTime.ParseExact(receivedDateString, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                string formattedDate = receivedDate.ToString("yyyy-MM-dd HH:mm:ss");
                strMsg.Append("<tr>");
                strMsg.Append($"<td style='border: 1px solid #ddd; padding: 8px;'>{fileStatus.FileName}</td>");
                strMsg.Append($"<td style='border: 1px solid #ddd; padding: 8px;'>{formattedDate}</td>");
                strMsg.Append($"<td style='border: 1px solid #ddd; padding: 8px;'>{fileStatus.FileStatus}</td>");
                strMsg.Append($"<td style='border: 1px solid #ddd; padding: 8px; {(fileStatus.Status == "KO" ? "color: red;" : "")}'>{fileStatus.Status}</td>");

                strMsg.Append("</tr>");
            }
            strMsg.Append(HTMLend);


            var bodyEmail = strMsg.ToString() ?? string.Empty;
            string subject = "[AML Notification] DRF Integration Report";
            await SendAllMail(EMAIL ?? string.Empty, bodyEmail, subject);
        }

        //diatas 2PM
        public async Task SendEmailWithResultsafter(List<FileStatusModel> fileStatusList, string datePart)
        {



            var EMAIL = _Emailsend;
            var strMsg = new StringBuilder();
            strMsg.Append(HTMLall);
            foreach (var fileStatus in fileStatusList)
            {
                string receivedDateString = fileStatus.ReceivedDate ?? string.Empty;
                DateTime receivedDate = DateTime.ParseExact(receivedDateString, "yyyyddMMHHmmss", CultureInfo.InvariantCulture);
                string formattedDate = receivedDate.ToString("yyyy-MM-dd HH:mm:ss");

                strMsg.Append("<tr>");
                strMsg.Append($"<td style='border: 1px solid #ddd; padding: 8px;'>{fileStatus.FileName}</td>");
                strMsg.Append($"<td style='border: 1px solid #ddd; padding: 8px;'>{formattedDate}</td>");
                strMsg.Append($"<td style='border: 1px solid #ddd; padding: 8px;'>{fileStatus.FileStatus}</td>");
                strMsg.Append($"<td style='border: 1px solid #ddd; padding: 8px; {(fileStatus.Status == "KO" ? "color: red;" : "")}'>{fileStatus.Status}</td>");
                strMsg.Append("</tr>");
            }

            strMsg.Append(HTMLend);

            var bodyEmail = strMsg.ToString() ?? string.Empty;

            string subject = "[AML Notification] DRF Integration Report – Warning Receipt Time";
            await SendAllMail(EMAIL ?? string.Empty, bodyEmail, subject);
        }

        //Send Email 3 Consum
        public async Task SendEmailWithConsume()
        {



            var EMAIL = _Emailsendadmin;
            var datetime = DateTime.Now;
            var strMsg = new StringBuilder();
            strMsg.Append(HTMLTemplateEmail);
            strMsg.Append($"<p>This is an automatic email generated from system to inform you that delta file you put on UDM at <b> {datetime} are already consumed. </b> </p> ");
            strMsg.Append(HTMLTemplateEmailEnd);

            var bodyEmail = strMsg.ToString() ?? string.Empty;

            string subject = "[AML Notification] File Consumed";
            await SendAllMail(EMAIL ?? string.Empty, bodyEmail, subject);
        }
        public async Task SendEmailWithJson()
        {


            var EMAIL = _Emailsend;
            var datetime = DateTime.Now;
            var strMsg = new StringBuilder();
            strMsg.Append(HTMLTemplateEmail);
            strMsg.Append($"<p>This is an automatic email generated from system to inform you JSON File for daily delta  <b> {datetime} are already created. </b></p> ");
            strMsg.Append(HTMLTemplateEmailEnd);

            var bodyEmail = strMsg.ToString() ?? string.Empty;
            string subject = "[AML Notification] JSON Generation";
            await SendAllMail(EMAIL ?? string.Empty, bodyEmail, subject);
        }

        public async Task SendEmailWithTrigger()
        {

            var EMAIL = _Emailsendadmin;
            var datetime = DateTime.Now;
            var strMsg = new StringBuilder();
            strMsg.Append(HTMLTemplateEmail);
            strMsg.Append($"<p>This is an automatic email generated from system to inform you JSON File encrypted in UDM and The Job For Send data is Triggered on   <b>  {datetime} </b> </p> ");
            strMsg.Append(HTMLTemplateEmailEnd);

            var bodyEmail = strMsg.ToString() ?? string.Empty;
            string subject = "[AML Notification] File Triggered";
            await SendAllMail(EMAIL ?? string.Empty, bodyEmail, subject);
        }

        public async Task SendAllMail(string strEmail, string strMsg, string subject, string from = _from)
        {
            using (var client = new HttpClient())
            {
                var contentType = new MediaTypeWithQualityHeaderValue(_appjson ?? string.Empty);
                var baseAddress = _baseAddress;
                var api = _baseapi;
                client.BaseAddress = new Uri(baseAddress ?? string.Empty);
                client.DefaultRequestHeaders.Accept.Add(contentType);

                var data = new
                {
                    fromMail = from,
                    toMail = strEmail,
                    subjectMail = subject,
                    bodyMail = strMsg.ToString()
                };

                var contentData = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, _appjson);

                try
                {
                    var response = await client.PostAsync(api, contentData);

                    if (response.IsSuccessStatusCode)
                    {
                        var stringData = await response.Content.ReadAsStringAsync();
                    
                        string logMessage = ($"Result Ditemukan {stringData}");
                        _log.Info(logMessage);
                    }

                }
                catch (Exception e)
                {
                    _log.Error(e);
                }
            }
        }

    }
}
