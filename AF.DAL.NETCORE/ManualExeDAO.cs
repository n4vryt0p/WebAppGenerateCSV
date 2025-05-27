using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace AF.DAL
{
	public class ManualExeDAO
	{
		private string cnnstr; private bool IsDisposed = false; private bool IsNew = true;
		public int ID { get; set; }
		public string TaskName { get; set; }
		public string Email { get; set; }
		public Boolean Status { get; set; }
		public DateTime FromDate { get; set; }
		public DateTime ToDate { get; set; }
		public int SourceRows { get; set; }
		public int ExtractedRows { get; set; }
		public DateTime CreateTS { get; set; }
		public DateTime UpdateTS { get; set; }

		public ManualExeDAO() { cnnstr = SharedUtils.GetDSN(); }

		public ManualExeDAO(int ID)
		{
			cnnstr = SharedUtils.GetDSN();
			SqlConnection cnn = new SqlConnection(cnnstr);
			cnn.Open();
			string vSql = "SELECT * FROM [WebApp].[ManualExe] WHERE ID=@ID ";
			SqlCommand com = new SqlCommand(vSql, cnn);
			com.Parameters.AddWithValue("@ID", ID);
			SqlDataReader reader = com.ExecuteReader();
			if (reader.Read())
			{
				IsNew = false;
				if (!reader.IsDBNull(reader.GetOrdinal("ID"))) this.ID = (int)reader["ID"];
				if (!reader.IsDBNull(reader.GetOrdinal("TaskName"))) this.TaskName = (string)reader["TaskName"];
				if (!reader.IsDBNull(reader.GetOrdinal("Email"))) this.Email = (string)reader["Email"];
				if (!reader.IsDBNull(reader.GetOrdinal("SourceRows"))) this.SourceRows = (int)reader["SourceRows"];
				if (!reader.IsDBNull(reader.GetOrdinal("ExtractedRows"))) this.ExtractedRows = (int)reader["ExtractedRows"];
				if (!reader.IsDBNull(reader.GetOrdinal("Status"))) this.Status = (bool)reader["Status"];
				if (!reader.IsDBNull(reader.GetOrdinal("FromDate"))) this.FromDate = (DateTime)reader["FromDate"];
				if (!reader.IsDBNull(reader.GetOrdinal("ToDate"))) this.ToDate = (DateTime)reader["ToDate"];
				if (!reader.IsDBNull(reader.GetOrdinal("CreateTS"))) this.CreateTS = (DateTime)reader["CreateTS"];
				if (!reader.IsDBNull(reader.GetOrdinal("UpdateTS"))) this.UpdateTS = (DateTime)reader["UpdateTS"];
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
				com.CommandText = "UPDATE  [WebApp].[ManualExe] SET Status=@Status,Email=@Email,FromDate=@FromDate,ToDate=@ToDate,UpdateTS=getdate() WHERE ID=@ID";
				com.Parameters.AddWithValue("@Status", this.Status);
				com.Parameters.AddWithValue("@Email", this.Email);
				com.Parameters.AddWithValue("@FromDate", this.FromDate);
				com.Parameters.AddWithValue("@ToDate", this.ToDate);
				com.Parameters.AddWithValue("@ID", this.ID);
			}
			else
			{
				com.CommandText = "INSERT INTO  [WebApp].[ManualExe] (ID,TaskName,Email,FromDate,ToDate,Status,CreateTS) VALUES (@ID,@TaskName,@Email,@FromDate,@ToDate,@Status,getdate())";
				com.Parameters.AddWithValue("@ID", this.ID);
				com.Parameters.AddWithValue("@TaskName", this.TaskName);
				com.Parameters.AddWithValue("@Email", this.Email);
				com.Parameters.AddWithValue("@FromDate", this.FromDate);
				com.Parameters.AddWithValue("@ToDate", this.ToDate);
				com.Parameters.AddWithValue("@Status", this.Status);
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

		public static DataTable GetManualTaskList()
		{
			SqlConnection cnn = new SqlConnection(SharedUtils.GetDSN());
			cnn.Open();
			DataTable vRet = new DataTable();
			string vSql = "SELECT [ID],[TaskName],FORMAT([FromDate], 'dd/MM/yyyy') as FromDate,FORMAT([ToDate], 'dd/MM/yyyy') as ToDate,[Email],[Status],[SourceRows],[ExtractedRows],[CreateTS],[UpdateTS] FROM [WebApp].[ManualExe]";
			//string vSql = "SELECT ID,TaskName,TaskGroup,TriggerName,TriggerGroup,secondPart,minutePart,hourPart,dayOfMonthPart,monthPart,dayOfWeekPart,CONVERT(varchar, fromdate, 101) as fromdate,CONVERT(varchar, todate, 101) as todate,email,CronExpression FROM [WebApp].[TaskScheduler]";
			SqlDataAdapter adap = new SqlDataAdapter(vSql, cnn);
			adap.Fill(vRet);
			adap.Dispose();
			cnn.Close();
			cnn.Dispose();
			return vRet;
		}

		public static DataTable GetSqlAgentTaskList()
		{
			SqlConnection cnn = new SqlConnection(SharedUtils.GetDSN());
			cnn.Open();
			DataTable vRet = new DataTable();
			//CONVERT(varchar, [FromDate], 101) as FromDate
			string vSql = @"SELECT [ID]
						  ,[TaskName]
						  ,[TaskGroup]
						  ,FORMAT ([FromDate], 'dd/MM/yyyy') as FromDate
						  ,FORMAT ([ToDate], 'dd/MM/yyyy') as ToDate
						  ,[ExeTime]
						  ,[EndTime]
						  ,[LogDate]
						  ,[SourceRows]
						  ,[ExtractedRows]
						  ,[ExtractionStatus]
						  ,[FileTransferStatus]
						  ,[ExceptionThrown]
						  ,[FilePath] 
					FROM [WebApp].[TaskProcessLog] 
					where  exetime>DATEADD(DAY, -90, getdate()) and ExtractionStatus=0 ORDER BY [ExeTime] desc";
			//string vSql = "SELECT ID,TaskName,TaskGroup,TriggerName,TriggerGroup,secondPart,minutePart,hourPart,dayOfMonthPart,monthPart,dayOfWeekPart,CONVERT(varchar, fromdate, 101) as fromdate,CONVERT(varchar, todate, 101) as todate,email,CronExpression FROM [WebApp].[TaskScheduler]";
			SqlDataAdapter adap = new SqlDataAdapter(vSql, cnn);
			adap.Fill(vRet);
			adap.Dispose();
			cnn.Close();
			cnn.Dispose();
			return vRet;
		}

        public static DataTable GetJobsHistory()
        {
            SqlConnection cnn = new SqlConnection(SharedUtils.GetDSN());
            cnn.Open();
            DataTable vRet = new DataTable();

            string vSql = @"SELECT [ID]
						  ,[TaskName]
						  ,[TaskGroup]
						  ,FORMAT ([FromDate], 'dd/MM/yyyy') as FromDate
						  ,FORMAT ([ToDate], 'dd/MM/yyyy') as ToDate
						  ,[ExeTime]
						  ,[EndTime]
						  ,[LogDate]
						  ,[SourceRows]
						  ,[ExtractedRows]
						  ,[ExtractionStatus]
						  ,[FileTransferStatus]
						  ,[ExceptionThrown]
						  ,[FilePath] 
					FROM [WebApp].[TaskProcessLog] 
					where  exetime>DATEADD(DAY, -90, getdate())";

            //Yesterday date
            //FORMAT (( DATEADD(day, -1, CAST(GETDATE() AS date))),  'dd/MM/yyyy ', 'en-us')
            SqlDataAdapter adap = new SqlDataAdapter(vSql, cnn);
            adap.Fill(vRet);
            adap.Dispose();
            cnn.Close();
            cnn.Dispose();
            return vRet;
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

			vSql += " * from  [WebApp].[ManualExe] ";
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

		public static DataTable GetListOfManual()
		{
			SqlConnection cnn = new SqlConnection(SharedUtils.GetDSN());
			cnn.Open();
			DataTable vRet = new DataTable();
			string vSql = "SELECT a.*,b.RoleName FROM [WebApp].[ManualExe] a left join [WebApp].[ManualExe] b on a.Status=b.ID ";
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
			string vSql = "delete from [WebApp].[ManualExe] where ID=@ID";
			SqlCommand com = new SqlCommand(vSql, cnn);
			com.Parameters.AddWithValue("@ID", this.ID);
			var i = com.ExecuteNonQuery();
			Console.WriteLine(i.ToString());
			com.Dispose();
			cnn.Close();
			cnn.Dispose();
		}

		public static string GetRoleByUserName(string username)
		{
			SqlConnection cnn = new SqlConnection(SharedUtils.GetDSN());
			string vReturn = "";
			string vSql = @"select r.RoleMenus 
							from [WebApp].[ManualExe] u 
							left join [WebApp].[UserRole] r on 
							u.Status=r.ID
						  ";
			vSql += " where lower(u.[TaskName])=lower('" + username + "') ;";
			SqlCommand cmd = new SqlCommand(vSql, cnn);
			try
			{
				cnn.Open();
				vReturn = (string)cmd.ExecuteScalar();
				cnn.Close();
				cnn.Dispose();
			}
			catch (Exception)
			{
				vReturn = "";
				//Console.WriteLine(ex.Message);
			}
			return vReturn;
		}

		public static int Delete(string ID)
		{
			int i = 0;
			SqlConnection cnn = new SqlConnection(SharedUtils.GetDSN());
			cnn.Open();
			string vSql = "delete from [WebApp].[ManualExe] where ID=@ID";
			SqlCommand com = new SqlCommand(vSql, cnn);
			com.Parameters.AddWithValue("@ID", ID);
			i = com.ExecuteNonQuery();
			Console.WriteLine(i.ToString());
			com.Dispose();
			cnn.Close();
			cnn.Dispose();
			return i;
		}

		public static string GetID(string condition)
		{
			string res = "";
			SqlConnection cnn = new SqlConnection(SharedUtils.GetDSN());
			cnn.Open();
			string vSql = ("select top 1 ID from [WebApp].[ManualExe] where " + condition);
			SqlCommand com = new SqlCommand(vSql, cnn);
			SqlDataReader reader = com.ExecuteReader();
			if (reader.Read())
			{
				if (!reader.IsDBNull(reader.GetOrdinal("ID")))
				{
					res = (reader["ID"]).ToString();
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

		

	}
}
