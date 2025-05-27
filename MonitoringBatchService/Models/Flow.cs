using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MonitoringBatchService.Models
{
    public class Flow
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string? Key { get; set; }
        public string? Value { get; set; }
        public string? Desc { get; set; }
        public string? Group { get; set; }
    }
}
