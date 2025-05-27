
using Microsoft.Data.SqlClient;

namespace AF.DAL
{
	public class CustomerID
	{
		public string RUN_TIMESTAMP { get; set; }
		public string CUSTOMER_SOURCE_UNIQUE_ID { get; set; }
		public string ORGUNIT_CODE { get; set; }
		public string PERSON_TITLE { get; set; }
		public string FIRST_NAME { get; set; }
		public string MIDDLE_NAMES { get; set; }
		public string LAST_NAME { get; set; }
		public string SUFFIX { get; set; }
		public string CUSTOMER_NAME { get; set; }
		public string COMPANY_NAME { get; set; }
		public string COMPANY_FORM { get; set; }
		public string REGISTERED_NUMBER { get; set; }
		public string INCORPORATION_DATE { get; set; }
		public string INCORPORATION_COUNTRY_CODE { get; set; }
		public string BUSINESS_TYPE { get; set; }
		public string BUSINESS_SEGMENT_1 { get; set; }
		public string BUSINESS_SEGMENT_2 { get; set; }
		public string INITIALS { get; set; }
		public string DATE_OF_BIRTH { get; set; }
		public string NAME_OF_BIRTH { get; set; }
		public string ADDRESS { get; set; }
		public string ZONE { get; set; }
		public string CITY { get; set; }
		public string POSTAL_CODE { get; set; }
		public string COUNTRY_OF_RESIDENCE { get; set; }
		public string COUNTRY_OF_ORIGIN { get; set; }
		public string NATIONALITY_CODE { get; set; }
		public string NATIONALITY_CODE_2 { get; set; }
		public string PLACE_OF_BIRTH { get; set; }
		public string GENDER_CODE { get; set; }
		public string PRIME_BRANCH_ID { get; set; }
		public string RELATIONSHIP_MGR_ID { get; set; }
		public string EMPLOYEE_FLAG { get; set; }
		public string EMPLOYEE_NUMBER { get; set; }
		public string MARITAL_STATUS { get; set; }
		public string OCCUPATION { get; set; }
		public string EMPLOYMENT_STATUS { get; set; }
		public string ACQUISITION_DATE { get; set; }
		public string CANCELLED_DATE { get; set; }
		public string CUSTOMER_TYPE_CODE { get; set; }
		public string CUSTOMER_STATUS_CODE { get; set; }
		public string CUSTOMER_SEGMENT_1 { get; set; }
		public string CUSTOMER_SEGMENT_2 { get; set; }
		public string CUSTOMER_SEGMENT_3 { get; set; }
		public string RESIDENCE_FLAG { get; set; }
		public string SPECIAL_ATTENTION_FLAG { get; set; }
		public string DECEASED_FLAG { get; set; }
		public string DORMANT_OVERRIDE_DATE { get; set; }
		public string RISK_SCORE { get; set; }
		public string BANKRUPT_FLAG { get; set; }
		public string COMPENSATION_REQD_FLAG { get; set; }
		public string CUSTOMER_COMPLAINT_FLAG { get; set; }
		public string END_RELATIONSHIP_FLAG { get; set; }
		public string MERCHANT_NUMBER { get; set; }
		public string FACE_TO_FACE_FLAG { get; set; }
		public string CHANNEL { get; set; }
		public string NEAR_BORDER_FLAG { get; set; }
		public string INTENDED_PRODUCT_USE { get; set; }
		public string SOURCE_OF_FUNDS { get; set; }
		public string COMPLEX_STRUCTURE { get; set; }
		public string EXPECTED_ANNUAL_TURNOVER { get; set; }
		public string TRADING_DURATION { get; set; }
		public string BALANCE_SHEET_TOTAL { get; set; }
		public string VAT_NUMBER { get; set; }
		public string BROKER_CODE { get; set; }
		public string BLACK_LISTED_FLAG { get; set; }
		public string DOMAIN_CODE { get; set; }
		public string COMMENTS { get; set; }
		public string PEP_FLAG_INGESTED { get; set; }
		public string WIRE_IN_NUMBER { get; set; }
		public string WIRE_OUT_NUMBER { get; set; }
		public string WIRE_IN_VOLUME { get; set; }
		public string WIRE_OUT_VOLUME { get; set; }
		public string CASH_IN_VOLUME { get; set; }
		public string CASH_OUT_VOLUME { get; set; }
		public string CHECK_IN_VOLUME { get; set; }
		public string CHECK_OUT_VOLUME { get; set; }
		public string OVERALL_SCORE_ADJUSTMENT { get; set; }
		public string TAX_NUMBER { get; set; }
		public string TAX_NUMBER_ISSUED_BY { get; set; }
		public string CUSTOMER_CATEGORY_CODE { get; set; }
		public string OWN_AFFILIATE_FLAG { get; set; }
		public string MARKETING_SERVICE_LEVEL { get; set; }
		public string SANCTIONED_FLAG_INGESTED { get; set; }
		public string PEP_TYPE_INGESTED { get; set; }
		public string RCA_FLAG_INGESTED { get; set; }
		public string ADDRESS_VALID_FROM { get; set; }
		public string ADDRESS_VALID_TO { get; set; }
		public string EMAIL { get; set; }
		public string EMAIL_VALID_FROM { get; set; }
		public string EMAIL_VALID_TO { get; set; }
		public string PHONE_COUNTRY_CODE { get; set; }
		public string PHONE_AREA_CODE { get; set; }
		public string PHONE_NUMBER { get; set; }
		public string PHONE_EXTENSION { get; set; }
		public string PHONE_VALID_FROM { get; set; }
		public string PHONE_VALID_TO { get; set; }
		public string ALTERNATE_NAME { get; set; }
		public string TAX_NUMBER_TYPE { get; set; }
		public string BUSINESS_CLASSIFICATION_CODE { get; set; }
		public string BUSINESS_CLASSIFICATION_SYSTEM { get; set; }
		public string CUSTOMER_CHANNEL_REMOTE_FLAG { get; set; }
		public string COUNTRY_OF_TAX_RESIDENCE { get; set; }
		public string COUNTRY_OF_HQ { get; set; }
		public string COUNTRY_OF_OPERATIONS { get; set; }
		public List<Customer_Identification> Customer_Identification { get; set; }
		public string DENTIFICATION_NUMBER { get; set; }
		public string ISSUING_AUTHORITY { get; set; }
		public string COUNTRY_CODE { get; set; }
		public string IDENTIFICATION_TYPE { get; set; }
		public string VALID_FROM { get; set; }
		public string VALID_TO { get; set; }
		public string DESCRIPTION { get; set; }
		public string DETAILS { get; set; }
		public string VISA_TYPE { get; set; }

		public string X_SUBSCRIPTION_KEYWORD { get; set; }
		public string X_SOURCE_SYSTEM { get; set; }
		public string X_SENSITIVE_CUSTOMER_FLAG { get; set; }
		public string X_OLD_CUSTOMER_SOURCE_UNIQUE_ID { get; set; }
		public string X_NEW_CUSTOMER_SOURCE_UNIQUE_ID { get; set; }
		public string X_CUSTOMER_INTERMEDIARY_REF_ID { get; set; }
		public string X_SCREENING_END_DATE { get; set; }


		public static CustomerID getColumns()
		{
			CustomerID c = new CustomerID();
			c.RUN_TIMESTAMP = "character";
			c.CUSTOMER_SOURCE_UNIQUE_ID = "character";
			c.ORGUNIT_CODE = "character";
			c.PERSON_TITLE = "character";
			c.FIRST_NAME = "character";
			c.MIDDLE_NAMES = "character";
			c.LAST_NAME = "character";
			c.SUFFIX = "character";
			c.CUSTOMER_NAME = "character";
			c.COMPANY_NAME = "character";
			c.COMPANY_FORM = "character";
			c.REGISTERED_NUMBER = "character";
			c.INCORPORATION_DATE = "character";
			c.INCORPORATION_COUNTRY_CODE = "character";
			c.BUSINESS_TYPE = "character";
			c.BUSINESS_SEGMENT_1 = "character";
			c.BUSINESS_SEGMENT_2 = "character";
			c.INITIALS = "character";
			c.DATE_OF_BIRTH = "character";
			c.NAME_OF_BIRTH = "character";
			c.ADDRESS = "character";
			c.ZONE = "character";
			c.CITY = "character";
			c.POSTAL_CODE = "character";
			c.COUNTRY_OF_RESIDENCE = "character";
			c.COUNTRY_OF_ORIGIN = "character";

			c.NATIONALITY_CODE = "character";
			c.NATIONALITY_CODE_2 = "character";

			c.PLACE_OF_BIRTH = "character";
			c.GENDER_CODE = "character";
			c.PRIME_BRANCH_ID = "character";
			c.RELATIONSHIP_MGR_ID = "character";
			c.EMPLOYEE_FLAG = "character";
			c.EMPLOYEE_NUMBER = "character";
			c.MARITAL_STATUS = "character";
			c.OCCUPATION = "character";
			c.EMPLOYMENT_STATUS = "character";
			c.ACQUISITION_DATE = "character";
			c.CANCELLED_DATE = "character";
			c.CUSTOMER_TYPE_CODE = "character";
			c.CUSTOMER_STATUS_CODE = "character";
			c.CUSTOMER_SEGMENT_1 = "character";
			c.CUSTOMER_SEGMENT_2 = "character";
			c.CUSTOMER_SEGMENT_3 = "character";
			c.RESIDENCE_FLAG = "character";
			c.SPECIAL_ATTENTION_FLAG = "character";
			c.DECEASED_FLAG = "character";
			c.DORMANT_OVERRIDE_DATE = "character";
			c.RISK_SCORE = "character";
			c.BANKRUPT_FLAG = "character";
			c.COMPENSATION_REQD_FLAG = "character";
			c.CUSTOMER_COMPLAINT_FLAG = "character";
			c.END_RELATIONSHIP_FLAG = "character";
			c.MERCHANT_NUMBER = "character";
			c.FACE_TO_FACE_FLAG = "character";
			c.CHANNEL = "character";
			c.NEAR_BORDER_FLAG = "character";
			c.INTENDED_PRODUCT_USE = "character";
			c.SOURCE_OF_FUNDS = "character";
			c.COMPLEX_STRUCTURE = "character";
			c.EXPECTED_ANNUAL_TURNOVER = "character";
			c.TRADING_DURATION = "character";
			c.BALANCE_SHEET_TOTAL = "character";
			c.VAT_NUMBER = "character";
			c.BROKER_CODE = "character";
			c.BLACK_LISTED_FLAG = "character";
			c.DOMAIN_CODE = "character";
			c.COMMENTS = "character";
			c.PEP_FLAG_INGESTED = "character";
			c.WIRE_IN_NUMBER = "character";
			c.WIRE_OUT_NUMBER = "character";
			c.WIRE_IN_VOLUME = "character";
			c.WIRE_OUT_VOLUME = "character";
			c.CASH_IN_VOLUME = "character";
			c.CASH_OUT_VOLUME = "character";
			c.CHECK_IN_VOLUME = "character";
			c.CHECK_OUT_VOLUME = "character";
			c.OVERALL_SCORE_ADJUSTMENT = "character";
			c.TAX_NUMBER = "character";
			c.TAX_NUMBER_ISSUED_BY = "character";
			c.CUSTOMER_CATEGORY_CODE = "character";
			c.OWN_AFFILIATE_FLAG = "character";
			c.MARKETING_SERVICE_LEVEL = "character";
			c.SANCTIONED_FLAG_INGESTED = "character";
			c.PEP_TYPE_INGESTED = "character";
			c.RCA_FLAG_INGESTED = "character";
			c.ADDRESS_VALID_FROM = "character";
			c.ADDRESS_VALID_TO = "character";
			c.EMAIL = "character";
			c.EMAIL_VALID_FROM = "character";
			c.EMAIL_VALID_TO = "character";
			c.PHONE_COUNTRY_CODE = "character";
			c.PHONE_AREA_CODE = "character";
			c.PHONE_NUMBER = "character";
			c.PHONE_EXTENSION = "character";
			c.PHONE_VALID_FROM = "character";
			c.PHONE_VALID_TO = "character";
			c.ALTERNATE_NAME = "character";
			c.TAX_NUMBER_TYPE = "character";
			c.BUSINESS_CLASSIFICATION_CODE = "character";
			c.BUSINESS_CLASSIFICATION_SYSTEM = "character";
			c.CUSTOMER_CHANNEL_REMOTE_FLAG = "character";
			c.COUNTRY_OF_TAX_RESIDENCE = "character";
			c.COUNTRY_OF_HQ = "character";
			c.COUNTRY_OF_OPERATIONS = "character";

			c.X_SUBSCRIPTION_KEYWORD = "character";
			c.X_SOURCE_SYSTEM = "character";
			c.X_SENSITIVE_CUSTOMER_FLAG = "character";
			c.X_OLD_CUSTOMER_SOURCE_UNIQUE_ID = "character";
			c.X_NEW_CUSTOMER_SOURCE_UNIQUE_ID = "character";
			c.X_CUSTOMER_INTERMEDIARY_REF_ID = "character";
			c.X_SCREENING_END_DATE = "character";

			List<Customer_Identification> lci = new List<Customer_Identification>();
			Customer_Identification ci = new Customer_Identification();
			ci.IDENTIFICATION_NUMBER = "character";
			ci.ISSUING_AUTHORITY = "character";
			ci.COUNTRY_CODE = "character";
			ci.IDENTIFICATION_TYPE = "character";
			ci.VALID_FROM = "character";
			ci.VALID_TO = "character";
			ci.DESCRIPTION = "character";
			ci.DETAILS = "character";
			ci.VISA_TYPE = "character";
			lci.Add(ci);
			c.Customer_Identification = lci;

			return c;
		}

	}
	public class Customer_Identification
	{
		public string IDENTIFICATION_NUMBER { get; set; }
		public string ISSUING_AUTHORITY { get; set; }
		public string COUNTRY_CODE { get; set; }
		public string IDENTIFICATION_TYPE { get; set; }
		public string VALID_FROM { get; set; }
		public string VALID_TO { get; set; }
		public string DESCRIPTION { get; set; }
		public string DETAILS { get; set; }
		public string VISA_TYPE { get; set; }

	}
	public class CustomerLinkID
	{
		public string CUSTOMER1_SOURCE_UNIQUE_ID { get; set; }
		public string CUSTOMER2_SOURCE_UNIQUE_ID { get; set; }
		public string LINK_TYPE { get; set; }
		public string PERCENTAGE_OWNERSHIP { get; set; }
		public string ULTIMATE_BENEFICIAL_OWNER_FLAG { get; set; }
		public string FROM_TIMESTAMP { get; set; }
		public string TO_TIMESTAMP { get; set; }
		public string X_ORGUNIT_CODE { get; set; }

