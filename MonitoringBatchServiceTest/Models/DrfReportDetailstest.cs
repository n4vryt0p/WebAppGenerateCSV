using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonitoringBatchService.Models;

namespace MonitoringBatchServiceTest.Models
{
    public class DrfReportDetailstest
    {

        [Fact]
        public void drfreport()
        {
            // Arrange
            var keymodel = new DrfReportDetails();

            // Act
            int actual = keymodel.ID;
            int lineid = keymodel.LINE_ID;
            int errorstatusrecode = keymodel.ERROR_STATUS_RECORD;
            DateTime createdate = keymodel.CREATE_DATE;
            int idheader = keymodel.ID_HEADER;

            // Assert
            Assert.Equal(keymodel.ID, actual);
            Assert.Null(keymodel.LINE_ID_Extra);
            Assert.Equal(keymodel.LINE_ID,lineid);
            Assert.Null(keymodel.DRF_ERROR_TYPE);
            Assert.Null(keymodel.ORIGIN);
            Assert.Null(keymodel.LINE_ERROR_MESSAGE);
            Assert.Null(keymodel.FIELD_NAME);
            Assert.Null(keymodel.FIELD_VALUE);
            Assert.Null(keymodel.ERROR_CODE);
            Assert.Null(keymodel.SOURCE_REF_IDS);
            Assert.Equal(keymodel.ERROR_STATUS_RECORD,errorstatusrecode);
            Assert.Equal(keymodel.CREATE_DATE,createdate);
            Assert.Null(keymodel.CREATE_BY);
            Assert.Null(keymodel.MODIFIED_DATE);
            Assert.Null(keymodel.MODIFIED_BY);
            Assert.Equal(keymodel.ID_HEADER, idheader);
            Assert.Null (keymodel.DRF_REPORT_HEADER);

        }
    }
}
