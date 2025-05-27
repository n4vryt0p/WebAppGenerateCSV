using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonitoringBatchService.Models;

namespace MonitoringBatchServiceTest.Models
{
    public class FileMonitoringTest
    {
        [Fact]
        public void filemonitoringtest()
        {
            // Arrange
            var keymodel = new FileMonitoring();
            int actual = keymodel.Id;
            // Act
            // Tidak ada logika khusus yang perlu diuji, hanya memeriksa nilai default.

            // Assert
            Assert.Equal(keymodel.Id,actual);
            Assert.Null(keymodel.requestID);
            Assert.Null(keymodel.txnLogID);
            Assert.Null(keymodel.clientID);
            Assert.Null(keymodel.clientIP);
            Assert.Null(keymodel.nodeIP);
            Assert.Null(keymodel.contentType);
            Assert.Null(keymodel.urlPath);
            Assert.Null(keymodel.trxRq_DT);
            Assert.Null(keymodel.fileSizeRq);
            Assert.Null(keymodel.fileRq);
            Assert.Null(keymodel.generateDT);
            Assert.Null(keymodel.triggerDT);
            Assert.Null(keymodel.consumeDT);
            Assert.Null(keymodel.responseDT);
            Assert.Null(keymodel.fileSizeRq);
            Assert.Null(keymodel.fileRs);
            Assert.Null(keymodel.statusCode);
            Assert.Null(keymodel.isInbound);
            Assert.Null(keymodel.CountFile);
            Assert.Null(keymodel.urlPathRs);
            float expectedFileSize = 123.45f;
            keymodel.fileSizeRs = expectedFileSize;

            // Act & Assert
            Assert.NotNull(keymodel.fileSizeRs);
        }
    }
}
