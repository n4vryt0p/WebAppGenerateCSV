using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace AF.DAL
{
	public class MenuDAO : IDisposable
	{
		private string cnnstr; private bool IsDisposed = false; private bool IsNew = true;
		public string ID { get; set; }
		public string MenuName { get; set; }
		public string OutPutText { get; set; }
		public string IdParent { get; set; }
		public string Seq { get; set; }
		public string Url { get; set; }
		public string Icon { get; set; }
		public DateTime CreateTS { get; set; }
		public DateTime UpdateTS { get; set; }

		public MenuDAO() { cnnstr = SharedUtils.GetDSN(); }

		public MenuDAO(string ID)
		{
			cnnstr = SharedUtils.GetDSN();
			SqlConnection cnn = new SqlConnection(cnnstr);
			cnn.Open();
			string vSql = "SELECT * FROM [WebApp].[Menu] WHERE ID=@ID ";
			SqlCommand com = new SqlCommand(vSql, cnn);
			com.Parameters.AddWithValue("@ID", ID);
			SqlDataReader reader = com.ExecuteReader();
			if (reader.Read())
			{
				IsNew = false;
				if (!reader.IsDBNull(reader.GetOrdinal("ID"))) this.ID = (string)reader["ID"];
				if (!reader.IsDBNull(reader.GetOrdinal("OutPutText"))) this.OutPutText = (string)reader["OutPutText"];
				if (!reader.IsDBNull(reader.GetOrdinal("MenuName"))) this.MenuName = (string)reader["MenuName"];
				if (!reader.IsDBNull(reader.GetOrdinal("IdParent"))) this.IdParent = reader["IdParent"].ToString();
				if (!reader.IsDBNull(reader.GetOrdinal("Seq"))) this.Seq = (string)reader["Seq"];
				if (!reader.IsDBNull(reader.GetOrdinal("Url"))) this.Url = (string)reader["Url"];
				if (!reader.IsDBNull(reader.GetOrdinal("Icon"))) this.Icon = (string)reader["Icon"];
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
				com.CommandText = "UPDATE [WebApp].[Menu] SET MenuName=@MenuName,IdParent=@IdParent,OutPutText=@OutPutText,Seq=@Seq,Url=@Url,Icon=@Icon,UpdateTS=getdate() WHERE ID=@ID";
				com.Parameters.AddWithValue("@MenuName", this.MenuName);
				com.Parameters.AddWithValue("@IdParent", this.IdParent);
				com.Parameters.AddWithValue("@OutPutText", this.OutPutText);
				com.Parameters.AddWithValue("@IdParent", this.IdParent);
				com.Parameters.AddWithValue("@Seq", this.Seq);
				com.Parameters.AddWithValue("@Url", this.Url);
				com.Parameters.AddWithValue("@Icon", this.Icon);
				com.Parameters.AddWithValue("@ID", this.ID);
			}
			else
			{
				com.CommandText = "INSERT INTO [WebApp].[Menu] (ID,MenuName,OutPutText,IdParent,Seq,Url,Icon,CreateTS) VALUES (@ID,@MenuName,@OutPutText,@IdParent,@Seq,@Url,@Icon,getdate())";
				com.Parameters.AddWithValue("@ID", this.ID);
				com.Parameters.AddWithValue("@MenuName", this.MenuName);
				com.Parameters.AddWithValue("@OutPutText", this.OutPutText);
				com.Parameters.AddWithValue("@IdParent", this.IdParent);
				com.Parameters.AddWithValue("@Seq", this.Seq);
				com.Parameters.AddWithValue("@Url", this.Url);
				com.Parameters.AddWithValue("@Icon", this.Icon);
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

			vSql += " * from  [WebApp].[Menu] ";
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

		public static DataTable GetMenus()
		{
			SqlConnection cnn = new SqlConnection(SharedUtils.GetDSN());
			cnn.Open();
			DataTable vRet = new DataTable();
			string vSql = "SELECT * FROM [WebApp].[Menu] order by IdParent ";
			SqlDataAdapter adap = new SqlDataAdapter(vSql, cnn);
			adap.Fill(vRet);
			adap.Dispose();
			cnn.Close();
			cnn.Dispose();
			return vRet;
		}

		public static DataTable GetMenusForCB()
		{
			SqlConnection cnn = new SqlConnection(SharedUtils.GetDSN());
			cnn.Open();
			DataTable vRet = new DataTable();
			string vSql = "SELECT ID,MenuName FROM [WebApp].[Menu] where IdParent<>0 order by IdParent ";
			SqlDataAdapter adap = new SqlDataAdapter(vSql, cnn);
			adap.Fill(vRet);
			adap.Dispose();
			cnn.Close();
			cnn.Dispose();
			return vRet;
		}

		public static DataTable GetMenusByRole(string roles)
		{
			SqlConnection cnn = new SqlConnection(SharedUtils.GetDSN());
			cnn.Open();
			DataTable vRet = new DataTable();
			string vSql = @"SELECT 
							   m.[MenuName] menuDispDerive
							  ,m.[OutPutText] htmlDerive
							  ,n.[MenuName] menuDispBase
							  ,n.[OutPutText] htmlBase
						  FROM [WebApp].[Menu] m
						  inner join WebApp.Menu n on n.ID =m.IdParent
						  ";
			vSql += " where m.ID in (" + roles +")";
			vSql += " order by m.Seq ";
			SqlDataAdapter adap = new SqlDataAdapter(vSql, cnn);
			//adap.SelectCommand.Parameters.AddWithValue("@prm", StringToIntList( roles ));
			adap.Fill(vRet);
			adap.Dispose();
			cnn.Close();
			cnn.Dispose();
			return vRet;
		}
        public static DataTable GetMenGETyRole(string roles)
        {
            SqlConnection cnn = new SqlConnection(SharedUtils.GetDSN());
            cnn.Open();
            DataTable vRet = new DataTable();
            string vSql = @"SELECT *
							 FROM WebApp.MenuRole
						   where ID in (" + roles + ")";
            SqlDataAdapter adap = new SqlDataAdapter(vSql, cnn);
            //adap.SelectCommand.Parameters.AddWithValue("@prm", StringToIntList( roles ));
            adap.Fill(vRet);
            adap.Dispose();
            cnn.Close();
            cnn.Dispose();
            return vRet;
        }
        public static IEnumerable<int> StringToIntList(string str)
		{
			if (String.IsNullOrEmpty(str))
				yield break;

			foreach (var s in str.Split(','))
			{
				int num;
				if (int.TryParse(s, out num))
					yield return num;
			}
		}

		public static string BuildMenuByRole(string roles)
		{
			string vRet = "";
			string curBase = "";
			//string openTag = "&lt;ul class=\"treeview - menu\"&gt;";
			string CloseTag = "&lt;/ul&gt;&lt;/li&gt;";
			DataTable dt = MenuDAO.GetMenusByRole(roles);
			foreach (DataRow dr in dt.Rows)
			{
				string md = (string)dr["menuDispDerive"];
				string hd = (string)dr["htmlDerive"];
				string mb = (string)dr["menuDispBase"];
				string hb = (string)dr["htmlBase"];
				//Console.WriteLine("menuDispDerive: " + md + " - htmlDerive: " + hd + "- menuDispBase : " + mb + "- htmlBase : " + hb);
				if (mb != curBase)
				{
					if (vRet == "")
					{
						vRet += hb;
						vRet += hd;
						curBase = mb;
					}
					else
					{
						vRet += CloseTag;
						vRet += hb;
						vRet += hd;
						curBase = mb;
						Console.WriteLine(vRet);
					}
				}
				else
				{
					vRet += hd;
					curBase = mb;
				}
			}
			vRet += CloseTag;

			return vRet;

		}
        

        public static string BuildMenu(string role)
		{
			string vRet = "";
			string curBase = "";
			//string openTag = "&lt;ul class=\"treeview - menu\"&gt;";
			string CloseTag = "&lt;/ul&gt;&lt;/li&gt;";
           
            DataTable dt = MenuDAO.GetMenGETyRole(role);
			foreach (DataRow dr in dt.Rows)
			{
				string md = (string)dr["ModuleName"];
				string hd = (string)dr["ActionLink"];
				string mb = (string)dr["FuncName"];
				string hb = (string)dr["ModuleLink"];
				//Console.WriteLine("menuDispDerive: " + md + " - htmlDerive: " + hd + "- menuDispBase : " + mb + "- htmlBase : " + hb);
				if (mb != curBase)
				{
					if (vRet == "")
					{
						vRet += hb;
						vRet += hd;
						curBase = mb;
					}
					else
					{
						vRet += CloseTag;
						vRet += hb;
						vRet += hd;
						curBase = mb;
						Console.WriteLine(vRet);
					}
				}
				else
				{
					vRet += hd;
					curBase = mb;
				}
			}
			vRet += CloseTag;

			return vRet;
		}


		public void Delete()
		{
			SqlConnection cnn = new SqlConnection(cnnstr);
			cnn.Open();
			string vSql = "delete from [WebApp].[Menu] where ID=@ID";
			SqlCommand com = new SqlCommand(vSql, cnn);
			com.Parameters.AddWithValue("@ID", this.ID);
			var i = com.ExecuteNonQuery();
			Console.WriteLine(i.ToString());
			com.Dispose();
			cnn.Close();
			cnn.Dispose();
		}

		public static int Delete(string ID)
		{
			int i = 0;
			SqlConnection cnn = new SqlConnection(SharedUtils.GetDSN());
			cnn.Open();
			string vSql = "delete from [WebApp].[Menu] where ID=@ID";
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
			string vSql = ("select top 1 ID from [WebApp].[Menu] where " + condition);
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
