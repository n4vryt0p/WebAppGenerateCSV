using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonitoringBatchService.Models;

namespace MonitoringBatchServiceTest.Models
{
    public class DrfReportHeadertest
    {
        [Fact]
        public void drfreport()
        {
            // Arrange
            var keymodel = new DrfReportHeader();

            // Act
            int actual = keymodel.ID;
            int lineid = keymodel.IMPACT;
            DateTime createdate = keymodel.CREATE_DATE;


            // Assert
            Assert.Equal(keymodel.ID, actual);
            Assert.Null(keymodel.DRF_FILENAME);
            Assert.Null(keymodel.DRF_FILEPATH);
            Assert.Null(keymodel.ORGUNIT);
            Assert.Null(keymodel.DRF_DATE);
            Assert.Null(keymodel.FLOW_FILENAME);
            Assert.Null(keymodel.FILE_STATUS);
            Assert.Null(keymodel.FILE_STATUS_CODE);
            Assert.Equal(keymodel.IMPACT, lineid);
            Assert.Null(keymodel.ERROR_STATUS);
            Assert.Null(keymodel.ERROR_STATUS);
            Assert.Null(keymodel.NB_SRC_LINES);
            Assert.Null(keymodel.NB_LINE_IN_REJECT);
            Assert.Null(keymodel.NB_LINE_IN_WARNING);
            Assert.Equal(keymodel.CREATE_DATE, createdate);
            Assert.Null(keymodel.CREATE_BY);
            Assert.Null(keymodel.MODIFIED_DATE);
            Assert.Null(keymodel.MODIFIED_BY);
           Assert.Null (keymodel.DRF_REPORT_DETAILS);

        }
      
    }
}
