namespace MonitoringBatchService.Models
{
    public class SqlmnlGetProducts
    {
        public string? PRODUCT_ID { get; set; }

        public string? PRODUCT_NAME { get; set; }

        public string? PRODUCT_GROUP { get; set; }

        public string? PRODUCT_CLASS { get; set; }

        public string? PRODUCT_LINE { get; set; }

        public string? X_ORGUNIT_CODE { get; set; }

        public DateTime CREATE_DATE { get; set; }

        public DateTime? MODIFIED_DATE { get; set; }

        public string? SystemSRC_CD { get; set; }

        public string? Diff_HK { get; set; }
    }
}
