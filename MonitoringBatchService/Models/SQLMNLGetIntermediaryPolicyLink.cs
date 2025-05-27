namespace MonitoringBatchService.Models
{
    public class SqlmnlGetIntermediaryPolicyLink
    {
        public string? POLICY_ID { get; set; }

        public string? INTERMEDIARY_ID { get; set; }

        public string? PARTY_ROLE_CODE { get; set; }

        public DateTime? From_Date { get; set; }

        public DateTime? TO_Date { get; set; }

        public string? X_ORGUNIT_CODE { get; set; }

        public string? PARTY_RELATIONSHIP { get; set; }

        public DateTime CREATE_DATE { get; set; }

        public DateTime? MODIFIED_DATE { get; set; }

        public string? Diff_HK { get; set; }

    }
}
