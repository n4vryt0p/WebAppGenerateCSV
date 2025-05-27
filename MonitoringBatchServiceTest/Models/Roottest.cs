using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonitoringBatchService.Models;

namespace MonitoringBatchServiceTest.Models
{
    public class Roottest
    {
        [Fact]
        public void datumtest()
        {
            // Arrange
            var keymodel = new Datum();

            // Act


            // Assert
            Assert.Null(keymodel.LINE_ID);
            Assert.Null(keymodel.ERROR_TYPE);
            Assert.Null(keymodel.ORIGIN);
            Assert.Null(keymodel.ERROR_MESSAGE);
            Assert.Null(keymodel.FIELD_VALUE);
            Assert.Null(keymodel.SOURCE_REF_IDS);
            Assert.Null(keymodel.FIELD_NAME);
            Assert.Null(keymodel.ERROR_CODE);

        }
        [Fact]
        public void roottest()
        {
            // Arrange
            var keymodel = new Root();

            // Act


            // Assert
            Assert.Null(keymodel.ORGUNIT);
            Assert.Null(keymodel.DATE);
            Assert.Null(keymodel.FILENAME);
            Assert.Null(keymodel.FILE_STATUS);
            Assert.Null(keymodel.NB_SRC_LINESX);
            Assert.Null(keymodel.NB_SRC_LINES);
            Assert.Null(keymodel.NB_LINE_IN_WARNINGX);
            Assert.Null(keymodel.NB_LINE_IN_WARNING);
            Assert.Null(keymodel.NB_LINE_IN_REJECTX);
            Assert.Null(keymodel.NB_LINE_IN_REJECT);
            Assert.Null(keymodel.data);

        }
    }
}
