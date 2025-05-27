using System.Globalization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MonitoringBatchService.Models;

namespace MonitoringBatchService.Services
{

    public interface IChecking
    {
        Task<bool> InsertDrf(string? filePath, string? jsonfile = null);
        Task<string> Status(string fileStatusCode);
        Task<int> Impact(string fileStatusCode);
    }


    public class ServiceinsertDrf : IChecking
    {

        private readonly IDbContextFactory<GfccstgDbContext> _ctx;

        public ServiceinsertDrf(IDbContextFactory<GfccstgDbContext> ctx)
        {

            _ctx = ctx;
        }

        public async Task<int> Impact(string fileStatusCode)
        {
            // Pastikan fileStatusCode tidak null atau kosong
            if (string.IsNullOrEmpty(fileStatusCode))
            {
                return 0;
            }

            using var context = await _ctx.CreateDbContextAsync();

            // Pastikan context dan MasterTable tidak null
            if (context?.MasterTable == null)
            {
                return 0;
            }

            // Query dengan penanganan null untuk e.ActualCd dan e.TableName
            var impact = await context.MasterTable
                .AsNoTracking()
                .Where(e => e != null && e.ActualCd == fileStatusCode && e.TableName == "Impact")
                .Select(r => r.Cd)
                .FirstOrDefaultAsync();

            // Pastikan impact tidak null atau kosong sebelum konversi
            if (string.IsNullOrEmpty(impact))
            {
                return 0;
            }

            // Coba konversi ke integer
            if (int.TryParse(impact, out int result))
            {
                return result;
            }

            // Jika konversi gagal, kembalikan 0
            return 0;
        }
        public async Task<string> Status(string fileStatusCode)
        {
            // Pastikan fileStatusCode tidak null atau kosong
            if (string.IsNullOrEmpty(fileStatusCode))
            {
                return "No impact";
            }

            using var context = await _ctx.CreateDbContextAsync();

            // Pastikan context dan MasterTable tidak null
            if (context?.MasterTable == null)
            {
                return "No impact";
            }

            // Query dengan penanganan null untuk e.ActualCd dan e.TableName
            var status = await context.MasterTable
                .AsNoTracking()
                .Where(e => e != null && e.ActualCd == fileStatusCode && e.TableName == "File Status")
                .Select(r => r.Cd)
                .FirstOrDefaultAsync();

            // Jika status null atau kosong, kembalikan "No impact"
            return string.IsNullOrEmpty(status) ? "No impact" : status;
        }

        public async Task<bool> InsertDrf(string filePath, string? jsonfile = null)
        {
             var fileName = Path.GetFileName(filePath);
                var dataFile = new Root();
                if (string.IsNullOrEmpty(jsonfile))
                {
                    using var isiFile = File.OpenRead(filePath);
                    dataFile = await JsonSerializer.DeserializeAsync<Root>(isiFile); 
                }
                else
                    dataFile = JsonSerializer.Deserialize<Root>(jsonfile);

                if (dataFile == null)
                    return false;
                var datee = DateTime.ParseExact(dataFile.DATE ?? string.Empty, "yyyyMMdd", CultureInfo.InvariantCulture).Date;
                var now = DateTime.Now;
                var checkImpact = await Impact(dataFile.FILE_STATUS ?? string.Empty);
                var checkStatus = await Status(dataFile.FILE_STATUS ?? string.Empty);

                var data = new DrfReportHeader
                {
                    CREATE_BY = "SYSTEM",
                    CREATE_DATE = now,
                    DRF_DATE = new DateOnly(datee.Year, datee.Month, datee.Day),
                    DRF_FILENAME = fileName,
                    DRF_FILEPATH = filePath,
                    ORGUNIT = dataFile.ORGUNIT,
                    FLOW_FILENAME = dataFile.FILENAME,
                    FILE_STATUS = checkStatus,
                    FILE_STATUS_CODE = dataFile.FILE_STATUS,
                    ERROR_STATUS = 0,
                    IMPACT = checkImpact,
                    NB_SRC_LINES = Convert.ToInt32(dataFile.NB_SRC_LINESX?.ToString() ?? 0),
                    NB_LINE_IN_WARNING = Convert.ToInt32(dataFile.NB_LINE_IN_WARNINGX?.ToString() ?? 0),
                    NB_LINE_IN_REJECT = Convert.ToInt32(dataFile.NB_LINE_IN_REJECTX?.ToString() ?? 0),
                    DRF_REPORT_DETAILS = []
                };
                if (dataFile.data?.Count > 0 && checkStatus == "KO")
                    foreach (var item in dataFile.data)
                    {

                        item.CREATE_BY = "SYSTEM";
                        item.CREATE_DATE = now;
                        item.ERROR_STATUS_RECORD = 0;
                        item.LINE_ID = Convert.ToInt32(item.LINE_ID_Extra ?? string.Empty.ToString());
                        data.DRF_REPORT_DETAILS.Add(item);
                    }
                using var context = await _ctx.CreateDbContextAsync();
                await context.DRF_REPORT_HEADER.AddAsync(data);
                await context.SaveChangesAsync();
                return true;
            
           
        }
    }
}
