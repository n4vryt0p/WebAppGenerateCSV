namespace MonitoringBatchService.Models
{
    public class SqlmnlGetPolicies
    {
        public DateTime RUN_TIMESTAMP { get; set; }

        public string? POLICY_SOURCE_UNIQUE_ID { get; set; }

        public string? PRIMARY_CUSTOMER_ID { get; set; }

        public string? POLICY_HOLDER_NAME { get; set; }

        public string? INTERMEDIARY_ID { get; set; }

        public string? PRODUCT_SOURCE_TYPE_CODE { get; set; }

        public string? PRODUCT_SOURCE_TYPE_DESC { get; set; }

        public string? POLICY_STATUS_CODE { get; set; }

        public string? COUNTRY_CODE { get; set; }

        public string? CURRENCY_CODE { get; set; }

        public DateTime? SUBSCRIPTION_DATE { get; set; }

        public DateTime? EFFECTIVE_DATE { get; set; }

        public DateTime? SURRENDER_DATE { get; set; }

        public decimal? POLICY_VALUE_TD { get; set; }

        public string? INTERMEDIARY_NAME { get; set; }

        public decimal? LAST_SINGLE_PREMIUM_AMOUNT { get; set; }

        public decimal? TOTAL_ANNUAL_PREMIUM_TD { get; set; }

        public string? ORGUNIT_CODE { get; set; }

        public decimal? SURRENDER_VALUE_TD { get; set; }

        public decimal? NON_AMORTIZED_VALUE_TD { get; set; }

        public string? POLICY_COHOLDER_NAME { get; set; }

        public string? POLICY_INSURED_NAME { get; set; }

        public string? POLICY_PAYOR_NAME { get; set; }

        public int? POLICY_DURATION { get; set; }

        public string? BENEFICIARY_CLAUSE { get; set; }

        public DateTime? BENEFICIARY_CLAUSE_LAST_UPDATE { get; set; }

        public decimal? INITIAL_AMOUNT { get; set; }

        public string? INSTALLMENT_FREQUENCY { get; set; }

        public DateTime? LAST_VALUE_DATE { get; set; }

        public decimal? TOTAL_DEPOSIT_TD { get; set; }

        public decimal? TOTAL_WITHDRAWAL_TD { get; set; }

        public decimal? TOTAL_ADVANCE_TD { get; set; }

        public decimal? TOTAL_REIMBURSMENT_TD { get; set; }

        public decimal? LAST_WITHDRAWAL_AMOUNT { get; set; }

        public DateTime? LAST_WITHDRAWAL_DATE { get; set; }

        public decimal? LAST_ADVANCE_AMOUNT { get; set; }

        public DateTime? LAST_ADVANCE_DATE { get; set; }

        public decimal? LAST_REIMBURSMENT_AMOUNT { get; set; }

        public DateTime? LAST_REIMBURSMENT_DATE { get; set; }

        public DateTime? LAST_SINGLE_PREMIUM_DATE { get; set; }

        public string? X_CHANNEL { get; set; }

        public DateTime? X_POLICY_END_DATE { get; set; }

        public string? BRANCH_ID { get; set; }

        public string? BRANCH_NAME { get; set; }

        public string? Diff_HK { get; set; }

        public DateTime CREATE_DATE { get; set; }

        public DateTime? MODIFIED_DATE { get; set; }

    }
}