		public static CustomerLinkID getColumns()
		{
			CustomerLinkID cl = new CustomerLinkID();
			cl.CUSTOMER1_SOURCE_UNIQUE_ID = "character";
			cl.CUSTOMER2_SOURCE_UNIQUE_ID = "character";
			cl.LINK_TYPE = "character";
			cl.PERCENTAGE_OWNERSHIP = "character";
			cl.ULTIMATE_BENEFICIAL_OWNER_FLAG = "character";
			cl.FROM_TIMESTAMP = "character";
			cl.TO_TIMESTAMP = "character";
			cl.X_ORGUNIT_CODE = "character";

			return cl;
		}
	}
	public class CustomerPolicyLink
	{
		public string POLICY_ID { get; set; }
		public string CUSTOMER_ID { get; set; }
		public string PARTY_ROLE_CODE { get; set; }
		public string PARTY_RELATIONSHIP { get; set; }
		public string FROM_DATE { get; set; }
		public string TO_DATE { get; set; }
		//public string LOGDATE { get; set; }
		public string IS_PAYOR { get; set; }
		public string X_ORGUNIT_CODE { get; set; }

		public static CustomerPolicyLink getColumns()
		{
			CustomerPolicyLink cpl = new CustomerPolicyLink();
			cpl.POLICY_ID = "character";
			cpl.CUSTOMER_ID = "character";
			cpl.PARTY_ROLE_CODE = "character";
			cpl.PARTY_RELATIONSHIP = "character";
			cpl.FROM_DATE = "character";
			cpl.TO_DATE = "character";
			//cpl.LOGDATE = "character";
			cpl.IS_PAYOR = "character";
			cpl.X_ORGUNIT_CODE = "character";
			return cpl;

		}
	}
	public class Intermediaries
	{
		public string RUN_TIMESTAMP { get; set; }
		public string INTERMEDIARY_SOURCE_UNIQUE_ID { get; set; }
		public string EMPLOYEE_ID { get; set; }
		public string INTERMEDIARY_NAME { get; set; }
		public string INTERMEDIARY_TYPE_CODE { get; set; }
		public string ADDRESS { get; set; }
		public string POSTAL_CODE { get; set; }
		public string CITY { get; set; }
		public string COUNTRY_CODE { get; set; }
		public string PHONE_NUMBER { get; set; }
		public string FAX_NUMBER { get; set; }
		public string EMAIL_ADDRESS { get; set; }
		public string TARGET_MARKET { get; set; }
		public string FROM_DATE { get; set; }
		public string TO_DATE { get; set; }
		public string APPROVED_FROM { get; set; }
		public string APPROVED_TO { get; set; }
		public string RISK_LEVEL { get; set; }
		public string RISK_SCORE { get; set; }
		public string ORGUNIT_CODE { get; set; }
		public string X_DISTRIBUTOR_TYPE_CODE { get; set; }
		public string X_STATUS_CODE { get; set; }
		public string X_CANCELLED_DATE { get; set; }
		public string X_COUNTRY_OF_HQ { get; set; }
		

		public static Intermediaries getColumns()
		{
			Intermediaries p = new Intermediaries();
			p.RUN_TIMESTAMP = "character";
			p.INTERMEDIARY_SOURCE_UNIQUE_ID = "character";
			p.EMPLOYEE_ID = "character";
			p.INTERMEDIARY_NAME = "character";
			p.INTERMEDIARY_TYPE_CODE = "character";
			p.ADDRESS = "character";
			p.POSTAL_CODE = "character";
			p.CITY = "character";
			p.COUNTRY_CODE = "character";
			p.PHONE_NUMBER = "character";
			p.FAX_NUMBER = "character";
			p.EMAIL_ADDRESS = "character";
			p.TARGET_MARKET = "character";
			p.FROM_DATE = "character";
			p.TO_DATE = "character";
			p.APPROVED_FROM = "character";
			p.APPROVED_TO = "character";
			p.RISK_LEVEL = "character";
			p.RISK_SCORE = "character";
			p.ORGUNIT_CODE = "character";

			p.X_DISTRIBUTOR_TYPE_CODE = "character";
			p.X_STATUS_CODE = "character";
			p.X_CANCELLED_DATE = "character";
			p.X_COUNTRY_OF_HQ = "character";
			return p;
		}

	}
	public class Intermediary_Policy_Link
	{
		public string POLICY_ID { get; set; }
		public string INTERMEDIARY_ID { get; set; }
		public string PARTY_ROLE_CODE { get; set; }
		public string PARTY_RELATIONSHIP { get; set; }
		public string FROM_DATE { get; set; }
		public string TO_DATE { get; set; }
		public string X_ORGUNIT_CODE { get; set; }
		

		public static Intermediary_Policy_Link getColumns()
		{
			Intermediary_Policy_Link ipl = new Intermediary_Policy_Link();
			ipl.POLICY_ID = "character";
			ipl.INTERMEDIARY_ID = "character";
			ipl.PARTY_ROLE_CODE = "character";
			ipl.PARTY_RELATIONSHIP = "character";
			ipl.FROM_DATE = "character";
			ipl.TO_DATE = "character";
			ipl.X_ORGUNIT_CODE = "character";
			return ipl;
		}
	}
	public class Product
	{
		public string PRODUCT_ID { get; set; }
		public string PRODUCT_NAME { get; set; }
		public string PRODUCT_GROUP { get; set; }
		public string PRODUCT_CLASS { get; set; }
		public string PRODUCT_LINE { get; set; }
		public string X_ORGUNIT_CODE { get; set; }
		
		public static Product getColumns()
		{
			Product prd = new Product();
			prd.PRODUCT_ID = "character";
			prd.PRODUCT_NAME = "character";
			prd.PRODUCT_GROUP = "character";
			prd.PRODUCT_CLASS = "character";
			prd.PRODUCT_LINE = "character";
			prd.X_ORGUNIT_CODE = "character";
			return prd;
		}


	}
	public class ProductSource
	{
		public string PRODUCT_SOURCE_TYPE_CODE { get; set; }
		public string PRODUCT_SOURCE_TYPE_DESC { get; set; }
		public string PRODUCT_ID { get; set; }
		public string X_ORGUNIT_CODE { get; set; }
		public static ProductSource getColumns()
		{
			ProductSource prds = new ProductSource();
			prds.PRODUCT_SOURCE_TYPE_CODE = "character";
			prds.PRODUCT_SOURCE_TYPE_DESC = "character";
			prds.PRODUCT_ID = "character";
			prds.X_ORGUNIT_CODE = "character";
			return prds;
		}

	}
	public class Operation
	{
		public string RUN_TIMESTAMP { get; set; }
		public string OPERATION_SOURCE_UNIQUE_ID { get; set; }
		public string POLICY_ID { get; set; }
		public string CUSTOMER_ID { get; set; }
		public string INTERMEDIARY_ID { get; set; }
		public string BRANCH_ID { get; set; }
		public string TXN_TYPE_CODE { get; set; }
		public string TXN_CHANNEL_CODE { get; set; }
		public string TXN_AMOUNT_BASE { get; set; }
		public string CURRENCY_CODE_BASE { get; set; }
		public string TXN_AMOUNT_ORIG { get; set; }
		public string CURRENCY_CODE_ORIG { get; set; }
		public string CREDIT_DEBIT_CODE { get; set; }
		public string PAYMENT_METHOD { get; set; }
		public string IBAN { get; set; }
		public string BIC { get; set; }
		public string ACCOUNT_NUMBER { get; set; }
		public string FOREIGN_FLAG { get; set; }
		public string SOURCE_OF_FUNDS_FLAG { get; set; }
		public string UNUSUAL_PAYMENT_METHOD_FLAG { get; set; }
		public string REIMBURSEMENT_FLAG { get; set; }
		public string PROGRAMMED_FLAG { get; set; }
		public string REJECTED_FLAG { get; set; }
		public string BENEFICIARY_CLAUSE { get; set; }
		public string ORGUNIT_CODE { get; set; }
		public string X_OPERATION_DATE { get; set; }
		public string X_OPERATION_SOURCE_SYSTEM { get; set; }
		public string X_INSURED_SOURCE_UNIQUE_ID { get; set; }
		public string X_POLICY_BENEFICIARY_SOURCE_UNIQUE_ID { get; set; }
		public string X_COHOLDER_SOURCE_UNIQUE_ID { get; set; }
		public string X_POLICY_PAYER_SOURCE_UNIQUE_ID { get; set; }
		public string X_OLD_CHG_SOURCE_UNIQUE_ID { get; set; }
		public string X_HOLDER_COUNTRY_OF_RESIDENCE { get; set; }
		public string X_POLICY_HOLDER_TYPE { get; set; }
		public string X_HOLDER_SPECIAL_MONITORING { get; set; }
		public string X_COHOLDER_SPECIAL_MONITORING { get; set; }
		public string X_INSURED_SPECIAL_MONITORING { get; set; }
		public string X_BENEFICIARY_SPECIAL_MONITORING { get; set; }
		public string X_PAYER_SPECIAL_MONITORING { get; set; }
		public string X_MAIN_TXN_TYPE_CODE { get; set; }
		public string X_BUSINESS_LINE { get; set; }
		public string X_BUSINESS_SUBLINE { get; set; }
		public string X_SUBSCRIPTION_DATE { get; set; }
		public string X_EFFECTIVE_DATE { get; set; }
		public string X_SURRENDER_DATE { get; set; }
		public string X_POLICY_VALUE_TD { get; set; }
		public string X_TOTAL_POLICIES_VALUE_TD { get; set; }
		public string X_TOTAL_ASSET_HOLDER { get; set; }
		public string X_CAPITAL_LOST { get; set; }
		public string X_EXPECTED_ANNUAL_TURNOVER { get; set; }
		public string X_PRODUCT_ID { get; set; }
		public string X_BANK_COUNTRY_CODE { get; set; }
		public string X_TXT_GROSS_INVST_AMOUNT_BASE { get; set; }
		public string X_PENSION_AGE { get; set; }
		public string X_HOLDER_AGE { get; set; }
		public string X_INTERMEDIARY_SEGMENT { get; set; }
		public string X_H_COUNTRY_OF_TAX_RESIDENCE { get; set; }


