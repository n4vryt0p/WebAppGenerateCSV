namespace MonitoringBatchService.Models
{
    public class SqlmnlGetCustomerCustomerLink
    {
        public string? CUSTOMER1_SOURCE_UNIQUE_ID { get; set; }

        public string? CUSTOMER2_SOURCE_UNIQUE_ID { get; set; }

        public string? LINK_TYPE { get; set; }

        public int? PERCENTAGE_OWNERSHIP { get; set; }

        public string? ULTIMATE_BENEFICIAL_OWNER_FLAG { get; set; }

        public string? X_ORGUNIT_CODE { get; set; }

        public DateTime FROM_TIMESTAMP { get; set; }

        public DateTime TO_TIMESTAMP { get; set; }

        public DateTime CREATE_DATE { get; set; }

        public DateTime? MODIFIED_DATE { get; set; }

    }
}