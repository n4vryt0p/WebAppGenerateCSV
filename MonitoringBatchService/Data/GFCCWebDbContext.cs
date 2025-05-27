using Microsoft.EntityFrameworkCore;
using MonitoringBatchService.Models;
using System.Reflection.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MonitoringBatchService
{
    public partial class GfccWebDbContext : DbContext
    {
        private readonly string? webapp = "WebApp";
        private readonly string? lil = "LIL";
        private readonly string? dbo = "dbo";
        public GfccWebDbContext(DbContextOptions<GfccWebDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Flow> MasterConfigs { get; set; }
        public virtual DbSet<SqlmnGetCustomer> SQLMNLGetCustomer { get; set; }
        public virtual  DbSet<SqlmnlGetCustomerCustomerLink> SQLMNLGetCustomerCustomerLink { get; set; }
        public virtual DbSet<SqlmnlGetCustomerPolicyLink> SQLMNLGetCustomerPolicyLink { get; set; }
        public virtual DbSet<SqlmnlGetIntermediaryPolicyLink> SQLMNLGetIntermediaryPolicyLink { get; set; }
        public virtual DbSet<SqlmnlGetIntermediaries> SQLMNLGetIntermediaries { get; set; }
        public virtual DbSet<SqlmnlGetPolicies> SQLMNLGetPolicies { get; set; }
        public virtual DbSet<SqlmnlGetProductSourceType> SQLMNLGetProductSourceType { get; set; }
        public virtual DbSet<SqlmnlGetProducts> SQLMNLGetProducts { get; set; }
        public virtual DbSet<SqlmnlGetOperation> SQLMNLGetOperation { get; set; }
        public virtual DbSet<FileMonitoring> FileMonitoring { get; set; }
        public virtual DbSet<WebApi_logs> WebApi_Logs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Flow>(entity =>
            {
                entity.ToTable("MasterConfigs", webapp);
            });
            modelBuilder.Entity<WebApi_logs>(entity =>
            {
                entity.ToTable("WebApi.Logs", dbo);
            });
            modelBuilder.Entity<SqlmnGetCustomer>(entity =>
            {
                entity.ToView("SQLMNLGetCustomer", webapp).HasNoKey();
            });
            modelBuilder.Entity<SqlmnlGetCustomerCustomerLink>(entity =>
            {
                entity.ToView("SQLMNLGetCustomerCustomerLink", webapp).HasNoKey();
            });
            modelBuilder.Entity<SqlmnlGetCustomerPolicyLink>(entity =>
            {
                entity.ToView("SQLMNLGetCustomerPolicyLink", webapp).HasNoKey();
            });
            modelBuilder.Entity<SqlmnlGetIntermediaryPolicyLink>(entity =>
            {
                entity.ToView("SQLMNLGetIntermediaryPolicyLink", webapp).HasNoKey();
            });
            modelBuilder.Entity<SqlmnlGetIntermediaries>(entity =>
            {
                entity.ToView("SQLMNLGetIntermediaries", webapp).HasNoKey();
            });
            
            modelBuilder.Entity<SqlmnlGetProductSourceType>(entity =>
            {
            entity.ToView("SQLMNLGetProductSourceType", webapp).HasNoKey();
            });
            modelBuilder.Entity<SqlmnlGetProducts>(entity =>
            {
                entity.ToView("SQLMNLGetProducts", webapp).HasNoKey();
            });
            modelBuilder.Entity<SqlmnlGetPolicies>(entity =>
            {
                entity.ToView("SQLMNLGetPolicies", webapp).HasNoKey();
            });
            modelBuilder.Entity<SqlmnlGetOperation>(entity =>
            {
                entity.ToView("SQLMNLGetOperation", webapp).HasNoKey();
            });
            modelBuilder.Entity<FileMonitoring>(entity =>
            {
                entity.ToView("FileMonitoring", lil).HasNoKey();
            });


            modelBuilder.Entity<SqlmnGetCustomer>(entity =>
            {
                entity.ToView("SQLMNLGetCustomer", webapp).HasNoKey();
            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

}
