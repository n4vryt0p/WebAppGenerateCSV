using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MonitoringBatchService.Models
{
    public class DrfReportDetails
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [JsonPropertyName("LINE_ID")]
        [NotMapped]
        public dynamic? LINE_ID_Extra { get; set; }
        [JsonPropertyName("LINE_IDX")]
        public int LINE_ID { get; set; }

        [JsonPropertyName("ERROR_TYPE")]
        public string? DRF_ERROR_TYPE { get; set; }
        [JsonPropertyName("ORIGIN")]
        public string? ORIGIN { get; set; }
        [JsonPropertyName("ERROR_MESSAGE")]
        public string? LINE_ERROR_MESSAGE { get; set; }
        [JsonPropertyName("FIELD_NAME")]
        public string? FIELD_NAME { get; set; }
        [JsonPropertyName("FIELD_VALUE")]
        public string? FIELD_VALUE { get; set; }
        [JsonPropertyName("ERROR_CODE")]
        public string? ERROR_CODE { get; set; }
        [JsonPropertyName("SOURCE_REF_IDS")]
        public string? SOURCE_REF_IDS { get; set; }
        public int ERROR_STATUS_RECORD { get; set; }

        public DateTime CREATE_DATE { get; set; }
        public string? CREATE_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public string? MODIFIED_BY { get; set; }

        public int ID_HEADER { get; set; }
        public virtual DrfReportHeader? DRF_REPORT_HEADER { get; set; }
    }
}
