namespace MonitoringBatchService.Models
{
    public class SqlmnlGetProductSourceType
    {
        public string? PRODUCT_ID { get; set; }

        public string? PRODUCT_SOURCE_TYPE_CODE { get; set; }

        public string? PRODUCT_SOURCE_TYPE_DESC { get; set; }

        public string? X_ORGUNIT_CODE { get; set; }

        public DateTime CREATE_DATE { get; set; }

        public DateTime? MODIFIED_DATE { get; set; }

    }
}