		public static Operation getColumns()
		{
			Operation op = new Operation();
			op.RUN_TIMESTAMP = "character";
			op.OPERATION_SOURCE_UNIQUE_ID = "character";
			op.POLICY_ID = "character";
			op.CUSTOMER_ID = "character";
			op.INTERMEDIARY_ID = "character";
			op.BRANCH_ID = "character";
			op.TXN_TYPE_CODE = "character";
			op.TXN_CHANNEL_CODE = "character";
			op.TXN_AMOUNT_BASE = "character";
			op.CURRENCY_CODE_BASE = "character";
			op.TXN_AMOUNT_ORIG = "character";
			op.CURRENCY_CODE_ORIG = "character";
			op.CREDIT_DEBIT_CODE = "character";
			op.PAYMENT_METHOD = "character";
			op.IBAN = "character";
			op.BIC = "character";
			op.ACCOUNT_NUMBER = "character";
			op.FOREIGN_FLAG = "character";
			op.SOURCE_OF_FUNDS_FLAG = "character";
			op.UNUSUAL_PAYMENT_METHOD_FLAG = "character";
			op.REIMBURSEMENT_FLAG = "character";
			op.PROGRAMMED_FLAG = "character";
			op.REJECTED_FLAG = "character";
			op.BENEFICIARY_CLAUSE = "character";
			op.ORGUNIT_CODE = "character";

			op.X_OPERATION_DATE = "character";
			op.X_OPERATION_SOURCE_SYSTEM = "character";
			op.X_INSURED_SOURCE_UNIQUE_ID = "character";
			op.X_POLICY_BENEFICIARY_SOURCE_UNIQUE_ID = "character";
			op.X_COHOLDER_SOURCE_UNIQUE_ID = "character";
			op.X_POLICY_PAYER_SOURCE_UNIQUE_ID = "character";
			op.X_OLD_CHG_SOURCE_UNIQUE_ID = "character";
			op.X_HOLDER_COUNTRY_OF_RESIDENCE = "character";
			op.X_POLICY_HOLDER_TYPE = "character";
			op.X_HOLDER_SPECIAL_MONITORING = "character";
			op.X_COHOLDER_SPECIAL_MONITORING = "character";
			op.X_INSURED_SPECIAL_MONITORING = "character";
			op.X_BENEFICIARY_SPECIAL_MONITORING = "character";
			op.X_PAYER_SPECIAL_MONITORING = "character";
			op.X_MAIN_TXN_TYPE_CODE = "character";
			op.X_BUSINESS_LINE = "character";
			op.X_BUSINESS_SUBLINE = "character";
			op.X_SUBSCRIPTION_DATE = "character";
			op.X_EFFECTIVE_DATE = "character";
			op.X_SURRENDER_DATE = "character";
			op.X_POLICY_VALUE_TD = "character";
			op.X_TOTAL_POLICIES_VALUE_TD = "character";
			op.X_TOTAL_ASSET_HOLDER = "character";
			op.X_CAPITAL_LOST = "character";
			op.X_EXPECTED_ANNUAL_TURNOVER = "character";
			op.X_PRODUCT_ID = "character";
			op.X_BANK_COUNTRY_CODE = "character";
			op.X_TXT_GROSS_INVST_AMOUNT_BASE = "character";
			op.X_PENSION_AGE = "character";
			op.X_HOLDER_AGE = "character";
			op.X_INTERMEDIARY_SEGMENT = "character";
			op.X_H_COUNTRY_OF_TAX_RESIDENCE = "character";


			return op;
		}
	}
	public class Policies
	{
		public string RUN_TIMESTAMP { get; set; }
		public string POLICY_SOURCE_UNIQUE_ID { get; set; }
		public string PRIMARY_CUSTOMER_ID { get; set; }
		public string POLICY_HOLDER_NAME { get; set; }
		public string POLICY_COHOLDER_NAME { get; set; }
		public string POLICY_INSURED_NAME { get; set; }
		public string POLICY_PAYOR_NAME { get; set; }
		public string INTERMEDIARY_ID { get; set; }
		public string INTERMEDIARY_NAME { get; set; }
		public string BRANCH_ID { get; set; }
		public string BRANCH_NAME { get; set; }
		public string PRODUCT_SOURCE_TYPE_CODE { get; set; }
		public string PRODUCT_SOURCE_TYPE_DESC { get; set; }
		public string POLICY_STATUS_CODE { get; set; }
		public string POLICY_DURATION { get; set; }
		public string COUNTRY_CODE { get; set; }
		public string CURRENCY_CODE { get; set; }
		public string BENEFICIARY_CLAUSE { get; set; }
		public string BENEFICIARY_CLAUSE_LAST_UPDATE { get; set; }
		public string SUBSCRIPTION_DATE { get; set; }
		public string EFFECTIVE_DATE { get; set; }
		public string SURRENDER_DATE { get; set; }
		public string INITIAL_AMOUNT { get; set; }
		public string INSTALLMENT_FREQUENCY { get; set; }
		public string POLICY_VALUE_TD { get; set; }
		public string SURRENDER_VALUE_TD { get; set; }
		public string NON_AMORTIZED_VALUE_TD { get; set; }
		public string LAST_VALUE_DATE { get; set; }
		public string TOTAL_DEPOSIT_TD { get; set; }
		public string TOTAL_WITHDRAWAL_TD { get; set; }
		public string TOTAL_ADVANCE_TD { get; set; }
		public string TOTAL_REIMBURSMENT_TD { get; set; }
		public string LAST_WITHDRAWAL_AMOUNT { get; set; }
		public string LAST_WITHDRAWAL_DATE { get; set; }
		public string LAST_ADVANCE_AMOUNT { get; set; }
		public string LAST_ADVANCE_DATE { get; set; }
		public string LAST_REIMBURSMENT_AMOUNT { get; set; }
		public string LAST_REIMBURSMENT_DATE { get; set; }
		public string LAST_SINGLE_PREMIUM_AMOUNT { get; set; }
		public string LAST_SINGLE_PREMIUM_DATE { get; set; }
		public string ORGUNIT_CODE { get; set; }
		public string TOTAL_ANNUAL_PREMIUM_TD { get; set; }
		public string X_CHANNEL { get; set; }
		public string X_POLICY_END_DATE { get; set; }

		public static Policies getColumns()
		{
			Policies pol = new Policies();
			pol.RUN_TIMESTAMP = "character";
			pol.POLICY_SOURCE_UNIQUE_ID = "character";
			pol.PRIMARY_CUSTOMER_ID = "character";
			pol.POLICY_HOLDER_NAME = "character";
			pol.POLICY_COHOLDER_NAME = "character";
			pol.POLICY_INSURED_NAME = "character";
			pol.POLICY_PAYOR_NAME = "character";
			pol.INTERMEDIARY_ID = "character";
			pol.INTERMEDIARY_NAME = "character";
			pol.BRANCH_ID = "character";
			pol.BRANCH_NAME = "character";
			pol.PRODUCT_SOURCE_TYPE_CODE = "character";
			pol.PRODUCT_SOURCE_TYPE_DESC = "character";
			pol.POLICY_STATUS_CODE = "character";
			pol.POLICY_DURATION = "character";
			pol.COUNTRY_CODE = "character";
			pol.CURRENCY_CODE = "character";
			pol.BENEFICIARY_CLAUSE = "character";
			pol.BENEFICIARY_CLAUSE_LAST_UPDATE = "character";
			pol.SUBSCRIPTION_DATE = "character";
			pol.EFFECTIVE_DATE = "character";
			pol.SURRENDER_DATE = "character";
			pol.INITIAL_AMOUNT = "character";
			pol.INSTALLMENT_FREQUENCY = "character";
			pol.POLICY_VALUE_TD = "character";
			pol.SURRENDER_VALUE_TD = "character";
			pol.NON_AMORTIZED_VALUE_TD = "character";
			pol.LAST_VALUE_DATE = "character";
			pol.TOTAL_DEPOSIT_TD = "character";
			pol.TOTAL_WITHDRAWAL_TD = "character";
			pol.TOTAL_ADVANCE_TD = "character";
			pol.TOTAL_REIMBURSMENT_TD = "character";
			pol.LAST_WITHDRAWAL_AMOUNT = "character";
			pol.LAST_WITHDRAWAL_DATE = "character";
			pol.LAST_ADVANCE_AMOUNT = "character";
			pol.LAST_ADVANCE_DATE = "character";
			pol.LAST_REIMBURSMENT_AMOUNT = "character";
			pol.LAST_REIMBURSMENT_DATE = "character";
			pol.LAST_SINGLE_PREMIUM_AMOUNT = "character";
			pol.LAST_SINGLE_PREMIUM_DATE = "character";
			pol.ORGUNIT_CODE = "character";
			pol.TOTAL_ANNUAL_PREMIUM_TD = "character";
			pol.X_CHANNEL = "character";
			pol.X_POLICY_END_DATE = "character";

			return pol;
		}

	}
	public static class ExtractionCanonicalDAO
	{
	
