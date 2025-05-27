using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace AF.DAL
{
	public class ManualProcessCanonical
	{
		//private static string strDataPeriod = " MODIFIED_DATE >= convert(datetime,'2021-07-13 16:32:00.317') or Create_DATE >= convert(datetime,'2021-07-13 16:32:00.317')  ";
		private static string strDataPeriod = @"(convert(date,Create_DATE) >= convert(date,convert(char(8),@FromDt)) and convert(date,Create_DATE) <= convert(date,convert(char(8),ToDt)))
			or (convert(date,MODIFIED_DATE) >= convert(date,convert(char(8),@FromDt)) and convert(date,MODIFIED_DATE) <= convert(date,convert(char(8),ToDt)))";

        private static bool extractStatus = false;
		private static string orgUnit = "054";

        public static bool extractedStatus()
        {
            return extractStatus;
        }

		

        public static string[] GetPeriodByID(int ID) {
			string[] vRet = new string[2];
			string str = SharedUtils.GetDataSingleValue(@"SELECT concat(FromDate,',',ToDate) as period FROM [WebApp].[ManualExe] where ID= " + ID.ToString() + ";", SharedUtils.GetDSN());
			string[] res = str.Split(',');
			vRet[0] = res[0];vRet[1] = res[1];
			return vRet;
		}
		public static string GetRangeCondition(string range)
		{
			string condition = "";
			if (range != "")
			{
				//'01202111'  '04202111'
				char[] delimiterChars = { '-', ',' };
				string[] w = range.Split(delimiterChars);
				//System.Console.WriteLine($"{ w.Length} words in text:");
				condition = " ( MODIFIED_DATE >=  '" + w[0] + w[1]  + w[2].Trim() +  " 00:00:00' and MODIFIED_DATE <= '"  + w[3] + w[4] + w[5].Trim() + " 23:59:59')";// AND X_SOURCE_SYSTEM IN ('LIAM', 'DIAM', 'G41IAM')
			}
			else
			{
				DateTime yesterDay = DateTime.Now.AddDays(-1);
				condition = " where MODIFIED_DATE >=  '" + yesterDay.ToString("yyyyMMdd") + "'";
			}
			return condition;
		}

		public static string GetRangeConditionByDate(DateTime d1,DateTime d2)
		{
			string condition = "";
			if (d1 != null && d2 != null)
			{
				condition = " ( MODIFIED_DATE >=  '" + d1.ToString("yyyy-MM-dd") + " 00:00:00' and MODIFIED_DATE <= '" + d2.ToString("yyyy-MM-dd") + " 23:59:59')"; // AND X_SOURCE_SYSTEM IN ('LIAM', 'DIAM', 'G41IAM')
			}
			return condition;
		}

		public static bool ManualExtractionByTaskID(string customRunTimestamp = "")
		{
			bool vRet = false;

			try
			{
				var isGood = ManualCustomerExtraction(customRunTimestamp);
				if (isGood) { vRet = true; }
				
			}
			catch (Exception e)
			{

				vRet = false;
			}

			return vRet;

		}


		public static bool ManualCustomerExtraction(string customTimestamp = "")
		{
			ManualExeDAO m = new ManualExeDAO(1);
			string period = GetRangeConditionByDate(m.FromDate,m.ToDate);

			var lExtract = new List<ExtractionLog>();
			
			try
			{
				var now = customTimestamp == null ? DateTime.Now : Convert.ToDateTime(customTimestamp);
				var sumExtractionFiles = 0;
				var orgUnit = SharedUtils.GetConfigs("OrgUnit");
				var timeLine = now.ToString("yyyyMMdd_yyyyMMddHHmmss");

				var runTimestamp = now.ToString("yyyyMMddHHmmss");
				
				if (!Directory.Exists(@ConfigurationManager.AppSettings["JsonDir"] + timeLine + @"\"))
				{
					DirectoryInfo folder = Directory.CreateDirectory(@ConfigurationManager.AppSettings["JsonDir"] + timeLine + @"\");
				}

				if (!Directory.Exists(@ConfigurationManager.AppSettings["JsonDst"] + timeLine + @"\"))
				{
					DirectoryInfo folder = Directory.CreateDirectory(@ConfigurationManager.AppSettings["JsonDst"] + timeLine + @"\");
				}
                //Testing 
                var conSourceData = SharedUtils.GetDSN();

              
                var hasilcont = Convert.ToString(SharedUtils.GetCountflow(conSourceData));

                foreach (var val in SharedUtils.GetConfigArray("GfccFlow"))
                {
					sumExtractionFiles++;
					
                    ExtractionLog l = new ExtractionLog();

					l = ExtractJsonBatching(orgUnit, val.Value, val.Desc, timeLine, runTimestamp); //ExtractDeltaCustomerSP(period);
					l.FromDate = DateTime.Now;
					l.ToDate = DateTime.Now;
					l.LogDate = DateTime.Now;
				
					extractStatus = l.ExtractionStatus;

					if (!extractStatus)
					{
						l.FileTransferStatus = false;
						//l.ExceptionThrown = "";

						//SharedUtils.SendEmailNotification("Customer Extraction", "Failed, to extract json file", m.Email);
					}
					else
					{
						l.FileTransferStatus = true;

						var fullName = orgUnit + "_" + val.Value + "_" + timeLine + ".json";
						
                        var nameEncrypt = JsonBatch.EncryptCMD(ConfigurationManager.AppSettings["JsonDir"] + timeLine + @"\", fullName, ConfigurationManager.AppSettings["JsonDir"] + timeLine + @"\", ConfigurationManager.AppSettings["KeyID_GPG"]);
						//var nameEncrypt = JsonBatch.EncryptCMDTest(@ConfigurationManager.AppSettings["JsonDir"] + timeLine + @"\", fullName);

						//SharedUtils.SendEmailNotification("Customer-Canonical Data Extraction", "Successfull", m.Email);
						l.ClienIp = timeLine;

                    }

					l.CountPath = hasilcont;

                    lExtract.Add(l);

					ExtractionLog.LogIt(l);
				}
			
				var okFile = orgUnit + "_" + timeLine + ".ok";
				//Baru ditambahkann
				var foals = Directory.CreateDirectory(@ConfigurationManager.AppSettings["JsonDir"] + timeLine +  @"\");
				var pathRs = foals + okFile;
                
                DirectoryInfo d = new DirectoryInfo(@ConfigurationManager.AppSettings["JsonDir"] + timeLine + @"\"); //Directory Generate Json File
				FileInfo[] Files = d.GetFiles("*.json");

				System.IO.File.WriteAllText(@ConfigurationManager.AppSettings["JsonDir"] + timeLine + @"\" + okFile, Files.Length.ToString());
                //System.IO.File.Copy(ConfigurationManager.AppSettings["JsonDir"] + okFile, @ConfigurationManager.AppSettings["JsonDst"] + okFile, true);
                ExtractionLog.LogOk(okFile, pathRs, Files.Length.ToString());
				return true;
            }
			catch (Exception e){

				System.IO.File.WriteAllText(@ConfigurationManager.AppSettings["WebLogs"], e.ToString());
				//extractStatus = l.ExtractionStatus = false;
				//l.ExceptionThrown += e.ToString();
				//SharedUtils.SendEmailNotification("Customer Extraction", "Failed, " + e.ToString(), m.Email);
				return false;
			}

            
		}
		public static void ManualCustomer2CustomerExtraction()
		{
			ManualExeDAO m = new ManualExeDAO(2);
			string period = GetRangeConditionByDate(m.FromDate, m.ToDate);
			ExtractionLog l;
			l = new ExtractionLog();
			try {
                l = ExtractDeltaCustomerLinkSP(period);
                l.FromDate = DateTime.Now; l.ToDate = DateTime.Now; l.LogDate = DateTime.Now;

                extractStatus = l.ExtractionStatus;

                if (!l.ExtractionStatus)
                {
                    l.FileTransferStatus = false; l.ExceptionThrown = "";
                   
                }
                else
                {
                   
                }
			}
			catch (Exception e)
			{
				
			}

			ExtractionLog.LogIt(l);

			
		}
		public static void ManualCustomer2PolicyExtraction()
		{
			ManualExeDAO m = new ManualExeDAO(3);
			string period = GetRangeConditionByDate(m.FromDate, m.ToDate);
			ExtractionLog l;
			l = new ExtractionLog();
			try {
                l = ExtractDeltaCustomer2PolicyLinkSP(m.FromDate, m.ToDate, period);
                l.FromDate = DateTime.Now; l.ToDate = DateTime.Now; l.LogDate = DateTime.Now;

                extractStatus = l.ExtractionStatus;

                if (!l.ExtractionStatus) {
                    l.FileTransferStatus = false; l.ExceptionThrown = "";
                   
                }
                else
                {
                    
                }
			}
			catch (Exception e)
			{
				l.ExtractionStatus = false; l.ExceptionThrown += e.ToString();
       
			}
			ExtractionLog.LogIt(l);
		}
		public static void ManualPoliciesExtraction()
		{
			ManualExeDAO m = new ManualExeDAO(4);
			string period = GetRangeConditionByDate(m.FromDate, m.ToDate);
			ExtractionLog l;
			l = new ExtractionLog();
			try {
                l = ExtractDeltaPoliciesSP(period);
                l.FromDate = DateTime.Now;
                l.ToDate = DateTime.Now; l.LogDate = DateTime.Now;

                extractStatus = l.ExtractionStatus;

                if (!l.ExtractionStatus) {
                    l.FileTransferStatus = false; l.ExceptionThrown = "";

                    
                }
                else
                {
                   
                }
			}
			catch (Exception e)
			{
				l.ExtractionStatus = false; l.ExceptionThrown += e.ToString();
               
            }
			ExtractionLog.LogIt(l);
		}
		public static void ManualOperationExtraction()
		{
			ManualExeDAO m = new ManualExeDAO(6);
			string period = GetRangeConditionByDate(m.FromDate, m.ToDate);
			ExtractionLog l;
			l = new ExtractionLog();
			try {
                l = ExtractDeltaOperationSP(period);
                l.FromDate = DateTime.Now;
                l.ToDate = DateTime.Now; l.LogDate = DateTime.Now;

                extractStatus = l.ExtractionStatus;

                if (!l.ExtractionStatus) {
                    l.FileTransferStatus = false; l.ExceptionThrown = "";
                  
                }
                else
                {
                   
                }
			}
			catch (Exception e)
			{
				l.ExtractionStatus = false; l.ExceptionThrown += e.ToString();
   
			}
			ExtractionLog.LogIt(l);

		}
		public static void ManualIntermediariesExtraction()
		{
			ManualExeDAO m = new ManualExeDAO(7);
			string period = GetRangeConditionByDate(m.FromDate, m.ToDate);
			ExtractionLog l;
			l = new ExtractionLog();
			try {
                l = ExtractDeltaIntermediarySP(period);
                l.FromDate = DateTime.Now;
                l.ToDate = DateTime.Now;
                l.LogDate = DateTime.Now;

                extractStatus = l.ExtractionStatus;

                if (!l.ExtractionStatus) {
                    l.FileTransferStatus = false; l.ExceptionThrown = "";
                    
                }
                else
                {

                   
                }
			}
			catch (Exception e)
			{
				l.ExtractionStatus = false; l.ExceptionThrown += e.ToString();
               
            }

			ExtractionLog.LogIt(l);

		}
		public static void ManualIntermediaryPolicyExtraction()
		{
			ManualExeDAO m = new ManualExeDAO(8);
			string period = GetRangeConditionByDate(m.FromDate, m.ToDate);
			ExtractionLog l;
			l = new ExtractionLog();
			try {
                l = ExtractDeltaIntermediaryPolicyLinkSP(period);
                l.FromDate = DateTime.Now;
                l.ToDate = DateTime.Now;
                l.LogDate = DateTime.Now;

                extractStatus = l.ExtractionStatus;

                if (!l.ExtractionStatus) {
                    l.FileTransferStatus = false; l.ExceptionThrown = "";
                   
                }
                else
                {

                }
			}
			catch (Exception e)
			{
				l.ExtractionStatus = false; l.ExceptionThrown += e.ToString();
               
			}
			ExtractionLog.LogIt(l);

		}
		public static void ManualProductExtraction()
		{
			ManualExeDAO m = new ManualExeDAO(9);
			string period = GetRangeConditionByDate(m.FromDate, m.ToDate);
			ExtractionLog l;
			l = new ExtractionLog();
			try {
                l = ExtractDeltaProductSP(period);
                l.FromDate = DateTime.Now;
                l.ToDate = DateTime.Now;
                l.LogDate = DateTime.Now;

                extractStatus = l.ExtractionStatus;

                if (!l.ExtractionStatus) {
                    l.FileTransferStatus = false; l.ExceptionThrown = "";
                    
                }
                else
                {

                 
                }
			}
			catch (Exception e)
			{
				l.ExtractionStatus = false; l.ExceptionThrown += e.ToString();
               
			}
			ExtractionLog.LogIt(l);

		}
		public static void ManualProductSourceTypeExtraction()
		{
			ManualExeDAO m = new ManualExeDAO(10);
			string period = GetRangeConditionByDate(m.FromDate, m.ToDate);
			ExtractionLog l;
			l = new ExtractionLog();
			try {
                l = ExtractDeltaProductSourceTypeSP(period);
                l.FromDate = DateTime.Now;
                l.ToDate = DateTime.Now;
                l.LogDate = DateTime.Now;

                extractStatus = l.ExtractionStatus;

                if (!l.ExtractionStatus) {
                    l.FileTransferStatus = false; l.ExceptionThrown = "";
                  
                }
                else
                {
                  
                }
			}
			catch (Exception e)
			{
				l.ExtractionStatus = false; l.ExceptionThrown += e.ToString();
             
			}
			ExtractionLog.LogIt(l);
            
		}

		public static bool ExtractionBundle()
		{
			string mailAddress = AF.DAL.TaskSchedulerDAO.GetEmailAddByID((1).ToString());
			bool vRet = false;
			ExtractionLog l;
			l = new ExtractionLog();
			try {
                //l = null;
                l.FromDate = DateTime.Now; l.ToDate = DateTime.Now; l.LogDate = DateTime.Now;
                l.ExtractionStatus = true; l.FileTransferStatus = false; l.ExceptionThrown = "";
            }
			catch (Exception e)
			{
				l.ExtractionStatus = false; l.ExceptionThrown += e.ToString();
                
			}
			ExtractionLog.LogIt(l);
			

			vRet = true;
			

			return vRet;

		}

		public static bool ReRunFailed(string tName)
		{
			bool vRet = false;

			string mailAddress = AF.DAL.TaskSchedulerDAO.GetEmailAddByID((1).ToString());
			ExtractionLog l;

			switch (tName)
			{
				case "CUSTOMERS":
					try
					{
						l = new ExtractionLog();
						try { l = CanonicalDelta.ExtractDeltaCustomerSP(); l.FromDate = DateTime.Now; l.ToDate = DateTime.Now; l.LogDate = DateTime.Now;
							if (l.ExtractionStatus) {
                                l.FileTransferStatus = false; l.ExceptionThrown = "";
                            }
						}
						catch (Exception e)
						{
							
						}
						ExtractionLog.LogIt(l);
						vRet = true;
					}
					catch
					{
						vRet = false;
					}
					break;
				case "CUSTOMER_CUSTOMER_LINK":
					try
					{
						l = new ExtractionLog();
						try { l = CanonicalDelta.ExtractDeltaCustomerLinkSP(); l.FromDate = DateTime.Now; l.ToDate = DateTime.Now; l.LogDate = DateTime.Now; l.ExtractionStatus = true; l.FileTransferStatus = false; l.ExceptionThrown = ""; }
						catch (Exception e)
						{
							
						}
						ExtractionLog.LogIt(l);
						vRet = true;
					}
					catch
					{
						vRet = false;
					}
					break;
				case "CUSTOMER_POLICY_LINK":
					try
					{
						l = new ExtractionLog();
						try { l = CanonicalDelta.ExtractDeltaCustomer2PolicyLinkSP(); l.FromDate = DateTime.Now; l.ToDate = DateTime.Now; l.LogDate = DateTime.Now; l.ExtractionStatus = true; l.FileTransferStatus = false; l.ExceptionThrown = ""; }
						catch (Exception e)
						{
							
						}
						ExtractionLog.LogIt(l);
						vRet = true;
					}
					catch
					{
						vRet = false;
					}
					break;
				case "INTERMEDIARIES":
					try
					{
						l = new ExtractionLog();
						try { l = CanonicalDelta.ExtractDeltaIntermediarySP(); l.FromDate = DateTime.Now; l.ToDate = DateTime.Now; l.LogDate = DateTime.Now; l.ExtractionStatus = true; l.FileTransferStatus = false; l.ExceptionThrown = ""; }
						catch (Exception e)
						{
							
						}
						ExtractionLog.LogIt(l);
						vRet = true;
					}
					catch
					{
						vRet = false;
					}
					break;
				case "INTERMEDIARY_POLICY_LINK":
					try
					{
						l = new ExtractionLog();
						try { l = CanonicalDelta.ExtractDeltaIntermediaryPolicyLinkSP(); l.FromDate = DateTime.Now; l.ToDate = DateTime.Now; l.LogDate = DateTime.Now; l.ExtractionStatus = true; l.FileTransferStatus = false; l.ExceptionThrown = ""; }
						catch (Exception e)
						{
							
						}
						ExtractionLog.LogIt(l);
						vRet = true;
					}
					catch
					{
						vRet = false;
					}
					break;
				case "OPERATIONS":
					try
					{
						l = new ExtractionLog();
						try { l = CanonicalDelta.ExtractDeltaOperationSP(); l.FromDate = DateTime.Now; l.ToDate = DateTime.Now; l.LogDate = DateTime.Now; l.ExtractionStatus = true; l.FileTransferStatus = false; l.ExceptionThrown = ""; }
						catch (Exception e)
						{
							
						}
						ExtractionLog.LogIt(l);
						vRet = true;
					}
					catch
					{
						vRet = false;
					}
					break;
				case "POLICIES":
					try
					{
						l = new ExtractionLog();
						try { l = CanonicalDelta.ExtractDeltaPoliciesSP(); l.FromDate = DateTime.Now; l.ToDate = DateTime.Now; l.LogDate = DateTime.Now; l.ExtractionStatus = true; l.FileTransferStatus = false; l.ExceptionThrown = ""; }
						catch (Exception e)
						{
							
						}
						ExtractionLog.LogIt(l);
						vRet = true;
					}
					catch
					{
						vRet = false;
					}
					break;
				case "PRODUCT":
					try
					{
						l = new ExtractionLog();
						try { l = CanonicalDelta.ExtractDeltaProductSP(); l.FromDate = DateTime.Now; l.ToDate = DateTime.Now; l.LogDate = DateTime.Now; l.ExtractionStatus = true; l.FileTransferStatus = false; l.ExceptionThrown = ""; }
						catch (Exception e)
						{
							
						}
						ExtractionLog.LogIt(l);
						vRet = true;
					}
					catch
					{
						vRet = false;
					}
					break;
				case "PRODUCT_SOURCE_TYPE":
					try
					{
						l = new ExtractionLog();
						try { l = CanonicalDelta.ExtractDeltaProductSourceTypeSP(); l.FromDate = DateTime.Now; l.ToDate = DateTime.Now; l.LogDate = DateTime.Now; l.ExtractionStatus = true; l.FileTransferStatus = false; l.ExceptionThrown = ""; }
						catch (Exception e)
						{
							
						}
						ExtractionLog.LogIt(l);
						vRet = true;
					}
					catch
					{
						vRet = false;
					}
					break;
			}

			//List<string> lPolno = new List<string>(new string[] { "CUSTOMERS", "CUSTOMER_CUSTOMER_LINK", "CUSTOMER_POLICY_LINK", "INTERMEDIARIES", "INTERMEDIARY_POLICY_LINK", "OPERATIONS", "POLICIES", "PRODUCT", "PRODUCT_SOURCE_TYPE" }); ;
			//int idx = lPolno.IndexOf(tName.ToLower().Trim());
			//if (idx < 0)
			//{
			//	//results.MdmIDs.Add(inputs.PolicyNo[x] + "=NotFound");
			//}

			return vRet;
		}

		public static ExtractionLog ExtractJsonBatching(string prefix, string title, string sProcedure, string timeLine, string runTimestamp = null)
		{
			Console.WriteLine("___________________________");
			Console.WriteLine("Start Extract Data {" + title + "} from DB!");
			Console.WriteLine("Path: " + ConfigurationManager.AppSettings["JsonDir"]);

			var conSourceData = SharedUtils.GetDSN();

			ExtractionLog l = new ExtractionLog();
			l.ExeTime = DateTime.Now;

			InputRequestParam p = new InputRequestParam();
			p.JsonTitle = title;// "CUSTOMERS";
			p.JsonPrefix = prefix;
			p.JsonTimeline = timeLine;
			p.JsonRecNum = Convert.ToDouble(SharedUtils.GetCount(sProcedure, conSourceData));

			l.SourceRows = p.JsonRecNum.ToString();
			l.ExceptionThrown = "";
			//l.SourceRows = RowsCount("select count(*) from GFCC.GFCC_FLOW.CUSTOMER where CUSTOMERSTATUSCODE = 'ACTIVE' and  CAST([MODIFIED_DATE] AS DATE) = CAST(GETDATE() AS DATE) and identification_number_type = 'KTP' and zone <> 'NULL' and occupation <> ''  and gender_code = 'F' ", SharedUtils.GetDSN()).ToString();

			//l.SourceRows = ExtractionCanonicalDAO.RowsCount("select count(*) from GFCC_FLOW.CUSTOMER where " + period, SharedUtils.GetDSN("GFCC")).ToString();
			l.TaskName = p.JsonTitle;
			p.StringSQLCommand = sProcedure;// "[WebApp].[SpMNLGetCustomer]";

			//and CAST([MODIFIED_DATE] AS DATE) = CAST(GETDATE() AS DATE)
			p.CanonicalModelVersion = "4.0";
			if (Convert.ToInt64(l.SourceRows) < 1)
			{
				Console.WriteLine("No Data Available -1");
				l.ExceptionThrown = "No Data!";
				l.ExtractionStatus = false;
				l.EndTime = DateTime.Now;
				//return l;
			}

			#region CreateFile
			p.GetColumns = JsonBatch.GetColumns(p.JsonTitle); // CustomerID.getColumns();

			var lDt = new List<CustomerID>();
			var oDr = new CustomerID();
			oDr.CUSTOMER_SOURCE_UNIQUE_ID = "1234";
			lDt.Add(oDr);
			p.GetInitiateData = lDt;
			CreateInitiateJsonFile c = new CreateInitiateJsonFile(p);

			l.FilePath = p.DbFileNamePath;

			if (JsonFileDAL.DeleteDataRow("data", p.FileNamePath, "CUSTOMER_SOURCE_UNIQUE_ID", "1234"))
			{
				//Console.WriteLine("Deleted Data");
				Console.WriteLine("Created File {" + title + "} : " + p.FileNamePath);
				Console.WriteLine("Rows Count to Process : " + p.JsonRecNum.ToString() + " data");
			}

			string allText = File.ReadAllText(p.FileNamePath);
			StreamWriter strm = File.CreateText(p.FileNamePath);
			strm.Flush();
			strm.Close();
			File.AppendAllText(p.FileNamePath, allText.Substring(0, allText.Length - 2));
			allText = "";

			Int32 ttlRows, numberOfRowsPerPage;

			ttlRows = 100;              // Convert.ToInt32(l.SourceRows); 
			numberOfRowsPerPage = 100;  // 10000;

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
				var jsonFields = JsonBatch.GetFields(p.JsonTitle);

				int rCntr = 0;

				//penyebab error karna jika datanya banyak dia kena koneksi sql
				//for (int i = 1; i <= numberOfLoop; i++)
				//{
					string strSqlCommand = $"SELECT * FROM {p.StringSQLCommand}";
					//using (SqlDataReader results = SharedUtils.ExecuteSPCommand(p.StringSQLCommand, SharedUtils.GetDSN("GFCC_WEB", "wrcsql17"), i, numberOfRowsPerPage))
					using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(strSqlCommand, SharedUtils.GetDSN()))
					{
						while (results.Read())
						{
							rCntr++;
							
							var data = new Dictionary<string, dynamic>();

							foreach (var item in jsonFields)
							{
								dynamic result = null;
								item.DefaultValue = (item.DefaultValue == null) ? "" : item.DefaultValue;

								if (!item.FieldExists)
								{
									result = item.DefaultValue;
								}
								else if (item.TypeData == "TIMESTAMP" || item.TypeData == "DATE")
								{
									if(item.FieldName == "RUN_TIMESTAMP" && runTimestamp != null)
                                    {
										result = runTimestamp;

                                    }
                                    else
                                    {
										result = (!results.IsDBNull(results.GetOrdinal(item.FieldName))) ? Convert.ToDateTime(results[item.FieldName]).ToString(item.LengthData).Trim() : item.DefaultValue;
									}

								}
								else if (item.TypeData == "ARRAY" || item.TypeData == "JSON OBJECT OF STRING")
								{
									var jValues = new Dictionary<string, string>();
									foreach (var JObject in item.JObject)
									{
										var fName = JObject.Key;
										if (item.TypeData == "JSON OBJECT OF STRING")
										{
											fName += "_" + 1;
										}

										if (!JObject.Value.FieldExists)
										{
											result = item.DefaultValue;
										}
										else if (JObject.Value.TypeData == "TIMESTAMP" || JObject.Value.TypeData == "DATE")
										{
											jValues.Add(fName, (!results.IsDBNull(results.GetOrdinal(JObject.Key))) ? Convert.ToDateTime(results[JObject.Key]).ToString(JObject.Value.LengthData) : JObject.Value.DefaultValue);
										}
										else if(item.TypeData == "JSON OBJECT OF STRING")
										{
											string jObj = (!results.IsDBNull(results.GetOrdinal(JObject.Key))) ? results[JObject.Key].ToString().Trim().Replace("  ", " ").Replace("  ", " ").Replace("  ", " ") : JObject.Value.DefaultValue;
											var jLoop = jObj.Split(',');
											var num = 1;
                                            
											if(jLoop.Length > 1)
                                            {
												jValues.Add(JObject.Key + "_" + num, "CC06");
                                            }
                                            else
                                            {
												jValues.Add(JObject.Key + "_" + num, jObj);
											}

                                        }
                                        else
                                        {
											
											string jValue = (!results.IsDBNull(results.GetOrdinal(JObject.Key))) ? results[JObject.Key].ToString().Trim().Replace("  ", " ").Replace("  ", " ").Replace("  ", " ") : JObject.Value.DefaultValue;
										
											if (jValue != null && !string.IsNullOrEmpty(jValue.ToString()))
											{
												jValues.Add(fName, jValue);
											}

										}
									
									}

									if (item.TypeData == "ARRAY")
									{
										var listArray = new List<dynamic>();
										listArray.Add(jValues);
										result = listArray;
									}
									else
									{
										result = jValues;
									}
								}
						
								
								else
								{
									result = (!results.IsDBNull(results.GetOrdinal(item.FieldName))) ? results[item.FieldName].ToString().Trim().Replace("  ", " ").Replace("  ", " ").Replace("  ", " ") : "";
									
								}

								//data.Add(item.FieldName, result);
								if (result != null && !string.IsNullOrEmpty(result.ToString()))
								{
									data.Add(item.FieldName, result);
								}

								//if (result != null && !string.IsNullOrEmpty(result.ToString()))
								//{
								//	if (item.FieldParent == "Customer_Identification")
								//	{

								//		if(result != null && !string.IsNullOrEmpty(result.ToString()))
								//		{
								//			data.Add(item.FieldName, result);
								//		}

								//	}
								//	else
								//	{
								//		data.Add(item.FieldName, result);
								//	}
								//}

							}
							
							var rslt = JsonFileDAL.WriteDataToFile(p.FileNamePath, data);
							if (rslt)
							{
								data = null;
							}
							
							if(rCntr < p.JsonRecNum)
                            {
								File.AppendAllText(p.FileNamePath, ",");
							}

							Console.WriteLine("Process Flow [" + p.JsonTitle + "] on [Rows:" + rCntr.ToString() + "] is Success");
						}

						l.SourceRows = l.ExtractedRows = rCntr.ToString();
						l.ExtractionStatus = true;
					}
				//}

				File.AppendAllText(p.FileNamePath, "]}");
				//if (JsonFileDAL.UpdateRecNoRow("lastRecNo", p.FileNamePath, rCntr.ToString()))
				//{
				//	Console.WriteLine("Updated RowsNumber");
				//}

				Console.WriteLine("Complete Extracted Data {" + title + "} : " + rCntr.ToString() + " rows");
				Console.WriteLine("=======================================");

			}
			catch (Exception e)
			{

				Console.WriteLine(e.ToString());

				File.AppendAllText(@ConfigurationManager.AppSettings["WebLogs"], e.ToString());

				l.ExtractionStatus = false;
				l.ExceptionThrown = e.ToString() + Environment.NewLine;

			}

			l.EndTime = DateTime.Now;
			#endregion

			return l;
		}

		public static ExtractionLog ExtractDeltaCustomerSP(string title, string sp)
		{

			ExtractionLog l = new ExtractionLog();
			l.ExeTime = DateTime.Now;

			InputRequestParam p = new InputRequestParam();
			p.JsonTitle = title;// "CUSTOMERS";

			l.SourceRows = "100";
			//l.SourceRows = RowsCount("select count(*) from GFCC.GFCC_FLOW.CUSTOMER where CUSTOMERSTATUSCODE = 'ACTIVE' and  CAST([MODIFIED_DATE] AS DATE) = CAST(GETDATE() AS DATE) and identification_number_type = 'KTP' and zone <> 'NULL' and occupation <> ''  and gender_code = 'F' ", SharedUtils.GetDSN()).ToString();
			
			//l.SourceRows = ExtractionCanonicalDAO.RowsCount("select count(*) from GFCC_FLOW.CUSTOMER where " + period, SharedUtils.GetDSN("GFCC")).ToString();
			l.TaskName = p.JsonTitle;
			p.StringSQLCommand = sp;// "[WebApp].[SpMNLGetCustomer]";

			//and CAST([MODIFIED_DATE] AS DATE) = CAST(GETDATE() AS DATE)
			p.CanonicalModelVersion = "4.0";
			if (Convert.ToInt64(l.SourceRows) < 1)
			{
				Console.WriteLine("No Data Available -1");
				l.ExceptionThrown = "No Data!";
				l.ExtractionStatus = false;
				l.EndTime = DateTime.Now;
				return l;
			}

			#region CreateFile
			p.GetColumns = JsonBatch.GetColumns(p.JsonTitle); // CustomerID.getColumns();

			var lDt = new List<CustomerID>();
			var oDr = new CustomerID();
			oDr.CUSTOMER_SOURCE_UNIQUE_ID = "1234";
			lDt.Add(oDr);
			p.GetInitiateData = lDt;
			CreateInitiateJsonFile c = new CreateInitiateJsonFile(p);

			l.FilePath = p.DbFileNamePath;

			if (JsonFileDAL.DeleteDataRow("data", p.FileNamePath, "CUSTOMER_SOURCE_UNIQUE_ID", "1234"))
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

			ttlRows = 100;				// Convert.ToInt32(l.SourceRows); 
			numberOfRowsPerPage = 100;	// 10000;

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
				var jsonFields = JsonBatch.GetFields(p.JsonTitle);

				int rCntr = 0;
				for (int i = 1; i <= numberOfLoop; i++)
				{
					using (SqlDataReader results = SharedUtils.ExecuteSPCommand(p.StringSQLCommand, SharedUtils.GetDSN(), i, numberOfRowsPerPage))
					{
						while (results.Read())
						{
							rCntr++;
							//CustomerID ci = new CustomerID();
							//ci.RUN_TIMESTAMP = (!results.IsDBNull(results.GetOrdinal("RUN_TIMESTAMP"))) ? Convert.ToDateTime(results["RUN_TIMESTAMP"]).ToString("yyyyMMddHHmmss") : DateTime.Now.ToString("yyyyMMddHHmmss");
							//ci.CUSTOMER_SOURCE_UNIQUE_ID = (!results.IsDBNull(results.GetOrdinal("CUSTOMER_SOURCE_UNIQUE_ID"))) ? orgUnit + "_" + results["CUSTOMER_SOURCE_UNIQUE_ID"].ToString() : "";
							//ci.ORGUNIT_CODE = (!results.IsDBNull(results.GetOrdinal("ORGUNIT_CODE"))) ? results["ORGUNIT_CODE"].ToString() : orgUnit;
							//ci.PERSON_TITLE = (!results.IsDBNull(results.GetOrdinal("PERSON_TITLE"))) ? results["PERSON_TITLE"].ToString() : "";
							//ci.FIRST_NAME = (!results.IsDBNull(results.GetOrdinal("FIRST_NAME"))) ? results["FIRST_NAME"].ToString() : "";
							//ci.MIDDLE_NAMES = (!results.IsDBNull(results.GetOrdinal("MIDDLE_NAMES"))) ? results["MIDDLE_NAMES"].ToString() : "";
							//ci.LAST_NAME = (!results.IsDBNull(results.GetOrdinal("LAST_NAME"))) ? results["LAST_NAME"].ToString() : "";
							//ci.SUFFIX = (!results.IsDBNull(results.GetOrdinal("SUFFIX"))) ? results["SUFFIX"].ToString() : "";
							//ci.CUSTOMER_NAME = (!results.IsDBNull(results.GetOrdinal("CUSTOMER_NAME"))) ? results["CUSTOMER_NAME"].ToString() : "";

							//ci.COMPANY_NAME = (!results.IsDBNull(results.GetOrdinal("COMPANY_NAME"))) ? results["COMPANY_NAME"].ToString() : "";
							//ci.COMPANY_FORM = (!results.IsDBNull(results.GetOrdinal("COMPANY_FORM"))) ? results["COMPANY_FORM"].ToString() : "";

							//ci.REGISTERED_NUMBER = (!results.IsDBNull(results.GetOrdinal("IDENTIFICATION_NUMBER"))) ? results["IDENTIFICATION_NUMBER"].ToString() : "";
							//ci.INCORPORATION_DATE = (!results.IsDBNull(results.GetOrdinal("INCORPORATION_DATE"))) ? Convert.ToDateTime(results["INCORPORATION_DATE"]).ToString("yyyyMMdd") : null;
							//ci.INCORPORATION_COUNTRY_CODE = (!results.IsDBNull(results.GetOrdinal("INCORPORATION_COUNTRY_CODE"))) ? results["INCORPORATION_COUNTRY_CODE"].ToString() : "";
							//ci.BUSINESS_TYPE = null; // (!results.IsDBNull(results.GetOrdinal("BUSINESS_TYPE"))) ? results["BUSINESS_TYPE"].ToString() : "";
							//ci.BUSINESS_SEGMENT_1 = (!results.IsDBNull(results.GetOrdinal("BUSINESS_SEGMENT"))) ? results["BUSINESS_SEGMENT"].ToString() : "";
							//ci.BUSINESS_SEGMENT_2 = null; // (!results.IsDBNull(results.GetOrdinal("BUSINESS_SEGMENT_2"))) ? results["BUSINESS_SEGMENT_2"].ToString() : "";
							//ci.INITIALS = (!results.IsDBNull(results.GetOrdinal("INITIALS"))) ? results["INITIALS"].ToString() : "";
							//ci.DATE_OF_BIRTH = (!results.IsDBNull(results.GetOrdinal("DATE_OF_BIRTH"))) ? Convert.ToDateTime(results["DATE_OF_BIRTH"]).ToString("yyyyMMdd") : null;
							//ci.NAME_OF_BIRTH = (!results.IsDBNull(results.GetOrdinal("NAME_OF_BIRTH"))) ? results["NAME_OF_BIRTH"].ToString() : "";
							//ci.ADDRESS = (!results.IsDBNull(results.GetOrdinal("ADDRESS"))) ? results["ADDRESS"].ToString() : "";
							//ci.ZONE = (!results.IsDBNull(results.GetOrdinal("ZONE"))) ? results["ZONE"].ToString() : "";
							//ci.CITY = (!results.IsDBNull(results.GetOrdinal("CITY"))) ? results["CITY"].ToString() : "";
							//ci.POSTAL_CODE = (!results.IsDBNull(results.GetOrdinal("POSTAL_CODE"))) ? results["POSTAL_CODE"].ToString() : "";
							//ci.COUNTRY_OF_RESIDENCE = (!results.IsDBNull(results.GetOrdinal("COUNTRY_OF_RESIDENCE"))) ? results["COUNTRY_OF_RESIDENCE"].ToString() : "";
							//ci.COUNTRY_OF_ORIGIN = (!results.IsDBNull(results.GetOrdinal("COUNTRY_OF_ORIGIN"))) ? results["COUNTRY_OF_ORIGIN"].ToString() : "";
							//ci.PLACE_OF_BIRTH = (!results.IsDBNull(results.GetOrdinal("PLACE_OF_BIRTH"))) ? results["PLACE_OF_BIRTH"].ToString() : "";
							//ci.GENDER_CODE = (!results.IsDBNull(results.GetOrdinal("GENDER_CODE"))) ? results["GENDER_CODE"].ToString() : "";
							//ci.PRIME_BRANCH_ID = null;
							//ci.RELATIONSHIP_MGR_ID = null;
							//ci.EMPLOYEE_FLAG = null;
							//ci.EMPLOYEE_NUMBER = null;
							//ci.MARITAL_STATUS = null;
							//ci.OCCUPATION = (!results.IsDBNull(results.GetOrdinal("OCCUPATION"))) ? results["OCCUPATION"].ToString() : "";
							//ci.EMPLOYMENT_STATUS = null;
							//ci.ACQUISITION_DATE = (!results.IsDBNull(results.GetOrdinal("ACQUISITION_DATE"))) ? Convert.ToDateTime(results["ACQUISITION_DATE"]).ToString("yyyyMMdd") : null;
							//ci.CANCELLED_DATE = (!results.IsDBNull(results.GetOrdinal("CANCELLED_DATE"))) ? Convert.ToDateTime(results["CANCELLED_DATE"]).ToString("yyyyMMdd") : null;
							//ci.CUSTOMER_TYPE_CODE = (!results.IsDBNull(results.GetOrdinal("CUSTOMER_TYPE_CODE"))) ? results["CUSTOMER_TYPE_CODE"].ToString() : "";
							//ci.CUSTOMER_STATUS_CODE = (!results.IsDBNull(results.GetOrdinal("CUSTOMER_STATUS_CODE"))) ? results["CUSTOMER_STATUS_CODE"].ToString() : "";

							//ci.CUSTOMER_SEGMENT_1 = null;
							//ci.CUSTOMER_SEGMENT_2 = null;
							//ci.CUSTOMER_SEGMENT_3 = null; // (!results.IsDBNull(results.GetOrdinal("customer_segment_3"))) ? results["customer_segment_3"].ToString() : "";

							//ci.RESIDENCE_FLAG = null;
							//ci.SPECIAL_ATTENTION_FLAG = null;
							//ci.DECEASED_FLAG = null;
							//ci.DORMANT_OVERRIDE_DATE = (!results.IsDBNull(results.GetOrdinal("DORMANT_OVERRIDE_DATE"))) ? Convert.ToDateTime(results["DORMANT_OVERRIDE_DATE"]).ToString("yyyyMMdd") : null;
							//ci.RISK_SCORE = null;
							//ci.BANKRUPT_FLAG = null;
							//ci.COMPENSATION_REQD_FLAG = null;
							//ci.CUSTOMER_COMPLAINT_FLAG = null;
							//ci.END_RELATIONSHIP_FLAG = null;
							//ci.MERCHANT_NUMBER = null;
							//ci.FACE_TO_FACE_FLAG = null;
							//ci.CHANNEL = (!results.IsDBNull(results.GetOrdinal("channel"))) ? results["channel"].ToString() : "";
							//ci.AGE = null; // (!results.IsDBNull(results.GetOrdinal("AGE"))) ? results["AGE"].ToString() : ""; // Used on Batch if dob is exists calc age of customer
							//ci.NEAR_BORDER_FLAG = null;
							//ci.INTENDED_PRODUCT_USE = null;
							//ci.SOURCE_OF_FUNDS = null;
							//ci.COMPLEX_STRUCTURE = null;
							//ci.EXPECTED_ANNUAL_TURNOVER = (!results.IsDBNull(results.GetOrdinal("EXPECTED_ANNUAL_TURNOVER"))) ? results["EXPECTED_ANNUAL_TURNOVER"].ToString() : "";
							//ci.TRADING_DURATION = null;
							//ci.BALANCE_SHEET_TOTAL = (!results.IsDBNull(results.GetOrdinal("BALANCE_SHEET_TOTAL"))) ? results["BALANCE_SHEET_TOTAL"].ToString() : "";
							//ci.VAT_NUMBER = null;
							//ci.BROKER_CODE = null;
							//ci.BLACK_LISTED_FLAG = null;
							//ci.DOMAIN_CODE = null;
							//ci.COMMENTS = null;
							//ci.PEP_FLAG_INGESTED = null;
							//ci.WIRE_IN_NUMBER = null;
							//ci.WIRE_OUT_NUMBER = null;
							//ci.WIRE_IN_VOLUME = null;
							//ci.WIRE_OUT_VOLUME = null;
							//ci.CASH_IN_VOLUME = null;
							//ci.CASH_OUT_VOLUME = null;
							//ci.CHECK_IN_VOLUME = null;
							//ci.CHECK_OUT_VOLUME = null;
							//ci.OVERALL_SCORE_ADJUSTMENT = null;
							//ci.TAX_NUMBER = null;
							//ci.TAX_NUMBER_ISSUED_BY = null;
							//ci.CUSTOMER_CATEGORY_CODE = (!results.IsDBNull(results.GetOrdinal("CUSTOMER_CATEGORY_CODE"))) ? results["CUSTOMER_CATEGORY_CODE"].ToString() : "";
							//ci.OWN_AFFILIATE_FLAG = null;
							//ci.MARKETING_SERVICE_LEVEL = null;
							//ci.SANCTIONED_FLAG_INGESTED = null;
							//ci.PEP_TYPE_INGESTED = null;
							//ci.RCA_FLAG_INGESTED = null;
							//ci.ADDRESS_VALID_FROM = (!results.IsDBNull(results.GetOrdinal("ADDRESS_VALID_FROM"))) ? Convert.ToDateTime(results["ADDRESS_VALID_FROM"]).ToString("yyyyMMddHHmmss") : null;
							//ci.ADDRESS_VALID_TO = (!results.IsDBNull(results.GetOrdinal("ADDRESS_VALID_TO"))) ? Convert.ToDateTime(results["ADDRESS_VALID_TO"]).ToString("yyyyMMddHHmmss") : null;

							//ci.EMAIL = (!results.IsDBNull(results.GetOrdinal("EMAIL"))) ? results["EMAIL"].ToString() : null;
							//ci.EMAIL_VALID_FROM = (!results.IsDBNull(results.GetOrdinal("EMAIL_VALID_FROM"))) ? Convert.ToDateTime(results["EMAIL_VALID_FROM"]).ToString("yyyyMMddHHmmss") : null;
							//ci.EMAIL_VALID_TO = (!results.IsDBNull(results.GetOrdinal("EMAIL_VALID_TO"))) ? Convert.ToDateTime(results["EMAIL_VALID_TO"]).ToString("yyyyMMddHHmmss") : null;

							//ci.PHONE_COUNTRY_CODE = (!results.IsDBNull(results.GetOrdinal("PHONE_COUNTRY_CODE"))) ? results["PHONE_COUNTRY_CODE"].ToString() : "";
							//ci.PHONE_AREA_CODE = (!results.IsDBNull(results.GetOrdinal("PHONE_AREA_CODE"))) ? results["PHONE_AREA_CODE"].ToString() : "";
							//ci.PHONE_NUMBER = (!results.IsDBNull(results.GetOrdinal("PHONE_NUMBER"))) ? results["PHONE_NUMBER"].ToString() : "";
							//ci.PHONE_EXTENSION = (!results.IsDBNull(results.GetOrdinal("PHONE_EXTENSION"))) ? results["PHONE_EXTENSION"].ToString() : "";
							//ci.PHONE_VALID_FROM = (!results.IsDBNull(results.GetOrdinal("PHONE_VALID_FROM"))) ? Convert.ToDateTime(results["PHONE_VALID_FROM"]).ToString("yyyyMMddHHmmss") : null;
							//ci.PHONE_VALID_TO = (!results.IsDBNull(results.GetOrdinal("PHONE_VALID_TO"))) ? Convert.ToDateTime(results["PHONE_VALID_TO"]).ToString("yyyyMMddHHmmss") : null;

							//ci.ALTERNATE_NAME = null;
							//ci.TAX_NUMBER_TYPE = null;

							//ci.BUSINESS_CLASSIFICATION_CODE = null;
							//ci.BUSINESS_CLASSIFICATION_SYSTEM = null;
							//ci.CUSTOMER_CHANNEL_REMOTE_FLAG = null;

							//ci.COUNTRY_OF_TAX_RESIDENCE = (!results.IsDBNull(results.GetOrdinal("COUNTRY_OF_TAX_RESIDENCE"))) ? results["COUNTRY_OF_TAX_RESIDENCE"].ToString() : "";
							//ci.COUNTRY_OF_HQ = (!results.IsDBNull(results.GetOrdinal("COUNTRY_OF_HQ"))) ? results["COUNTRY_OF_HQ"].ToString() : "";
							//ci.COUNTRY_OF_OPERATIONS = (!results.IsDBNull(results.GetOrdinal("COUNTRY_OF_OPERATIONS"))) ? results["COUNTRY_OF_OPERATIONS"].ToString() : "";

							//ci.NATIONALITY_CODE = (!results.IsDBNull(results.GetOrdinal("NATIONALITY_CODE"))) ? results["NATIONALITY_CODE"].ToString() : "";
							//ci.NATIONALITY_CODE = (!results.IsDBNull(results.GetOrdinal("NATIONALITY_CODE"))) ? results["NATIONALITY_CODE"].ToString() : "";

							//List<Customer_Identification> lci = new List<Customer_Identification>();
							//Customer_Identification cid = new Customer_Identification();
							//cid.IDENTIFICATION_NUMBER = (!results.IsDBNull(results.GetOrdinal("IDENTIFICATION_NUMBER"))) ? results["IDENTIFICATION_NUMBER"].ToString() : "";
							//cid.ISSUING_AUTHORITY = (!results.IsDBNull(results.GetOrdinal("ISSUING_AUTHORITY"))) ? results["ISSUING_AUTHORITY"].ToString() : null;
							//cid.COUNTRY_CODE = (!results.IsDBNull(results.GetOrdinal("COUNTRY_CODE"))) ? results["COUNTRY_CODE"].ToString() : null;
							//cid.IDENTIFICATION_TYPE = (!results.IsDBNull(results.GetOrdinal("IDENTIFICATION_TYPE"))) ? results["IDENTIFICATION_TYPE"].ToString() : "";
							//cid.VALID_FROM = (!results.IsDBNull(results.GetOrdinal("VALID_FROM"))) ? Convert.ToDateTime(results["VALID_FROM"]).ToString("yyyyMMddHHmmss") : "19000101000001";
							//cid.VALID_TO = (!results.IsDBNull(results.GetOrdinal("VALID_TO"))) ? Convert.ToDateTime(results["VALID_TO"]).ToString("yyyyMMddHHmmss") : "99991231235959";
							//cid.DESCRIPTION = null;// (!results.IsDBNull(results.GetOrdinal("DESCRIPTION"))) ? results["DESCRIPTION"].ToString() : "";
							//cid.DETAILS = null;//(!results.IsDBNull(results.GetOrdinal("DETAILS"))) ? results["DETAILS"].ToString() : "";
							//cid.VISA_TYPE = "";//(!results.IsDBNull(results.GetOrdinal("VISA_TYPE"))) ? results["VISA_TYPE"].ToString() : "";

							//ci.X_SUBSCRIPTION_KEYWORD = (!results.IsDBNull(results.GetOrdinal("X_SUBSCRIPTION_KEYWORD"))) ? results["X_SUBSCRIPTION_KEYWORD"].ToString() : "";
							//ci.X_SOURCE_SYSTEM = (!results.IsDBNull(results.GetOrdinal("X_SOURCE_SYSTEM"))) ? results["X_SOURCE_SYSTEM"].ToString() : "";
							//ci.X_SENSITIVE_CUSTOMER_FLAG = (!results.IsDBNull(results.GetOrdinal("X_SENSITIVE_CUSTOMER_FLAG"))) ? results["X_SENSITIVE_CUSTOMER_FLAG"].ToString() : "";
							//ci.X_OLD_CUSTOMER_SOURCE_UNIQUE_ID = (!results.IsDBNull(results.GetOrdinal("X_OLD_CUSTOMER_SOURCE_UNIQUE_ID"))) ? results["X_OLD_CUSTOMER_SOURCE_UNIQUE_ID"].ToString() : "";
							//ci.X_NEW_CUSTOMER_SOURCE_UNIQUE_ID = (!results.IsDBNull(results.GetOrdinal("X_NEW_CUSTOMER_SOURCE_UNIQUE_ID"))) ? results["X_NEW_CUSTOMER_SOURCE_UNIQUE_ID"].ToString() : "";
							//ci.X_CUSTOMER_INTERMEDIARY_REF_ID = (!results.IsDBNull(results.GetOrdinal("X_CUSTOMER_INTERMEDIARY_REF_ID"))) ? results["X_CUSTOMER_INTERMEDIARY_REF_ID"].ToString() : "";

							//ci.X_SCREENING_END_DATE = (!results.IsDBNull(results.GetOrdinal("X_SCREENING_END_DATE"))) ? Convert.ToDateTime(results["X_SCREENING_END_DATE"]).ToString("yyyyMMdd") : null;

							//lci.Add(cid);
							//ci.Customer_Identification = lci;

							var data = new Dictionary<string, dynamic>();

							foreach(var item in jsonFields)
                            {
								dynamic result = null;
								item.DefaultValue = (item.DefaultValue == null) ? "" : item.DefaultValue;

								if (!item.FieldExists)
                                {
									result = item.DefaultValue;
								}
								else if (item.TypeData == "TIMESTAMP" || item.TypeData == "DATE")
                                {

									result = (!results.IsDBNull(results.GetOrdinal(item.FieldName))) ? Convert.ToDateTime(results[item.FieldName]).ToString(item.LengthData).Trim() : item.DefaultValue;

								}
								else if (item.TypeData == "ARRAY" || item.TypeData == "JSON OBJECT OF STRING")
								{
									var jValues = new Dictionary<string, string>();
									foreach (var JObject in item.JObject)
									{
										var fName = JObject.Key;
										if(item.TypeData == "JSON OBJECT OF STRING")
                                        {
											fName += "_" + 1;
                                        }

										if (!JObject.Value.FieldExists)
										{
											result = item.DefaultValue;
										}
										else if (JObject.Value.TypeData == "TIMESTAMP" || JObject.Value.TypeData == "DATE")
                                        {
											jValues.Add(fName, (!results.IsDBNull(results.GetOrdinal(JObject.Key))) ? Convert.ToDateTime(results[JObject.Key]).ToString(JObject.Value.LengthData) : JObject.Value.DefaultValue);
                                        }
                                        else
                                        {
											jValues.Add(fName, (!results.IsDBNull(results.GetOrdinal(JObject.Key))) ? results[JObject.Key].ToString() : JObject.Value.DefaultValue);
										}
                                    }

									if(item.TypeData == "ARRAY")
                                    {
										var listArray = new List<dynamic>();
										listArray.Add(jValues);
										result = listArray;
                                    }
                                    else
                                    {
										result = jValues;
                                    }
								}
								else
                                {
									result = (!results.IsDBNull(results.GetOrdinal(item.FieldName))) ? results[item.FieldName].ToString() : "";
								}

								data.Add(item.FieldName, result);
                            }

							var rslt = JsonFileDAL.WriteDataToFile(p.FileNamePath, data);
							if (rslt)
							{
								data = null;
							}
							File.AppendAllText(p.FileNamePath, ",");
							l.ExtractedRows = rCntr.ToString();
						}
					}
				}

				File.AppendAllText(p.FileNamePath, "]}");
				if (JsonFileDAL.UpdateRecNoRow("lastRecNo", p.FileNamePath, rCntr.ToString()))
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

		public static ExtractionLog ExtractDeltaCustomerLinkSP(string period)
		{

			ExtractionLog l = new ExtractionLog();
			l.ExeTime = DateTime.Now;

			InputRequestParam p = new InputRequestParam();
			p.JsonTitle = "CUSTOMER_CUSTOMER_LINK";

			//l.SourceRows = "10";
			//l.SourceRows = RowsCount("select count(*) from GFCC.GFCC_FLOW.CUSTOMER where CUSTOMERSTATUSCODE = 'ACTIVE' and  CAST([MODIFIED_DATE] AS DATE) = CAST(GETDATE() AS DATE) and identification_number_type = 'KTP' and zone <> 'NULL' and occupation <> ''  and gender_code = 'F' ", SharedUtils.GetDSN()).ToString();
			l.SourceRows = ExtractionCanonicalDAO.RowsCount("select count(*) from GFCC_FLOW.CUSTOMER_LINK where " + period, SharedUtils.GetDSNGFCC()).ToString();
			l.TaskName = p.JsonTitle;
			p.StringSQLCommand = "[WebApp].[SpMNLGetCustomerCustomerLink]";

			//where CAST([MODIFIED_DATE] AS DATE) = CAST(GETDATE() AS DATE)
			//and CAST([MODIFIED_DATE] AS DATE) = CAST(GETDATE() AS DATE)
			p.CanonicalModelVersion = "4.0";

			if (Convert.ToInt64(l.SourceRows) < 1)
			{
				Console.WriteLine("No Data Available -2");
				l.ExceptionThrown = "No Data!";
				l.ExtractionStatus = false;
				l.EndTime = DateTime.Now;
				return l;
			}


			if (Convert.ToInt64(l.SourceRows) > 0)
			{

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

				ttlRows = 100; // Convert.ToInt32(l.SourceRows); 
				numberOfRowsPerPage = 100; // 10000;

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
									TO_TIMESTAMP = (!results.IsDBNull(results.GetOrdinal("TO_TIMESTAMP"))) ? Convert.ToDateTime(results["TO_TIMESTAMP"]).ToString("dd/MM/yyyy") : "00/00/00",
									X_ORGUNIT_CODE = (!results.IsDBNull(results.GetOrdinal("X_ORGUNIT_CODE"))) ? results["X_ORGUNIT_CODE"].ToString() : ""
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

		public static ExtractionLog ExtractDeltaCustomer2PolicyLinkSP(DateTime d1, DateTime d2, string period)
		{

			ExtractionLog l = new ExtractionLog();
			l.ExeTime = DateTime.Now;

			InputRequestParam p = new InputRequestParam();
			p.JsonTitle = "CUSTOMER_POLICY_LINK";

			l.SourceRows = "100";
			//l.SourceRows = RowsCount("select count(*) from GFCC.GFCC_FLOW.CUSTOMER where CUSTOMERSTATUSCODE = 'ACTIVE' and  CAST([MODIFIED_DATE] AS DATE) = CAST(GETDATE() AS DATE) and identification_number_type = 'KTP' and zone <> 'NULL' and occupation <> ''  and gender_code = 'F' ", SharedUtils.GetDSN()).ToString();
			
			//l.SourceRows = ExtractionCanonicalDAO.RowsCount("select count(*) from GFCC_FLOW.CUSTOMER_POLICY_LINK where " + period, SharedUtils.GetDSN("GFCC")).ToString();
			l.TaskName = p.JsonTitle;
			p.StringSQLCommand = "[WebApp].[SpMNLGetCustomerPolicyLink]";
			if (Convert.ToInt64(l.SourceRows) < 1)
			{
				Console.WriteLine("No Data Available -3");
				l.ExceptionThrown = "No Data!";
				l.ExtractionStatus = false;
				l.EndTime = DateTime.Now;
				return l;
			}

			//where CAST([MODIFIED_DATE] AS DATE) = CAST(GETDATE() AS DATE)
			//and CAST([MODIFIED_DATE] AS DATE) = CAST(GETDATE() AS DATE)
			p.CanonicalModelVersion = "4.0";
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

			ttlRows = 100; // Convert.ToInt32(l.SourceRows); 
			numberOfRowsPerPage = 100; // 10000;

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

		public static ExtractionLog ExtractDeltaIntermediarySP(string period)
		{

			ExtractionLog l = new ExtractionLog();
			l.ExeTime = DateTime.Now;

			InputRequestParam p = new InputRequestParam();
			p.JsonTitle = "INTERMEDIARIES";

			l.SourceRows = "100";
			//l.SourceRows = RowsCount("select count(*) from GFCC.GFCC_FLOW.CUSTOMER where CUSTOMERSTATUSCODE = 'ACTIVE' and  CAST([MODIFIED_DATE] AS DATE) = CAST(GETDATE() AS DATE) and identification_number_type = 'KTP' and zone <> 'NULL' and occupation <> ''  and gender_code = 'F' ", SharedUtils.GetDSN()).ToString();
			//l.SourceRows = ExtractionCanonicalDAO.RowsCount("select count(*) from GFCC_FLOW.INTERMEDIARIES where " + period, SharedUtils.GetDSN("GFCC")).ToString();
			l.TaskName = p.JsonTitle;
			p.StringSQLCommand = "[WebApp].[SpMNLGetIntermediaries]";
			if (Convert.ToInt64(l.SourceRows) < 1)
			{
				Console.WriteLine("No Data Available -4");
				l.ExceptionThrown = "No Data!";
				l.ExtractionStatus = false;
				l.EndTime = DateTime.Now;
				return l;
			}
			//where CAST([MODIFIED_DATE] AS DATE) = CAST(GETDATE() AS DATE)
			//and CAST([MODIFIED_DATE] AS DATE) = CAST(GETDATE() AS DATE)
			p.CanonicalModelVersion = "4.0";
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

			ttlRows = 100;// Convert.ToInt32(l.SourceRows); 
			numberOfRowsPerPage = 100;// 10000;

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
								ORGUNIT_CODE = (!results.IsDBNull(results.GetOrdinal("ORGUNIT_CODE"))) ? results["ORGUNIT_CODE"].ToString() : "",

								X_DISTRIBUTOR_TYPE_CODE = (!results.IsDBNull(results.GetOrdinal("X_DISTRIBUTOR_TYPE_CODE"))) ? results["X_DISTRIBUTOR_TYPE_CODE"].ToString() : "",
								X_STATUS_CODE = (!results.IsDBNull(results.GetOrdinal("X_STATUS_CODE"))) ? results["X_STATUS_CODE"].ToString() : "",
								X_CANCELLED_DATE = (!results.IsDBNull(results.GetOrdinal("X_CANCELLED_DATE"))) ? Convert.ToDateTime(results["X_CANCELLED_DATE"]).ToString("dd/MM/yyyy") : "00/00/0000",
								X_COUNTRY_OF_HQ = (!results.IsDBNull(results.GetOrdinal("X_COUNTRY_OF_HQ"))) ? results["X_COUNTRY_OF_HQ"].ToString() : ""
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

		public static ExtractionLog ExtractDeltaIntermediaryPolicyLinkSP(string period)
		{

			ExtractionLog l = new ExtractionLog();
			l.ExeTime = DateTime.Now;

			InputRequestParam p = new InputRequestParam();
			p.JsonTitle = "INTERMEDIARY_POLICY_LINK";

			l.SourceRows = "100";
			//l.SourceRows = RowsCount("select count(*) from GFCC.GFCC_FLOW.CUSTOMER where CUSTOMERSTATUSCODE = 'ACTIVE' and  CAST([MODIFIED_DATE] AS DATE) = CAST(GETDATE() AS DATE) and identification_number_type = 'KTP' and zone <> 'NULL' and occupation <> ''  and gender_code = 'F' ", SharedUtils.GetDSN()).ToString();
			
			//l.SourceRows = ExtractionCanonicalDAO.RowsCount("select count(*) from GFCC_FLOW.INTERMEDIARY_POLICY_LINK where " + period, SharedUtils.GetDSN("GFCC")).ToString();
			l.TaskName = p.JsonTitle;
			p.StringSQLCommand = "[WebApp].[SpMNLGetIntermediaryPolicyLink]";
			if (Convert.ToInt64(l.SourceRows) < 1)
			{
				Console.WriteLine("No Data Available -5");
				l.ExceptionThrown = "No Data!";
				l.ExtractionStatus = false;
				l.EndTime = DateTime.Now;
				return l;
			}
			//where CAST([MODIFIED_DATE] AS DATE) = CAST(GETDATE() AS DATE)
			//and CAST([MODIFIED_DATE] AS DATE) = CAST(GETDATE() AS DATE)
			p.CanonicalModelVersion = "4.0";
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

			ttlRows = 100; // Convert.ToInt32(l.SourceRows); 
			numberOfRowsPerPage = 100; // 10000;

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
								PARTY_RELATIONSHIP = (!results.IsDBNull(results.GetOrdinal("PARTY_RELATIONSHIP"))) ? results["PARTY_RELATIONSHIP"].ToString() : "",
								FROM_DATE = (!results.IsDBNull(results.GetOrdinal("FROM_DATE"))) ? Convert.ToDateTime(results["FROM_DATE"]).ToString("dd/MM/yyyy") : "00/00/0000",
								TO_DATE = (!results.IsDBNull(results.GetOrdinal("TO_DATE"))) ? Convert.ToDateTime(results["TO_DATE"]).ToString("dd/MM/yyyy") : "00/00/0000",
								X_ORGUNIT_CODE = (!results.IsDBNull(results.GetOrdinal("X_ORG_UNIT_CODE"))) ? results["X_ORG_UNIT_CODE"].ToString() : ""
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

		public static ExtractionLog ExtractDeltaOperationSP(string period)
		{

			ExtractionLog l = new ExtractionLog();
			l.ExeTime = DateTime.Now;

			InputRequestParam p = new InputRequestParam();
			p.JsonTitle = "OPERATIONS";

			l.SourceRows = "100";
			//l.SourceRows = RowsCount("select count(*) from GFCC.GFCC_FLOW.CUSTOMER where CUSTOMERSTATUSCODE = 'ACTIVE' and  CAST([MODIFIED_DATE] AS DATE) = CAST(GETDATE() AS DATE) and identification_number_type = 'KTP' and zone <> 'NULL' and occupation <> ''  and gender_code = 'F' ", SharedUtils.GetDSN()).ToString();
			//l.SourceRows = ExtractionCanonicalDAO.RowsCount("select count(*) from GFCC_FLOW.OPERATIONS where " + period, SharedUtils.GetDSN("GFCC")).ToString();
			l.TaskName = p.JsonTitle;
			p.StringSQLCommand = "[WebApp].[SpMNLGetOperation]";
			if (Convert.ToInt64(l.SourceRows) < 1)
			{
				Console.WriteLine("No Data Available -6");
				l.ExceptionThrown = "No Data!";
				l.ExtractionStatus = false;
				l.EndTime = DateTime.Now;
				return l;
			}
			//where CAST([MODIFIED_DATE] AS DATE) = CAST(GETDATE() AS DATE)
			//and CAST([MODIFIED_DATE] AS DATE) = CAST(GETDATE() AS DATE)
			p.CanonicalModelVersion = "4.0";
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

			ttlRows = 100; // Convert.ToInt32(l.SourceRows);
			numberOfRowsPerPage = 100;// 10000;

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
							cpl.RUN_TIMESTAMP = (!results.IsDBNull(results.GetOrdinal("RUN_TIMESTAMP"))) ? Convert.ToDateTime(results["RUN_TIMESTAMP"]).ToString("dd/MM/yyyy H:mm:ss") : "00/00/0000 00:00:00";
							cpl.OPERATION_SOURCE_UNIQUE_ID = (!results.IsDBNull(results.GetOrdinal("OPERATION_SOURCE_UNIQUE_ID"))) ? results["OPERATION_SOURCE_UNIQUE_ID"].ToString() : "";
							cpl.POLICY_ID = (!results.IsDBNull(results.GetOrdinal("POLICY_ID"))) ? results["POLICY_ID"].ToString() : "";
							cpl.CUSTOMER_ID = (!results.IsDBNull(results.GetOrdinal("CUSTOMERID"))) ? results["CUSTOMERID"].ToString() : "";
							cpl.INTERMEDIARY_ID = (!results.IsDBNull(results.GetOrdinal("INTERMEDIARY_ID"))) ? results["INTERMEDIARY_ID"].ToString() : "";
							cpl.BRANCH_ID = (!results.IsDBNull(results.GetOrdinal("BRANCH_ID"))) ? results["BRANCH_ID"].ToString() : "";
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
							cpl.ACCOUNT_NUMBER = (!results.IsDBNull(results.GetOrdinal("ACCOUNT_NUMBER"))) ? results["ACCOUNT_NUMBER"].ToString() : "";
							cpl.FOREIGN_FLAG = (!results.IsDBNull(results.GetOrdinal("FOREIGN_FLAG"))) ? results["FOREIGN_FLAG"].ToString() : "";
							cpl.SOURCE_OF_FUNDS_FLAG = null;
							cpl.UNUSUAL_PAYMENT_METHOD_FLAG = null;
							cpl.REIMBURSEMENT_FLAG = null;
							cpl.PROGRAMMED_FLAG = null;
							cpl.REJECTED_FLAG = (!results.IsDBNull(results.GetOrdinal("REJECTED_FLAG"))) ? results["REJECTED_FLAG"].ToString() : "";
							cpl.BENEFICIARY_CLAUSE = null;
							cpl.ORGUNIT_CODE = (!results.IsDBNull(results.GetOrdinal("ORGUNIT_CODE"))) ? results["ORGUNIT_CODE"].ToString() : "";


							cpl.X_OPERATION_DATE = null;
							cpl.X_OPERATION_SOURCE_SYSTEM = (!results.IsDBNull(results.GetOrdinal("X_OPERATION_SOURCE_SYSTEM"))) ? results["X_OPERATION_SOURCE_SYSTEM"].ToString() : "";
							cpl.X_INSURED_SOURCE_UNIQUE_ID = (!results.IsDBNull(results.GetOrdinal("X_INSURED_SOURCE_UNIQUE_ID"))) ? results["X_INSURED_SOURCE_UNIQUE_ID"].ToString() : "";
							cpl.X_POLICY_BENEFICIARY_SOURCE_UNIQUE_ID = (!results.IsDBNull(results.GetOrdinal("X_POLICY_BENEFICIARY_SOURCE_UNIQUE_ID"))) ? results["X_POLICY_BENEFICIARY_SOURCE_UNIQUE_ID"].ToString() : "";
							cpl.X_COHOLDER_SOURCE_UNIQUE_ID = null;
							cpl.X_POLICY_PAYER_SOURCE_UNIQUE_ID = (!results.IsDBNull(results.GetOrdinal("X_POLICY_PAYER_SOURCE_UNIQUE_ID"))) ? results["X_POLICY_PAYER_SOURCE_UNIQUE_ID"].ToString() : "";
							cpl.X_OLD_CHG_SOURCE_UNIQUE_ID = (!results.IsDBNull(results.GetOrdinal("X_OLD_CHG_SOURCE_UNIQUE_ID"))) ? results["X_OLD_CHG_SOURCE_UNIQUE_ID"].ToString() : "";
							cpl.X_HOLDER_COUNTRY_OF_RESIDENCE = (!results.IsDBNull(results.GetOrdinal("X_HOLDER_COUNTRY_OF_RESIDENCE"))) ? results["X_HOLDER_COUNTRY_OF_RESIDENCE"].ToString() : "";
							cpl.X_POLICY_HOLDER_TYPE = null;
							cpl.X_HOLDER_SPECIAL_MONITORING = (!results.IsDBNull(results.GetOrdinal("X_HOLDER_SPECIAL_MONITORING"))) ? results["X_HOLDER_SPECIAL_MONITORING"].ToString() : "";
							cpl.X_COHOLDER_SPECIAL_MONITORING = null;
							cpl.X_INSURED_SPECIAL_MONITORING = null;
							cpl.X_BENEFICIARY_SPECIAL_MONITORING = null;
							cpl.X_PAYER_SPECIAL_MONITORING = null;
							cpl.X_MAIN_TXN_TYPE_CODE = null;
							cpl.X_BUSINESS_LINE = (!results.IsDBNull(results.GetOrdinal("X_BUSINESS_LINE"))) ? results["X_BUSINESS_LINE"].ToString() : "";
							cpl.X_BUSINESS_SUBLINE = null;
							cpl.X_SUBSCRIPTION_DATE = null;
							cpl.X_EFFECTIVE_DATE = null;
							cpl.X_SURRENDER_DATE = null;
							cpl.X_POLICY_VALUE_TD = null;
							cpl.X_TOTAL_POLICIES_VALUE_TD = null;
							cpl.X_TOTAL_ASSET_HOLDER = null;
							cpl.X_CAPITAL_LOST = null;
							cpl.X_EXPECTED_ANNUAL_TURNOVER = null;
							cpl.X_PRODUCT_ID = null;
							cpl.X_BANK_COUNTRY_CODE = null;
							cpl.X_TXT_GROSS_INVST_AMOUNT_BASE = null;
							cpl.X_PENSION_AGE = null;
							cpl.X_HOLDER_AGE = null;
							cpl.X_INTERMEDIARY_SEGMENT = null;
							cpl.X_H_COUNTRY_OF_TAX_RESIDENCE = null;


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

		public static ExtractionLog ExtractDeltaPoliciesSP(string period)
		{

			ExtractionLog l = new ExtractionLog();
			l.ExeTime = DateTime.Now;

			InputRequestParam p = new InputRequestParam();
			p.JsonTitle = "POLICIES";

			l.SourceRows = "100";
			//l.SourceRows = RowsCount("select count(*) from GFCC.GFCC_FLOW.CUSTOMER where CUSTOMERSTATUSCODE = 'ACTIVE' and  CAST([MODIFIED_DATE] AS DATE) = CAST(GETDATE() AS DATE) and identification_number_type = 'KTP' and zone <> 'NULL' and occupation <> ''  and gender_code = 'F' ", SharedUtils.GetDSN()).ToString();
			
			//l.SourceRows = ExtractionCanonicalDAO.RowsCount("select count(*) from GFCC_FLOW.POLICIES where " + period, SharedUtils.GetDSN("GFCC")).ToString();
			l.TaskName = p.JsonTitle;
			p.StringSQLCommand = "[WebApp].[SpMNLGetPolicies]";
			if (Convert.ToInt64(l.SourceRows) < 1)
			{
				Console.WriteLine("No Data Available -7");
				l.ExceptionThrown = "No Data!";
				l.ExtractionStatus = false;
				l.EndTime = DateTime.Now;
				return l;
			}
			//where CAST([MODIFIED_DATE] AS DATE) = CAST(GETDATE() AS DATE)
			//and CAST([MODIFIED_DATE] AS DATE) = CAST(GETDATE() AS DATE)
			p.CanonicalModelVersion = "4.0";
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

			ttlRows = 100; //Convert.ToInt32(l.SourceRows); 
			numberOfRowsPerPage = 100; // 10000;

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
								TOTAL_ANNUAL_PREMIUM_TD = (!results.IsDBNull(results.GetOrdinal("TOTAL_ANNUAL_PREMIUM_TD"))) ? results["TOTAL_ANNUAL_PREMIUM_TD"].ToString() : "",
								X_CHANNEL = (!results.IsDBNull(results.GetOrdinal("X_CHANNEL"))) ? results["X_CHANNEL"].ToString() : "",
								X_POLICY_END_DATE = (!results.IsDBNull(results.GetOrdinal("X_POLICY_END_DATE"))) ? Convert.ToDateTime(results["X_POLICY_END_DATE"]).ToString("dd/MM/yyyy") : "00/00/0000"
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

		public static ExtractionLog ExtractDeltaProductSP(string period)
		{

			ExtractionLog l = new ExtractionLog();
			l.ExeTime = DateTime.Now;

			InputRequestParam p = new InputRequestParam();
			p.JsonTitle = "PRODUCT";

			l.SourceRows = "100";
			//l.SourceRows = RowsCount("select count(*) from GFCC.GFCC_FLOW.CUSTOMER where CUSTOMERSTATUSCODE = 'ACTIVE' and  CAST([MODIFIED_DATE] AS DATE) = CAST(GETDATE() AS DATE) and identification_number_type = 'KTP' and zone <> 'NULL' and occupation <> ''  and gender_code = 'F' ", SharedUtils.GetDSN()).ToString();
			//l.SourceRows = ExtractionCanonicalDAO.RowsCount("select count(*) from GFCC_FLOW.PRODUCT where " + period, SharedUtils.GetDSN("GFCC")).ToString();
			l.TaskName = p.JsonTitle;
			p.StringSQLCommand = "[WebApp].[SpMNLGetProducts]";
			if (Convert.ToInt64(l.SourceRows) < 1)
			{
				Console.WriteLine("No Data Available -8");
				l.ExceptionThrown = "No Data!";
				l.ExtractionStatus = false;
				l.EndTime = DateTime.Now;
				return l;
			}
			//where CAST([MODIFIED_DATE] AS DATE) = CAST(GETDATE() AS DATE)
			//and CAST([MODIFIED_DATE] AS DATE) = CAST(GETDATE() AS DATE)
			p.CanonicalModelVersion = "4.0";
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

			ttlRows = 100; // Convert.ToInt32(l.SourceRows); 
			numberOfRowsPerPage = 100; // 10000;

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
								PRODUCT_CLASS = (!results.IsDBNull(results.GetOrdinal("PRODUCT_CLASS"))) ? results["PRODUCT_CLASS"].ToString() : "",
								PRODUCT_LINE = (!results.IsDBNull(results.GetOrdinal("PRODUCT_LINE"))) ? results["PRODUCT_LINE"].ToString() : "",
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

		public static ExtractionLog ExtractDeltaProductSourceTypeSP(string period)
		{

			ExtractionLog l = new ExtractionLog();
			l.ExeTime = DateTime.Now;

			InputRequestParam p = new InputRequestParam();
			p.JsonTitle = "PRODUCT_SOURCE_TYPE";

			l.SourceRows = "100";
			//l.SourceRows = RowsCount("select count(*) from GFCC.GFCC_FLOW.CUSTOMER where CUSTOMERSTATUSCODE = 'ACTIVE' and  CAST([MODIFIED_DATE] AS DATE) = CAST(GETDATE() AS DATE) and identification_number_type = 'KTP' and zone <> 'NULL' and occupation <> ''  and gender_code = 'F' ", SharedUtils.GetDSN()).ToString();
			
			//l.SourceRows = ExtractionCanonicalDAO.RowsCount("select count(*) from GFCC_FLOW.PRODUCT_SOURCE_TYPE where " + period, SharedUtils.GetDSN("GFCC")).ToString();
			l.TaskName = p.JsonTitle;
			p.StringSQLCommand = "[WebApp].[SpMNLGetProductSourceType]";
			if (Convert.ToInt64(l.SourceRows) < 1)
			{
				Console.WriteLine("No Data Available -9");
				l.ExceptionThrown = "No Data!";
				l.ExtractionStatus = false;
				l.EndTime = DateTime.Now;
				return l;
			}
			//where CAST([MODIFIED_DATE] AS DATE) = CAST(GETDATE() AS DATE)
			//and CAST([MODIFIED_DATE] AS DATE) = CAST(GETDATE() AS DATE)
			p.CanonicalModelVersion = "4.0";
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

			ttlRows = 100; // Convert.ToInt32(l.SourceRows); 
			numberOfRowsPerPage = 100; // 10000;

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
								PRODUCT_ID = (!results.IsDBNull(results.GetOrdinal("PRODUCT_ID"))) ? results["PRODUCT_ID"].ToString() : "",

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

	}
}
