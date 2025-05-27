using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonitoringBatchService.Models;

namespace MonitoringBatchServiceTest.Models
{
    public class SqlmnlGetOperationTest
    {
        [Fact]
        public void SqlmnlGetOperationTestopr()
        {
            // Arrange
            var keymodel = new SqlmnlGetOperation();

            // Properti nullable (string?, DateTime?, decimal?, int?)
            Assert.Null(keymodel.Entity_CD);
            Assert.Null(keymodel.SystemSRC_CD);
            Assert.Null(keymodel.TXN_TYPE_CODE);
            Assert.Null(keymodel.TXN_TYPE_DESC);
            Assert.Null(keymodel.OPERATION_SOURCE_UNIQUE_ID);
            Assert.Null(keymodel.POLICY_ID);
            Assert.Null(keymodel.CUSTOMER_ID);
            Assert.Null(keymodel.INTERMEDIARY_ID);
            Assert.Null(keymodel.TXN_CHANNEL_CODE);
            Assert.Null(keymodel.TXN_AMOUNT_BASE);
            Assert.Null(keymodel.CURRENCY_CODE_BASE);
            Assert.Null(keymodel.TXN_AMOUNT_ORIG);
            Assert.Null(keymodel.CURRENCY_CODE_ORIG);
            Assert.Null(keymodel.CREDIT_DEBIT_CODE);
            Assert.Null(keymodel.PAYMENT_METHOD);
            Assert.Null(keymodel.IBAN);
            Assert.Null(keymodel.FOREIGN_FLAG);
            Assert.Null(keymodel.ORGUNIT_CODE);
            Assert.Null(keymodel.BIC);
            Assert.Null(keymodel.ACCOUNT_NUMBER);
            Assert.Null(keymodel.SOURCE_OF_FUNDS_FLAG);
            Assert.Null(keymodel.UNUSUAL_PAYMENT_METHOD_FLAG);
            Assert.Null(keymodel.X_OPERATION_SOURCE_SYSTEM);
            Assert.Null(keymodel.X_INSURED_SOURCE_UNIQUE_ID);
            Assert.Null(keymodel.X_POLICY_BENEFICIARY_SOURCE_UNIQUE_ID);
            Assert.Null(keymodel.X_COHOLDER_SOURCE_UNIQUE_ID);
            Assert.Null(keymodel.X_POLICY_PAYER_SOURCE_UNIQUE_ID);
            Assert.Null(keymodel.X_HOLDER_COUNTRY_OF_RESIDENCE);
            Assert.Null(keymodel.X_POLICY_HOLDER_TYPE);
            Assert.Null(keymodel.X_MAIN_TXN_TYPE_CODE);
            Assert.Null(keymodel.X_BUSINESS_LINE);
            Assert.Null(keymodel.X_BUSINESS_SUBLINE);
            Assert.Null(keymodel.BRANCH_ID);
            Assert.Null(keymodel.REIMBURSEMENT_FLAG);
            Assert.Null(keymodel.PROGRAMMED_FLAG);
            Assert.Null(keymodel.REJECTED_FLAG);
            Assert.Null(keymodel.BENEFICIARY_CLAUSE);
            Assert.Null(keymodel.X_SUBSCRIPTION_DATE);
            Assert.Null(keymodel.X_EFFECTIVE_DATE);
            Assert.Null(keymodel.X_SURRENDER_DATE);
            Assert.Null(keymodel.X_POLICY_VALUE_TD);
            Assert.Null(keymodel.X_CAPITAL_LOST);
            Assert.Null(keymodel.X_EXPECTED_ANNUAL_TURNOVER);
            Assert.Null(keymodel.X_BANK_COUNTRY_CODE);
            Assert.Null(keymodel.X_INTERMEDIARY_SEGMENT);
            Assert.Null(keymodel.X_OLD_CHG_SOURCE_UNIQUE_ID);
            Assert.Null(keymodel.X_HOLDER_SPECIAL_MONITORING);
            Assert.Null(keymodel.X_COHOLDER_SPECIAL_MONITORING);
            Assert.Null(keymodel.X_INSURED_SPECIAL_MONITORING);
            Assert.Null(keymodel.X_BENEFICIARY_SPECIAL_MONITORING);
            Assert.Null(keymodel.X_PAYER_SPECIAL_MONITORING);
            Assert.Null(keymodel.X_TOTAL_POLICIES_VALUE_TD);
            Assert.Null(keymodel.X_TOTAL_ASSET_HOLDER);
            Assert.Null(keymodel.X_TXT_GROSS_INVST_AMOUNT_BASE);
            Assert.Null(keymodel.X_PENSION_AGE);
            Assert.Null(keymodel.X_HOLDER_AGE);
            Assert.Null(keymodel.X_PRODUCT_ID);
            Assert.Null(keymodel.X_H_COUNTRY_OF_TAX_RESIDENCE);
            Assert.Null(keymodel.MODIFIED_DATE);
            Assert.Null(keymodel.Diff_HK);

            // Properti non-nullable (DateTime)
            Assert.Equal(default(DateTime), keymodel.RUN_TIMESTAMP); // atau gunakan nilai yang diharapkan
            Assert.Equal(default(DateTime), keymodel.X_OPERATION_DATE); // atau gunakan nilai yang diharapkan
            Assert.Equal(default(DateTime), keymodel.CREATE_DATE); // atau gunakan nilai yang diharapkan

        }
        }
    }
