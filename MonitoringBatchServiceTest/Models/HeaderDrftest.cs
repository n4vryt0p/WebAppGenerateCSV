using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonitoringBatchService.Models;

namespace MonitoringBatchServiceTest.Models
{
    public class HeaderDrftest
    {
        [Fact]
        public void drfheadertest()
        {
            // Arrange
            var keymodel = new HeaderDrf();

            // Act
            int actual = keymodel.ID;
            int errorstatus = keymodel.ERROR_STATUS;
            int srcline = keymodel.NB_SRC_LINES;
            int waringing = keymodel.NB_LINE_IN_WARNING;
            int inject = keymodel.NB_LINE_IN_REJECT;
            DateTime drfdate = keymodel.DRF_DATE;
            DateTime createdate = keymodel.CREATE_DATE;


            // Assert
            Assert.Equal(keymodel.ID, actual);
            Assert.Null(keymodel.DRF_FILENAME);
            Assert.Null(keymodel.DRF_FILEPATH);
            Assert.Null(keymodel.ORGUNIT);
            Assert.Equal(keymodel.DRF_DATE,drfdate);
            Assert.Null(keymodel.FLOW_FILENAME);
            Assert.Null(keymodel.FILE_STATUS);
            Assert.Null(keymodel.IMPACT);
            Assert.Null(keymodel.FILE_STATUS_CODE);
            Assert.Equal(keymodel.ERROR_STATUS,errorstatus);
            Assert.Equal(keymodel.NB_SRC_LINES,srcline);
            Assert.Equal(keymodel.NB_LINE_IN_REJECT,inject);
            Assert.Equal(keymodel.NB_LINE_IN_WARNING,waringing);
            Assert.Equal(keymodel.CREATE_DATE, createdate);
            Assert.Null(keymodel.CREATE_BY);
            Assert.Null(keymodel.MODIFIED_DATE);
            Assert.Null(keymodel.MODIFIED_BY);


        }
    }
}
