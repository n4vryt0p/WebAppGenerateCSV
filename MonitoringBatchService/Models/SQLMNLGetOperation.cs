namespace MonitoringBatchService.Models
{
    public class SqlmnlGetOperation
    {
        public string? Entity_CD { get; set; }

        public string? SystemSRC_CD { get; set; }

        public string? TXN_TYPE_CODE { get; set; }

        public string? TXN_TYPE_DESC { get; set; }

        public DateTime RUN_TIMESTAMP { get; set; }

        public string? OPERATION_SOURCE_UNIQUE_ID { get; set; }

        public string? POLICY_ID { get; set; }

        public string? CUSTOMER_ID { get; set; }

        public string? INTERMEDIARY_ID { get; set; }

        public string? TXN_CHANNEL_CODE { get; set; }

        public decimal? TXN_AMOUNT_BASE { get; set; }

        public string? CURRENCY_CODE_BASE { get; set; }

        public decimal? TXN_AMOUNT_ORIG { get; set; }

        public string? CURRENCY_CODE_ORIG { get; set; }

        public string? CREDIT_DEBIT_CODE { get; set; }

        public string? PAYMENT_METHOD { get; set; }

        public string? IBAN { get; set; }

        public string? FOREIGN_FLAG { get; set; }

        public string? ORGUNIT_CODE { get; set; }

        public string? BIC { get; set; }

        public string? ACCOUNT_NUMBER { get; set; }

        public string? SOURCE_OF_FUNDS_FLAG { get; set; }

        public string? UNUSUAL_PAYMENT_METHOD_FLAG { get; set; }

        public DateTime X_OPERATION_DATE { get; set; }

        public string? X_OPERATION_SOURCE_SYSTEM { get; set; }

        public string? X_INSURED_SOURCE_UNIQUE_ID { get; set; }

        public string? X_POLICY_BENEFICIARY_SOURCE_UNIQUE_ID { get; set; }

        public string? X_COHOLDER_SOURCE_UNIQUE_ID { get; set; }

        public string? X_POLICY_PAYER_SOURCE_UNIQUE_ID { get; set; }

        public string? X_HOLDER_COUNTRY_OF_RESIDENCE { get; set; }

        public string? X_POLICY_HOLDER_TYPE { get; set; }

        public string? X_MAIN_TXN_TYPE_CODE { get; set; }

        public string? X_BUSINESS_LINE { get; set; }

        public string? X_BUSINESS_SUBLINE { get; set; }

        public string? BRANCH_ID { get; set; }

        public string? REIMBURSEMENT_FLAG { get; set; }

        public string? PROGRAMMED_FLAG { get; set; }

        public string? REJECTED_FLAG { get; set; }

        public string? BENEFICIARY_CLAUSE { get; set; }

        public DateTime? X_SUBSCRIPTION_DATE { get; set; }

        public DateTime? X_EFFECTIVE_DATE { get; set; }

        public DateTime? X_SURRENDER_DATE { get; set; }

        public decimal? X_POLICY_VALUE_TD { get; set; }

        public decimal? X_CAPITAL_LOST { get; set; }

        public decimal? X_EXPECTED_ANNUAL_TURNOVER { get; set; }

        public string? X_BANK_COUNTRY_CODE { get; set; }

        public string? X_INTERMEDIARY_SEGMENT { get; set; }

        public string? X_OLD_CHG_SOURCE_UNIQUE_ID { get; set; }

        public string? X_HOLDER_SPECIAL_MONITORING { get; set; }

        public string? X_COHOLDER_SPECIAL_MONITORING { get; set; }

        public string? X_INSURED_SPECIAL_MONITORING { get; set; }

        public string? X_BENEFICIARY_SPECIAL_MONITORING { get; set; }

        public string? X_PAYER_SPECIAL_MONITORING { get; set; }

        public decimal? X_TOTAL_POLICIES_VALUE_TD { get; set; }

        public decimal? X_TOTAL_ASSET_HOLDER { get; set; }

        public decimal? X_TXT_GROSS_INVST_AMOUNT_BASE { get; set; }

        public int? X_PENSION_AGE { get; set; }

        public int? X_HOLDER_AGE { get; set; }

        public string? X_PRODUCT_ID { get; set; }

        public string? X_H_COUNTRY_OF_TAX_RESIDENCE { get; set; }

        public DateTime CREATE_DATE { get; set; }

        public DateTime? MODIFIED_DATE { get; set; }

        public string? Diff_HK { get; set; }
    }
}
