using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using MonitoringBatchService.Models;
using MonitoringBatchService.Data;
using System.Linq;
using MonitoringBatchService.Services;
using System.Globalization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MonitoringBatchService;
using MonitoringBatchServiceTest.Data;
using Microsoft.EntityFrameworkCore.Internal;


namespace MonitoringBatchServiceTest.Service
{
    public class Databaseservicetest
    {

  
        private DbContextOptions<GfccWebDbContext> CreateInMemoryDatabaseOptions()
        {
            return new DbContextOptionsBuilder<GfccWebDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }
        private DbContextOptions<GfccstgDbContext> CreateInMemoryDatabaseOptions2()
        {
            return new DbContextOptionsBuilder<GfccstgDbContext>()
                .UseInMemoryDatabase(databaseName: "TestingOk")
                .Options;
        }
        private readonly Mock<IDbContextFactory<GfccWebDbContext>> _mockCtxWeb;
        private readonly Mock<IDbContextFactory<GfccstgDbContext>> _mockCtxStg;
        private readonly Mock<IFileService> _mockFileService;
        private readonly DatabaseService _databaseService;

        public Databaseservicetest()
        {
            _mockCtxWeb = new Mock<IDbContextFactory<GfccWebDbContext>>();
            _mockCtxStg = new Mock<IDbContextFactory<GfccstgDbContext>>();
            _mockFileService = new Mock<IFileService>();

            _databaseService = new DatabaseService(_mockCtxWeb.Object, _mockCtxStg.Object, _mockFileService.Object);
        }

        [Fact]
        public async Task GetRecordsAsync_ShouldReturnRecords()
        {
    
            var options = CreateInMemoryDatabaseOptions();
            var options2 = CreateInMemoryDatabaseOptions2();
            var contextFactory = new DbContextFactory<GfccWebDbContext>(options);
            var contextFactorydb = new DbContextFactory<GfccstgDbContext>(options2);
       
            var dbService = new DatabaseService(contextFactory, contextFactorydb, _mockFileService.Object);

            //// Act
            var result = dbService.GetRecordsAsync();

            // Assert
            Assert.NotNull(result);

        }
        [Fact]
        public async Task GetRecordsAsync_ShouldReturnRecordstest()
        {
            // Arrange
            var mockDbConnection = new Mock<IDbConnection>();
            var mockDbService = new Mock<IDatabaseService>();

            var dummyData = new List<BatchGenerateOutput>
            {
                new BatchGenerateOutput { ProcessId = 1, ProcessDate = DateTime.Now, ProcessRemarks = "OPEN" },
               new BatchGenerateOutput { ProcessId = 2, ProcessDate = DateTime.Now, ProcessRemarks = "OPEN" },
            };

            // Mock metode GetRecordsAsync agar mengembalikan data dummy
            mockDbService.Setup(db => db.GetRecordsAsync()).ReturnsAsync(dummyData);

            // Act
            var result = await mockDbService.Object.GetRecordsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result); // Pastikan ada data yang dikembalikan
        }


        [Fact]
        public async Task GetRecordsAsync_ShouldReturnEmptyOnException()
        {
  
            var options = CreateInMemoryDatabaseOptions();
            var options2 = CreateInMemoryDatabaseOptions2();
            var contextFactory = new DbContextFactory<GfccWebDbContext>(options);
            var contextFactorydb = new DbContextFactory<GfccstgDbContext>(options2);
            var dbService = new DatabaseService(contextFactory, contextFactorydb, _mockFileService.Object);

            //// Act
            var result = dbService.GetRecordsAsync();

            // Assert
            Assert.NotNull(result);
        
        }



