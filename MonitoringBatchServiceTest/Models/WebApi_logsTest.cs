using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonitoringBatchService.Models;

namespace MonitoringBatchServiceTest.Models
{
    public class WebApi_logsTest
    {

        [Fact]
        public void WebApiLogsTest()
        {
            // Arrange
            var keymodel = new WebApi_logs();

            // Act


            // Assert
            Assert.NotNull(keymodel.Id);
            Assert.Null(keymodel.requestID);
            Assert.Null(keymodel.txnLogID);
            Assert.Null(keymodel.clientID);
            Assert.Null(keymodel.clientIP);
            Assert.Null(keymodel.nodeIP);
            Assert.Null(keymodel.contentType);
            Assert.Null(keymodel.method);
            Assert.Null(keymodel.urlPath);
            Assert.Null(keymodel.trxRq_DT);
            Assert.Null(keymodel.payloadSizeRq);
            Assert.Null(keymodel.payloadRq);
            Assert.Null(keymodel.trxRs_DT);
            Assert.Null(keymodel.processingTime);
            Assert.Null(keymodel.httpStatus_CD);
            Assert.Null(keymodel.PayloadRs);
            Assert.Null(keymodel.isInbound);

        }
    }
}
