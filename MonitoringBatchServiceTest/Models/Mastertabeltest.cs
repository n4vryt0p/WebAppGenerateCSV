using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonitoringBatchService.Models;

namespace MonitoringBatchServiceTest.Models
{
    public class Mastertabeltest
    {
        [Fact]
        public void mastertabeltest()
        {
            // Arrange
            var keymodel = new Mastertabel();

            // Act
  

            // Assert
            Assert.Null(keymodel.TableName);
            Assert.Null(keymodel.Cd);
            Assert.Null(keymodel.ShortDesc);
            Assert.Null(keymodel.LongDesc);
            Assert.Null(keymodel.ActualCd);
            Assert.Null(keymodel.Logdate);
           


        }
    }
}
