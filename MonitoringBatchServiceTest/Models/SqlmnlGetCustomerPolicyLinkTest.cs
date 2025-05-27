using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonitoringBatchService.Models;

namespace MonitoringBatchServiceTest.Models
{
    public class SqlmnlGetCustomerPolicyLinkTest
    {

        [Fact]
        public void SqlmnlGetCustomerPolicyLink()
        {
            // Arrange
            var keymodel = new SqlmnlGetCustomerPolicyLink();

            Assert.Null(keymodel.POLICY_ID);
            Assert.Null(keymodel.CUSTOMER_ID);
            Assert.Null(keymodel.PARTY_ROLE_CODE);
            Assert.Null(keymodel.PARTY_RELATIONSHIP);
            Assert.Null(keymodel.IS_PAYOR);
            Assert.Null(keymodel.X_ORGUNIT_CODE);
            Assert.Null(keymodel.MODIFIED_DATE);
            Assert.Null(keymodel.CustPolicy_HK);
            Assert.Null(keymodel.Diff_HK);
            Assert.Equal(default(DateTime), keymodel.FROM_DATE); // Contoh nilai yang diharapkan
            Assert.Equal(default(DateTime), keymodel.TO_DATE); // Contoh nilai yang diharapkan
            Assert.Equal(default(DateTime), keymodel.LOGDATE); // Contoh nilai yang diharapkan
            Assert.Equal(default(DateTime), keymodel.CREATE_DATE); // Contoh nilai yang diharapkan
        }
        }
}
