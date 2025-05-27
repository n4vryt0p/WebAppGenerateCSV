using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MonitoringBatchService.Models
{
    public class Mastertabel
    {
        [Key]
        [Column("Table_Name")]
        [StringLength(250)]
        public string? TableName { get; set; }
       
        [Column("CD")]
        [StringLength(250)]
        public string? Cd { get; set; }
        [StringLength(1000)]
        public string? ShortDesc { get; set; }
        [StringLength(1000)]
        public string? LongDesc { get; set; }
        
        [Column("Actual_CD")]
        [StringLength(250)]
        public string? ActualCd { get; set; }
        [Column("LOGDATE", TypeName = "datetime")]
        public DateTime? Logdate { get; set; }
    }
}
