using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonitoringBatchService.Models;

namespace MonitoringBatchServiceTest.Models
{
    public class Batchgenerateoutputtest
    {

        [Fact]
        public void flowtest()
        {
            // Arrange
            var keymodel = new BatchGenerateOutput();

            // Act
            DateTime process = keymodel.ProcessDate;
            int actual = keymodel.ProcessId = 1;

            // Assert
            Assert.Equal(keymodel.ProcessId, actual);
            Assert.Equal(keymodel.ProcessDate, process);
            Assert.Null(keymodel.ProcessRemarks);
            
            
        }
    }
}
