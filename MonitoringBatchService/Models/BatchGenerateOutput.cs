using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringBatchService.Models
{
    public class BatchGenerateOutput
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProcessId { get; set; }
        public DateTime ProcessDate { get; set; }
        public string? ProcessRemarks { get; set; }
    }
}
