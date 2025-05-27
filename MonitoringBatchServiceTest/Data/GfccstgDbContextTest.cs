using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore.InMemory;
using MonitoringBatchService;
using Microsoft.EntityFrameworkCore;
using MonitoringBatchService.Models;
using System.Reflection.Emit;

namespace MonitoringBatchServiceTest.Data
{
    public class GfccstgDbContextTest
    {
        public GfccstgDbContext getinmemory()
        {
            var optons = new DbContextOptionsBuilder<GfccstgDbContext>()
                .UseInMemoryDatabase(databaseName: "Tesing")
                .Options;
            return new GfccstgDbContext(optons);
        }
        [Fact]
        public void Mastetabeltestcon()
        {
            var dbContext = getinmemory();
            var newmaster = new Mastertabel { TableName = "Testing", ActualCd = "Tesign" };
            
            dbContext.MasterTable.Add(newmaster);
           

            var saveditem = dbContext.MasterTable.FirstOrDefault(m => m.TableName == "Testing");
            Assert.Null(saveditem);
        }
        [Fact]
        public void DrfReportHeaderTest()
        {
            var dbContext = getinmemory(); 

            var saveditem = dbContext.DRF_REPORT_HEADER.FirstOrDefault(m => m.DRF_FILENAME == "Testing");
            Assert.Null(saveditem);
        }
        [Fact]
        public void DrfReportDetailsTest()
        {
            var dbContext = getinmemory();

            var saveditem = dbContext.DRF_REPORT_DETAILS.FirstOrDefault(m => m.DRF_ERROR_TYPE == "Testing");
            Assert.Null(saveditem);
        }
   
        
    }
}
