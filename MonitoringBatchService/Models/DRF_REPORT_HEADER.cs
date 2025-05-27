using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MonitoringBatchService.Models
{
    public class DrfReportHeader
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string? DRF_FILENAME { get; set; }
        public string? DRF_FILEPATH { get; set; }
        public string? ORGUNIT { get; set; }
        public DateOnly? DRF_DATE { get; set; }
        public string? FLOW_FILENAME { get; set; }
        public string? FILE_STATUS { get; set; }
        public string? FILE_STATUS_CODE { get; set; }
        public int IMPACT { get; set; }
        public int? ERROR_STATUS { get; set; }
        public int? NB_SRC_LINES { get; set; }
        public int? NB_LINE_IN_WARNING { get; set; }
        public int? NB_LINE_IN_REJECT { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public string? CREATE_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public string? MODIFIED_BY { get; set; }
        public virtual ICollection<DrfReportDetails>? DRF_REPORT_DETAILS { get; set; }
    }
}
