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

namespace MonitoringBatchServiceTest.Models
{
    public class FileStatusModelTest
    {
        [Fact]
        public void FileStatusModel_DefaultValues_ShouldBeNull()
        {
            // Arrange
            var fileStatusModel = new FileStatusModel();

          
            // Act
            // Tidak ada logika khusus yang perlu diuji, hanya memeriksa nilai default.

            // Assert
            Assert.Null(fileStatusModel.FileName);
            Assert.Null(fileStatusModel.FileStatus);
            Assert.Null(fileStatusModel.ReceivedDate);
            Assert.Null(fileStatusModel.FileRequest);
            Assert.Null(fileStatusModel.FileReqSplit);
            Assert.Null(fileStatusModel.Status);
            Assert.Null(fileStatusModel.PatchRs);

        }
    }
}
