using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace AF.DAL
{
	public class ConfigLogsDAO : IDisposable
	{
		private string cnnstr; private bool IsDisposed = false; private bool IsNew = true;
		public DateTime LogDate { get; set; }
		public string UserID { get; set; }
		public string IterationChange { get; set; }
		public string TSChange { get; set; }

		public ConfigLogsDAO() { cnnstr = SharedUtils.GetDSN(); }


        public ConfigLogsDAO(string USERID)
		{
			cnnstr = SharedUtils.GetDSN();
			SqlConnection cnn = new SqlConnection(cnnstr);
			cnn.Open();
			string vSql = "SELECT * FROM [WebApp].[ConfigLogs] WHERE UserID=@UserID ";
			SqlCommand com = new SqlCommand(vSql, cnn);
			com.Parameters.AddWithValue("@UserID", USERID);
			SqlDataReader reader = com.ExecuteReader();
			if (reader.Read())
			{
				IsNew = false;
				if (!reader.IsDBNull(reader.GetOrdinal("LogDate"))) this.LogDate = (DateTime)reader["LogDate"];
				if (!reader.IsDBNull(reader.GetOrdinal("UserID"))) this.UserID = (string)reader["UserID"];
				if (!reader.IsDBNull(reader.GetOrdinal("IterationChange"))) this.IterationChange = (string)reader["IterationChange"];
				if (!reader.IsDBNull(reader.GetOrdinal("TSChange"))) this.TSChange = (string)reader["TSChange"];
			}
			reader.Close();
			com.Dispose();
			cnn.Close();
			cnn.Dispose();
		}

		public void Save()
		{
			SqlConnection cnn = new SqlConnection(cnnstr);
			cnn.Open();
			SqlCommand com = new SqlCommand();
			com.Connection = cnn;
			if (!IsNew)
			{
				com.CommandText = "UPDATE  [WebApp].[ConfigLogs] SET LogDate=getdate() WHERE UserID=@UserID";
				com.Parameters.AddWithValue("@UserID", this.UserID);
			}
			else
			{
				com.CommandText = "INSERT INTO  [WebApp].[ConfigLogs] (LogDate,UserID,IterationChange,TSChange) VALUES (getdate(),@UserID,@IterationChange,@TSChange)";
				com.Parameters.AddWithValue("@UserID", this.UserID);
				com.Parameters.AddWithValue("@IterationChange", this.IterationChange);
				com.Parameters.AddWithValue("@TSChange", this.TSChange);
			}
			try { com.ExecuteNonQuery(); }
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			com.Dispose();
			cnn.Close();
			cnn.Dispose();
			IsNew = false;
		}
		//Update Db 

		public void UpdateProses()
		{
            SqlConnection cnn = new SqlConnection(SharedUtils.GetDSN2());
            cnn.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = cnn;
            
                com.CommandText = "UPDATE STG_DB.LIL.BatchGenerateOutput SET ProcessId = 1, ProcessRemarks = 'IN PROGRESS'";
               
           
            try { com.ExecuteNonQuery(); }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            com.Dispose();
            cnn.Close();
            cnn.Dispose();
            IsNew = false;
        }
		public static DataTable GetList(string SortBy, string Condition, int top)
		{
			SqlConnection cnn = new SqlConnection(SharedUtils.GetDSN());
			cnn.Open();
			DataTable vRet = new DataTable();
			string vSql = "select ";
			if ((top != 0))
			{
				vSql = (vSql + ("top "
							+ (Convert.ToString(top) + " ")));
			}

			vSql += " * from  [WebApp].[ConfigLogs] ";
			if ((Condition != ""))
			{
				vSql = (vSql + (" where " + Condition));
			}

			if ((SortBy != ""))
			{
				vSql = (vSql + (" order by " + SortBy));
			}

			SqlDataAdapter adap = new SqlDataAdapter(vSql, cnn);
			adap.Fill(vRet);
			adap.Dispose();
			cnn.Close();
			cnn.Dispose();
			return vRet;
		}

		public static DataTable GetHistory()
		{
			SqlConnection cnn = new SqlConnection(SharedUtils.GetDSN());
			cnn.Open();
			DataTable vRet = new DataTable();
			string vSql = @"select 
							ROW_NUMBER() OVER (
								ORDER BY TaskGroup
							   ) row_num,
							TaskGroup,FORMAT (logdate,  'dd/MM/yyyy ', 'en-us') as ExeDate,'Daily' as filepath,
							FORMAT (EndTime,  'dd/MM/yyyy ', 'en-us') as EndTime,
							FORMAT ((max(EndTime)-min(exetime)), 'hh:mm:ss tt')  dura,
							convert(varchar(5),DateDiff(s, min(exetime), max(EndTime))/3600)+':'+convert(varchar(5),
                            DateDiff(s,  min(exetime), max(EndTime))%3600/60)+':'+convert(varchar(5), (DateDiff(s, min(exetime), 
                            max(EndTime))%60)) as [Duration],COUNT(CASE WHEN ExtractionStatus=1 THEN 1 END) AS success,
							COUNT(CASE WHEN ExtractionStatus=0 THEN 1 END) AS Failed 
							from WebApp.TaskProcessLog 
							where  FORMAT (logdate,  'dd/MM/yyyy ', 'en-us') = FORMAT (GETDATE(),  'dd/MM/yyyy ', 'en-us') 
							group by TaskGroup,FORMAT (logdate,  'dd/MM/yyyy ', 'en-us'),filepath,
							FORMAT (EndTime,  'dd/MM/yyyy ', 'en-us')";
			//Yesterday date
			//FORMAT (( DATEADD(day, -1, CAST(GETDATE() AS date))),  'dd/MM/yyyy ', 'en-us')
			SqlDataAdapter adap = new SqlDataAdapter(vSql, cnn);
			adap.Fill(vRet);
			adap.Dispose();
			cnn.Close();
			cnn.Dispose();
			return vRet;
		}

        public void Delete()
		{
			SqlConnection cnn = new SqlConnection(cnnstr);
			cnn.Open();
			string vSql = "delete from [WebApp].[ConfigLogs] where UserID=@UserID";
			SqlCommand com = new SqlCommand(vSql, cnn);
			com.Parameters.AddWithValue("@UserID", this.UserID);
			var i = com.ExecuteNonQuery();
			Console.WriteLine(i.ToString());
			com.Dispose();
			cnn.Close();
			cnn.Dispose();
		}

		public void Delete(string userid)
		{
			SqlConnection cnn = new SqlConnection(SharedUtils.GetDSN());
			cnn.Open();
			string vSql = "delete from [WebApp].[ConfigLogs] where UserID=@UserID";
			SqlCommand com = new SqlCommand(vSql, cnn);
			com.Parameters.AddWithValue("@UserID", userid);
			var i = com.ExecuteNonQuery();
			Console.WriteLine(i.ToString());
			com.Dispose();
			cnn.Close();
			cnn.Dispose();
		}

		public static string GetID(string condition)
		{
			string res = "";
			SqlConnection cnn = new SqlConnection(SharedUtils.GetDSN());
			cnn.Open();
			string vSql = ("select top 1 UserID from [WebApp].[ConfigLogs] where " + condition);
			SqlCommand com = new SqlCommand(vSql, cnn);
			SqlDataReader reader = com.ExecuteReader();
			if (reader.Read())
			{
				if (!reader.IsDBNull(reader.GetOrdinal("UserID")))
				{
					res = (reader["UserID"]).ToString();
				}
			}
			else
			{
				res = "";
			}

			reader.Close();
			com.Dispose();
			cnn.Close();
			cnn.Dispose();
			return res;
		}

		protected virtual void Dispose(bool disposing)
		{
			this.IsDisposed = true;
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}


	}
}
