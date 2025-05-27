
using Microsoft.Data.SqlClient;

namespace AF.DAL
{
	public class CanonicalDelta
	{
		private static string strDataPeriod = " MODIFIED_DATE >= convert(datetime,'2021-11-17 20:36:19.947') or Create_DATE >= convert(datetime,'2021-11-17 20:36:19.947')  ";

		public static bool ExtractionBundle()
		{
			string mailAddress = AF.DAL.TaskSchedulerDAO.GetEmailAddByID((1).ToString());
			bool vRet = false;
			ExtractionLog l;

			l = new ExtractionLog();
			try { l = ExtractDeltaPoliciesSP(); l.FromDate = DateTime.Now; l.ToDate = DateTime.Now; l.LogDate = DateTime.Now; l.ExtractionStatus = true; l.FileTransferStatus = false; l.ExceptionThrown = ""; }
			catch (Exception e) { l.ExtractionStatus = false; l.ExceptionThrown = e.ToString();  }
			ExtractionLog.LogIt(l);

			vRet = true;
			

			return vRet;

		}

		public static ExtractionLog ExtractDeltaCustomerSP()
		{

			ExtractionLog l = new ExtractionLog();
			l.ExeTime = DateTime.Now;

			InputRequestParam p = new InputRequestParam();
			p.JsonTitle = "CUSTOMERS";


            l.SourceRows = ExtractionCanonicalDAO.RowsCount(@"select count(*) FROM [GFCC_FLOW].[CUSTOMER_POLICY_LINK] t1  
inner join [GFCC_FLOW].[POLICIES] t2 on t1.POLICY_ID=t2.POLICY_SOURCE_UNIQUE_ID  
INNER JOIN GFCC_DM.dbo.Policy t3 ON t3.PolicyNumber = t2.POLICY_SOURCE_UNIQUE_ID AND t3.SystemSRC_CD = 'DPIAM'  
left join GFCC_FLOW.CUSTOMER t4 on t1.CUSTOMER_ID=t4.CUSTOMER_SOURCE_UNIQUE_ID 
 where t1.MODIFIED_DATE >= convert(datetime,'2021-11-17 20:36:19.947') or t1.Create_DATE >= convert(datetime,'2021-11-17 20:36:19.947')"
, SharedUtils.GetDSNGFCC()).ToString();
			l.TaskName = p.JsonTitle;
			p.StringSQLCommand = "[WebApp].[SpGetCustomerDplk]";

            p.CanonicalModelVersion = "2.0";
			if (Convert.ToInt64(l.SourceRows) < 1)
			{
				Console.WriteLine("No Data Available -1");
				l.ExceptionThrown = "No Data!";
				l.ExtractionStatus = false;
				l.EndTime = DateTime.Now;
				return l;
			}

			#region CreateFile

			p.GetColumns = CustomerID.getColumns();
			var lDt = new List<CustomerID>();
			var oDr = new CustomerID();
			oDr.CUSTOMER_SOURCE_UNIQUE_ID = "1234";
			lDt.Add(oDr);
			p.GetInitiateData = lDt;
			CreateInitiateJsonFile c = new CreateInitiateJsonFile(p);
			l.FilePath = p.DbFileNamePath;

			if (JsonFileDAL.DeleteDataRow("Data", p.FileNamePath, "CUSTOMER_SOURCE_UNIQUE_ID", "1234"))
			{
				Console.WriteLine("Deleted Data");
			}
			string allText = File.ReadAllText(p.FileNamePath);
			StreamWriter strm = File.CreateText(p.FileNamePath);
			strm.Flush();
			strm.Close();
			File.AppendAllText(p.FileNamePath, allText.Substring(0, allText.Length - 2));
			allText = "";

			Int32 ttlRows, numberOfRowsPerPage;
			ttlRows = Convert.ToInt32(l.SourceRows); numberOfRowsPerPage = 10000;

			Int32 numberOfLoop;
			if (ttlRows < numberOfRowsPerPage)
			{
				numberOfLoop = 1;
			}
			else
			{
				numberOfLoop = SharedUtils.NumberOfPages(ttlRows, numberOfRowsPerPage);
			}


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
							ci.CANCELLED_DATE = (!results.IsDBNull(results.GetOrdinal("CANCELLEDDATE"))) ? results["CANCELLEDDATE"].ToString() : "";
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
							if (rslt)
							{
								ci = null;
							}
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
			catch (Exception e)
			{
				//Console.WriteLine(e.ToString());
				l.ExceptionThrown = e.ToString() + Environment.NewLine;
			}

			l.ExtractionStatus = true;
			l.EndTime = DateTime.Now;
			#endregion

			return l;
		}

