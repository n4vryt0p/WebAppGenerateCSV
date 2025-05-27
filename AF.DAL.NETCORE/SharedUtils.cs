using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace AF.DAL
{
	public class SharedUtils
	{
		public static string GetDSN()
		{
            return ConfigurationManager.ConnectionStrings["GFCCTWEBContext"].ConnectionString;
		}
		//public static string GetDSN(string db, string source = "wrdsql09", string dbuser = "gfcc", string dbpass = "A^c9D2,6-x%KmXL)QT.`!R;")
		//{
		//	//string strCon = ConfigurationManager.ConnectionStrings["GFCCTDataContext"].ConnectionString;
		//	//return @"Data Source=wrbapp01\sqlserver;initial catalog=GFCC;user id=idas_sql;password=P@ssw0rd;";

		//	//return @"Data Source=" + source + ";initial catalog=" + db + ";user id=" + dbuser + ";password=" + dbpass + ";";
  //          return ConfigurationManager.ConnectionStrings["GFCCTWEBContext"].ConnectionString;
  //      }

		public static string GetDSNGFCC()
		{
            return ConfigurationManager.ConnectionStrings["GFCCTContext"].ConnectionString;
        }

        public static string GetDSN2()
		{
            return ConfigurationManager.ConnectionStrings["GFCCTSTGContext"].ConnectionString;
		}

        public static string GetDSNApp09()
		{
			return ConfigurationManager.ConnectionStrings["GFCCTWEB09Context"].ConnectionString;
		}
	
		public static string SqlBuilder(string sqlCommand, string[] inputParam, string sortBy)
		{
			string vRet = "";
			var sql = new System.Text.StringBuilder();
			sql.Append(sqlCommand);
			for (var i = 0; i < inputParam.Length; i++)
			{
				//cmd.Parameters.Add(new SqlParameter("@" + i, inputParam[i]));
				if (i > 0) sql.Append(", ");
				sql.Append("'" + inputParam[i] + "'");
			}
			sql.Append(")");
			if (sortBy != "")
			{
				sql.Append(" order by " + sortBy);
			}

			vRet = sql.ToString();
			return vRet;

		}

		public static DataTable ExecuteSqlCommandReturnDt(string sqlCommand, string connString, Dictionary<string, object> paras, string[] inputparam, string orderBy)
		{
			DataTable dt = null;
			using (SqlConnection con = new SqlConnection(connString))
			{
				string strSql = SqlBuilder(sqlCommand, inputparam, orderBy);
				using (SqlCommand cmd = new SqlCommand(strSql, con))
				{
					cmd.CommandType = CommandType.Text;
					using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
					{
						using (dt = new DataTable())
						{
							sda.Fill(dt);
						}
					}
				}
			}
			return dt;
		}

		public static DataTable ExecuteSqlCommandReturnDt(string sqlCommand, string connString)
		{
			DataTable dt = null;
			try
			{
				using (SqlConnection con = new SqlConnection(connString))
				{
					using (SqlCommand cmd = new SqlCommand(sqlCommand, con))
					{
						cmd.CommandType = CommandType.Text;
						using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
						{
							using (dt = new DataTable())
							{
								sda.Fill(dt);
							}
						}
					}
				}

			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			return dt;
		}

		public static string GetDataSingleValue(string sql, string con)
		{
			string vRet = "";
			SqlConnection cnn;
			SqlCommand cmd;
			cnn = new SqlConnection(con);
			try
			{
				cnn.Open();
				cmd = new SqlCommand(sql, cnn);
				vRet = (cmd.ExecuteScalar().ToString());
				cmd.Dispose();
				cnn.Close();
			}
			catch (Exception ex)
			{
				vRet = "";
			}
			return vRet;

		}

		public static DataTable GetDataCustomerByPage(Int32 pageNumber,Int32 pageSize)
		{
			DataTable vRet = null;

			var sql = "[WebApp].[SpGetCustomer]";
			SqlConnection conn = new SqlConnection(GetDSN());
			using (SqlCommand command = new SqlCommand(sql, conn))
			{
				command.CommandType = CommandType.StoredProcedure;

				command.Parameters.AddWithValue("@PageNumber", pageNumber);
				command.Parameters.AddWithValue("@PageSize", pageSize);

				try
				{
					conn.Open();
					vRet = new DataTable();
					SqlDataAdapter adap = new SqlDataAdapter();
					adap.SelectCommand = command;
					adap.Fill(vRet);
					adap.Dispose();
					conn.Close();
					conn.Dispose();
				}
				catch(Exception e)
				{
					Console.WriteLine(e.ToString()); }
			}
			return vRet; 
		}

		public static Int32 NumberOfPages(Int32 ttl, Int32 dvdr)
		{
			Int32 vRet = 0;

			decimal rt = 0;
			rt = ttl % dvdr;
			vRet = ttl / dvdr;
			if (rt > 0){ vRet++; }

			return vRet;
		}

		public static string MigrationDateAndIsComplete()
		{
			// CAST(GETDATE() AS DATE)
			string vRet = null;
			Nullable<DateTime> strDate = null;
			string sqlCom = @"SELECT TOP 1 [START_DATE] FROM [STG_DB].[dbo].[MIGRATION_STATUS]
							WHERE 1 = 1 
							AND TABLE_DESTINATION = 'All Canonical'
							AND MIGRATE_STATUS_SOURCE = 'C' 
							ORDER BY MIGRATION_STATUS_ID DESC
						";
			using (SqlConnection conn = new SqlConnection(GetDSN2()))
			{
				SqlCommand cmd = new SqlCommand(sqlCom, conn);
				try
				{
					conn.Open();
					strDate = (DateTime)cmd.ExecuteScalar();
					if (strDate!=null) { vRet = strDate.ToString(); }
				}
				catch(Exception e)
				{
					vRet = null;
				}
			}
			return vRet;
		}

		public static bool MigrationIsComplete()
		{
			// CAST(GETDATE() AS DATE)
			bool vRet = false;
			Int32 rowCount = 0;
			string sqlCom = @"SELECT count(MIGRATION_STATUS_ID) FROM [STG_DB].[dbo].[MIGRATION_STATUS]
							WHERE 1 = 1 
							AND TABLE_SOURCE = '00_GFCCT_CANONICAL' 
							AND TABLE_DESTINATION = 'All Canonical'
							AND MIGRATE_STATUS_SOURCE = 'C' 
							AND CAST(LOGDATE AS DATE) = '2021-03-22'
						";
			using (SqlConnection conn = new SqlConnection(GetDSN2()))
			{
				SqlCommand cmd = new SqlCommand(sqlCom, conn);
				try
				{
					conn.Open();
					rowCount = (Int32)cmd.ExecuteScalar();
					if (rowCount > 0) { vRet = true; }
				}
				catch
				{
					vRet = false;
				}
			}
			return vRet;
		}
	
		public static string GetRangeCondition()
		{
			throw new NotImplementedException();
		}
		public static bool MenuIsExists(string p, string arrStr)
		{
			bool ret = false;
			char[] delimiterChars = { ',' };
			string[] words = arrStr.Split(delimiterChars);
			int index = Array.IndexOf(words, p);
			if (index < 0) { Console.WriteLine("NotFound"); ret = false; } else { Console.WriteLine("Found"); ret = true; }
			return ret;
		}
		public static string GetCanonicalDatePeriod()
		{
			TaskSchedulerDAO t = new TaskSchedulerDAO(1);
			string fromDate = t.FromDate.ToString("yyyyMMdd");
			string stringDate =" '" + t.FromDate.ToString("yyyyMMdd") + "' and '" + t.ToDate.ToString("yyyyMMdd") + "' ";
			t.Dispose() ; 
			return stringDate;
		}
		public static string GetRangeCondition(string range) {
			//string vRet = "";
			string condition = "";
			if (range != "")
			{
				char[] delimiterChars = { '/','-' };
				string[] w = range.Split(delimiterChars);
				string startDate = w[2].Trim() + w[0].PadLeft(2, '0') + w[1].PadLeft(2, '0') + " 00:00:00";
				string endDate = w[5] + w[3].Trim().PadLeft(2, '0') + w[4].PadLeft(2, '0') + " 23:59:59";

				condition = " where LogDate between '" + startDate + "' and '" + endDate + "'";
				//System.Console.WriteLine($"{w.Length} words in text:");
				//condition = " where LogDate between '" + w[2].Trim() + w[0] + w[1] + "' and '" + w[5] + w[3].Trim() + w[4] + "'";
			}
			else
			{
				DateTime yesterDay = DateTime.Now.AddDays(-1);
				condition = " where LogDate >= '" + yesterDay.ToString("yyyyMMdd") + "'";
			}
			return condition;
		}
		public static string[] FromRangeTodate(string range)
		{
			string[] vRet= new string[2] ;
			if (range != "")
			{
				char[] delimiterChars = { '/', '-' };
				string[] w = range.Split(delimiterChars);
				System.Console.WriteLine($"{w.Length} words in text:");
				vRet[0] = w[0].Trim() + "/" + w[1].Trim() + "/" + w[2].Trim() + " 00:00:00";
				vRet[1] = w[3].Trim() + "/" + w[4].Trim() + "/" + w[5].Trim() + " 23:59:59";
			}
			
			return vRet;
		}
		public static string TimeTo24Hour(string strTime) {
			string vRet = "";
			string n = strTime.Substring(strTime.Length-2,2);
			if (n == "PM")
			{
				int j = Convert.ToInt16(strTime.Substring(0, 2)) + 12;
				if (j == 24)
				{
					vRet = "00" + strTime.Substring(2, 3);
				}
				else
				{
					vRet = (Convert.ToInt16(strTime.Substring(0, 2)) + 12).ToString() + strTime.Substring(2, 3);
				}
			}
			else if (n == "AM")
			{
				vRet = strTime.Substring(0, 2) + strTime.Substring(2, 3);
			}

			return vRet;
		}
		public static SqlDataReader ExecuteSqlCommand(string sqlCommand, string connString)
		{
			SqlConnection conn = new SqlConnection(connString);
			conn.Open();
			SqlCommand comm = conn.CreateCommand();
			comm.CommandText = sqlCommand;
			comm.CommandTimeout = 180;
			return comm.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
		}
		public static SqlDataReader ExecuteSPCommand(string sqlCommand, string connString, Int32 pageNumber, Int32 pageSize)
		{
			SqlConnection conn = new SqlConnection(connString);
			conn.Open();
			
			SqlCommand comm = conn.CreateCommand();
			comm.CommandType= CommandType.StoredProcedure;

			comm.Parameters.AddWithValue("@PageNumber", pageNumber);
			comm.Parameters.AddWithValue("@PageSize", pageSize); 
			comm.CommandText = sqlCommand;
			comm.CommandTimeout = 600000;
			return comm.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
		}
		public Task<DataSet> GetDataSetAsync(string sConnectionString, string sSQL, params SqlParameter[] parameters)
		{
			return Task.Run(() =>
			{
				using (var newConnection = new SqlConnection(sConnectionString))
				using (var mySQLAdapter = new SqlDataAdapter(sSQL, newConnection))
				{
					mySQLAdapter.SelectCommand.CommandType = CommandType.Text;
					if (parameters != null) mySQLAdapter.SelectCommand.Parameters.AddRange(parameters);

					DataSet myDataSet = new DataSet();
					mySQLAdapter.Fill(myDataSet);
					return myDataSet;
				}
			});
		}
		public Task<int> ExecuteAsync(string sConnectionString, string sSQL, params SqlParameter[] parameters)
		{
			return Task.Run(() =>
			{
				using (var newConnection = new SqlConnection(sConnectionString))
				using (var newCommand = new SqlCommand(sSQL, newConnection))
				{
					newCommand.CommandType = CommandType.Text;
					if (parameters != null) newCommand.Parameters.AddRange(parameters);

					newConnection.Open();
					return newCommand.ExecuteNonQuery();
				}
			});
		}
		public async Task<int> ExecuteAsyncSimply(string sConnectionString,string sSQL, params SqlParameter[] parameters)
		{
			using (var newConnection = new SqlConnection(sConnectionString))
			using (var newCommand = new SqlCommand(sSQL, newConnection))
			{
				newCommand.CommandType = CommandType.Text;
				if (parameters != null) newCommand.Parameters.AddRange(parameters);

				await newConnection.OpenAsync().ConfigureAwait(false);
				return await newCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
			}
		}
		public static string SafeGetString(SqlDataReader reader, int colIndex)
		{
			if (!reader.IsDBNull(colIndex))
			{
				return reader.GetString(colIndex).ToString();
			}
			else
			{
				return string.Empty;
			}

		}

		public static string GetConfigs(string Key, string Conditional = null)
        {
			var resp = "";
            try
            {
				using (SqlDataReader results = ExecuteSqlCommand("SELECT [Value] FROM WebApp.MasterConfigs WHERE [Key] = '" + Key + "'", GetDSN()))
				{
					while (results.Read())
					{
						resp = results["Value"].ToString();
					}
						
				}

				return resp;
			}catch(Exception e)
            {
				return e.Message;
            }
        }

		public static string GetCount(string tableName, string conString)
		{
			var resp = "";
			try
			{
				using (SqlDataReader results = ExecuteSqlCommand($"SELECT count(*) as count FROM {tableName}", conString))
				{
					while (results.Read())
					{
						resp = results["count"].ToString();
					}

				}

				return resp;
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}
        //AddCOunt data
        public static string GetCountflow(string conString)
        {
            var resp = "";
            try
            {
                using (SqlDataReader results = ExecuteSqlCommand($"SELECT count(*) as count  FROM WebApp.MasterConfigs WHERE [Group] = 'GfccFlow'", conString))

				{
                    while (results.Read())
                    {
                        resp = results["count"].ToString();
                    }

                }

                return resp;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static List<dynamic> GetConfigArray(string GroupKey)
		{
			var resp = new List<dynamic>();
			try
			{
				using (SqlDataReader results = ExecuteSqlCommand("SELECT [Value],[Desc] FROM WebApp.MasterConfigs WHERE [Group] = '" + GroupKey + "' ORDER BY [Key]", GetDSN()))
				{
					while (results.Read())
					{
						var Value = results["Value"].ToString();
						var Desc = results["Desc"].ToString();

						resp.Add(new
						{
							Value = Value,
							Desc = Desc
						});
					}

				}

				return resp;
			}
			catch (Exception e)
			{
				resp.Add(e.Message);
				return resp;
			}
		}

		public static string CreateMD5(string input)
		{
			// Use input string to calculate MD5 hash
			using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
			{
				byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
				byte[] hashBytes = md5.ComputeHash(inputBytes);

				var hexString = BitConverter.ToString(hashBytes);

				return hexString.Replace("-", ""); // .NET 5 +

			}
		}
	}
}
