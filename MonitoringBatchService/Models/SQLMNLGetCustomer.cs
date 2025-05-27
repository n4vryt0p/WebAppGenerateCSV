namespace MonitoringBatchService.Models
{
    public class SqlmnGetCustomer
    {
        public string? CUSTOMER_SOURCE_UNIQUE_ID { get; set; }

        public string? ORGUNIT_CODE { get; set; }

        public string? CUSTOMER_NAME { get; set; }

        public string? COMPANY_FORM { get; set; }

        public string? INCORPORATION_COUNTRY_CODE { get; set; }

        public string? BUSINESS_SEGMENT_1 { get; set; }

        public DateTime? DATE_OF_BIRTH { get; set; }

        public string? ADDRESS { get; set; }

        public string? ZONE { get; set; }

        public string? COUNTRY_OF_RESIDENCE { get; set; }

        public string? COUNTRY_OF_ORIGIN { get; set; }

        public string? GENDER_CODE { get; set; }

        public string? PRIME_BRANCH_ID { get; set; }

        public string? RELATIONSHIP_MGR_ID { get; set; }

        public string? NATIONALITY_CODE { get; set; }

        public string? OCCUPATION { get; set; }

        public DateTime? CANCELLED_DATE { get; set; }

        public string? CUSTOMER_TYPE_CODE { get; set; }

        public string? CUSTOMER_STATUS_CODE { get; set; }

        public decimal? EXPECTED_ANNUAL_TURNOVER { get; set; }

        public decimal? BALANCE_SHEET_TOTAL { get; set; }

        public string? CUSTOMER_CATEGORY_CODE { get; set; }

        public string? COUNTRY_OF_TAX_RESIDENCE { get; set; }

        public string? COUNTRY_OF_HQ { get; set; }

        public string? COUNTRY_OF_OPERATIONS { get; set; }

        public string? IDENTIFICATION_NUMBER { get; set; }

        public string? ISSUING_AUTHORITY { get; set; }

        public string? COUNTRY_CODE { get; set; }

        public DateTime VALID_FROM { get; set; }

        public DateTime VALID_TO { get; set; }

        public DateTime RUN_TIMESTAMP { get; set; }

        public string? VAT_NUMBER { get; set; }

        public DateTime? DORMANT_OVERRIDE_DATE { get; set; }

        public string? PERSON_TITLE { get; set; }

        public string? FIRST_NAME { get; set; }

        public string? MIDDLE_NAMES { get; set; }

        public string? LAST_NAME { get; set; }

        public string? SUFFIX { get; set; }

        public string? COMPANY_NAME { get; set; }

        public string? REGISTERED_NUMBER { get; set; }

        public DateTime? INCORPORATION_DATE { get; set; }

        public string? BUSINESS_SEGMENT { get; set; }

        public string? INITIALS { get; set; }

        public string? NAME_OF_BIRTH { get; set; }

        public string? PLACE_OF_BIRTH { get; set; }

        public string? EMPLOYEE_FLAG { get; set; }

        public string? EMPLOYEE_NUMBER { get; set; }

        public string? ALTERNATE_NAME { get; set; }

        public string? CITY { get; set; }

        public string? POSTAL_CODE { get; set; }

        public DateTime? ADDRESS_VALID_FROM { get; set; }

        public DateTime? ADDRESS_VALID_TO { get; set; }

        public DateTime? ACQUISITION_DATE { get; set; }

        public string? RESIDENCE_FLAG { get; set; }

        public string? SPECIAL_ATTENTION_FLAG { get; set; }

        public string? DECEASED_FLAG { get; set; }

        public string? COMPLEX_STRUCTURE { get; set; }

        public string? BLACK_LISTED_FLAG { get; set; }

        public string? EMAIL { get; set; }

        public DateTime? EMAIL_VALID_FROM { get; set; }

        public DateTime? EMAIL_VALID_TO { get; set; }

        public string? PHONE_COUNTRY_CODE { get; set; }

        public string? PHONE_AREA_CODE { get; set; }

        public string? PHONE_NUMBER { get; set; }

        public string? PHONE_EXTENSION { get; set; }

        public DateTime? PHONE_VALID_FROM { get; set; }

        public DateTime? PHONE_VALID_TO { get; set; }

        public string? TAX_NUMBER { get; set; }

        public string? TAX_NUMBER_ISSUED_BY { get; set; }

        public string? IDENTIFICATION_TYPE { get; set; }

        public string? TAX_NUMBER_TYPE { get; set; }

        public string? Customer_Identification { get; set; }

        public DateTime? IDDoc_VALID_FROM { get; set; }

        public DateTime? IDDoc_VALID_TO { get; set; }

        public string? X_SUBSCRIPTION_KEYWORD { get; set; }

        public string? X_SOURCE_SYSTEM { get; set; }

        public string? X_SENSITIVE_CUSTOMER_FLAG { get; set; }

        public string? X_NEW_CUSTOMER_SOURCE_UNIQUE_ID { get; set; }

        public string? X_OLD_CUSTOMER_SOURCE_UNIQUE_ID { get; set; }

        public string? X_CUSTOMER_INTERMEDIARY_REF_ID { get; set; }

        public DateTime? X_SCREENING_END_DATE { get; set; }

        public decimal? WIRE_IN_VOLUME { get; set; }

        public decimal? WIRE_OUT_VOLUME { get; set; }

        public decimal? CASH_IN_VOLUME { get; set; }

        public decimal? CASH_OUT_VOLUME { get; set; }

        public decimal? CHECK_IN_VOLUME { get; set; }

        public decimal? CHECK_OUT_VOLUME { get; set; }

        public decimal? OVERALL_SCORE_ADJUSTMENT { get; set; }

        public string? OWN_AFFILIATE_FLAG { get; set; }

        public string? MARKETING_SERVICE_LEVEL { get; set; }

        public string? SANCTIONED_FLAG_INGESTED { get; set; }

        public string? PEP_TYPE_INGESTED { get; set; }

        public string? RCA_FLAG_INGESTED { get; set; }

        public string? BUSINESS_CLASSIFICATION_CODE { get; set; }

        public string? BUSINESS_CLASSIFICATION_SYSTEM { get; set; }

        public string? CUSTOMER_CHANNEL_REMOTE_FLAG { get; set; }

        public string? NATIONALITY_CODE_2 { get; set; }

        public string? DESCRIPTION { get; set; }

        public string? DETAILS { get; set; }

        public string? VISA_TYPE { get; set; }

        public string? BUSINESS_TYPE { get; set; }

        public string? BUSINESS_SEGMENT_2 { get; set; }

        public string? MARITAL_STATUS { get; set; }

        public string? EMPLOYMENT_STATUS { get; set; }

        public string? CUSTOMER_SEGMENT_1 { get; set; }

        public string? CUSTOMER_SEGMENT_2 { get; set; }

        public string? CUSTOMER_SEGMENT_3 { get; set; }

        public float? RISK_SCORE { get; set; }

        public string? BANKRUPT_FLAG { get; set; }

        public string? COMPENSATION_REQD_FLAG { get; set; }

        public string? CUSTOMER_COMPLAINT_FLAG { get; set; }

        public string? END_RELATIONSHIP_FLAG { get; set; }

        public string? MERCHANT_NUMBER { get; set; }

        public string? FACE_TO_FACE_FLAG { get; set; }

        public string? NEAR_BORDER_FLAG { get; set; }

        public string? INTENDED_PRODUCT_USE { get; set; }

        public string? SOURCE_OF_FUNDS { get; set; }

        public int? TRADING_DURATION { get; set; }

        public string? BROKER_CODE { get; set; }

        public string? DOMAIN_CODE { get; set; }

        public string? COMMENTS { get; set; }

        public string? PEP_FLAG_INGESTED { get; set; }

        public int? WIRE_IN_NUMBER { get; set; }

        public int? WIRE_OUT_NUMBER { get; set; }

        public string? channel { get; set; }

        public int? AGE { get; set; }

        public DateTime CREATE_DATE { get; set; }

        public DateTime? MODIFIED_DATE { get; set; }
    }
}