		public static bool ExtractionBundle()
		{
			string mailAddress = AF.DAL.TaskSchedulerDAO.GetEmailAddByID((1).ToString());
			bool vRet = false;
			ExtractionLog l;
			l = new ExtractionLog();
			try { l = ExtractDeltaCustomerSP(); l.FromDate = DateTime.Now; l.ToDate = DateTime.Now; l.LogDate = DateTime.Now; l.ExtractionStatus = true; l.FileTransferStatus = false; l.ExceptionThrown = ""; }
			catch (Exception e) { l.ExtractionStatus = false; l.ExceptionThrown = e.ToString();  }
			ExtractionLog.LogIt(l);
		

			vRet = true;
		

			return vRet;

		}
		public static long RowsCount(string sql, string con)
		{
			long vRet = 0;
			SqlConnection cnn;
			SqlCommand cmd;
			cnn = new SqlConnection(con);
			try
			{
				cnn.Open();
				cmd = new SqlCommand(sql, cnn);
				vRet = Convert.ToInt64(cmd.ExecuteScalar());
				cmd.Dispose();
				cnn.Close();
			}
			catch (Exception ex)
			{
				vRet = 0;
			}
			return vRet;

		}
		#region New Extraction Methods
		public static ExtractionLog ExtractDeltaCustomerSP()
		{

			ExtractionLog l = new ExtractionLog();
			l.ExeTime = DateTime.Now;

			InputRequestParam p = new InputRequestParam();
			p.JsonTitle = "CUSTOMERS";
            l.SourceRows = RowsCount("select count(*) from GFCC_FLOW.CUSTOMER  ", SharedUtils.GetDSN()).ToString();
			l.TaskName = p.JsonTitle;
			p.StringSQLCommand = "[WebApp].[SpGetCustomerDD]";
            p.CanonicalModelVersion = "2.0";
			p.GetColumns = CustomerID.getColumns();
			var lDt = new List<CustomerID>();
			var oDr = new CustomerID();
			oDr.CUSTOMER_SOURCE_UNIQUE_ID = "1234";
			lDt.Add(oDr);
			p.GetInitiateData = lDt;
			CreateInitiateJsonFile c = new CreateInitiateJsonFile(p);
			l.FilePath = p.FileNamePath;

			if (JsonFileDAL.DeleteDataRow("Data", p.FileNamePath, "CUSTOMER_SOURCE_UNIQUE_ID", "1234"))
			{
				Console.WriteLine("Deleted Data");
			}
			string allText = File.ReadAllText(p.FileNamePath);
			StreamWriter strm = File.CreateText(p.FileNamePath);
			strm.Flush();
			strm.Close();
			File.AppendAllText(p.FileNamePath, allText.Substring(0, allText.Length - 2));
			//allText = "";

			Int32 ttlRows, numberOfRowsPerPage;
			ttlRows = Convert.ToInt32(l.SourceRows); numberOfRowsPerPage = 10000;
			Int32 numberOfLoop = SharedUtils.NumberOfPages(ttlRows, numberOfRowsPerPage);

			try
			{
				int rCntr = 0;
				for (int i = 1; i <= numberOfLoop; i++)
				{
					using (SqlDataReader results = SharedUtils.ExecuteSPCommand(p.StringSQLCommand, SharedUtils.GetDSN(), i, numberOfRowsPerPage))
					{
						while (results.Read())
						{
							rCntr++;
							CustomerID ci = new CustomerID();
							ci.RUN_TIMESTAMP = (!results.IsDBNull(results.GetOrdinal("RUN_TIMESTAMP"))) ? Convert.ToDateTime(results["RUN_TIMESTAMP"]).ToString("dd/MM/yyyy H:mm:ss") : "00/00/0000 00:00:00";
							ci.CUSTOMER_SOURCE_UNIQUE_ID = (!results.IsDBNull(results.GetOrdinal("CUSTOMER_SOURCE_UNIQUE_ID"))) ? results["CUSTOMER_SOURCE_UNIQUE_ID"].ToString() : "";
							ci.ORGUNIT_CODE = (!results.IsDBNull(results.GetOrdinal("ORGUNITCODE"))) ? results["ORGUNITCODE"].ToString() : "";
							ci.PERSON_TITLE = null;
							ci.FIRST_NAME = null;
							ci.MIDDLE_NAMES = null;
							ci.LAST_NAME = null;
							ci.SUFFIX = null;
							ci.CUSTOMER_NAME = (!results.IsDBNull(results.GetOrdinal("CUSTOMER_NAME"))) ? results["CUSTOMER_NAME"].ToString() : "";
							ci.COMPANY_NAME = null;
							ci.COMPANY_FORM = (!results.IsDBNull(results.GetOrdinal("COMPANY_FORM"))) ? results["COMPANY_FORM"].ToString() : "";
							ci.REGISTERED_NUMBER = (!results.IsDBNull(results.GetOrdinal("IDENTIFICATION_NUMBER"))) ? results["IDENTIFICATION_NUMBER"].ToString() : "";
							ci.INCORPORATION_DATE = null;
							ci.INCORPORATION_COUNTRY_CODE = (!results.IsDBNull(results.GetOrdinal("INCORPORATION_COUNTRY_CODE"))) ? results["INCORPORATION_COUNTRY_CODE"].ToString() : "";
							ci.BUSINESS_TYPE = null;
							ci.BUSINESS_SEGMENT_1 = (!results.IsDBNull(results.GetOrdinal("BUSINESS_SEGMENT_1"))) ? results["BUSINESS_SEGMENT_1"].ToString() : "";
							ci.BUSINESS_SEGMENT_2 = null;
							ci.INITIALS = null;
							ci.DATE_OF_BIRTH = (!results.IsDBNull(results.GetOrdinal("DATE_OF_BIRTH"))) ? results["DATE_OF_BIRTH"].ToString() : "";
							ci.NAME_OF_BIRTH = null;
							ci.ADDRESS = (!results.IsDBNull(results.GetOrdinal("ADDRESS_AD"))) ? results["ADDRESS_AD"].ToString() : "";
							ci.ZONE = (!results.IsDBNull(results.GetOrdinal("ZONE"))) ? results["ZONE"].ToString() : "";
							ci.CITY = null;
							ci.POSTAL_CODE = null;
							ci.COUNTRY_OF_RESIDENCE = (!results.IsDBNull(results.GetOrdinal("COUNTRY_OF_RESIDENCE"))) ? results["COUNTRY_OF_RESIDENCE"].ToString() : "";
							ci.COUNTRY_OF_ORIGIN = (!results.IsDBNull(results.GetOrdinal("COUNTRY_OF_ORIGIN"))) ? results["COUNTRY_OF_ORIGIN"].ToString() : "";
							ci.NATIONALITY_CODE = (!results.IsDBNull(results.GetOrdinal("NATIONALITY_CODE"))) ? results["NATIONALITY_CODE"].ToString() : "";
							ci.PLACE_OF_BIRTH = null;
							ci.GENDER_CODE = (!results.IsDBNull(results.GetOrdinal("GENDER_CODE"))) ? results["GENDER_CODE"].ToString() : "";
							ci.PRIME_BRANCH_ID = null;
							ci.RELATIONSHIP_MGR_ID = null;
							ci.EMPLOYEE_FLAG = null;
							ci.EMPLOYEE_NUMBER = null;
							ci.MARITAL_STATUS = null;
							ci.OCCUPATION = (!results.IsDBNull(results.GetOrdinal("OCCUPATION"))) ? results["OCCUPATION"].ToString() : "";
							ci.EMPLOYMENT_STATUS = null;
							ci.ACQUISITION_DATE = null;
							ci.CANCELLED_DATE = (!results.IsDBNull(results.GetOrdinal("canceleddate"))) ? results["canceleddate"].ToString() : "";
							ci.CUSTOMER_TYPE_CODE = (!results.IsDBNull(results.GetOrdinal("CUSTOMERTYPECODE"))) ? results["CUSTOMERTYPECODE"].ToString() : "";
							ci.CUSTOMER_STATUS_CODE = (!results.IsDBNull(results.GetOrdinal("CUSTOMERSTATUSCODE"))) ? results["CUSTOMERSTATUSCODE"].ToString() : "";
							ci.CUSTOMER_SEGMENT_1 = null;
							ci.CUSTOMER_SEGMENT_2 = null;
							ci.CUSTOMER_SEGMENT_3 = (!results.IsDBNull(results.GetOrdinal("customer_segment_3"))) ? results["customer_segment_3"].ToString() : "";
							ci.RESIDENCE_FLAG = null;
							ci.SPECIAL_ATTENTION_FLAG = null;
							ci.DECEASED_FLAG = null;
							ci.DORMANT_OVERRIDE_DATE = null;
							ci.RISK_SCORE = null;
							ci.BANKRUPT_FLAG = null;
							ci.COMPENSATION_REQD_FLAG = null;
							ci.CUSTOMER_COMPLAINT_FLAG = null;
							ci.END_RELATIONSHIP_FLAG = null;
							ci.MERCHANT_NUMBER = null;
							ci.FACE_TO_FACE_FLAG = null;
							ci.CHANNEL = (!results.IsDBNull(results.GetOrdinal("channel"))) ? results["channel"].ToString() : "";
							ci.NEAR_BORDER_FLAG = null;
							ci.INTENDED_PRODUCT_USE = null;
							ci.SOURCE_OF_FUNDS = null;
							ci.COMPLEX_STRUCTURE = null;
							ci.EXPECTED_ANNUAL_TURNOVER = (!results.IsDBNull(results.GetOrdinal("EXPECTED_ANNUAL_TURNOVER"))) ? results["EXPECTED_ANNUAL_TURNOVER"].ToString() : "";
							ci.TRADING_DURATION = null;
							ci.BALANCE_SHEET_TOTAL = (!results.IsDBNull(results.GetOrdinal("BALANCE_SHEET_TOTAL"))) ? results["BALANCE_SHEET_TOTAL"].ToString() : "";
							ci.VAT_NUMBER = null;
							ci.BROKER_CODE = null;
							ci.BLACK_LISTED_FLAG = null;
							ci.DOMAIN_CODE = null;
							ci.COMMENTS = null;
							ci.PEP_FLAG_INGESTED = null;
							ci.WIRE_IN_NUMBER = null;
							ci.WIRE_OUT_NUMBER = null;
							ci.WIRE_IN_VOLUME = null;
							ci.WIRE_OUT_VOLUME = null;
							ci.CASH_IN_VOLUME = null;
							ci.CASH_OUT_VOLUME = null;
							ci.CHECK_IN_VOLUME = null;
							ci.CHECK_OUT_VOLUME = null;
							ci.OVERALL_SCORE_ADJUSTMENT = null;
							ci.TAX_NUMBER = null;
							ci.TAX_NUMBER_ISSUED_BY = null;
							ci.CUSTOMER_CATEGORY_CODE = (!results.IsDBNull(results.GetOrdinal("CUSTOMER_CATEGORY_CODE"))) ? results["CUSTOMER_CATEGORY_CODE"].ToString() : "";
							ci.OWN_AFFILIATE_FLAG = null;
							ci.MARKETING_SERVICE_LEVEL = null;
							ci.SANCTIONED_FLAG_INGESTED = null;
							ci.PEP_TYPE_INGESTED = null;
							ci.RCA_FLAG_INGESTED = null;
							ci.ADDRESS_VALID_FROM = null;
							ci.ADDRESS_VALID_TO = null;
							ci.EMAIL = null;
							ci.EMAIL_VALID_FROM = null;
							ci.EMAIL_VALID_TO = null;
							ci.PHONE_COUNTRY_CODE = null;
							ci.PHONE_AREA_CODE = null;
							ci.PHONE_NUMBER = null;
							ci.PHONE_EXTENSION = null;
							ci.PHONE_VALID_FROM = null;
							ci.PHONE_VALID_TO = null;
							ci.ALTERNATE_NAME = null;
							ci.TAX_NUMBER_TYPE = null;
							ci.BUSINESS_CLASSIFICATION_CODE = null;
							ci.BUSINESS_CLASSIFICATION_SYSTEM = null;
							ci.CUSTOMER_CHANNEL_REMOTE_FLAG = null;
							ci.COUNTRY_OF_TAX_RESIDENCE = (!results.IsDBNull(results.GetOrdinal("COUNTRY_OF_TAX_RESIDENCE"))) ? results["COUNTRY_OF_TAX_RESIDENCE"].ToString() : "";
							ci.COUNTRY_OF_HQ = (!results.IsDBNull(results.GetOrdinal("COUNTRY_OF_HQ"))) ? results["COUNTRY_OF_HQ"].ToString() : "";
							ci.COUNTRY_OF_OPERATIONS = (!results.IsDBNull(results.GetOrdinal("COUNTRY_OF_OPERATIONS"))) ? results["COUNTRY_OF_OPERATIONS"].ToString() : "";

							List<Customer_Identification> lci = new List<Customer_Identification>();
							Customer_Identification cid = new Customer_Identification();
							cid.IDENTIFICATION_NUMBER = (!results.IsDBNull(results.GetOrdinal("IDENTIFICATION_NUMBER"))) ? results["IDENTIFICATION_NUMBER"].ToString() : "";
							cid.ISSUING_AUTHORITY = (!results.IsDBNull(results.GetOrdinal("ISSUING_AUTHORITY"))) ? results["ISSUING_AUTHORITY"].ToString() : "";
							cid.COUNTRY_CODE = (!results.IsDBNull(results.GetOrdinal("COUNTRY_CODE"))) ? results["COUNTRY_CODE"].ToString() : "";
							cid.IDENTIFICATION_TYPE = (!results.IsDBNull(results.GetOrdinal("IDENTIFICATION_NUMBER_TYPE"))) ? results["IDENTIFICATION_NUMBER_TYPE"].ToString() : "";
							cid.VALID_FROM = (!results.IsDBNull(results.GetOrdinal("VALID_FROM_DTTM"))) ? results["VALID_FROM_DTTM"].ToString() : "";
							cid.VALID_TO = (!results.IsDBNull(results.GetOrdinal("VALID_TO_DTTM"))) ? results["VALID_TO_DTTM"].ToString() : "";
							cid.DESCRIPTION = null;
							cid.DETAILS = null;
							cid.VISA_TYPE = null;

							lci.Add(cid);
							ci.Customer_Identification = lci;

							var rslt = JsonFileDAL.WriteDataToFile(p.FileNamePath, ci);
							
							File.AppendAllText(p.FileNamePath, ",");
							l.ExtractedRows = rCntr.ToString();
						}
					}
				}
				File.AppendAllText(p.FileNamePath, "]}");
				if (JsonFileDAL.UpdateRecNoRow("LastRecNo", p.FileNamePath, rCntr.ToString()))
				{
					Console.WriteLine("Updated RowsNumber");
				}
			}
			catch(Exception e)
			{
				Console.WriteLine(e.ToString());
			}

			l.ExtractionStatus = true;
			l.EndTime = DateTime.Now;

			return l;
		}
		#endregion
        
