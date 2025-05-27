using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace AF.DAL
{
	public class TaskSchedulerDAO : IDisposable
	{
		private string cnnstr; private bool IsDisposed = false; private bool IsNew = true;
		public string ID { get; set; }
		public string TaskName { get; set; }
		public string TaskGroup { get; set; }
		public string TriggerName { get; set; }
		public string TriggerGroup { get; set; }
		public string Email { get; set; }
		public string CronExpression { get; set; }
		public DateTime FromDate { get; set; }
		public DateTime ToDate { get; set; }
		public string secondPart { get; set; }
		public string minutePart { get; set; }
		public string hourPart { get; set; }
		public string dayOfMonthPart { get; set; }
		public string monthPart { get; set; }
		public string dayOfWeekPart { get; set; }
		public DateTime CreateTS { get; set; }
		public DateTime UpdateTS { get; set; }

		public TaskSchedulerDAO() { cnnstr = SharedUtils.GetDSN(); }

		public TaskSchedulerDAO(int ID)
		{
			cnnstr = SharedUtils.GetDSN();
			SqlConnection cnn = new SqlConnection(cnnstr);
			cnn.Open();
			string vSql = "SELECT * FROM [WebApp].[TaskScheduler] WHERE ID=@ID ";
			SqlCommand com = new SqlCommand(vSql, cnn);
			com.Parameters.AddWithValue("@ID", ID);
			SqlDataReader reader = com.ExecuteReader();
			if (reader.Read())
			{
				IsNew = false;
				if (!reader.IsDBNull(reader.GetOrdinal("ID"))) this.ID = reader["ID"].ToString();
				if (!reader.IsDBNull(reader.GetOrdinal("TaskName"))) this.TaskName = (string)reader["TaskName"];
				if (!reader.IsDBNull(reader.GetOrdinal("TaskGroup"))) this.TaskGroup = (string)reader["TaskGroup"];
				if (!reader.IsDBNull(reader.GetOrdinal("TriggerName"))) this.TriggerName = (string)reader["TriggerName"];
				if (!reader.IsDBNull(reader.GetOrdinal("TriggerGroup"))) this.TriggerGroup = (string)reader["TriggerGroup"];
				if (!reader.IsDBNull(reader.GetOrdinal("secondPart"))) this.secondPart = (string)reader["secondPart"];
				if (!reader.IsDBNull(reader.GetOrdinal("minutePart"))) this.minutePart = (string)reader["minutePart"];
				if (!reader.IsDBNull(reader.GetOrdinal("hourPart"))) this.hourPart = (string)reader["hourPart"];
				if (!reader.IsDBNull(reader.GetOrdinal("dayOfMonthPart"))) this.dayOfMonthPart = (string)reader["dayOfMonthPart"];
				if (!reader.IsDBNull(reader.GetOrdinal("monthPart"))) this.monthPart = (string)reader["monthPart"];
				if (!reader.IsDBNull(reader.GetOrdinal("dayOfWeekPart"))) this.dayOfWeekPart = (string)reader["dayOfWeekPart"];
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
				com.CommandText = "UPDATE  [WebApp].[TaskScheduler] SET TaskName=@TaskName,secondPart=@secondPart,minutePart=@minutePart,hourPart=@hourPart,dayOfMonthPart=@dayOfMonthPart,monthPart=@monthPart,dayOfWeekPart=@dayOfWeekPart,UpdateTS=getdate() WHERE ID=@ID";
				com.Parameters.AddWithValue("@TaskName", this.TaskName);
				com.Parameters.AddWithValue("@secondPart", this.secondPart);
				com.Parameters.AddWithValue("@minutePart", this.minutePart);
				com.Parameters.AddWithValue("@hourPart", this.hourPart);
				com.Parameters.AddWithValue("@dayOfMonthPart", this.dayOfMonthPart);
				com.Parameters.AddWithValue("@monthPart", this.monthPart);
				com.Parameters.AddWithValue("@dayOfWeekPart", this.dayOfWeekPart);
				com.Parameters.AddWithValue("@FromDate", this.FromDate);
				com.Parameters.AddWithValue("@ToDate", this.ToDate);
				com.Parameters.AddWithValue("@ID", this.ID);
			}
			else
			{
				com.CommandText = "INSERT INTO  [WebApp].[TaskScheduler] (TaskName,secondPart,minutePart,hourPart,dayOfMonthPart,monthPart,dayOfWeekPart) VALUES (@TaskName,@secondPart,@minutePart,@hourPart,@dayOfMonthPart,@monthPart,@dayOfWeekPart)";
				com.Parameters.AddWithValue("@TaskName", this.TaskName);
				com.Parameters.AddWithValue("@secondPart", this.secondPart);
				com.Parameters.AddWithValue("@minutePart", this.minutePart);
				com.Parameters.AddWithValue("@hourPart", this.hourPart);
				com.Parameters.AddWithValue("@dayOfMonthPart", this.dayOfMonthPart);
				com.Parameters.AddWithValue("@monthPart", this.monthPart);
				com.Parameters.AddWithValue("@dayOfWeekPart", this.dayOfWeekPart);
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

			vSql += " * from  [WebApp].[TaskScheduler] ";
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

		public int UpdateTask()
		{
			int vRet = 0;
			SqlConnection cnn = new SqlConnection(cnnstr);
			cnn.Open();
			SqlCommand com = new SqlCommand();
			string strSql = " UPDATE  [WebApp].[TaskScheduler] SET ";
			strSql += " hourPart=@hourPart,minutePart=@minutePart,Email=@Email,CronExpression=@CronExpression,FromDate=@FromDate,ToDate=@ToDate, ";
			strSql += " UpdateTS=getdate()  ";
			strSql += " WHERE ID=@ID ";
			com.Connection = cnn;
			com.CommandText = strSql;
			com.Parameters.AddWithValue("@hourPart", this.hourPart);
			com.Parameters.AddWithValue("@minutePart", this.minutePart);
			com.Parameters.AddWithValue("@Email", this.Email);
			com.Parameters.AddWithValue("@CronExpression", this.CronExpression);
			com.Parameters.AddWithValue("@FromDate", this.FromDate);
			com.Parameters.AddWithValue("@ToDate", this.ToDate);
			com.Parameters.AddWithValue("@ID", this.ID);
			try {
				vRet = com.ExecuteNonQuery();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			com.Dispose();
			cnn.Close();
			cnn.Dispose();
			return vRet;
		}


		public static DataTable GetTasks()
		{
			SqlConnection cnn = new SqlConnection(SharedUtils.GetDSN());
			cnn.Open();
			DataTable vRet = new DataTable();
			string vSql = "SELECT ID,TaskName,TaskGroup,TriggerName,TriggerGroup,secondPart,minutePart,hourPart,dayOfMonthPart,monthPart,dayOfWeekPart,CONVERT(varchar, fromdate, 101) as fromdate,CONVERT(varchar, todate, 101) as todate,email,CronExpression FROM [WebApp].[TaskScheduler]";
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
			string vSql = "delete from [WebApp].[TaskScheduler] where ID=@ID";
			SqlCommand com = new SqlCommand(vSql, cnn);
			com.Parameters.AddWithValue("@ID", this.ID);
			var i = com.ExecuteNonQuery();
			Console.WriteLine(i.ToString());
			com.Dispose();
			cnn.Close();
			cnn.Dispose();
		}

		public void Delete(int ID)
		{
			SqlConnection cnn = new SqlConnection(SharedUtils.GetDSN());
			cnn.Open();
			string vSql = "delete from [WebApp].[TaskScheduler] where ID=@ID";
			SqlCommand com = new SqlCommand(vSql, cnn);
			com.Parameters.AddWithValue("@ID", ID);
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
			string vSql = ("select top 1 ID from [WebApp].[TaskScheduler] where " + condition);
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

		public static string GetEmailManualAddByID(string ID)
		{
			SqlConnection cnn = new SqlConnection(SharedUtils.GetDSN());
			string vReturn = "";
			string vSql = @"select [Email] from [WebApp].[ManualExe]  ";
			vSql += " where ID=" + ID + ";";
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

		public static string GetEmailAddByID(string ID)
		{
			SqlConnection cnn = new SqlConnection(SharedUtils.GetDSN());
			string vReturn = "";
			string vSql = @"select [Email] from [WebApp].[TaskScheduler]  ";
			vSql += " where ID=" + ID + ";";
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
