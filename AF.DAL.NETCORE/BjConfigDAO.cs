using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace AF.DAL
{
	public class BjConfigDAO : IDisposable
	{
		private string cnnstr; private bool IsDisposed = false; private bool IsNew = true;
		public int ID { get; set; }
		public DateTime FromDate { get; set; }
		public DateTime ToDate { get; set; }
		public string Iteration { get; set; }
		public string ExecutionTime { get; set; }

		public BjConfigDAO() { cnnstr = SharedUtils.GetDSN(); }

		public BjConfigDAO(int ID)
		{
			cnnstr = SharedUtils.GetDSN();
			SqlConnection cnn = new SqlConnection(cnnstr);
			cnn.Open();
			string vSql = "SELECT * FROM [WebApp].[ConfigBackGroundJob] WHERE ID=@ID ";
			SqlCommand com = new SqlCommand(vSql, cnn);
			com.Parameters.AddWithValue("@ID", ID);
			SqlDataReader reader = com.ExecuteReader();
			if (reader.Read())
			{
				IsNew = false;
				if (!reader.IsDBNull(reader.GetOrdinal("ID"))) this.ID = (int)reader["ID"];
				if (!reader.IsDBNull(reader.GetOrdinal("FromDate"))) this.FromDate = (DateTime)reader["FromDate"];
				if (!reader.IsDBNull(reader.GetOrdinal("ToDate"))) this.ToDate = (DateTime)reader["ToDate"];
				if (!reader.IsDBNull(reader.GetOrdinal("Iteration"))) this.Iteration = (string)reader["Iteration"];
				if (!reader.IsDBNull(reader.GetOrdinal("ExecutionTime"))) this.ExecutionTime = (string)reader["ExecutionTime"];
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
				com.CommandText = "UPDATE [WebApp].[ConfigBackGroundJob] SET FromDate=@FromDate,Iteration=@Iteration,ExecutionTime=@ExecutionTime,ToDate=@ToDate WHERE ID=@ID";
				com.Parameters.AddWithValue("@FromDate", this.FromDate);
				com.Parameters.AddWithValue("@Iteration", this.Iteration);
				com.Parameters.AddWithValue("@ExecutionTime", this.ExecutionTime);
				com.Parameters.AddWithValue("@ToDate", this.ToDate);
				com.Parameters.AddWithValue("@ID", this.ID);
			}
			else
			{
				com.CommandText = "INSERT INTO [WebApp].[ConfigBackGroundJob] (ID,FromDate,ToDate,Iteration,ExecutionTime) VALUES (@ID,@FromDate,@ToDate,@Iteration,@ExecutionTime)";
				com.Parameters.AddWithValue("@ID", this.ID);
				com.Parameters.AddWithValue("@FromDate", this.FromDate);
				com.Parameters.AddWithValue("@ToDate", this.ToDate);
				com.Parameters.AddWithValue("@Iteration", this.Iteration);
				com.Parameters.AddWithValue("@ExecutionTime", this.ExecutionTime);
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

			vSql += " * from  [WebApp].[ConfigBackGroundJob] ";
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


		public void Delete()
		{
			SqlConnection cnn = new SqlConnection(cnnstr);
			cnn.Open();
			string vSql = "delete from [WebApp].[ConfigBackGroundJob] where ID=@ID";
			SqlCommand com = new SqlCommand(vSql, cnn);
			com.Parameters.AddWithValue("@ID", this.ID);
			var i = com.ExecuteNonQuery();
			Console.WriteLine(i.ToString());
			com.Dispose();
			cnn.Close();
			cnn.Dispose();
		}

		public void Delete(string ID)
		{
			SqlConnection cnn = new SqlConnection(SharedUtils.GetDSN());
			cnn.Open();
			string vSql = "delete from [WebApp].[ConfigBackGroundJob] where ID=@ID";
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
			string vSql = ("select top 1 ID from [WebApp].[ConfigBackGroundJob] where " + condition);
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
