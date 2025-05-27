using Microsoft.Data.SqlClient;
using System.Data;

namespace AF.DAL
{
	class TaskProcessLogDAO
	{
	}
	public class ExtractionLog2
	{
		public string ID { get; set; }
		public string TaskName { get; set; }
		public string TaskGroup { get; set; }
		public Nullable<DateTime> FromDate { get; set; }
		public Nullable<DateTime> ToDate { get; set; }
		public Nullable<DateTime> ExeTime { get; set; }
		public Nullable<DateTime> EndTime { get; set; }
		public Nullable<DateTime> LogDate { get; set; }
		public string SourceRows { get; set; }
		public string ExtractedRows { get; set; }
		public Boolean ExtractionStatus { get; set; }
		public Boolean FileTransferStatus { get; set; }
		public string ExceptionThrown { get; set; }
		public string FilePath { get; set; }

		public static List<ExtractionLog2> GetAllLog()
		{
			List<ExtractionLog2> lgs = new List<ExtractionLog2>();
			int rCntr = 0;
			var sqlcom = "select * from [WebApp].[TaskProcessLog] ;";
			using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(sqlcom, SharedUtils.GetDSN()))
			{
				while (results.Read())
				{
					rCntr++;
					ExtractionLog2 lg = new ExtractionLog2();
					lg.ID = results["ID"].ToString();
					lg.TaskName = (!results.IsDBNull(results.GetOrdinal("TaskName"))) ? results["TaskName"].ToString() : "";
					lg.ExeTime = Convert.ToDateTime(results["ExeTime"]);
					//lg.ExeTime = (!results.IsDBNull(results.GetOrdinal("ExeTime"))) ? DateTime.TryParseExact(results["ExeTime"]).ToString("dd/MM/yyyy H:mm:ss") : "00/00/0000 00:00:00";
					lg.EndTime = Convert.ToDateTime(results["EndTime"]);
					lg.FromDate = Convert.ToDateTime(results["FromDate"]);
					lg.ToDate = Convert.ToDateTime(results["ToDate"]);
					//lg.LogDate = (!results.IsDBNull(results.GetOrdinal("LogDate"))) ? Convert.ToDateTime(results["LogDate"]).ToString("dd/MM/yyyy") : "00/00/00";
					lg.LogDate = Convert.ToDateTime(results["LogDate"]);
					lg.SourceRows = (!results.IsDBNull(results.GetOrdinal("SourceRows"))) ? results["SourceRows"].ToString() : "0";
					lg.ExtractedRows = (!results.IsDBNull(results.GetOrdinal("ExtractedRows"))) ? results["ExtractedRows"].ToString() : "0";
					lg.ExtractionStatus = (!results.IsDBNull(results.GetOrdinal("ExtractionStatus"))) ? Convert.ToBoolean(results["ExtractionStatus"]) : false;
					//lg.FileTransferStatus = (!results.IsDBNull(results.GetOrdinal("FileTransferStatus"))) ? results["FileTransferStatus"].ToString() : "0";
					lg.FileTransferStatus = (!results.IsDBNull(results.GetOrdinal("FileTransferStatus"))) ? Convert.ToBoolean(results["FileTransferStatus"]) : false;
					lg.ExceptionThrown = (!results.IsDBNull(results.GetOrdinal("ExceptionThrown"))) ? results["ExceptionThrown"].ToString() : "";
					lg.FilePath = (!results.IsDBNull(results.GetOrdinal("FilePath"))) ? results["FilePath"].ToString() : "";
					lgs.Add(lg);
				}
			}
			return lgs;
		}

