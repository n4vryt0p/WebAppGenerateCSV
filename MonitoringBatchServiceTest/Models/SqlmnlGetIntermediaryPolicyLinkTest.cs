using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonitoringBatchService.Models;

namespace MonitoringBatchServiceTest.Models
{
    public class SqlmnlGetIntermediaryPolicyLinkTest
    {
        
        [Fact]
        public void SqlmnlGetIntermediaryPolicyLink()
        {
            // Arrange
            var keymodel = new SqlmnlGetIntermediaryPolicyLink();

            // Properti nullable (string? dan DateTime?)
            Assert.Null(keymodel.POLICY_ID);
            Assert.Null(keymodel.INTERMEDIARY_ID);
            Assert.Null(keymodel.PARTY_ROLE_CODE);
            Assert.Null(keymodel.From_Date);
            Assert.Null(keymodel.TO_Date);
            Assert.Null(keymodel.X_ORGUNIT_CODE);
            Assert.Null(keymodel.PARTY_RELATIONSHIP);
            Assert.Null(keymodel.MODIFIED_DATE);
            Assert.Null(keymodel.Diff_HK);

            // Properti non-nullable (DateTime)
            Assert.Equal(default(DateTime), keymodel.CREATE_DATE); // atau gunakan nilai yang diharapkan
        }
        }
}