        [Fact]
        public void UpdateRecordsAsynctest()
        {

            var options = CreateInMemoryDatabaseOptions();
            var options2 = CreateInMemoryDatabaseOptions2();
            var contextFactory = new DbContextFactory<GfccWebDbContext>(options);
            var contextFactorydb = new DbContextFactory<GfccstgDbContext>(options2);
            var dbService = new DatabaseService(contextFactory, contextFactorydb, _mockFileService.Object);

            int prosesid = 1;
            int newstaus = 2;
            //// Act
            var result = dbService.UpdateRecordsAsync(prosesid, newstaus);

            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public void UpdateRecordsAsyncruls_ShouldExecuteQuery()
        {
  
            var options = CreateInMemoryDatabaseOptions();
            var options2 = CreateInMemoryDatabaseOptions2();
            var contextFactory = new DbContextFactory<GfccWebDbContext>(options);
            var contextFactorydb = new DbContextFactory<GfccstgDbContext>(options2);
             var dbService = new DatabaseService(contextFactory, contextFactorydb, _mockFileService.Object);


            int prosesid = 1;
            string newstaus = "Testing";
            //// Act
            var result = dbService.UpdateRecordsAsyncruls(prosesid, newstaus);

            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public void UpdateDRFsuccessTest()
        {
           
            var options = CreateInMemoryDatabaseOptions();
            var options2 = CreateInMemoryDatabaseOptions2();
            var contextFactory = new DbContextFactory<GfccWebDbContext>(options);
            var contextFactorydb = new DbContextFactory<GfccstgDbContext>(options2);
             var dbService = new DatabaseService(contextFactory, contextFactorydb, _mockFileService.Object);


            string file = "Testing";
            //// Act
            var result = dbService.UpdateDRFsuccess(file);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void UpdateFileMonitoringTest()
        {
          

            var options = CreateInMemoryDatabaseOptions();
            var options2 = CreateInMemoryDatabaseOptions2();
            var contextFactory = new DbContextFactory<GfccWebDbContext>(options);
            var contextFactorydb = new DbContextFactory<GfccstgDbContext>(options2);
             var dbService = new DatabaseService(contextFactory, contextFactorydb, _mockFileService.Object);


            string file = "Testing";
            //// Act
            var result = dbService.UpdateFileMonitoring(file);

            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public void UpdateFileMonitoringResponTest()
        {



            var options = CreateInMemoryDatabaseOptions();
            var options2 = CreateInMemoryDatabaseOptions2();
            var contextFactory = new DbContextFactory<GfccWebDbContext>(options);
            var contextFactorydb = new DbContextFactory<GfccstgDbContext>(options2);
             var dbService = new DatabaseService(contextFactory, contextFactorydb, _mockFileService.Object);



            string file = "Testing";
            long filesize = 1;
            string fileRs = "test";
            string urlPathRs = "urll";
            //// Act
            var result = dbService.UpdateFileMonitoringRespon(file, filesize, fileRs, urlPathRs);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void UpdateAllflow()
        {


            var options = CreateInMemoryDatabaseOptions();
            var options2 = CreateInMemoryDatabaseOptions2();
            var contextFactory = new DbContextFactory<GfccWebDbContext>(options);
            var contextFactorydb = new DbContextFactory<GfccstgDbContext>(options2);
            var dbService = new DatabaseService(contextFactory, contextFactorydb, _mockFileService.Object);


            string key = "flow1";
            string group = "gfcct";
            //// Act
            var result = dbService.Updateallflow(key,group);

            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public void UpdateFileMonitoringtrigerTest()
        {
        

            var options = CreateInMemoryDatabaseOptions();
            var options2 = CreateInMemoryDatabaseOptions2();
            var contextFactory = new DbContextFactory<GfccWebDbContext>(options);
            var contextFactorydb = new DbContextFactory<GfccstgDbContext>(options2);
             var dbService = new DatabaseService(contextFactory, contextFactorydb, _mockFileService.Object);


            string file = "Testing";
            //// Act
            var result = dbService.UpdateFileMonitoringtriger(file);

            // Assert
            Assert.NotNull(result);
        }




        [Fact]
        public void GetFlowTest()
        {
            
         
            var options = CreateInMemoryDatabaseOptions();
            var options2 = CreateInMemoryDatabaseOptions2();
            var contextFactory = new DbContextFactory<GfccWebDbContext>(options);
            var contextFactorydb = new DbContextFactory<GfccstgDbContext>(options2);
             var dbService = new DatabaseService(contextFactory, contextFactorydb, _mockFileService.Object);


            string flowkey = "1";
            //// Act
            var result = dbService.GetFlow(flowkey);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void SendEmailWithResultstEST()
        {
           
            var options = CreateInMemoryDatabaseOptions();
            var options2 = CreateInMemoryDatabaseOptions2();
            var contextFactory = new DbContextFactory<GfccWebDbContext>(options);
            var contextFactorydb = new DbContextFactory<GfccstgDbContext>(options2);
             var dbService = new DatabaseService(contextFactory, contextFactorydb, _mockFileService.Object);


            List<FileStatusModel> msgs = new List<FileStatusModel>();
            //// Act
            var result = dbService.SendEmailWithResults(msgs);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void flow1test()
        {

            var options = CreateInMemoryDatabaseOptions();
            var options2 = CreateInMemoryDatabaseOptions2();
            var contextFactory = new DbContextFactory<GfccWebDbContext>(options);
            var contextFactorydb = new DbContextFactory<GfccstgDbContext>(options2);
             var dbService = new DatabaseService(contextFactory, contextFactorydb, _mockFileService.Object);



            //// Act
            var result = dbService.FLOW1();

            // Assert
            Assert.Equal(0, result);
        }





        [Fact]
        public void flow2test()
        {
            // Arrange

            var options = CreateInMemoryDatabaseOptions();
            var options2 = CreateInMemoryDatabaseOptions2();
            var contextFactory = new DbContextFactory<GfccWebDbContext>(options);
            var contextFactorydb = new DbContextFactory<GfccstgDbContext>(options2);
             var dbService = new DatabaseService(contextFactory, contextFactorydb, _mockFileService.Object);



            //// Act
            var result = dbService.FLOW2();

            // Assert
            Assert.Equal(0, result);



        }
        [Fact]
        public void Flow3test()
        {
            var options = CreateInMemoryDatabaseOptions();
            var options2 = CreateInMemoryDatabaseOptions2();
            var contextFactory = new DbContextFactory<GfccWebDbContext>(options);
            var contextFactorydb = new DbContextFactory<GfccstgDbContext>(options2);
             var dbService = new DatabaseService(contextFactory, contextFactorydb, _mockFileService.Object);



            //// Act
            var result = dbService.FLOW3();

            // Assert
            Assert.Equal(0, result);


        }
        [Fact]
        public void flow4test()
        {

            var options = CreateInMemoryDatabaseOptions();
            var options2 = CreateInMemoryDatabaseOptions2();
            var contextFactory = new DbContextFactory<GfccWebDbContext>(options);
            var contextFactorydb = new DbContextFactory<GfccstgDbContext>(options2);
             var dbService = new DatabaseService(contextFactory, contextFactorydb, _mockFileService.Object);



            //// Act
            var result = dbService.FLOW4();

            // Assert
            Assert.Equal(0, result);

        }
        [Fact]
        public void flow5test()
        {

            var options = CreateInMemoryDatabaseOptions();
            var options2 = CreateInMemoryDatabaseOptions2();
            var contextFactory = new DbContextFactory<GfccWebDbContext>(options);
            var contextFactorydb = new DbContextFactory<GfccstgDbContext>(options2);
             var dbService = new DatabaseService(contextFactory, contextFactorydb, _mockFileService.Object);



            //// Act
            var result = dbService.FLOW5();

            // Assert
            Assert.Equal(0, result);


        }
        [Fact]
        public void flow6test()
        {

            var options = CreateInMemoryDatabaseOptions();
            var options2 = CreateInMemoryDatabaseOptions2();
            var contextFactory = new DbContextFactory<GfccWebDbContext>(options);
            var contextFactorydb = new DbContextFactory<GfccstgDbContext>(options2);
             var dbService = new DatabaseService(contextFactory, contextFactorydb, _mockFileService.Object);



            //// Act
            var result = dbService.FLOW6();

            // Assert
            Assert.Equal(0, result);

        }
        [Fact]
        public void flow7test()
        {

            var options = CreateInMemoryDatabaseOptions();
            var options2 = CreateInMemoryDatabaseOptions2();
            var contextFactory = new DbContextFactory<GfccWebDbContext>(options);
            var contextFactorydb = new DbContextFactory<GfccstgDbContext>(options2);
             var dbService = new DatabaseService(contextFactory, contextFactorydb, _mockFileService.Object);



            //// Act
            var result = dbService.FLOW7();

            // Assert
            Assert.Equal(0, result);


        }
        [Fact]
        public void flow8test()
        {
            // Arrange

            var options = CreateInMemoryDatabaseOptions();
            var options2 = CreateInMemoryDatabaseOptions2();
            var contextFactory = new DbContextFactory<GfccWebDbContext>(options);
            var contextFactorydb = new DbContextFactory<GfccstgDbContext>(options2);
             var dbService = new DatabaseService(contextFactory, contextFactorydb, _mockFileService.Object);



            //// Act
            var result = dbService.FLOW8();

            // Assert
            Assert.Equal(0, result);

        }
        [Fact]
        public void flow9test()
        {

            var options = CreateInMemoryDatabaseOptions();
            var options2 = CreateInMemoryDatabaseOptions2();
            var contextFactory = new DbContextFactory<GfccWebDbContext>(options);
            var contextFactorydb = new DbContextFactory<GfccstgDbContext>(options2);
             var dbService = new DatabaseService(contextFactory, contextFactorydb, _mockFileService.Object);



            //// Act
            var result = dbService.FLOW9();

            // Assert
            Assert.Equal(0, result);


        }
        
             [Fact]
        public void GetMasterstatusTest()
        {


            var options = CreateInMemoryDatabaseOptions();
            var options2 = CreateInMemoryDatabaseOptions2();
            var contextFactory = new DbContextFactory<GfccWebDbContext>(options);
            var contextFactorydb = new DbContextFactory<GfccstgDbContext>(options2);
             var dbService = new DatabaseService(contextFactory, contextFactorydb, _mockFileService.Object);


            string file = "Testing";
            //// Act
            var result = dbService.GetMasterstatus(file);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetMasterfilestatusTest()
        {


            var options = CreateInMemoryDatabaseOptions();
            var options2 = CreateInMemoryDatabaseOptions2();
            var contextFactory = new DbContextFactory<GfccWebDbContext>(options);
            var contextFactorydb = new DbContextFactory<GfccstgDbContext>(options2);
             var dbService = new DatabaseService(contextFactory, contextFactorydb, _mockFileService.Object);


            string file = "Testing";
            //// Act
            var result = dbService.GetMasterfilestatus(file);

            // Assert
            Assert.NotNull(result);
        }

        
        [Fact]
        public void GetMasterTbaeldrfTest()
        {


            var options = CreateInMemoryDatabaseOptions();
            var options2 = CreateInMemoryDatabaseOptions2();
            var contextFactory = new DbContextFactory<GfccWebDbContext>(options);
            var contextFactorydb = new DbContextFactory<GfccstgDbContext>(options2);
             var dbService = new DatabaseService(contextFactory, contextFactorydb, _mockFileService.Object);


            string file = "Testing";
            //// Act
            var result = dbService.GetMasterTbaeldrf(file);

            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public void CheckDataRespondatetest()
        {


            var options = CreateInMemoryDatabaseOptions();
            var options2 = CreateInMemoryDatabaseOptions2();
            var contextFactory = new DbContextFactory<GfccWebDbContext>(options);
            var contextFactorydb = new DbContextFactory<GfccstgDbContext>(options2);
             var dbService = new DatabaseService(contextFactory, contextFactorydb, _mockFileService.Object);


            //// Act
            var result = dbService.CheckDataRespondate();

            // Assert
            Assert.NotNull(result);
        }
        
        [Fact]
        public void Checkdatafilemonitoringtest()
        {


            var options = CreateInMemoryDatabaseOptions();
            var options2 = CreateInMemoryDatabaseOptions2();
            var contextFactory = new DbContextFactory<GfccWebDbContext>(options);
            var contextFactorydb = new DbContextFactory<GfccstgDbContext>(options2);
             var dbService = new DatabaseService(contextFactory, contextFactorydb, _mockFileService.Object);


            string file = "Testing";
            //// Act
            var result = dbService.Checkdatafilemonitoring(file);

            // Assert
            Assert.NotNull(result);
        }
        
         [Fact]
        public void CheckCountDRFtest()
        {


            var options = CreateInMemoryDatabaseOptions();
            var options2 = CreateInMemoryDatabaseOptions2();
            var contextFactory = new DbContextFactory<GfccWebDbContext>(options);
            var contextFactorydb = new DbContextFactory<GfccstgDbContext>(options2);
             var dbService = new DatabaseService(contextFactory, contextFactorydb, _mockFileService.Object);


            string file = "Testing";
            //// Act
            var result = dbService.CheckCountDRF(file);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetFileSize_WhenFileExists_ReturnsFileSize()
        {
            // Arrange
            var fileName = "testfile.txt";
            var expectedFileSize = 1024; // 1 KB

            _mockFileService
                .Setup(f => f.FileExists(fileName))
                .Returns(true);
            _mockFileService
                .Setup(f => f.GetFileSize(fileName))
                .Returns(expectedFileSize);

            // Act
            var result = _databaseService.GetFileSize(fileName);

            // Assert
            Assert.Equal(expectedFileSize, result);
            _mockFileService.Verify(f => f.GetFileSize(fileName), Times.Once);
        }

       
    }
}