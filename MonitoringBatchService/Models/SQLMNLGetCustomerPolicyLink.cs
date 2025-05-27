namespace MonitoringBatchService.Models
{
    public class SqlmnlGetCustomerPolicyLink
    {
        public string? POLICY_ID { get; set; }

        public string? CUSTOMER_ID { get; set; }

        public string? PARTY_ROLE_CODE { get; set; }

        public string? PARTY_RELATIONSHIP { get; set; }

        public string? IS_PAYOR { get; set; }

        public string? X_ORGUNIT_CODE { get; set; }

        public DateTime FROM_DATE { get; set; }

        public DateTime TO_DATE { get; set; }

        public DateTime LOGDATE { get; set; }

        public DateTime CREATE_DATE { get; set; }

        public DateTime? MODIFIED_DATE { get; set; }

        public string? CustPolicy_HK { get; set; }

        public string? Diff_HK { get; set; }
    }
}
