using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonitoringBatchService.Models;

namespace MonitoringBatchServiceTest.Models
{
    public class SqlmnlGetPoliciesTest
    {
        [Fact]
        public void SqlmnlGetPolicies()
        {
            // Arrange
            var keymodel = new SqlmnlGetPolicies();
            // Properti non-nullable (DateTime)
            Assert.Equal(default(DateTime), keymodel.RUN_TIMESTAMP); // atau gunakan nilai yang diharapkan
            Assert.Equal(default(DateTime), keymodel.CREATE_DATE); // atau gunakan nilai yang diharapkan

            // Properti nullable (string?, DateTime?, decimal?, int?)
            Assert.Null(keymodel.POLICY_SOURCE_UNIQUE_ID);
            Assert.Null(keymodel.PRIMARY_CUSTOMER_ID);
            Assert.Null(keymodel.POLICY_HOLDER_NAME);
            Assert.Null(keymodel.INTERMEDIARY_ID);
            Assert.Null(keymodel.PRODUCT_SOURCE_TYPE_CODE);
            Assert.Null(keymodel.PRODUCT_SOURCE_TYPE_DESC);
            Assert.Null(keymodel.POLICY_STATUS_CODE);
            Assert.Null(keymodel.COUNTRY_CODE);
            Assert.Null(keymodel.CURRENCY_CODE);
            Assert.Null(keymodel.SUBSCRIPTION_DATE);
            Assert.Null(keymodel.EFFECTIVE_DATE);
            Assert.Null(keymodel.SURRENDER_DATE);
            Assert.Null(keymodel.POLICY_VALUE_TD);
            Assert.Null(keymodel.INTERMEDIARY_NAME);
            Assert.Null(keymodel.LAST_SINGLE_PREMIUM_AMOUNT);
            Assert.Null(keymodel.TOTAL_ANNUAL_PREMIUM_TD);
            Assert.Null(keymodel.ORGUNIT_CODE);
            Assert.Null(keymodel.SURRENDER_VALUE_TD);
            Assert.Null(keymodel.NON_AMORTIZED_VALUE_TD);
            Assert.Null(keymodel.POLICY_COHOLDER_NAME);
            Assert.Null(keymodel.POLICY_INSURED_NAME);
            Assert.Null(keymodel.POLICY_PAYOR_NAME);
            Assert.Null(keymodel.POLICY_DURATION);
            Assert.Null(keymodel.BENEFICIARY_CLAUSE);
            Assert.Null(keymodel.BENEFICIARY_CLAUSE_LAST_UPDATE);
            Assert.Null(keymodel.INITIAL_AMOUNT);
            Assert.Null(keymodel.INSTALLMENT_FREQUENCY);
            Assert.Null(keymodel.LAST_VALUE_DATE);
            Assert.Null(keymodel.TOTAL_DEPOSIT_TD);
            Assert.Null(keymodel.TOTAL_WITHDRAWAL_TD);
            Assert.Null(keymodel.TOTAL_ADVANCE_TD);
            Assert.Null(keymodel.TOTAL_REIMBURSMENT_TD);
            Assert.Null(keymodel.LAST_WITHDRAWAL_AMOUNT);
            Assert.Null(keymodel.LAST_WITHDRAWAL_DATE);
            Assert.Null(keymodel.LAST_ADVANCE_AMOUNT);
            Assert.Null(keymodel.LAST_ADVANCE_DATE);
            Assert.Null(keymodel.LAST_REIMBURSMENT_AMOUNT);
            Assert.Null(keymodel.LAST_REIMBURSMENT_DATE);
            Assert.Null(keymodel.LAST_SINGLE_PREMIUM_DATE);
            Assert.Null(keymodel.X_CHANNEL);
            Assert.Null(keymodel.X_POLICY_END_DATE);
            Assert.Null(keymodel.BRANCH_ID);
            Assert.Null(keymodel.BRANCH_NAME);
            Assert.Null(keymodel.Diff_HK);
            Assert.Null(keymodel.MODIFIED_DATE);
        }
        }
}
