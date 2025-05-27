using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MonitoringBatchService.Data;
using MonitoringBatchService.Models;

namespace MonitoringBatchServiceTest.Data
{
    public class AppDbContextTests
    {
        [Fact]
        public void AppDbContext_CanInitialize_WithInMemoryDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Act
            using (var context = new AppDbContext(options))
            {
                // Assert
                Assert.NotNull(context);
                Assert.NotNull(context.batchGenerateOutputs); // Memastikan DbSet diinisialisasi
            }
        }

       
    }
}