		public static ExtractionLog ExtractDeltaCustomer()
		{

			ExtractionLog l = new ExtractionLog();
			l.ExeTime = DateTime.Now;

			InputRequestParam p = new InputRequestParam();
			p.JsonTitle = "CUSTOMERS";

            l.SourceRows = RowsCount("select count(*) from GFCC_FLOW.CUSTOMER  ", SharedUtils.GetDSN()).ToString();
			l.TaskName = p.JsonTitle; 
			p.StringSQLCommand = @"
							select top 10
								CUSTOMER_SOURCE_UNIQUE_ID,ORGUNITCODE,CUSTOMER_NAME,INCORPORATION_COUNTRY_CODE,
								BUSINESS_SEGMENT_1,DATE_OF_BIRTH,ADDRESS_AD,ZONE,COUNTRY_OF_RESIDENCE,COUNTRY_OF_ORIGIN,
								GENDER_CODE,OCCUPATION,CUSTOMERTYPECODE,CUSTOMERSTATUSCODE,EXPECTED_ANNUAL_TURNOVER,
								BALANCE_SHEET_TOTAL,CUSTOMER_CATEGORY_CODE,COUNTRY_OF_TAX_RESIDENCE,COUNTRY_OF_HQ,
								IDENTIFICATION_NUMBER,ISSUING_AUTHORITY,COUNTRY_CODE,VALID_FROM_DTTM,VALID_TO_DTTM,
								RUN_TIMESTAMP,COMPANY_FORM,NATIONALITY_CODE,COUNTRY_OF_OPERATIONS,IDENTIFICATION_NUMBER_TYPE,
								ADDRESS_TYPE_CODE,customer_segment_3,channel,canceleddate
						from GFCC_FLOW.CUSTOMER 
					where CUSTOMERSTATUSCODE = 'ACTIVE' 
					and identification_number_type = 'KTP' and zone <> 'NULL' and occupation <> ''  and gender_code = 'F' 
					";

			p.CanonicalModelVersion = "2.0";
			p.GetColumns = CustomerID.getColumns();
			var lDt = new List<CustomerID>();
			var oDr = new CustomerID();
			oDr.CUSTOMER_SOURCE_UNIQUE_ID = "1234";
			lDt.Add(oDr);
			p.GetInitiateData = lDt;
			CreateInitiateJsonFile c = new CreateInitiateJsonFile(p);
			l.FilePath = p.FileNamePath;

			if (JsonFileDAL.DeleteDataRow("Data", p.FileNamePath, "CUSTOMER_SOURCE_UNIQUE_ID", "1234"))
			{
				Console.WriteLine("Deleted Data");
			}
			string allText = File.ReadAllText(p.FileNamePath);
			StreamWriter strm = File.CreateText(p.FileNamePath);
			strm.Flush();
			strm.Close();
			File.AppendAllText(p.FileNamePath, allText.Substring(0, allText.Length - 2));
			
			int rCntr = 0;
			using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(p.StringSQLCommand, SharedUtils.GetDSN()))
			{
				while (results.Read())
				{
					rCntr++;
					CustomerID ci = new CustomerID();
					ci.RUN_TIMESTAMP = (!results.IsDBNull(results.GetOrdinal("RUN_TIMESTAMP"))) ? Convert.ToDateTime(results["RUN_TIMESTAMP"]).ToString("dd/MM/yyyy H:mm:ss") : "00/00/0000 00:00:00";
					ci.CUSTOMER_SOURCE_UNIQUE_ID = (!results.IsDBNull(results.GetOrdinal("CUSTOMER_SOURCE_UNIQUE_ID"))) ? results["CUSTOMER_SOURCE_UNIQUE_ID"].ToString() : "";
					ci.ORGUNIT_CODE = (!results.IsDBNull(results.GetOrdinal("ORGUNITCODE"))) ? results["ORGUNITCODE"].ToString() : "";
					ci.PERSON_TITLE = null;
					ci.FIRST_NAME = null;
					ci.MIDDLE_NAMES = null;
					ci.LAST_NAME = null;
					ci.SUFFIX = null;
					ci.CUSTOMER_NAME = (!results.IsDBNull(results.GetOrdinal("CUSTOMER_NAME"))) ? results["CUSTOMER_NAME"].ToString() : "";
					ci.COMPANY_NAME = null;
					ci.COMPANY_FORM = (!results.IsDBNull(results.GetOrdinal("COMPANY_FORM"))) ? results["COMPANY_FORM"].ToString() : "";
					ci.REGISTERED_NUMBER = (!results.IsDBNull(results.GetOrdinal("IDENTIFICATION_NUMBER"))) ? results["IDENTIFICATION_NUMBER"].ToString() : "";
					ci.INCORPORATION_DATE = null;
					ci.INCORPORATION_COUNTRY_CODE = (!results.IsDBNull(results.GetOrdinal("INCORPORATION_COUNTRY_CODE"))) ? results["INCORPORATION_COUNTRY_CODE"].ToString() : "";
					ci.BUSINESS_TYPE = null;
					ci.BUSINESS_SEGMENT_1 = (!results.IsDBNull(results.GetOrdinal("BUSINESS_SEGMENT_1"))) ? results["BUSINESS_SEGMENT_1"].ToString() : "";
					ci.BUSINESS_SEGMENT_2 = null;
					ci.INITIALS = null;
					ci.DATE_OF_BIRTH = (!results.IsDBNull(results.GetOrdinal("DATE_OF_BIRTH"))) ? results["DATE_OF_BIRTH"].ToString() : "";
					ci.NAME_OF_BIRTH = null;
					ci.ADDRESS = (!results.IsDBNull(results.GetOrdinal("ADDRESS_AD"))) ? results["ADDRESS_AD"].ToString() : "";
					ci.ZONE = (!results.IsDBNull(results.GetOrdinal("ZONE"))) ? results["ZONE"].ToString() : "";
					ci.CITY = null;
					ci.POSTAL_CODE = null;
					ci.COUNTRY_OF_RESIDENCE = (!results.IsDBNull(results.GetOrdinal("COUNTRY_OF_RESIDENCE"))) ? results["COUNTRY_OF_RESIDENCE"].ToString() : "";
					ci.COUNTRY_OF_ORIGIN = (!results.IsDBNull(results.GetOrdinal("COUNTRY_OF_ORIGIN"))) ? results["COUNTRY_OF_ORIGIN"].ToString() : "";
					ci.NATIONALITY_CODE = (!results.IsDBNull(results.GetOrdinal("NATIONALITY_CODE"))) ? results["NATIONALITY_CODE"].ToString() : "";
					ci.PLACE_OF_BIRTH = null;
					ci.GENDER_CODE = (!results.IsDBNull(results.GetOrdinal("GENDER_CODE"))) ? results["GENDER_CODE"].ToString() : "";
					ci.PRIME_BRANCH_ID = null;
					ci.RELATIONSHIP_MGR_ID = null;
					ci.EMPLOYEE_FLAG = null;
					ci.EMPLOYEE_NUMBER = null;
					ci.MARITAL_STATUS = null;
					ci.OCCUPATION = (!results.IsDBNull(results.GetOrdinal("OCCUPATION"))) ? results["OCCUPATION"].ToString() : "";
					ci.EMPLOYMENT_STATUS = null;
					ci.ACQUISITION_DATE = null;
					ci.CANCELLED_DATE = (!results.IsDBNull(results.GetOrdinal("canceleddate"))) ? results["canceleddate"].ToString() : "";
					ci.CUSTOMER_TYPE_CODE = (!results.IsDBNull(results.GetOrdinal("CUSTOMERTYPECODE"))) ? results["CUSTOMERTYPECODE"].ToString() : "";
					ci.CUSTOMER_STATUS_CODE = (!results.IsDBNull(results.GetOrdinal("CUSTOMERSTATUSCODE"))) ? results["CUSTOMERSTATUSCODE"].ToString() : "";
					ci.CUSTOMER_SEGMENT_1 = null;
					ci.CUSTOMER_SEGMENT_2 = null;
					ci.CUSTOMER_SEGMENT_3 = (!results.IsDBNull(results.GetOrdinal("customer_segment_3"))) ? results["customer_segment_3"].ToString() : "";
					ci.RESIDENCE_FLAG = null;
					ci.SPECIAL_ATTENTION_FLAG = null;
					ci.DECEASED_FLAG = null;
					ci.DORMANT_OVERRIDE_DATE = null;
					ci.RISK_SCORE = null;
					ci.BANKRUPT_FLAG = null;
					ci.COMPENSATION_REQD_FLAG = null;
					ci.CUSTOMER_COMPLAINT_FLAG = null;
					ci.END_RELATIONSHIP_FLAG = null;
					ci.MERCHANT_NUMBER = null;
					ci.FACE_TO_FACE_FLAG = null;
					ci.CHANNEL = (!results.IsDBNull(results.GetOrdinal("channel"))) ? results["channel"].ToString() : "";
					ci.NEAR_BORDER_FLAG = null;
					ci.INTENDED_PRODUCT_USE = null;
					ci.SOURCE_OF_FUNDS = null;
					ci.COMPLEX_STRUCTURE = null;
					ci.EXPECTED_ANNUAL_TURNOVER = (!results.IsDBNull(results.GetOrdinal("EXPECTED_ANNUAL_TURNOVER"))) ? results["EXPECTED_ANNUAL_TURNOVER"].ToString() : "";
					ci.TRADING_DURATION = null;
					ci.BALANCE_SHEET_TOTAL = (!results.IsDBNull(results.GetOrdinal("BALANCE_SHEET_TOTAL"))) ? results["BALANCE_SHEET_TOTAL"].ToString() : "";
					ci.VAT_NUMBER = null;
					ci.BROKER_CODE = null;
					ci.BLACK_LISTED_FLAG = null;
					ci.DOMAIN_CODE = null;
					ci.COMMENTS = null;
					ci.PEP_FLAG_INGESTED = null;
					ci.WIRE_IN_NUMBER = null;
					ci.WIRE_OUT_NUMBER = null;
					ci.WIRE_IN_VOLUME = null;
					ci.WIRE_OUT_VOLUME = null;
					ci.CASH_IN_VOLUME = null;
					ci.CASH_OUT_VOLUME = null;
					ci.CHECK_IN_VOLUME = null;
					ci.CHECK_OUT_VOLUME = null;
					ci.OVERALL_SCORE_ADJUSTMENT = null;
					ci.TAX_NUMBER = null;
					ci.TAX_NUMBER_ISSUED_BY = null;
					ci.CUSTOMER_CATEGORY_CODE = (!results.IsDBNull(results.GetOrdinal("CUSTOMER_CATEGORY_CODE"))) ? results["CUSTOMER_CATEGORY_CODE"].ToString() : "";
					ci.OWN_AFFILIATE_FLAG = null;
					ci.MARKETING_SERVICE_LEVEL = null;
					ci.SANCTIONED_FLAG_INGESTED = null;
					ci.PEP_TYPE_INGESTED = null;
					ci.RCA_FLAG_INGESTED = null;
					ci.ADDRESS_VALID_FROM = null;
					ci.ADDRESS_VALID_TO = null;
					ci.EMAIL = null;
					ci.EMAIL_VALID_FROM = null;
					ci.EMAIL_VALID_TO = null;
					ci.PHONE_COUNTRY_CODE = null;
					ci.PHONE_AREA_CODE = null;
					ci.PHONE_NUMBER = null;
					ci.PHONE_EXTENSION = null;
					ci.PHONE_VALID_FROM = null;
					ci.PHONE_VALID_TO = null;
					ci.ALTERNATE_NAME = null;
					ci.TAX_NUMBER_TYPE = null;
					ci.BUSINESS_CLASSIFICATION_CODE = null;
					ci.BUSINESS_CLASSIFICATION_SYSTEM = null;
					ci.CUSTOMER_CHANNEL_REMOTE_FLAG = null;
					ci.COUNTRY_OF_TAX_RESIDENCE = (!results.IsDBNull(results.GetOrdinal("COUNTRY_OF_TAX_RESIDENCE"))) ? results["COUNTRY_OF_TAX_RESIDENCE"].ToString() : "";
					ci.COUNTRY_OF_HQ = (!results.IsDBNull(results.GetOrdinal("COUNTRY_OF_HQ"))) ? results["COUNTRY_OF_HQ"].ToString() : "";
					ci.COUNTRY_OF_OPERATIONS = (!results.IsDBNull(results.GetOrdinal("COUNTRY_OF_OPERATIONS"))) ? results["COUNTRY_OF_OPERATIONS"].ToString() : "";

					List<Customer_Identification> lci = new List<Customer_Identification>();
					Customer_Identification cid = new Customer_Identification();
					cid.IDENTIFICATION_NUMBER = (!results.IsDBNull(results.GetOrdinal("IDENTIFICATION_NUMBER"))) ? results["IDENTIFICATION_NUMBER"].ToString() : "";
					cid.ISSUING_AUTHORITY = (!results.IsDBNull(results.GetOrdinal("ISSUING_AUTHORITY"))) ? results["ISSUING_AUTHORITY"].ToString() : "";
					cid.COUNTRY_CODE = (!results.IsDBNull(results.GetOrdinal("COUNTRY_CODE"))) ? results["COUNTRY_CODE"].ToString() : "";
					cid.IDENTIFICATION_TYPE = (!results.IsDBNull(results.GetOrdinal("IDENTIFICATION_NUMBER_TYPE"))) ? results["IDENTIFICATION_NUMBER_TYPE"].ToString() : "";
					cid.VALID_FROM = (!results.IsDBNull(results.GetOrdinal("VALID_FROM_DTTM"))) ? results["VALID_FROM_DTTM"].ToString() : "";
					cid.VALID_TO = (!results.IsDBNull(results.GetOrdinal("VALID_TO_DTTM"))) ? results["VALID_TO_DTTM"].ToString() : "";
					cid.DESCRIPTION = null;
					cid.DETAILS = null;
					cid.VISA_TYPE = null;

					lci.Add(cid);
					ci.Customer_Identification = lci;

					var rslt = JsonFileDAL.WriteDataToFile(p.FileNamePath, ci);
					
					File.AppendAllText(p.FileNamePath, ",");
					l.ExtractedRows = rCntr.ToString();
				}
				File.AppendAllText(p.FileNamePath, "]}");
				if (JsonFileDAL.UpdateRecNoRow("LastRecNo", p.FileNamePath, rCntr.ToString()))
				{
					Console.WriteLine("Updated RowsNumber");
				}
			}
			l.ExtractionStatus = true;
			l.EndTime = DateTime.Now;

