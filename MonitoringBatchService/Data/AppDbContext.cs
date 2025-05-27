using Microsoft.EntityFrameworkCore;
using MonitoringBatchService.Models;

namespace MonitoringBatchService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }
        public DbSet<BatchGenerateOutput> batchGenerateOutputs { get; set; }



    }
}
    