namespace MonitoringBatchService.Models
{
    public class WebApi_logs
    {
        public int Id { get; set; }

        public string? requestID { get; set; }

        public string? txnLogID { get; set; }

        public string? clientID { get; set; }

        public string? clientIP { get; set; }

        public string? nodeIP { get; set; }

        public string? contentType { get; set; }

        public string? method { get; set; }

        public string? urlPath { get; set; }

        public DateTime? trxRq_DT { get; set; }

        public int? payloadSizeRq { get; set; }

        public string? payloadRq { get; set; }

        public DateTime? trxRs_DT { get; set; }

        public int? processingTime { get; set; }

        public string? httpStatus_CD { get; set; }

        public int? payloadSizeRs { get; set; }

        public string? PayloadRs { get; set; }

        public bool? isInbound { get; set; }
    }
}