			return l;
		}
		public static ExtractionLog ExtractDeltaCustomer2CustomerLink()
		{
			ExtractionLog l = new ExtractionLog();
			l.ExeTime = DateTime.Now;
			InputRequestParam p = new InputRequestParam();
			p.JsonTitle = "CUSTOMER_CUSTOMER_LINK";
			l.TaskName = p.JsonTitle;
			l.SourceRows = RowsCount("select count(*) from GFCC_FLOW.CUSTOMER_LINK where link_type = 'S' ", SharedUtils.GetDSN()).ToString();
			p.StringSQLCommand = @"
							select top 10
								CUSTOMER1_SOURCE_UNIQUE_ID
								,CUSTOMER2_SOURCE_UNIQUE_ID
								,LINK_TYPE
								,FROM_TIMESTAMP
								,TO_TIMESTAMP
						from GFCC_FLOW.CUSTOMER_LINK 
						where link_type = 'S'
					";
			p.CanonicalModelVersion = "2.0";
			p.GetColumns = CustomerLinkID.getColumns();
			var lDt = new List<CustomerLinkID>();
			var oDr = new CustomerLinkID();
			oDr.CUSTOMER1_SOURCE_UNIQUE_ID = "1234";
			lDt.Add(oDr);
			p.GetInitiateData = lDt;
			CreateInitiateJsonFile c = new CreateInitiateJsonFile(p);
			l.FilePath = p.FileNamePath;
			if (JsonFileDAL.DeleteDataRow("Data", p.FileNamePath, "CUSTOMER1_SOURCE_UNIQUE_ID", "1234"))
			{
				Console.WriteLine("Deleted Data");
			}
			string allText = File.ReadAllText(p.FileNamePath);
			StreamWriter strm = File.CreateText(p.FileNamePath);
			strm.Flush();
			strm.Close();
			File.AppendAllText(p.FileNamePath, allText.Substring(0, allText.Length - 2));
			
			int rCntr = 0;
			using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(p.StringSQLCommand, SharedUtils.GetDSNGFCC()))
			{
				while (results.Read())
				{
					rCntr++;
					CustomerLinkID cpl = new CustomerLinkID()
					{
						CUSTOMER1_SOURCE_UNIQUE_ID = (!results.IsDBNull(results.GetOrdinal("CUSTOMER1_SOURCE_UNIQUE_ID"))) ? results["CUSTOMER1_SOURCE_UNIQUE_ID"].ToString() : "",
						CUSTOMER2_SOURCE_UNIQUE_ID = (!results.IsDBNull(results.GetOrdinal("CUSTOMER2_SOURCE_UNIQUE_ID"))) ? results["CUSTOMER2_SOURCE_UNIQUE_ID"].ToString() : "",
						LINK_TYPE = (!results.IsDBNull(results.GetOrdinal("LINK_TYPE"))) ? results["LINK_TYPE"].ToString() : "",
						PERCENTAGE_OWNERSHIP = null,
						ULTIMATE_BENEFICIAL_OWNER_FLAG = null,
						FROM_TIMESTAMP = (!results.IsDBNull(results.GetOrdinal("FROM_TIMESTAMP"))) ? Convert.ToDateTime(results["FROM_TIMESTAMP"]).ToString("dd/MM/yyyy") : "00/00/00",
						TO_TIMESTAMP = (!results.IsDBNull(results.GetOrdinal("TO_TIMESTAMP"))) ? Convert.ToDateTime(results["TO_TIMESTAMP"]).ToString("dd/MM/yyyy") : "00/00/00"
					};
					var rslt = JsonFileDAL.WriteDataToFile(p.FileNamePath, cpl);
					
					File.AppendAllText(p.FileNamePath, ",");
					l.ExtractedRows = rCntr.ToString();
				}
				File.AppendAllText(p.FileNamePath, "]}");
				if (JsonFileDAL.UpdateRecNoRow("LastRecNo", p.FileNamePath, rCntr.ToString()))
				{
					Console.WriteLine("Updated RowsNumber");
				}
			}
			l.EndTime = DateTime.Now;

