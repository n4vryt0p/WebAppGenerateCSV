namespace MonitoringBatchService.Models
{
    public class FileMonitoring
    {
        public int Id { get; set; }

        public string? requestID { get; set; }

        public string? txnLogID { get; set; }

        public string? clientID { get; set; }

        public string? clientIP { get; set; }

        public string? nodeIP { get; set; }

        public string? contentType { get; set; }

        public string? urlPath { get; set; }

        public DateTime? trxRq_DT { get; set; }

        public Single? fileSizeRq { get; set; }

        public string? fileRq { get; set; }

        public DateTime? generateDT { get; set; }

        public DateTime? triggerDT { get; set; }

        public DateTime? consumeDT { get; set; }

        public DateTime? responseDT { get; set; }

        public Single? fileSizeRs { get; set; }

        public string? fileRs { get; set; }

        public string? statusCode { get; set; }

        public bool? isInbound { get; set; }

        public int? CountFile { get; set; }

        public string? urlPathRs { get; set; }

    }
}
