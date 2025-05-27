using Microsoft.EntityFrameworkCore;
using MonitoringBatchService.Models;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MonitoringBatchService
{
    public partial class GfccstgDbContext : DbContext
    {
        public GfccstgDbContext(DbContextOptions<GfccstgDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Mastertabel> MasterTable { get; set; }

        public virtual DbSet<DrfReportHeader> DRF_REPORT_HEADER { get; set; }
        public virtual DbSet<DrfReportDetails> DRF_REPORT_DETAILS { get; set; }
        public virtual DbSet<BatchGenerateOutput> BatchGenerateOutput { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BatchGenerateOutput>(entity =>
            {
                entity.ToTable("BatchGenerateOutput", "LIL");
            });

            modelBuilder.Entity<DrfReportHeader>(entity =>
            {
                entity.ToTable("DRF_REPORT_HEADER", "LIL");
                entity.Property(e => e.CREATE_DATE).HasDefaultValueSql("(GETDATE())");
            });

            modelBuilder.Entity<DrfReportDetails>(entity =>
            {
                entity.ToTable("DRF_REPORT_DETAILS", "LIL");
                entity.HasOne(r => r.DRF_REPORT_HEADER)
                    .WithMany(r => r.DRF_REPORT_DETAILS)
                    .HasForeignKey(r => r.ID_HEADER)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.Property(e => e.CREATE_DATE).HasDefaultValueSql("(GETDATE())");
            });

            modelBuilder.Entity<Mastertabel>(entity =>
            {
                entity.ToTable("Master_Table", "dbo");
                entity.HasKey(e => new { e.TableName, e.ActualCd });

                entity.Property(e => e.Logdate).HasDefaultValueSql("(GETDATE())");
            });

          
        }

       
    }
}