		public static List<ExtractionLog2> GetAllLog(string range)
		{
			#region ParserString
			string condition = AF.DAL.SharedUtils.GetRangeCondition(range);
			#endregion
			List<ExtractionLog2> lgs = new List<ExtractionLog2>();
			int rCntr = 0;
			var sqlcom = "select * from [WebApp].[TaskProcessLog] " + condition;
			var constring = AF.DAL.SharedUtils.GetDSN();
			using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(sqlcom, constring))
			{
				while (results.Read())
				{
					rCntr++;
					ExtractionLog2 lg = new ExtractionLog2();
					lg.ID = results["ID"].ToString();
					lg.TaskName = (!results.IsDBNull(results.GetOrdinal("TaskName"))) ? results["TaskName"].ToString() : "";
					lg.ExeTime = Convert.ToDateTime(results["ExeTime"]);
					//lg.ExeTime = (!results.IsDBNull(results.GetOrdinal("ExeTime"))) ? DateTime.TryParseExact(results["ExeTime"]).ToString("dd/MM/yyyy H:mm:ss") : "00/00/0000 00:00:00";
					lg.EndTime = Convert.ToDateTime(results["EndTime"]);
					lg.FromDate = Convert.ToDateTime(results["FromDate"]);
					lg.ToDate = Convert.ToDateTime(results["ToDate"]);
					//lg.LogDate = (!results.IsDBNull(results.GetOrdinal("LogDate"))) ? Convert.ToDateTime(results["LogDate"]).ToString("dd/MM/yyyy") : "00/00/00";
					lg.LogDate = Convert.ToDateTime(results["LogDate"]);
					lg.SourceRows = (!results.IsDBNull(results.GetOrdinal("SourceRows"))) ? results["SourceRows"].ToString() : "0";
					lg.ExtractedRows = (!results.IsDBNull(results.GetOrdinal("ExtractedRows"))) ? results["ExtractedRows"].ToString() : "0";
					lg.ExtractionStatus = (!results.IsDBNull(results.GetOrdinal("ExtractionStatus"))) ? Convert.ToBoolean(results["ExtractionStatus"]) : false;
					//lg.FileTransferStatus = (!results.IsDBNull(results.GetOrdinal("FileTransferStatus"))) ? results["FileTransferStatus"].ToString() : "0";
					lg.FileTransferStatus = (!results.IsDBNull(results.GetOrdinal("FileTransferStatus"))) ? Convert.ToBoolean(results["FileTransferStatus"]) : false;
					lg.ExceptionThrown = (!results.IsDBNull(results.GetOrdinal("ExceptionThrown"))) ? results["ExceptionThrown"].ToString() : "";
					lg.FilePath = (!results.IsDBNull(results.GetOrdinal("FilePath"))) ? results["FilePath"].ToString() : "";
					lgs.Add(lg);
				}
			}
			return lgs;
		}

		public async Task<int> ExecuteAsync(string sConnectionString, string sSQL, params SqlParameter[] parameters)
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

		public async Task<int> LogItAsync(ExtractionLog2 l)
		{
			string vsql = "";
			vsql += "INSERT INTO WebApp.TaskProcessLog ";
			vsql += "(TaskName,TaskGroup,FromDate,ToDate,ExeTime,EndTime,SourceRows, ";
			vsql += "ExtractedRows,ExtractionStatus,FileTransferStatus,ExceptionThrown,FilePath) ";
			vsql += " VALUES (@TaskName,@TaskGroup,@FromDate,@ToDate,@ExeTime,@EndTime,@SourceRows,@ExtractedRows,@ExtractionStatus,@FileTransferStatus,@ExceptionThrown,@FilePath) ";


			//create amri 
			vsql += "INSERT INTO LIL.FileMonitoring ";
			vsql += "(requestID ,txnLogID,clientID,clientIP,nodeIP,contentType,urlPath,trxRq_DT,fileSizeR,fileRq) ";
			vsql += " VALUES ('testfile','testfile','','localcreate','type','c:/','2023-01-01 10:00:00.000','100','customer')";

			using (var newConnection = new SqlConnection(SharedUtils.GetDSN()))
			using (var newCommand = new SqlCommand(vsql, newConnection))
			{

				newCommand.CommandType = CommandType.Text;
				//if (parameters != null) newCommand.Parameters.AddRange(parameters);
				newCommand.Parameters.AddWithValue("@TaskName", l.TaskName);
				newCommand.Parameters.AddWithValue("@TaskGroup", "CANONICAL");
				newCommand.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(String.Format("{0: yyyy.MM.dd}", l.FromDate)));
				newCommand.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(String.Format("{0: yyyy.MM.dd}", l.ToDate)));
				newCommand.Parameters.AddWithValue("@ExeTime", Convert.ToDateTime(String.Format("{0: yyyy.MM.dd H:mm:ss }", l.ExeTime)));
				newCommand.Parameters.AddWithValue("@EndTime", Convert.ToDateTime(String.Format("{0: yyyy.MM.dd H:mm:ss }", l.EndTime)));
				newCommand.Parameters.AddWithValue("@SourceRows", Convert.ToInt64(l.SourceRows));
				newCommand.Parameters.AddWithValue("@ExtractedRows", Convert.ToInt64(l.ExtractedRows));
				newCommand.Parameters.AddWithValue("@ExtractionStatus", l.ExtractionStatus);
				newCommand.Parameters.AddWithValue("@FileTransferStatus", l.FileTransferStatus);
				newCommand.Parameters.AddWithValue("@ExceptionThrown", l.ExceptionThrown);
				newCommand.Parameters.AddWithValue("@FilePath", l.FilePath);

				await newConnection.OpenAsync().ConfigureAwait(false);
				return await newCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
			}
		}

		public static void LogIt(ExtractionLog2 l)
		{
			string vsql = "";
			vsql += "INSERT INTO WebApp.TaskProcessLog ";
			vsql += "(TaskName,TaskGroup,FromDate,ToDate,ExeTime,EndTime,SourceRows, ";
			vsql += "ExtractedRows,ExtractionStatus,FileTransferStatus,ExceptionThrown,FilePath) ";
			vsql += " VALUES (@TaskName,@TaskGroup,@FromDate,@ToDate,@ExeTime,@EndTime,@SourceRows,@ExtractedRows,@ExtractionStatus,@FileTransferStatus,@ExceptionThrown,@FilePath) ";

			vsql += "INSERT INTO LIL.FileMonitoring ";
			vsql += "(requestID ,txnLogID,clientID,clientIP,nodeIP,contentType,urlPath,trxRq_DT,fileSizeR,fileRq) ";
			vsql += " VALUES ('testfile','testfile','','localcreate','type','c:/','2023-01-01 10:00:00.000','100','customer')";
			using (SqlConnection con = new SqlConnection(SharedUtils.GetDSN()))
			{
				using (SqlCommand cmd = new SqlCommand(vsql, con))
				{
					cmd.Parameters.AddWithValue("@TaskName", l.TaskName);
					cmd.Parameters.AddWithValue("@TaskGroup", "CANONICAL");
					cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(String.Format("{0: yyyy.MM.dd}", l.FromDate)));
					cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(String.Format("{0: yyyy.MM.dd}", l.ToDate)));
					cmd.Parameters.AddWithValue("@ExeTime", Convert.ToDateTime(String.Format("{0: yyyy.MM.dd H:mm:ss }", l.ExeTime)));
					cmd.Parameters.AddWithValue("@EndTime", Convert.ToDateTime(String.Format("{0: yyyy.MM.dd H:mm:ss }", l.EndTime)));
					cmd.Parameters.AddWithValue("@SourceRows", Convert.ToInt64(l.SourceRows));
					cmd.Parameters.AddWithValue("@ExtractedRows", Convert.ToInt64(l.ExtractedRows));
					cmd.Parameters.AddWithValue("@ExtractionStatus", l.ExtractionStatus);
					cmd.Parameters.AddWithValue("@FileTransferStatus", l.FileTransferStatus);
					cmd.Parameters.AddWithValue("@ExceptionThrown", l.ExceptionThrown);
					cmd.Parameters.AddWithValue("@FilePath", l.FilePath);
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
