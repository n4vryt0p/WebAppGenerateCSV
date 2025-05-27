using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF.DAL
{
    class FileMonitoring
    {
		public string requestID {get; set;}
		public string txnLogID {get; set;}
		public string clientID {get; set;}
		public string clientIP {get; set;}
		public string nodeIP {get; set;}
		public string contentType {get; set;}
		public string urlPath {get; set;}
		public string trxRq_DT {get; set;}
		public int fileSizeRq {get; set;}
		public string fileRq {get; set;}
		public string generateDT {get; set;}
		public string triggerDT {get; set;}
		public string consumeDT {get; set;}
		public string responseDT {get; set;}
		public int fileSizeRs {get; set;}
		public string fileRs {get; set;}
		public string statusCode {get; set;}
		public string isInbound { get; set; }

		public FileMonitoring(string clientId)
        {
			this.requestID = this.txnLogID = SharedUtils.CreateMD5(DateTime.Now.Ticks.ToString());
			this.clientID = clientId;

			this.clientIP = "";
			this.nodeIP = "";
			this.statusCode = "";

			this.generateDT = this.trxRq_DT = DateTime.Now.ToString("yyyy/MM/dd H:m:s");
			this.consumeDT = this.triggerDT = DateTime.Now.ToString("yyyy/MM/dd H:m:s");
		}

		public static void LogIt(FileMonitoring fm)
		{
			string vsql = "";
			vsql += "INSERT INTO [LIL].[FileMonitoring] ";
			vsql += "(requestID,txnLogID,clientID,clientIP,nodeIP,contentType,urlPath,trxRq_DT,fileSizeRq,fileRq,generateDT,";
			vsql += "triggerDT,consumeDT,responseDT,fileSizeRs,fileRs,statusCode,isInbound) ";
			vsql += " VALUES (@requestID,@txnLogID,@clientID,@clientIP,@nodeIP,@contentType,@urlPath,@trxRq_DT,@fileSizeRq,@fileRq,@generateDT,@triggerDT,@consumeDT,@responseDT,@fileSizeRs,@fileRs,@statusCode,@isInbound) ";
			using (SqlConnection con = new SqlConnection(SharedUtils.GetDSN()))
			{
				using (SqlCommand cmd = new SqlCommand(vsql, con))
				{
					cmd.Parameters.AddWithValue("@requestID", fm.requestID);
					cmd.Parameters.AddWithValue("@txnLogID", fm.txnLogID);
					cmd.Parameters.AddWithValue("@clientID", fm.clientID);
					cmd.Parameters.AddWithValue("@clientIP", fm.clientIP);
					cmd.Parameters.AddWithValue("@nodeIP", fm.nodeIP);
					cmd.Parameters.AddWithValue("@contentType", fm.contentType);
					cmd.Parameters.AddWithValue("@urlPath", fm.urlPath);
					cmd.Parameters.AddWithValue("@trxRq_DT", fm.trxRq_DT);
					cmd.Parameters.AddWithValue("@fileSizeRq", fm.fileSizeRq);
					cmd.Parameters.AddWithValue("@fileRq", fm.fileRq);
					cmd.Parameters.AddWithValue("@generateDT", fm.generateDT);
					cmd.Parameters.AddWithValue("@triggerDT", fm.triggerDT);
					cmd.Parameters.AddWithValue("@consumeDT", fm.consumeDT);
					cmd.Parameters.AddWithValue("@responseDT", fm.responseDT);
					cmd.Parameters.AddWithValue("@fileSizeRs", fm.fileSizeRs);
					cmd.Parameters.AddWithValue("@fileRs", fm.fileRs);
					cmd.Parameters.AddWithValue("@statusCode", fm.statusCode);
					cmd.Parameters.AddWithValue("@isInbound", 0);


					con.Open();
					try { cmd.ExecuteNonQuery(); }
					catch (Exception e)
					{
						throw e;
					}

					//Console.WriteLine("");
					con.Close();
				}

			}

		}
	}
}
