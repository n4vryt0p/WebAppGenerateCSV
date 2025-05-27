using Microsoft.EntityFrameworkCore;
using MonitoringBatchService.Services;
using MonitoringBatchService;

namespace MonitoringBatchServiceTest.Service
{
    public class ServiceinsertDrfTest
    {
  


        private DbContextOptions<GfccstgDbContext> CreateInMemoryDatabaseOptions()
        {
            return new DbContextOptionsBuilder<GfccstgDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }


        [Fact]
        public async Task StatusTest()
        {
            // Arrange
            string filePath = "0";

            var options = CreateInMemoryDatabaseOptions();
            var contextFactory = new DbContextFactory<GfccstgDbContext>(options);

            var userService = new ServiceinsertDrf(contextFactory);

            // Act
            var result = await userService.Status(filePath);

            // Assert
            Assert.NotNull(result);

        }
        [Fact]
        public async Task impactest()
        {
            // Arrange
            string filePath = "0";

            var options = CreateInMemoryDatabaseOptions();
            var contextFactory = new DbContextFactory<GfccstgDbContext>(options);

            var userService = new ServiceinsertDrf(contextFactory);

            // Act
            var result = await userService.Impact(filePath);

            // Assert
            Assert.Equal(Convert.ToString(result), filePath);

        }
        [Fact]
        public async Task InsertTest()
        {
            var options = CreateInMemoryDatabaseOptions();
            var contextFactory = new DbContextFactory<GfccstgDbContext>(options);

            var userService = new ServiceinsertDrf(contextFactory);
            var jsonString = @$"{{
                                ""ORGUNIT"": ""054"",
                                ""DATE"": ""20250203"",
                                ""FILENAME"": ""054_CUSTOMERS_20250203_20250203170000.json"",
                                ""FILE_STATUS"": ""CIL_REJECTED"",
                                ""NB_SRC_LINES"": ""2"",
                                ""NB_LINE_IN_WARNING"": ""0"",
                                ""NB_LINE_IN_REJECT"": ""2"",
                                ""data"": [
                                {{
                                    ""LINE_ID"": 24,
                                    ""ERROR_TYPE"": ""FILE REJECTED"",
                                    ""ORIGIN"": ""CIL"",
                                    ""ERROR_MESSAGE"": ""Duplicated primary key found"",
                                    ""FIELD_VALUE"": ""R2001@@@FILE REJECTED@@@CUSTOMER_SOURCE_UNIQUE_ID@@@154717184@@@CIL@@@Duplicated primary key found@@@154717184@@@24"",
                                    ""SOURCE_REF_IDS"": ""054_0C3E25E6A0CBD111F92427E4AAFDE997"",
                                    ""FIELD_NAME"": ""CUSTOMER_SOURCE_UNIQUE_ID"",
                                    ""ERROR_CODE"": ""R2001""
                                }},
                                {{
                                    ""LINE_ID"": 24,
                                    ""ERROR_TYPE"": ""RECORD REJECTED"",
                                    ""ORIGIN"": ""CIL"",
                                    ""ERROR_MESSAGE"": ""Duplicated composed key found for same Customer Input"",
                                    ""FIELD_VALUE"": ""R2001@@@RECORD REJECTED@@@IDENTIFICATION_NUMBER@@@1547171843374112709760003@@@CIL@@@Duplicated composed key found for same Customer Input@@@1547171843374112709760003@@@24"",
                                    ""SOURCE_REF_IDS"": ""054_12D42BF1203DDBBAFC50A1B7FB869810"",
                                    ""FIELD_NAME"": ""IDENTIFICATION_NUMBER"",
                                    ""ERROR_CODE"": ""R2001""
                                }}
                                ]
                            }}";

            // Act
            var result = await userService.InsertDrf("files/DRF_054_CUSTOMERS.json", jsonString);
            
            // Assert
            Assert.True(result);

        }



    }
    public class DbContextFactory<TContext> : IDbContextFactory<TContext> where TContext : DbContext
    {
        private readonly DbContextOptions<TContext> _options;

        public DbContextFactory(DbContextOptions<TContext> options)
        {
            _options = options;
        }

        public TContext CreateDbContext()
        {
            return (TContext)Activator.CreateInstance(typeof(TContext), _options);
        }
    }
}