		public static ExtractionLog ExtractDeltaCustomerLinkSP()
		{

			ExtractionLog l = new ExtractionLog();
			l.ExeTime = DateTime.Now;

			InputRequestParam p = new InputRequestParam();
			p.JsonTitle = "CUSTOMER_CUSTOMER_LINK";
            
            l.SourceRows = ExtractionCanonicalDAO.RowsCount(@"select count(*) FROM [GFCC_FLOW].[CUSTOMER_POLICY_LINK] t1  
            inner join [GFCC_FLOW].[POLICIES] t2 on t1.POLICY_ID = t2.POLICY_SOURCE_UNIQUE_ID
            INNER JOIN GFCC_DM.dbo.Policy t3 ON t3.PolicyNumber = t2.POLICY_SOURCE_UNIQUE_ID AND t3.SystemSRC_CD = 'DPIAM'
            inner join GFCC_FLOW.CUSTOMER_LINK t4 on t1.CUSTOMER_ID = t4.CUSTOMER2_SOURCE_UNIQUE_ID
             where t1.MODIFIED_DATE >= convert(datetime, '2021-11-17 20:36:19.947')
             or t1.Create_DATE >= convert(datetime, '2021-11-17 20:36:19.947')", SharedUtils.GetDSNGFCC()).ToString();
            
			l.TaskName = p.JsonTitle;
			p.StringSQLCommand = "[WebApp].[SpGetCustomerCustomerLinkDPLK]";

			p.CanonicalModelVersion = "2.0";

			if (Convert.ToInt64(l.SourceRows) < 1)
			{
				Console.WriteLine("No Data Available -2");
				l.ExceptionThrown = "No Data!";
				l.ExtractionStatus = false;
				l.EndTime = DateTime.Now;
				return l;
			}
            
			if (Convert.ToInt64(l.SourceRows) > 0) {

			#region CreateFile
			p.GetColumns = CustomerLinkID.getColumns();
			var lDt = new List<CustomerLinkID>();
			var oDr = new CustomerLinkID();
			oDr.CUSTOMER1_SOURCE_UNIQUE_ID = "1234";
			lDt.Add(oDr);
			p.GetInitiateData = lDt;
			CreateInitiateJsonFile c = new CreateInitiateJsonFile(p);
			l.FilePath = p.DbFileNamePath;

			if (JsonFileDAL.DeleteDataRow("Data", p.FileNamePath, "CUSTOMER1_SOURCE_UNIQUE_ID", "1234"))
			{
				Console.WriteLine("Deleted Data");
			}
			string allText = File.ReadAllText(p.FileNamePath);
			StreamWriter strm = File.CreateText(p.FileNamePath);
			strm.Flush();
			strm.Close();
			File.AppendAllText(p.FileNamePath, allText.Substring(0, allText.Length - 2));
			allText = "";

			Int32 ttlRows, numberOfRowsPerPage;
			ttlRows = Convert.ToInt32(l.SourceRows); numberOfRowsPerPage = 10000;

			Int32 numberOfLoop;
			if (ttlRows < numberOfRowsPerPage)
			{
				numberOfLoop = 1;
			}
			else
			{
				numberOfLoop = SharedUtils.NumberOfPages(ttlRows, numberOfRowsPerPage);
			}

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
							if (rslt)
							{
								cpl = null;
							}
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
			catch (Exception e)
			{
				//Console.WriteLine(e.ToString());
				l.ExceptionThrown = e.ToString() + Environment.NewLine;
			}

				l.ExtractionStatus = true;
				l.EndTime = DateTime.Now;
				
				#endregion

			}


			return l;
		}

		public static ExtractionLog ExtractDeltaCustomer2PolicyLinkSP()
		{

			ExtractionLog l = new ExtractionLog();
			l.ExeTime = DateTime.Now;

			InputRequestParam p = new InputRequestParam();
			p.JsonTitle = "CUSTOMER_POLICY_LINK";

			l.SourceRows = ExtractionCanonicalDAO.RowsCount(@"select count(*) FROM [GFCC_FLOW].[CUSTOMER_POLICY_LINK] t1  
inner join [GFCC_FLOW].[POLICIES] t2 on t1.POLICY_ID = t2.POLICY_SOURCE_UNIQUE_ID
INNER JOIN GFCC_DM.dbo.Policy t3 ON t3.PolicyNumber = t2.POLICY_SOURCE_UNIQUE_ID AND t3.SystemSRC_CD = 'DPIAM'
 where t1.MODIFIED_DATE >= convert(datetime, '2021-11-21 21:55:59.947')
 or t1.Create_DATE >= convert(datetime, '2021-11-21 21:55:59.947') ", SharedUtils.GetDSNGFCC()).ToString();
			l.TaskName = p.JsonTitle;
			p.StringSQLCommand = "[WebApp].[SpGetCustomerPolicyLinkDPLK]";
			if (Convert.ToInt64(l.SourceRows) < 1)
			{
				Console.WriteLine("No Data Available -3");
				l.ExceptionThrown = "No Data!";
				l.ExtractionStatus = false;
				l.EndTime = DateTime.Now;
				return l;
			}

			p.CanonicalModelVersion = "2.0";
			p.GetColumns = CustomerPolicyLink.getColumns();
			var lDt = new List<CustomerPolicyLink>();
			var oDr = new CustomerPolicyLink();
			oDr.CUSTOMER_ID = "1234";
			lDt.Add(oDr);
			p.GetInitiateData = lDt;
			CreateInitiateJsonFile c = new CreateInitiateJsonFile(p);
			l.FilePath = p.DbFileNamePath;

			if (JsonFileDAL.DeleteDataRow("Data", p.FileNamePath, "CUSTOMER_ID", "1234"))
			{
				Console.WriteLine("Deleted Data");
			}
			string allText = File.ReadAllText(p.FileNamePath);
			StreamWriter strm = File.CreateText(p.FileNamePath);
			strm.Flush();
			strm.Close();
			File.AppendAllText(p.FileNamePath, allText.Substring(0, allText.Length - 2));
			allText = "";

			Int32 ttlRows, numberOfRowsPerPage;
			ttlRows = Convert.ToInt32(l.SourceRows); numberOfRowsPerPage = 10000;

			Int32 numberOfLoop;
			if (ttlRows < numberOfRowsPerPage)
			{
				numberOfLoop = 1;
			}
			else
			{
				numberOfLoop = SharedUtils.NumberOfPages(ttlRows, numberOfRowsPerPage);
			}

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
							CustomerPolicyLink ci = new CustomerPolicyLink()
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

							var rslt = JsonFileDAL.WriteDataToFile(p.FileNamePath, ci);
							if (rslt)
							{
								ci = null;
							}
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
			catch (Exception e)
			{
				//Console.WriteLine(e.ToString());
				l.ExceptionThrown = e.ToString() + Environment.NewLine;
			}

			l.ExtractionStatus = true;
			l.EndTime = DateTime.Now;

			return l;
		}

		public static ExtractionLog ExtractDeltaIntermediarySP()
		{

			ExtractionLog l = new ExtractionLog();
			l.ExeTime = DateTime.Now;

			InputRequestParam p = new InputRequestParam();
			p.JsonTitle = "INTERMEDIARIES";

			l.SourceRows = ExtractionCanonicalDAO.RowsCount("select count(*) from GFCC_FLOW.INTERMEDIARIES where " + strDataPeriod, SharedUtils.GetDSNGFCC()).ToString();
			l.TaskName = p.JsonTitle;
			p.StringSQLCommand = "[WebApp].[SpGetIntermediaries]";
			if (Convert.ToInt64(l.SourceRows) < 1)
			{
				Console.WriteLine("No Data Available -4");
				l.ExceptionThrown = "No Data!";
				l.ExtractionStatus = false;
				l.EndTime = DateTime.Now;
				return l;
			}

            p.CanonicalModelVersion = "2.0";
			p.GetColumns = Intermediaries.getColumns();
			var lDt = new List<Intermediaries>();
			var oDr = new Intermediaries();
			oDr.INTERMEDIARY_SOURCE_UNIQUE_ID = "1234";
			lDt.Add(oDr);
			p.GetInitiateData = lDt;
			CreateInitiateJsonFile c = new CreateInitiateJsonFile(p);
			l.FilePath = p.DbFileNamePath;

			if (JsonFileDAL.DeleteDataRow("Data", p.FileNamePath, "INTERMEDIARY_SOURCE_UNIQUE_ID", "1234"))
			{
				Console.WriteLine("Deleted Data");
			}
			string allText = File.ReadAllText(p.FileNamePath);
			StreamWriter strm = File.CreateText(p.FileNamePath);
			strm.Flush();
			strm.Close();
			File.AppendAllText(p.FileNamePath, allText.Substring(0, allText.Length - 2));
			allText = "";

			Int32 ttlRows, numberOfRowsPerPage;
			ttlRows = Convert.ToInt32(l.SourceRows); numberOfRowsPerPage = 10000;

			Int32 numberOfLoop;
			if (ttlRows < numberOfRowsPerPage)
			{
				numberOfLoop = 1;
			}
			else
			{
				numberOfLoop = SharedUtils.NumberOfPages(ttlRows, numberOfRowsPerPage);
			}

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
							Intermediaries ci = new Intermediaries()
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

							var rslt = JsonFileDAL.WriteDataToFile(p.FileNamePath, ci);
							if (rslt)
							{
								ci = null;
							}
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
			catch (Exception e)
			{
				//Console.WriteLine(e.ToString());
				l.ExceptionThrown = e.ToString() + Environment.NewLine;
			}

			l.ExtractionStatus = true;
			l.EndTime = DateTime.Now;

			return l;
		}

		public static ExtractionLog ExtractDeltaIntermediaryPolicyLinkSP()
		{

			ExtractionLog l = new ExtractionLog();
			l.ExeTime = DateTime.Now;

			InputRequestParam p = new InputRequestParam();
			p.JsonTitle = "INTERMEDIARY_POLICY_LINK";

			l.SourceRows = ExtractionCanonicalDAO.RowsCount("select count(*) from GFCC_FLOW.INTERMEDIARY_POLICY_LINK where " + strDataPeriod, SharedUtils.GetDSNGFCC()).ToString();
			l.TaskName = p.JsonTitle;
			p.StringSQLCommand = "[WebApp].[SpGetIntermediaryPolicyLink]";
			if (Convert.ToInt64(l.SourceRows) < 1)
			{
				Console.WriteLine("No Data Available -5");
				l.ExceptionThrown = "No Data!";
				l.ExtractionStatus = false;
				l.EndTime = DateTime.Now;
				return l;
			}

            p.CanonicalModelVersion = "2.0";
			p.GetColumns = Intermediary_Policy_Link.getColumns();
			var lDt = new List<Intermediary_Policy_Link>();
			var oDr = new Intermediary_Policy_Link();
			oDr.INTERMEDIARY_ID = "1234";
			lDt.Add(oDr);
			p.GetInitiateData = lDt;
			CreateInitiateJsonFile c = new CreateInitiateJsonFile(p);
			l.FilePath = p.DbFileNamePath;

			if (JsonFileDAL.DeleteDataRow("Data", p.FileNamePath, "INTERMEDIARY_ID", "1234"))
			{
				Console.WriteLine("Deleted Data");
			}
			string allText = File.ReadAllText(p.FileNamePath);
			StreamWriter strm = File.CreateText(p.FileNamePath);
			strm.Flush();
			strm.Close();
			File.AppendAllText(p.FileNamePath, allText.Substring(0, allText.Length - 2));
			allText = "";

			Int32 ttlRows, numberOfRowsPerPage;
			ttlRows = Convert.ToInt32(l.SourceRows); numberOfRowsPerPage = 10000;

			Int32 numberOfLoop;
			if (ttlRows < numberOfRowsPerPage)
			{
				numberOfLoop = 1;
			}
			else
			{
				numberOfLoop = SharedUtils.NumberOfPages(ttlRows, numberOfRowsPerPage);
			}

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
							Intermediary_Policy_Link ci = new Intermediary_Policy_Link()
							{
								POLICY_ID = (!results.IsDBNull(results.GetOrdinal("POLICY_ID"))) ? results["POLICY_ID"].ToString() : "",
								INTERMEDIARY_ID = (!results.IsDBNull(results.GetOrdinal("INTERMEDIARY_ID"))) ? results["INTERMEDIARY_ID"].ToString() : "",
								PARTY_ROLE_CODE = (!results.IsDBNull(results.GetOrdinal("PARTY_ROLE_CODE"))) ? results["PARTY_ROLE_CODE"].ToString() : "",
								PARTY_RELATIONSHIP = null,
								FROM_DATE = (!results.IsDBNull(results.GetOrdinal("FROM_DATE"))) ? Convert.ToDateTime(results["FROM_DATE"]).ToString("dd/MM/yyyy") : "00/00/0000",
								TO_DATE = (!results.IsDBNull(results.GetOrdinal("TO_DATE"))) ? Convert.ToDateTime(results["TO_DATE"]).ToString("dd/MM/yyyy") : "00/00/0000",
							};

							var rslt = JsonFileDAL.WriteDataToFile(p.FileNamePath, ci);
							if (rslt)
							{
								ci = null;
							}
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
			catch (Exception e)
			{
				//Console.WriteLine(e.ToString());
				l.ExceptionThrown = e.ToString() + Environment.NewLine;
			}

			l.ExtractionStatus = true;
			l.EndTime = DateTime.Now;

			return l;
		}

		public static ExtractionLog ExtractDeltaOperationSP()
		{

			ExtractionLog l = new ExtractionLog();
			l.ExeTime = DateTime.Now;

			InputRequestParam p = new InputRequestParam();
			p.JsonTitle = "OPERATIONS";

			l.SourceRows = ExtractionCanonicalDAO.RowsCount("select count(*) from GFCC_FLOW.OPERATIONS where TXN_TYPE_CODE IN ('TT09', 'TT06', 'TT11')" , SharedUtils.GetDSNGFCC()).ToString();

            l.TaskName = p.JsonTitle;
			p.StringSQLCommand = "[WebApp].[SpGetOperation]";
			if (Convert.ToInt64(l.SourceRows) < 1)
			{
				Console.WriteLine("No Data Available -6");
				l.ExceptionThrown = "No Data!";
				l.ExtractionStatus = false;
				l.EndTime = DateTime.Now;
				return l;
			}

            p.CanonicalModelVersion = "2.0";
			p.GetColumns = Operation.getColumns();
			var lDt = new List<Operation>();
			var oDr = new Operation();
			oDr.OPERATION_SOURCE_UNIQUE_ID = "1234";
			lDt.Add(oDr);
			p.GetInitiateData = lDt;
			CreateInitiateJsonFile c = new CreateInitiateJsonFile(p);
			l.FilePath = p.DbFileNamePath;

			if (JsonFileDAL.DeleteDataRow("Data", p.FileNamePath, "OPERATION_SOURCE_UNIQUE_ID", "1234"))
			{
				Console.WriteLine("Deleted Data");
			}
			string allText = File.ReadAllText(p.FileNamePath);
			StreamWriter strm = File.CreateText(p.FileNamePath);
			strm.Flush();
			strm.Close();
			File.AppendAllText(p.FileNamePath, allText.Substring(0, allText.Length - 2));
			allText = "";

			Int32 ttlRows, numberOfRowsPerPage;
			ttlRows = Convert.ToInt32(l.SourceRows); numberOfRowsPerPage = 10000;

			Int32 numberOfLoop;
			if (ttlRows < numberOfRowsPerPage)
			{
				numberOfLoop = 1;
			}
			else
			{
				numberOfLoop = SharedUtils.NumberOfPages(ttlRows, numberOfRowsPerPage);
			}

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


							var rslt = JsonFileDAL.WriteDataToFile(p.FileNamePath, cpl);
							if (rslt)
							{
								cpl = null;
							}
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
			catch (Exception e)
			{
				//Console.WriteLine(e.ToString());
				l.ExceptionThrown = e.ToString() + Environment.NewLine;
			}

			l.ExtractionStatus = true;
			l.EndTime = DateTime.Now;

			return l;
		}

		public static ExtractionLog ExtractDeltaPoliciesSP()
		{

			ExtractionLog l = new ExtractionLog();
			l.ExeTime = DateTime.Now;

			InputRequestParam p = new InputRequestParam();
			p.JsonTitle = "POLICIES";


            l.SourceRows = ExtractionCanonicalDAO.RowsCount(@"select count(*) from GFCC_FLOW.POLICIES  t1 
INNER JOIN GFCC_DM.dbo.Policy t2 ON t2.PolicyNumber = t1.POLICY_SOURCE_UNIQUE_ID AND t2.SystemSRC_CD = 'DPIAM' 
where t1.MODIFIED_DATE >= convert(datetime, '2021-11-21 21:55:59.947')
 or t1.Create_DATE >= convert(datetime, '2021-11-21 21:55:59.947') " , SharedUtils.GetDSNGFCC()).ToString();
			l.TaskName = p.JsonTitle;
			p.StringSQLCommand = "[WebApp].[SpGetPoliciesDPLK]";
			if (Convert.ToInt64(l.SourceRows) < 1)
			{
				Console.WriteLine("No Data Available -7");
				l.ExceptionThrown = "No Data!";
				l.ExtractionStatus = false;
				l.EndTime = DateTime.Now;
				return l;
			}

            p.CanonicalModelVersion = "2.0";
			p.GetColumns = Policies.getColumns();
			var lDt = new List<Policies>();
			var oDr = new Policies();
			oDr.POLICY_SOURCE_UNIQUE_ID = "1234";
			lDt.Add(oDr);
			p.GetInitiateData = lDt;
			CreateInitiateJsonFile c = new CreateInitiateJsonFile(p);
			l.FilePath = p.DbFileNamePath;

			if (JsonFileDAL.DeleteDataRow("Data", p.FileNamePath, "POLICY_SOURCE_UNIQUE_ID", "1234"))
			{
				Console.WriteLine("Deleted Data");
			}
			string allText = File.ReadAllText(p.FileNamePath);
			StreamWriter strm = File.CreateText(p.FileNamePath);
			strm.Flush();
			strm.Close();
			File.AppendAllText(p.FileNamePath, allText.Substring(0, allText.Length - 2));
			allText = "";

			Int32 ttlRows, numberOfRowsPerPage;
			ttlRows = Convert.ToInt32(l.SourceRows); numberOfRowsPerPage = 10000;

			Int32 numberOfLoop;
			if (ttlRows < numberOfRowsPerPage)
			{
				numberOfLoop = 1;
			}
			else
			{
				numberOfLoop = SharedUtils.NumberOfPages(ttlRows, numberOfRowsPerPage);
			}

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
							Policies ci = new Policies()
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

							var rslt = JsonFileDAL.WriteDataToFile(p.FileNamePath, ci);
							if (rslt)
							{
								ci = null;
							}
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
			catch (Exception e)
			{
				//Console.WriteLine(e.ToString());
				l.ExceptionThrown = e.ToString() + Environment.NewLine;
			}

			l.ExtractionStatus = true;
			l.EndTime = DateTime.Now;

			return l;
		}

		public static ExtractionLog ExtractDeltaProductSP()
		{

			ExtractionLog l = new ExtractionLog();
			l.ExeTime = DateTime.Now;

			InputRequestParam p = new InputRequestParam();
			p.JsonTitle = "PRODUCT";

			l.SourceRows = ExtractionCanonicalDAO.RowsCount("select count(*) from GFCC_FLOW.PRODUCT where " + strDataPeriod, SharedUtils.GetDSNGFCC()).ToString();
			l.TaskName = p.JsonTitle;
			p.StringSQLCommand = "[WebApp].[SpGetProducts]";
			if (Convert.ToInt64(l.SourceRows) < 1)
			{
				Console.WriteLine("No Data Available -8");
				l.ExceptionThrown = "No Data!";
				l.ExtractionStatus = false;
				l.EndTime = DateTime.Now;
				return l;
			}

            p.CanonicalModelVersion = "2.0";
			p.GetColumns = Product.getColumns();
			var lDt = new List<Product>();
			var oDr = new Product();
			oDr.PRODUCT_ID = "1234";
			lDt.Add(oDr);
			p.GetInitiateData = lDt;
			CreateInitiateJsonFile c = new CreateInitiateJsonFile(p);
			l.FilePath = p.DbFileNamePath;

			if (JsonFileDAL.DeleteDataRow("Data", p.FileNamePath, "PRODUCT_ID", "1234"))
			{
				Console.WriteLine("Deleted Data");
			}
			string allText = File.ReadAllText(p.FileNamePath);
			StreamWriter strm = File.CreateText(p.FileNamePath);
			strm.Flush();
			strm.Close();
			File.AppendAllText(p.FileNamePath, allText.Substring(0, allText.Length - 2));
			allText = "";

			Int32 ttlRows, numberOfRowsPerPage;
			ttlRows = Convert.ToInt32(l.SourceRows); numberOfRowsPerPage = 10000;

			Int32 numberOfLoop;
			if (ttlRows < numberOfRowsPerPage)
			{
				numberOfLoop = 1;
			}
			else
			{
				numberOfLoop = SharedUtils.NumberOfPages(ttlRows, numberOfRowsPerPage);
			}

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
							Product ci = new Product()
							{
								PRODUCT_ID = (!results.IsDBNull(results.GetOrdinal("PRODUCT_ID"))) ? results["PRODUCT_ID"].ToString() : "",
								PRODUCT_NAME = (!results.IsDBNull(results.GetOrdinal("PRODUCT_NAME"))) ? results["PRODUCT_NAME"].ToString() : "",
								PRODUCT_GROUP = (!results.IsDBNull(results.GetOrdinal("PRODUCT_GROUP"))) ? results["PRODUCT_GROUP"].ToString() : "",
								PRODUCT_CLASS = (!results.IsDBNull(results.GetOrdinal("PRODUCT_CATEGORY"))) ? results["PRODUCT_CATEGORY"].ToString() : "",
								PRODUCT_LINE = null
							};

							var rslt = JsonFileDAL.WriteDataToFile(p.FileNamePath, ci);
							if (rslt)
							{
								ci = null;
							}
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
			catch (Exception e)
			{
				//Console.WriteLine(e.ToString());
				l.ExceptionThrown = e.ToString() + Environment.NewLine;
			}

			l.ExtractionStatus = true;
			l.EndTime = DateTime.Now;

			return l;
		}

		public static ExtractionLog ExtractDeltaProductSourceTypeSP()
		{

			ExtractionLog l = new ExtractionLog();
			l.ExeTime = DateTime.Now;

			InputRequestParam p = new InputRequestParam();
			p.JsonTitle = "PRODUCT_SOURCE_TYPE";

			l.SourceRows = ExtractionCanonicalDAO.RowsCount("select count(*) from GFCC_FLOW.PRODUCT_SOURCE_TYPE where " + strDataPeriod, SharedUtils.GetDSNGFCC()).ToString();
			l.TaskName = p.JsonTitle;
			p.StringSQLCommand = "[WebApp].[SpGetProductSourceType]";
			if (Convert.ToInt64(l.SourceRows) < 1)
			{
				Console.WriteLine("No Data Available -9");
				l.ExceptionThrown = "No Data!";
				l.ExtractionStatus = false;
				l.EndTime = DateTime.Now;
				return l;
			}

            p.CanonicalModelVersion = "2.0";
			p.GetColumns = ProductSource.getColumns();
			var lDt = new List<ProductSource>();
			var oDr = new ProductSource();
			oDr.PRODUCT_ID = "1234";
			lDt.Add(oDr);
			p.GetInitiateData = lDt;
			CreateInitiateJsonFile c = new CreateInitiateJsonFile(p);
			l.FilePath = p.DbFileNamePath;

			if (JsonFileDAL.DeleteDataRow("Data", p.FileNamePath, "PRODUCT_ID", "1234"))
			{
				Console.WriteLine("Deleted Data");
			}
			string allText = File.ReadAllText(p.FileNamePath);
			StreamWriter strm = File.CreateText(p.FileNamePath);
			strm.Flush();
			strm.Close();
			File.AppendAllText(p.FileNamePath, allText.Substring(0, allText.Length - 2));
			allText = "";

			Int32 ttlRows, numberOfRowsPerPage;
			ttlRows = Convert.ToInt32(l.SourceRows); numberOfRowsPerPage = 10000;

			Int32 numberOfLoop;
			if (ttlRows < numberOfRowsPerPage)
			{
				numberOfLoop = 1;
			}
			else
			{
				numberOfLoop = SharedUtils.NumberOfPages(ttlRows, numberOfRowsPerPage);
			}

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
							ProductSource ci = new ProductSource()
							{
								PRODUCT_SOURCE_TYPE_CODE = (!results.IsDBNull(results.GetOrdinal("PRODUCT_SOURCE_TYPE_CODE"))) ? results["PRODUCT_SOURCE_TYPE_CODE"].ToString() : "",
								PRODUCT_SOURCE_TYPE_DESC = (!results.IsDBNull(results.GetOrdinal("PRODUCT_SOURCE_TYPE_DESC"))) ? results["PRODUCT_SOURCE_TYPE_DESC"].ToString() : "",
								PRODUCT_ID = (!results.IsDBNull(results.GetOrdinal("PRODUCT_ID"))) ? results["PRODUCT_ID"].ToString() : ""

							};

							var rslt = JsonFileDAL.WriteDataToFile(p.FileNamePath, ci);
							if (rslt)
							{
								ci = null;
							}
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
			catch (Exception e)
			{
				//Console.WriteLine(e.ToString());
				l.ExceptionThrown = e.ToString() + Environment.NewLine;
			}

			l.ExtractionStatus = true;
			l.EndTime = DateTime.Now;

			return l;
		}

	}
}