			return l;
		}
		public static ExtractionLog ExtractDeltaCustomerPolicyLink()
		{
			ExtractionLog l = new ExtractionLog();
			l.ExeTime = DateTime.Now;
			InputRequestParam p = new InputRequestParam();
			p.JsonTitle = "CUSTOMER_POLICY_LINK";
			l.TaskName = p.JsonTitle;
			l.SourceRows = RowsCount("select count(*) from GFCC_FLOW.CUSTOMER_POLICY_LINK where POLICY_ID in('514-5416656','514-7625973','516-5010546','511-7253228','516-1009591','517-1442907','513-7574835','514-8828477','515-7479501','515-3987606')", SharedUtils.GetDSNGFCC()).ToString();
			p.StringSQLCommand = @"
							select  top 10
								POLICY_ID
								,CUSTOMER_ID
								,PARTY_ROLE_CODE
								,PARTY_RELATIONSHIP
								,IS_PAYOR
								,FROM_DATE
								,TO_DATE
								,LOGDATE
								,CREATE_DATE
								,MODIFIED_DATE
						from GFCC_FLOW.CUSTOMER_POLICY_LINK
where POLICY_ID in('514-5416656','514-7625973','516-5010546','511-7253228','516-1009591','517-1442907','513-7574835','514-8828477','515-7479501','515-3987606')
					";
			p.CanonicalModelVersion = "2.0";
			p.GetColumns = CustomerPolicyLink.getColumns();
			var lDt = new List<CustomerPolicyLink>();
			var oDr = new CustomerPolicyLink();
			oDr.CUSTOMER_ID = "1234";
			lDt.Add(oDr);
			p.GetInitiateData = lDt;
			CreateInitiateJsonFile c = new CreateInitiateJsonFile(p);
			l.FilePath = p.FileNamePath;
			if (JsonFileDAL.DeleteDataRow("Data", p.FileNamePath, "CUSTOMER_ID", "1234"))
			{
				Console.WriteLine("Deleted Data");
			}
			string allText = File.ReadAllText(p.FileNamePath);
			StreamWriter strm = File.CreateText(p.FileNamePath);
			strm.Flush();
			strm.Close();
			File.AppendAllText(p.FileNamePath, allText.Substring(0, allText.Length - 2));
			
			int rCntr = 0;
			using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(p.StringSQLCommand, SharedUtils.GetDSNGFCC()))
			{
				while (results.Read())
				{
					rCntr++;
					CustomerPolicyLink cpl = new CustomerPolicyLink()
					{
						POLICY_ID = (!results.IsDBNull(results.GetOrdinal("POLICY_ID"))) ? results["POLICY_ID"].ToString() : "",
						CUSTOMER_ID = (!results.IsDBNull(results.GetOrdinal("CUSTOMER_ID"))) ? results["CUSTOMER_ID"].ToString() : "",
						PARTY_ROLE_CODE = (!results.IsDBNull(results.GetOrdinal("PARTY_ROLE_CODE"))) ? results["PARTY_ROLE_CODE"].ToString() : "",
						PARTY_RELATIONSHIP = (!results.IsDBNull(results.GetOrdinal("PARTY_RELATIONSHIP"))) ? results["PARTY_RELATIONSHIP"].ToString() : "",
						FROM_DATE = (!results.IsDBNull(results.GetOrdinal("FROM_DATE"))) ? Convert.ToDateTime(results["FROM_DATE"]).ToString("dd/MM/yyyy") : "00/00/00",
						TO_DATE = (!results.IsDBNull(results.GetOrdinal("TO_DATE"))) ? Convert.ToDateTime(results["TO_DATE"]).ToString("dd/MM/yyyy") : "00/00/00",
						//LOGDATE = (!results.IsDBNull(results.GetOrdinal("LOGDATE"))) ? Convert.ToDateTime(results["LOGDATE"]).ToString("dd/MM/yyyy") : "00/00/00",
						IS_PAYOR = (!results.IsDBNull(results.GetOrdinal("IS_PAYOR"))) ? results["IS_PAYOR"].ToString() : "",
						X_ORGUNIT_CODE = (!results.IsDBNull(results.GetOrdinal("X_ORGUNIT_CODE"))) ? results["X_ORGUNIT_CODE"].ToString() : ""
					};
					var rslt = JsonFileDAL.WriteDataToFile(p.FileNamePath, cpl);
					
					File.AppendAllText(p.FileNamePath, ",");
					l.ExtractedRows = rCntr.ToString();
				}
				File.AppendAllText(p.FileNamePath, "]}");
				if (JsonFileDAL.UpdateRecNoRow("LastRecNo", p.FileNamePath, rCntr.ToString()))
				{
					Console.WriteLine("Updated RowsNumber");
				}
			}
			l.EndTime = DateTime.Now;
			return l;
		}
		public static ExtractionLog ExtractDeltaIntermediaries()
		{
			string tblName = " GFCC_FLOW.INTERMEDIARIES ";
			ExtractionLog l = new ExtractionLog();
			l.ExeTime = DateTime.Now;
			InputRequestParam p = new InputRequestParam();
			p.JsonTitle = "INTERMEDIARIES";
			l.TaskName = p.JsonTitle;
			p.StringSQLCommand = @"
							select top 10
								RUN_TIMESTAMP
								,INTERMEDIARY_SOURCE_UNIQUE_ID
								,INTERMEDIARY_NAME
								,INTERMEDIARY_TYPE_CODE
								,COUNTRY_CODE
								,FROM_DATE
								,TO_DATE
								,ORGUNIT_CODE
						from " + tblName + " where FORMAT (modified_date,  'dd/MM/yyyy ', 'en-us') ='05/01/2021' ";
			p.CanonicalModelVersion = "2.0";
			p.GetColumns = Intermediaries.getColumns();
			var lDt = new List<Intermediaries>();
			var oDr = new Intermediaries();
			oDr.INTERMEDIARY_SOURCE_UNIQUE_ID = "1234";
			lDt.Add(oDr);
			p.GetInitiateData = lDt;
			CreateInitiateJsonFile c = new CreateInitiateJsonFile(p);
			l.FilePath = p.FileNamePath;
			if (JsonFileDAL.DeleteDataRow("Data", p.FileNamePath, "INTERMEDIARY_SOURCE_UNIQUE_ID", "1234"))
			{
				Console.WriteLine("Deleted Data");
			}
			string allText = File.ReadAllText(p.FileNamePath);
			StreamWriter strm = File.CreateText(p.FileNamePath);
			strm.Flush();
			strm.Close();
			File.AppendAllText(p.FileNamePath, allText.Substring(0, allText.Length - 2));
			
			int rCntr = 0;
			using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(p.StringSQLCommand, SharedUtils.GetDSNGFCC()))
			{
				while (results.Read())
				{
					rCntr++;
					Intermediaries cpl = new Intermediaries()
					{
						RUN_TIMESTAMP = (!results.IsDBNull(results.GetOrdinal("RUN_TIMESTAMP"))) ? Convert.ToDateTime(results["RUN_TIMESTAMP"]).ToString("dd/MM/yyyy H:mm:ss") : "00/00/0000 00:00:00",
						INTERMEDIARY_SOURCE_UNIQUE_ID = (!results.IsDBNull(results.GetOrdinal("INTERMEDIARY_SOURCE_UNIQUE_ID"))) ? results["INTERMEDIARY_SOURCE_UNIQUE_ID"].ToString() : "",
						EMPLOYEE_ID = null,
						INTERMEDIARY_NAME = (!results.IsDBNull(results.GetOrdinal("INTERMEDIARY_NAME"))) ? results["INTERMEDIARY_NAME"].ToString() : "",
						INTERMEDIARY_TYPE_CODE = (!results.IsDBNull(results.GetOrdinal("INTERMEDIARY_TYPE_CODE"))) ? results["INTERMEDIARY_TYPE_CODE"].ToString() : "",
						ADDRESS = null,
						POSTAL_CODE = null,
						CITY = null,
						COUNTRY_CODE = (!results.IsDBNull(results.GetOrdinal("COUNTRY_CODE"))) ? results["COUNTRY_CODE"].ToString() : "",
						PHONE_NUMBER = null,
						FAX_NUMBER = null,
						EMAIL_ADDRESS = null,
						TARGET_MARKET = null,
						FROM_DATE = (!results.IsDBNull(results.GetOrdinal("FROM_DATE"))) ? Convert.ToDateTime(results["FROM_DATE"]).ToString("dd/MM/yyyy") : "00/00/0000",
						TO_DATE = (!results.IsDBNull(results.GetOrdinal("TO_DATE"))) ? Convert.ToDateTime(results["TO_DATE"]).ToString("dd/MM/yyyy") : "00/00/0000",
						APPROVED_FROM = null,
						APPROVED_TO = null,
						RISK_LEVEL = null,
						RISK_SCORE = null,
						ORGUNIT_CODE = (!results.IsDBNull(results.GetOrdinal("ORGUNIT_CODE"))) ? results["ORGUNIT_CODE"].ToString() : ""
					};
					var rslt = JsonFileDAL.WriteDataToFile(p.FileNamePath, cpl);
					
					File.AppendAllText(p.FileNamePath, ",");
					l.ExtractedRows = rCntr.ToString();
				}
				File.AppendAllText(p.FileNamePath, "]}");
				if (JsonFileDAL.UpdateRecNoRow("LastRecNo", p.FileNamePath, rCntr.ToString()))
				{
					Console.WriteLine("Updated RowsNumber");
				}
			}
			l.SourceRows = RowsCount("select count(*) from " + tblName + " where FORMAT (modified_date,  'dd/MM/yyyy ', 'en-us') ='05/01/2021' ", SharedUtils.GetDSN()).ToString();
			l.EndTime = DateTime.Now;

			return l;
		}
		public static ExtractionLog ExtractDeltaIntermediariesPolicyLink()
		{
			ExtractionLog l = new ExtractionLog();
			l.ExeTime = DateTime.Now;
			InputRequestParam p = new InputRequestParam();
			p.JsonTitle = "INTERMEDIARY_POLICY_LINK";
			l.TaskName = p.JsonTitle;
			p.StringSQLCommand = @"
							select top 10
								POLICY_ID
								,INTERMEDIARY_ID
								,PARTY_ROLE_CODE
								,From_Date
								,TO_Date
						from GFCC_FLOW.INTERMEDIARY_POLICY_LINK
					";

			p.CanonicalModelVersion = "2.0";
			p.GetColumns = Intermediary_Policy_Link.getColumns();
			var lDt = new List<Intermediary_Policy_Link>();
			var oDr = new Intermediary_Policy_Link();
			oDr.INTERMEDIARY_ID = "1234";
			lDt.Add(oDr);
			p.GetInitiateData = lDt;
			CreateInitiateJsonFile c = new CreateInitiateJsonFile(p);
			l.FilePath = p.FileNamePath;
			if (JsonFileDAL.DeleteDataRow("Data", p.FileNamePath, "INTERMEDIARY_ID", "1234"))
			{
				Console.WriteLine("Deleted Data");
			}
			string allText = File.ReadAllText(p.FileNamePath);
			StreamWriter strm = File.CreateText(p.FileNamePath);
			strm.Flush();
			strm.Close();
			File.AppendAllText(p.FileNamePath, allText.Substring(0, allText.Length - 2));
			
			int rCntr = 0;
			using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(p.StringSQLCommand, SharedUtils.GetDSNGFCC()))
			{
				while (results.Read())
				{
					rCntr++;
					Intermediary_Policy_Link cpl = new Intermediary_Policy_Link()
					{
						POLICY_ID = (!results.IsDBNull(results.GetOrdinal("POLICY_ID"))) ? results["POLICY_ID"].ToString() : "",
						INTERMEDIARY_ID = (!results.IsDBNull(results.GetOrdinal("INTERMEDIARY_ID"))) ? results["INTERMEDIARY_ID"].ToString() : "",
						PARTY_ROLE_CODE = (!results.IsDBNull(results.GetOrdinal("PARTY_ROLE_CODE"))) ? results["PARTY_ROLE_CODE"].ToString() : "",
						PARTY_RELATIONSHIP = null,
						FROM_DATE = (!results.IsDBNull(results.GetOrdinal("FROM_DATE"))) ? Convert.ToDateTime(results["FROM_DATE"]).ToString("dd/MM/yyyy") : "00/00/0000",
						TO_DATE = (!results.IsDBNull(results.GetOrdinal("TO_DATE"))) ? Convert.ToDateTime(results["TO_DATE"]).ToString("dd/MM/yyyy") : "00/00/0000",
					};
					var rslt =JsonFileDAL.WriteDataToFile(p.FileNamePath, cpl);
					
					File.AppendAllText(p.FileNamePath, ",");
					l.ExtractedRows = rCntr.ToString();
				}
				File.AppendAllText(p.FileNamePath, "]}");
				if (JsonFileDAL.UpdateRecNoRow("LastRecNo", p.FileNamePath, rCntr.ToString()))
				{
					Console.WriteLine("Updated RowsNumber");
				}
			}
			l.SourceRows = RowsCount("select count(*) from GFCC_FLOW.INTERMEDIARY_POLICY_LINK where FORMAT(modified_date, 'dd/MM/yyyy ', 'en-us') = '05/01/2021' ", SharedUtils.GetDSN()).ToString();
			l.EndTime = DateTime.Now;

			return l;
		}
		public static ExtractionLog ExtractDeltaProductSourceType()
		{
			ExtractionLog l = new ExtractionLog();
			l.ExeTime = DateTime.Now;
			InputRequestParam p = new InputRequestParam();
			p.JsonTitle = "PRODUCT_SOURCE_TYPE";
			l.TaskName = p.JsonTitle;
			p.StringSQLCommand = @"
							select top 10
								PRODUCT_ID
								,PRODUCT_SOURCE_TYPE_CODE
								,PRODUCT_SOURCE_TYPE_DESC
						from GFCC_FLOW.PRODUCT_SOURCE_TYPE
					";
			p.CanonicalModelVersion = "2.0";
			p.GetColumns = ProductSource.getColumns();
			var lDt = new List<ProductSource>();
			var oDr = new ProductSource();
			oDr.PRODUCT_ID = "1234";
			lDt.Add(oDr);
			p.GetInitiateData = lDt;
			CreateInitiateJsonFile c = new CreateInitiateJsonFile(p);
			l.FilePath = p.FileNamePath;
			if ( JsonFileDAL.DeleteDataRow("Data", p.FileNamePath, "PRODUCT_ID", "1234"))
			{
				Console.WriteLine("Deleted Data");
			}
			string allText = File.ReadAllText(p.FileNamePath);
			StreamWriter strm = File.CreateText(p.FileNamePath);
			strm.Flush();
			strm.Close();
			File.AppendAllText(p.FileNamePath, allText.Substring(0, allText.Length - 2));
			
			int rCntr = 0;
			using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(p.StringSQLCommand, SharedUtils.GetDSNGFCC()))
			{
				while (results.Read())
				{
					rCntr++;
					ProductSource cpl = new ProductSource()
					{
						PRODUCT_SOURCE_TYPE_CODE = (!results.IsDBNull(results.GetOrdinal("PRODUCT_SOURCE_TYPE_CODE"))) ? results["PRODUCT_SOURCE_TYPE_CODE"].ToString() : "",
						PRODUCT_SOURCE_TYPE_DESC = (!results.IsDBNull(results.GetOrdinal("PRODUCT_SOURCE_TYPE_DESC"))) ? results["PRODUCT_SOURCE_TYPE_DESC"].ToString() : "",
						PRODUCT_ID = (!results.IsDBNull(results.GetOrdinal("PRODUCT_ID"))) ? results["PRODUCT_ID"].ToString() : ""

					};
					var rslt =  JsonFileDAL.WriteDataToFile(p.FileNamePath, cpl);
					
					File.AppendAllText(p.FileNamePath, ",");
					l.ExtractedRows = rCntr.ToString();
				}
				File.AppendAllText(p.FileNamePath, "]}");
				if ( JsonFileDAL.UpdateRecNoRow("LastRecNo", p.FileNamePath, rCntr.ToString()))
				{
					Console.WriteLine("Updated RowsNumber");
				}

				//vRet = true;
			}
			l.SourceRows = RowsCount("select count(*) from GFCC_FLOW.PRODUCT_SOURCE_TYPE", SharedUtils.GetDSNGFCC()).ToString();
			l.EndTime = DateTime.Now;

			return l;
		}
		public static ExtractionLog ExtractInitiatialProducts()
		{
			ExtractionLog l = new ExtractionLog();
			l.ExeTime = DateTime.Now;
			InputRequestParam p = new InputRequestParam();
			p.JsonTitle = "PRODUCT";
			l.TaskName = p.JsonTitle;
			p.StringSQLCommand = @"
							select top 10
								PRODUCT_ID
								,PRODUCT_NAME
								,PRODUCT_GROUP
								,PRODUCT_CATEGORY
						from GFCC_FLOW.PRODUCT
					";
			p.CanonicalModelVersion = "2.0";
			p.GetColumns = Product.getColumns();
			var lDt = new List<Product>();
			var oDr = new Product();
			oDr.PRODUCT_ID = "1234";
			lDt.Add(oDr);
			p.GetInitiateData = lDt;
			CreateInitiateJsonFile c = new CreateInitiateJsonFile(p);
			l.FilePath = p.FileNamePath;
			if ( JsonFileDAL.DeleteDataRow("Data", p.FileNamePath, "PRODUCT_ID", "1234"))
			{
				Console.WriteLine("Deleted Data");
			}
			string allText = File.ReadAllText(p.FileNamePath);
			StreamWriter strm = File.CreateText(p.FileNamePath);
			strm.Flush();
			strm.Close();
			File.AppendAllText(p.FileNamePath, allText.Substring(0, allText.Length - 2));
			
			int rCntr = 0;
			using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(p.StringSQLCommand, SharedUtils.GetDSNGFCC()))
			{
				while (results.Read())
				{
					rCntr++;
					Product cpl = new Product()
					{
						PRODUCT_ID = (!results.IsDBNull(results.GetOrdinal("PRODUCT_ID"))) ? results["PRODUCT_ID"].ToString() : "",
						PRODUCT_NAME = (!results.IsDBNull(results.GetOrdinal("PRODUCT_NAME"))) ? results["PRODUCT_NAME"].ToString() : "",
						PRODUCT_GROUP = (!results.IsDBNull(results.GetOrdinal("PRODUCT_GROUP"))) ? results["PRODUCT_GROUP"].ToString() : "",
						PRODUCT_CLASS = (!results.IsDBNull(results.GetOrdinal("PRODUCT_CATEGORY"))) ? results["PRODUCT_CATEGORY"].ToString() : "",
						PRODUCT_LINE = null
					};
					var rslt =  JsonFileDAL.WriteDataToFile(p.FileNamePath, cpl);
					
					File.AppendAllText(p.FileNamePath, ",");
					l.ExtractedRows = rCntr.ToString();
				}
				File.AppendAllText(p.FileNamePath, "]}");
				if ( JsonFileDAL.UpdateRecNoRow("LastRecNo", p.FileNamePath, rCntr.ToString()))
				{
					Console.WriteLine("Updated RowsNumber");
				}
			}
			l.SourceRows = RowsCount("select count(*) from GFCC_FLOW.PRODUCT", SharedUtils.GetDSNGFCC()).ToString();
			l.EndTime = DateTime.Now;

			return l;
		}
		public static ExtractionLog ExtractDeltaOperations()
		{
			ExtractionLog l = new ExtractionLog();
			l.ExeTime = DateTime.Now;
			InputRequestParam p = new InputRequestParam();
			p.JsonTitle = "OPERATIONS";
			l.TaskName = p.JsonTitle;
			p.StringSQLCommand = @"
							select  top 10
								CREATE_DATE
								,OPERATION_SOURCE_UNIQUE_ID
								,POLICY_ID
								,TXN_TYPE_CODE
								,TXN_CHANNEL_CODE
								,TXN_AMOUNT_BASE
								,TXN_AMOUNT_ORIG
								,CURRENCY_CODE_ORIG
								,CREDIT_DEBIT_CODE
								,PAYMENT_METHOD
								,PAYMENT_MODE
								,IBAN
								,FOREIGN_FLAG
								,REJECTED_FLAG
								,ORGUNIT_CODE
								,RUNTIME_STAMP
								,CUSTOMERID
								,CURRENCY_CODE_BASE
						from GFCC_FLOW.OPERATIONS
					";
			p.CanonicalModelVersion = "2.0";
			p.GetColumns = Operation.getColumns();
			var lDt = new List<Operation>();
			var oDr = new Operation();
			oDr.OPERATION_SOURCE_UNIQUE_ID = "1234";
			lDt.Add(oDr);
			p.GetInitiateData = lDt;
			CreateInitiateJsonFile c = new CreateInitiateJsonFile(p);
			l.FilePath = p.FileNamePath;
			if ( JsonFileDAL.DeleteDataRow("Data", p.FileNamePath, "OPERATION_SOURCE_UNIQUE_ID", "1234"))
			{
				Console.WriteLine("Deleted Data");
			}
			string allText = File.ReadAllText(p.FileNamePath);
			StreamWriter strm = File.CreateText(p.FileNamePath);
			strm.Flush();
			strm.Close();
			File.AppendAllText(p.FileNamePath, allText.Substring(0, allText.Length - 2));
			
			int rCntr = 0;
			using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(p.StringSQLCommand, SharedUtils.GetDSNGFCC()))
			{
				while (results.Read())
				{
					rCntr++;
					Operation cpl = new Operation();

					cpl.RUN_TIMESTAMP = (!results.IsDBNull(results.GetOrdinal("RUNTIME_STAMP"))) ? Convert.ToDateTime(results["RUNTIME_STAMP"]).ToString("dd/MM/yyyy H:mm:ss") : "00/00/0000 00:00:00";
					cpl.OPERATION_SOURCE_UNIQUE_ID = (!results.IsDBNull(results.GetOrdinal("OPERATION_SOURCE_UNIQUE_ID"))) ? results["OPERATION_SOURCE_UNIQUE_ID"].ToString() : "";
					cpl.POLICY_ID = (!results.IsDBNull(results.GetOrdinal("POLICY_ID"))) ? results["POLICY_ID"].ToString() : "";
					cpl.CUSTOMER_ID = (!results.IsDBNull(results.GetOrdinal("CUSTOMERID"))) ? results["CUSTOMERID"].ToString() : "";
					cpl.INTERMEDIARY_ID = null;
					cpl.BRANCH_ID = null;
					cpl.TXN_TYPE_CODE = (!results.IsDBNull(results.GetOrdinal("TXN_TYPE_CODE"))) ? results["TXN_TYPE_CODE"].ToString() : "";
					cpl.TXN_CHANNEL_CODE = (!results.IsDBNull(results.GetOrdinal("TXN_CHANNEL_CODE"))) ? results["TXN_CHANNEL_CODE"].ToString() : "";
					cpl.TXN_AMOUNT_BASE = (!results.IsDBNull(results.GetOrdinal("TXN_AMOUNT_BASE"))) ? results["TXN_AMOUNT_BASE"].ToString() : "";
					cpl.CURRENCY_CODE_BASE = (!results.IsDBNull(results.GetOrdinal("CURRENCY_CODE_BASE"))) ? results["CURRENCY_CODE_BASE"].ToString() : "";
					cpl.TXN_AMOUNT_ORIG = (!results.IsDBNull(results.GetOrdinal("TXN_AMOUNT_ORIG"))) ? results["TXN_AMOUNT_ORIG"].ToString() : "";
					cpl.CURRENCY_CODE_ORIG = (!results.IsDBNull(results.GetOrdinal("CURRENCY_CODE_ORIG"))) ? results["CURRENCY_CODE_ORIG"].ToString() : "";
					cpl.CREDIT_DEBIT_CODE = (!results.IsDBNull(results.GetOrdinal("CREDIT_DEBIT_CODE"))) ? results["CREDIT_DEBIT_CODE"].ToString() : "";
					cpl.PAYMENT_METHOD = (!results.IsDBNull(results.GetOrdinal("PAYMENT_METHOD"))) ? results["PAYMENT_METHOD"].ToString() : "";
					cpl.IBAN = (!results.IsDBNull(results.GetOrdinal("IBAN"))) ? results["IBAN"].ToString() : "";
					cpl.BIC = null;
					cpl.ACCOUNT_NUMBER = null;
					cpl.FOREIGN_FLAG = (!results.IsDBNull(results.GetOrdinal("FOREIGN_FLAG"))) ? results["FOREIGN_FLAG"].ToString() : "";
					cpl.SOURCE_OF_FUNDS_FLAG = null;
					cpl.UNUSUAL_PAYMENT_METHOD_FLAG = null;
					cpl.REIMBURSEMENT_FLAG = null;
					cpl.PROGRAMMED_FLAG = null;
					cpl.REJECTED_FLAG = (!results.IsDBNull(results.GetOrdinal("REJECTED_FLAG"))) ? results["REJECTED_FLAG"].ToString() : "";
					cpl.BENEFICIARY_CLAUSE = null;
					cpl.ORGUNIT_CODE = (!results.IsDBNull(results.GetOrdinal("ORGUNIT_CODE"))) ? results["ORGUNIT_CODE"].ToString() : "";

					var rslt =  JsonFileDAL.WriteDataToFile(p.FileNamePath, cpl);
					
					File.AppendAllText(p.FileNamePath, ",");
					l.ExtractedRows = rCntr.ToString();
				}
				File.AppendAllText(p.FileNamePath, "]}");
				if ( JsonFileDAL.UpdateRecNoRow("LastRecNo", p.FileNamePath, rCntr.ToString()))
				{
					Console.WriteLine("Updated RowsNumber");
				}
			}
			l.SourceRows = RowsCount("select count(*) from GFCC_FLOW.OPERATIONS", SharedUtils.GetDSNGFCC()).ToString();
			l.EndTime = DateTime.Now;

			return l;
		}
		public static ExtractionLog ExtractDeltaPolicies()
		{
			ExtractionLog l = new ExtractionLog();
			l.ExeTime = DateTime.Now;
			InputRequestParam p = new InputRequestParam();
			p.JsonTitle = "POLICIES";
			l.TaskName = p.JsonTitle;
			p.StringSQLCommand = @"
							select  top 10
								RUN_TIMESTAMP
								,POLICY_SOURCE_UNIQUE_ID
								,PRIMARY_CUSTOMER_ID
								,CUSTOMER_ID
								,POLICY_HOLDER_NAME
								,PRODUCT_SOURCE_TYPE_CODE
								,PRODUCT_SOURCE_TYPE_DESC
								,POLICY_STATUS_CODE
								,CURRENCY_CODE
								,SUBSCRIPTION_DATE
								,EFFECTIVE_DATE
								,SURRENDER_DATE
								,POLICY_VALUE_TD
								,LAST_SINGLE_PREMIUM_AMOUNT
								,ORGUNIT_CODE
								,TOTAL_ANNUAL_PREMIUM_TD
						from GFCC_FLOW.POLICIES 
					";

			p.CanonicalModelVersion = "2.0";
			p.GetColumns = Policies.getColumns();
			var lDt = new List<Policies>();
			var oDr = new Policies();
			oDr.POLICY_SOURCE_UNIQUE_ID = "1234";
			lDt.Add(oDr);
			p.GetInitiateData = lDt;
			CreateInitiateJsonFile c = new CreateInitiateJsonFile(p);
			l.FilePath = p.FileNamePath;
			if ( JsonFileDAL.DeleteDataRow("Data", p.FileNamePath, "POLICY_SOURCE_UNIQUE_ID", "1234"))
			{
				Console.WriteLine("Deleted Data");
			}
			string allText = File.ReadAllText(p.FileNamePath);
			StreamWriter strm = File.CreateText(p.FileNamePath);
			strm.Flush();
			strm.Close();
			File.AppendAllText(p.FileNamePath, allText.Substring(0, allText.Length - 2));
			
			int rCntr = 0;
			using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(p.StringSQLCommand, SharedUtils.GetDSNGFCC()))
			{
				while (results.Read())
				{
					rCntr++;
					Policies cpl = new Policies()
					{
						RUN_TIMESTAMP = (!results.IsDBNull(results.GetOrdinal("RUN_TIMESTAMP"))) ? Convert.ToDateTime(results["RUN_TIMESTAMP"]).ToString("dd/MM/yyyy H:mm:ss") : "00/00/0000 00:00:00",
						POLICY_SOURCE_UNIQUE_ID = (!results.IsDBNull(results.GetOrdinal("POLICY_SOURCE_UNIQUE_ID"))) ? results["POLICY_SOURCE_UNIQUE_ID"].ToString() : "",
						PRIMARY_CUSTOMER_ID = (!results.IsDBNull(results.GetOrdinal("PRIMARY_CUSTOMER_ID"))) ? results["PRIMARY_CUSTOMER_ID"].ToString() : "",
						POLICY_HOLDER_NAME = (!results.IsDBNull(results.GetOrdinal("POLICY_HOLDER_NAME"))) ? results["POLICY_HOLDER_NAME"].ToString() : "",
						POLICY_COHOLDER_NAME = null,
						POLICY_INSURED_NAME = null,
						POLICY_PAYOR_NAME = null,
						INTERMEDIARY_ID = null,
						BRANCH_ID = null,
						BRANCH_NAME = null,
						PRODUCT_SOURCE_TYPE_CODE = (!results.IsDBNull(results.GetOrdinal("PRODUCT_SOURCE_TYPE_CODE"))) ? results["PRODUCT_SOURCE_TYPE_CODE"].ToString() : "",
						PRODUCT_SOURCE_TYPE_DESC = (!results.IsDBNull(results.GetOrdinal("PRODUCT_SOURCE_TYPE_DESC"))) ? results["PRODUCT_SOURCE_TYPE_DESC"].ToString() : "",
						POLICY_STATUS_CODE = (!results.IsDBNull(results.GetOrdinal("POLICY_STATUS_CODE"))) ? results["POLICY_STATUS_CODE"].ToString() : "",
						POLICY_DURATION = null,
						COUNTRY_CODE = null,
						CURRENCY_CODE = (!results.IsDBNull(results.GetOrdinal("CURRENCY_CODE"))) ? results["CURRENCY_CODE"].ToString() : "",
						BENEFICIARY_CLAUSE = null,
						BENEFICIARY_CLAUSE_LAST_UPDATE = null,
						SUBSCRIPTION_DATE = (!results.IsDBNull(results.GetOrdinal("SUBSCRIPTION_DATE"))) ? Convert.ToDateTime(results["SUBSCRIPTION_DATE"]).ToString("dd/MM/yyyy") : "00/00/0000",
						EFFECTIVE_DATE = (!results.IsDBNull(results.GetOrdinal("EFFECTIVE_DATE"))) ? Convert.ToDateTime(results["EFFECTIVE_DATE"]).ToString("dd/MM/yyyy") : "00/00/0000",
						INITIAL_AMOUNT = null,
						INSTALLMENT_FREQUENCY = null,
						POLICY_VALUE_TD = (!results.IsDBNull(results.GetOrdinal("POLICY_VALUE_TD"))) ? results["POLICY_VALUE_TD"].ToString() : "",
						SURRENDER_VALUE_TD = null,
						NON_AMORTIZED_VALUE_TD = null,
						LAST_VALUE_DATE = null,
						TOTAL_DEPOSIT_TD = null,
						TOTAL_WITHDRAWAL_TD = null,
						TOTAL_ADVANCE_TD = null,
						TOTAL_REIMBURSMENT_TD = null,
						LAST_WITHDRAWAL_AMOUNT = null,
						LAST_WITHDRAWAL_DATE = null,
						LAST_ADVANCE_AMOUNT = null,
						LAST_ADVANCE_DATE = null,
						LAST_REIMBURSMENT_AMOUNT = null,
						LAST_REIMBURSMENT_DATE = null,
						LAST_SINGLE_PREMIUM_AMOUNT = (!results.IsDBNull(results.GetOrdinal("LAST_SINGLE_PREMIUM_AMOUNT"))) ? results["LAST_SINGLE_PREMIUM_AMOUNT"].ToString() : "",
						LAST_SINGLE_PREMIUM_DATE = null,
						ORGUNIT_CODE = (!results.IsDBNull(results.GetOrdinal("ORGUNIT_CODE"))) ? results["ORGUNIT_CODE"].ToString() : "",
						TOTAL_ANNUAL_PREMIUM_TD = (!results.IsDBNull(results.GetOrdinal("TOTAL_ANNUAL_PREMIUM_TD"))) ? results["TOTAL_ANNUAL_PREMIUM_TD"].ToString() : ""
					};
					var rslt =  JsonFileDAL.WriteDataToFile(p.FileNamePath, cpl);
					
					File.AppendAllText(p.FileNamePath, ",");
					l.ExtractedRows = rCntr.ToString();
				}
				File.AppendAllText(p.FileNamePath, "]}");
				if ( JsonFileDAL.UpdateRecNoRow("LastRecNo", p.FileNamePath, rCntr.ToString()))
				{
					Console.WriteLine("Updated RowsNumber");
				}
			}

			l.SourceRows = RowsCount("select count(*) from GFCC_FLOW.POLICIES where FORMAT (modified_date,  'dd/MM/yyyy ', 'en-us') ='01/02/2021'", SharedUtils.GetDSN()).ToString();
			l.EndTime = DateTime.Now;

			return l;
		}

	}
}
