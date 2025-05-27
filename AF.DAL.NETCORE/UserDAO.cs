using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace AF.DAL
{
	public class UserDAO : IDisposable
	{
		private string cnnstr; private bool IsDisposed = false; private bool IsNew = true;
		public string UserID { get; set; }
		public string UserText { get; set; }
		public string UserPass { get; set; }
		public string RoleID { get; set; }
		public string RoleName { get; set; }
		public string RoleMenus { get; set; }
		public string RoleCreate { get; set; }
		public string RoleUpdate { get; set; }
		public string RoleDelete { get; set; }
		public string RoleAccess { get; set; }
		public string DataLevel { get; set; }
		public string SourceSystem { get; set; }
		public string ProductType { get; set; }
		public double MinAmount { get; set; }
		public double MaxAmount { get; set; }
		public DateTime CreateTS { get; set; }
		public DateTime UpdateTS { get; set; }

		public UserDAO() { cnnstr = SharedUtils.GetDSN(); }

		public UserDAO(string USERID)
		{
			cnnstr = SharedUtils.GetDSN();
			SqlConnection cnn = new SqlConnection(cnnstr);
			cnn.Open();
			string vSql = "SELECT UserID,UserPass,UserText,RoleID,[WebApp].[User].CreateTS,[WebApp].[User].UpdateTS " +
							",RoleMenus,RoleCreate,RoleUpdate,RoleDelete,RoleName,RoleAccess,DataLevel,SourceSystem,ProductType,MinAmount,MaxAmount " +
							"FROM [WebApp].[User], [WebApp].[UserRole] WHERE UserID=@UserID AND [WebApp].[User].RoleID = [WebApp].[UserRole].ID";
			
			SqlCommand com = new SqlCommand(vSql, cnn);
			com.Parameters.AddWithValue("@UserID", USERID);
			SqlDataReader reader = com.ExecuteReader();
			if (reader.Read())
			{
				IsNew = false;
				if (!reader.IsDBNull(reader.GetOrdinal("UserID"))) this.UserID = (string)reader["UserID"];
				if (!reader.IsDBNull(reader.GetOrdinal("UserPass"))) this.UserPass = (string)reader["UserPass"];
				if (!reader.IsDBNull(reader.GetOrdinal("UserText"))) this.UserText = (string)reader["UserText"];
				if (!reader.IsDBNull(reader.GetOrdinal("RoleID"))) this.RoleID = reader["RoleID"].ToString();
				if (!reader.IsDBNull(reader.GetOrdinal("CreateTS"))) this.CreateTS = (DateTime)reader["CreateTS"];
				if (!reader.IsDBNull(reader.GetOrdinal("UpdateTS"))) this.UpdateTS = (DateTime)reader["UpdateTS"];

				if (!reader.IsDBNull(reader.GetOrdinal("RoleName"))) this.RoleName = (string)reader["RoleName"];
				if (!reader.IsDBNull(reader.GetOrdinal("RoleMenus"))) this.RoleMenus = (string)reader["RoleMenus"];

				if (!reader.IsDBNull(reader.GetOrdinal("RoleCreate"))) this.RoleCreate = (string)reader["RoleCreate"];
				if (!reader.IsDBNull(reader.GetOrdinal("RoleUpdate"))) this.RoleUpdate = (string)reader["RoleUpdate"];
				if (!reader.IsDBNull(reader.GetOrdinal("RoleDelete"))) this.RoleDelete = (string)reader["RoleDelete"];

				if (!reader.IsDBNull(reader.GetOrdinal("RoleAccess"))) this.RoleAccess = (string)reader["RoleAccess"];
				if (!reader.IsDBNull(reader.GetOrdinal("DataLevel"))) this.DataLevel = (string)reader["DataLevel"];

				if (!reader.IsDBNull(reader.GetOrdinal("SourceSystem"))) this.SourceSystem = (string)reader["SourceSystem"];
				if (!reader.IsDBNull(reader.GetOrdinal("ProductType"))) this.ProductType = reader["ProductType"].ToString();

				if (!reader.IsDBNull(reader.GetOrdinal("MinAmount"))) this.MinAmount = (double)reader["MinAmount"];
				if (!reader.IsDBNull(reader.GetOrdinal("MaxAmount"))) this.MaxAmount = (double)reader["MaxAmount"];
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
				com.CommandText = "UPDATE  [WebApp].[User] SET UserText=@UserText,RoleID=@RoleID,UserPass=@UserPass,UpdateTS=getdate() WHERE UserID=@UserID";
				com.Parameters.AddWithValue("@UserText", this.UserText);
				com.Parameters.AddWithValue("@RoleID", this.RoleID);
				com.Parameters.AddWithValue("@UserPass", this.UserPass);
				com.Parameters.AddWithValue("@UserID", this.UserID);
			}else {
				com.CommandText = "INSERT INTO  [WebApp].[User] (UserID,UserText,UserPass,RoleID,CreateTS) VALUES (@UserID,@UserText,@UserPass,@RoleID,getdate())";
				com.Parameters.AddWithValue("@UserID", this.UserID);
				com.Parameters.AddWithValue("@UserText", this.UserText);
				com.Parameters.AddWithValue("@UserPass", this.UserPass);
				com.Parameters.AddWithValue("@RoleID", this.RoleID);
			}
			try { com.ExecuteNonQuery(); }
			catch (Exception e) {
				Console.WriteLine(e.ToString()); }
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

			vSql += " * from  [WebApp].[User] ";
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

		public static DataTable GetUsers()
		{
			SqlConnection cnn = new SqlConnection(SharedUtils.GetDSN());
			cnn.Open();
			DataTable vRet = new DataTable();
			string vSql = "SELECT a.*,b.RoleName FROM [WebApp].[User] a left join [WebApp].[UserRole] b on a.RoleID=b.ID ORDER BY a.UpdateTS DESC";
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
			string vSql = "delete from [WebApp].[User] where UserID=@UserID";
			SqlCommand com = new SqlCommand(vSql, cnn);
			com.Parameters.AddWithValue("@UserID", this.UserID);
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
							from [WebApp].[User] u 
							left join [WebApp].[UserRole] r on 
							u.RoleID=r.ID
						  ";
			vSql += " where lower(u.[UserText])=lower('" + username + "') ;" ;
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

		public static int  Delete(string userid)
		{
			int i = 0;
			SqlConnection cnn = new SqlConnection(SharedUtils.GetDSN());
			cnn.Open();
			string vSql = "delete from [WebApp].[User] where UserID=@UserID";
			SqlCommand com = new SqlCommand(vSql, cnn);
			com.Parameters.AddWithValue("@UserID", userid);
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
			string vSql = ("select top 1 UserID from [WebApp].[User] where " + condition );
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
