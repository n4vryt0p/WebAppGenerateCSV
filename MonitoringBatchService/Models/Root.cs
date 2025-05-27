using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MonitoringBatchService.Models
{
    public class Datum
    {
        [JsonPropertyName("LINE_ID")]
        public int? LINE_ID { get; set; }

        [JsonPropertyName("ERROR_TYPE")]
        public string? ERROR_TYPE { get; set; }

        [JsonPropertyName("ORIGIN")]
        public string? ORIGIN { get; set; }

        [JsonPropertyName("ERROR_MESSAGE")]
        public string? ERROR_MESSAGE { get; set; }

        [JsonPropertyName("FIELD_VALUE")]
        public string? FIELD_VALUE { get; set; }

        [JsonPropertyName("SOURCE_REF_IDS")]
        public string? SOURCE_REF_IDS { get; set; }

        [JsonPropertyName("FIELD_NAME")]
        public string? FIELD_NAME { get; set; }

        [JsonPropertyName("ERROR_CODE")]
        public string? ERROR_CODE { get; set; }
    }

    public class Root
    {
        [JsonPropertyName("ORGUNIT")]
        public string? ORGUNIT { get; set; }

        [JsonPropertyName("DATE")]
        public string? DATE { get; set; }

        [JsonPropertyName("FILENAME")]
        public string? FILENAME { get; set; }

        [JsonPropertyName("FILE_STATUS")]
        public string? FILE_STATUS { get; set; }

        [JsonPropertyName("NB_SRC_LINES")]
        public dynamic? NB_SRC_LINESX { get; set; }
        [JsonPropertyName("NB_SRC_LINESX")] 
        public int? NB_SRC_LINES { get; set; }
        [JsonPropertyName("NB_LINE_IN_WARNING")]
        public dynamic? NB_LINE_IN_WARNINGX { get; set; }
        [JsonPropertyName("NB_LINE_IN_WARNINGX")] 
        public int? NB_LINE_IN_WARNING { get; set; }
        [JsonPropertyName("NB_LINE_IN_REJECT")]
        public dynamic? NB_LINE_IN_REJECTX { get; set; }
        [JsonPropertyName("NB_LINE_IN_REJECTX")] 
        public int? NB_LINE_IN_REJECT { get; set; }

        [JsonPropertyName("data")]
        public List<DrfReportDetails>? data { get; set; }
    }
}
