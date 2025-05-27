using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonitoringBatchService.Models;

namespace MonitoringBatchServiceTest.Models
{
    public class SqlmnlGetIntermediariesTest
    {

        
       [Fact]
        public void SqlmnlGetIntermediariestesta()
        {
            // Arrange
            var keymodel = new SqlmnlGetIntermediaries();

            Assert.Equal(default(DateTime), keymodel.RUN_TIMESTAMP);
            Assert.Equal(default(DateTime), keymodel.CREATE_DATE);
            Assert.Null(keymodel.INTERMEDIARY_SOURCE_UNIQUE_ID);
            Assert.Null(keymodel.INTERMEDIARY_NAME);
            Assert.Null(keymodel.INTERMEDIARY_TYPE_CODE);
            Assert.Null(keymodel.COUNTRY_CODE);
            Assert.Null(keymodel.FROM_DATE);
            Assert.Null(keymodel.TO_DATE);
            Assert.Null(keymodel.ORGUNIT_CODE);
            Assert.Null(keymodel.ADDRESS);
            Assert.Null(keymodel.POSTAL_CODE);
            Assert.Null(keymodel.CITY);
            Assert.Null(keymodel.PHONE_NUMBER);
            Assert.Null(keymodel.FAX_NUMBER);
            Assert.Null(keymodel.EMAIL_ADDRESS);
            Assert.Null(keymodel.TARGET_MARKET);
            Assert.Null(keymodel.APPROVED_FROM);
            Assert.Null(keymodel.APPROVED_TO);
            Assert.Null(keymodel.X_DISTRIBUTOR_TYPE_CODE);
            Assert.Null(keymodel.X_STATUS_CODE);
            Assert.Null(keymodel.X_CANCELLED_DATE);
            Assert.Null(keymodel.X_COUNTRY_OF_HQ);
            Assert.Null(keymodel.EMPLOYEE_ID);
            Assert.Null(keymodel.RISK_LEVEL);
            Assert.Null(keymodel.RISK_SCORE);
            Assert.Null(keymodel.MODIFIED_DATE);
            Assert.Null(keymodel.INTERMEDIARIES_HK);
            Assert.Null(keymodel.Diff_HK);
        }
        }
}
