namespace MonitoringBatchService.Models
{
    public class SqlmnlGetIntermediaries
    {

        public DateTime RUN_TIMESTAMP { get; set; }

        public string? INTERMEDIARY_SOURCE_UNIQUE_ID { get; set; }

        public string? INTERMEDIARY_NAME { get; set; }

        public string? INTERMEDIARY_TYPE_CODE { get; set; }

        public string? COUNTRY_CODE { get; set; }

        public DateTime? FROM_DATE { get; set; }

        public DateTime? TO_DATE { get; set; }

        public string? ORGUNIT_CODE { get; set; }

        public string? ADDRESS { get; set; }

        public string? POSTAL_CODE { get; set; }

        public string? CITY { get; set; }

        public string? PHONE_NUMBER { get; set; }

        public string? FAX_NUMBER { get; set; }

        public string? EMAIL_ADDRESS { get; set; }

        public string? TARGET_MARKET { get; set; }

        public DateTime? APPROVED_FROM { get; set; }

        public DateTime? APPROVED_TO { get; set; }

        public string? X_DISTRIBUTOR_TYPE_CODE { get; set; }

        public string? X_STATUS_CODE { get; set; }

        public DateTime? X_CANCELLED_DATE { get; set; }

        public string? X_COUNTRY_OF_HQ { get; set; }

        public string? EMPLOYEE_ID { get; set; }

        public string? RISK_LEVEL { get; set; }

        public Single? RISK_SCORE { get; set; }

        public DateTime CREATE_DATE { get; set; }

        public DateTime? MODIFIED_DATE { get; set; }

        public string? INTERMEDIARIES_HK { get; set; }

        public string? Diff_HK { get; set; }


    }
}
