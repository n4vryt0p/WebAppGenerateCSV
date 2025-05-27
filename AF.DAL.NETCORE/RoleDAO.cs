using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AF.DAL
{
	public class RoleDAO : IDisposable
	{
		private string cnnstr; private bool IsDisposed = false; private bool IsNew = true;
		public int ID { get; set; }
		public string RoleName { get; set; }
		public string Description { get; set; }
		public string RoleMenus { get; set; }
		public string RoleAccess { get; set; }
		public string DataLevel { get; set; }
		public string sourceSystem { get; set; }
		public string productType { get; set; }
		public int minAmount { get; set; }
		public int maxAmount { get; set; }
		public DateTime CreateTS { get; set; }
		public DateTime UpdateTS { get; set; }

		public RoleDAO() { cnnstr = SharedUtils.GetDSN(); }

		public RoleDAO(int ID)
		{
			cnnstr = SharedUtils.GetDSN();
			SqlConnection cnn = new SqlConnection(cnnstr);
			cnn.Open();
			string vSql = "SELECT * FROM [WebApp].[UserRole] WHERE ID=@ID ";
			SqlCommand com = new SqlCommand(vSql, cnn);
			com.Parameters.AddWithValue("@ID", ID);
			SqlDataReader reader = com.ExecuteReader();
			if (reader.Read())
			{
				IsNew = false;
				if (!reader.IsDBNull(reader.GetOrdinal("ID"))) this.ID = Convert.ToInt32(reader["ID"]);
				if (!reader.IsDBNull(reader.GetOrdinal("RoleName"))) this.RoleName = (string)reader["RoleName"];
				if (!reader.IsDBNull(reader.GetOrdinal("Description"))) this.Description = (string)reader["Description"];
				if (!reader.IsDBNull(reader.GetOrdinal("RoleMenus"))) this.RoleMenus = (string)reader["RoleMenus"];
				if (!reader.IsDBNull(reader.GetOrdinal("RoleAccess"))) this.DataLevel = (string)reader["RoleAccess"];
				if (!reader.IsDBNull(reader.GetOrdinal("DataLevel"))) this.DataLevel = (string)reader["DataLevel"];
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
				com.CommandText = "UPDATE  [WebApp].[UserRole] SET RoleName=@RoleName,RoleMenus=@RoleMenus,Description=@Description,DataLevel=@DataLevel,UpdateTS=getdate() WHERE ID=@ID";
				com.Parameters.AddWithValue("@RoleName", this.RoleName);
				com.Parameters.AddWithValue("@RoleMenus", this.RoleMenus);
				com.Parameters.AddWithValue("@Description", this.Description);
				com.Parameters.AddWithValue("@DataLevel", this.DataLevel);
				com.Parameters.AddWithValue("@ID", this.ID);
			}
			else
			{
				com.CommandText = "INSERT INTO  [WebApp].[UserRole] (RoleName,Description,RoleMenus,DataLevel) VALUES (@RoleName,@Description,@RoleMenus,@DataLevel)";
				com.Parameters.AddWithValue("@RoleName", this.RoleName);
				com.Parameters.AddWithValue("@Description", this.Description);
				com.Parameters.AddWithValue("@RoleMenus", this.RoleMenus);
				com.Parameters.AddWithValue("@DataLevel", this.DataLevel);
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

			vSql += " * from  [WebApp].[UserRole] ";
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

		public static DataTable GetRoles()
		{
			SqlConnection cnn = new SqlConnection(SharedUtils.GetDSN());
			cnn.Open();
			DataTable vRet = new DataTable();
			string vSql = "SELECT [ID],[RoleName] FROM [WebApp].[UserRole]";
			SqlDataAdapter adap = new SqlDataAdapter(vSql, cnn);
			adap.Fill(vRet);
			adap.Dispose();
			cnn.Close();
			cnn.Dispose();
			return vRet;
		}

		public static DataTable GetDataRoles()
		{
			SqlConnection cnn = new SqlConnection(SharedUtils.GetDSN());
			cnn.Open();
			DataTable vRet = new DataTable();
			string vSql = "SELECT [ID],[RoleName],[Description],[RoleMenus],[DataLevel] FROM [WebApp].[UserRole]";
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
			string vSql = "delete from [WebApp].[UserRole] where ID=@ID";
			SqlCommand com = new SqlCommand(vSql, cnn);
			com.Parameters.AddWithValue("@ID", this.ID);
			var i = com.ExecuteNonQuery();
			Console.WriteLine(i.ToString());
			com.Dispose();
			cnn.Close();
			cnn.Dispose();
		}

		public static int Delete(string roleID)
		{
			int i = 0;
			SqlConnection cnn = new SqlConnection(SharedUtils.GetDSN());
			cnn.Open();
			string vSql = "delete from [WebApp].[UserRole] where ID=@ID";
			SqlCommand com = new SqlCommand(vSql, cnn);
			com.Parameters.AddWithValue("@ID", roleID);
			try {
				i = com.ExecuteNonQuery();
			} catch {
				i = 0;
			}
			Console.WriteLine(i.ToString());
			com.Dispose();
			cnn.Close();
			cnn.Dispose();
			return i;
		}

		public void Delete(int ID)
		{
			SqlConnection cnn = new SqlConnection(SharedUtils.GetDSN());
			cnn.Open();
			string vSql = "delete from [WebApp].[UserRole] where ID=@ID";
			SqlCommand com = new SqlCommand(vSql, cnn);
			com.Parameters.AddWithValue("@ID", ID);
			var i = com.ExecuteNonQuery();
			Console.WriteLine(i.ToString());
			com.Dispose();
			cnn.Close();
			cnn.Dispose();
		}

		public static int GetID(string condition)
		{
			int res = 0;
			SqlConnection cnn = new SqlConnection(SharedUtils.GetDSN());
			cnn.Open();
			string vSql = ("select top 1 ID from [WebApp].[UserRole] where " + condition);
			SqlCommand com = new SqlCommand(vSql, cnn);
			SqlDataReader reader = com.ExecuteReader();
			if (reader.Read())
			{
				if (!reader.IsDBNull(reader.GetOrdinal("ID")))
				{
					res = Convert.ToInt16(reader["ID"]);
				}
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
