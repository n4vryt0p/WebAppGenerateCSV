using System.Data;
using Newtonsoft.Json;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace AF.DAL
{
	public class InputRequestParam
	{
		public InputRequestParam() { }
		public string FileNamePath { get; set; }
		public string DbFileNamePath { get; set; }
		public string StringSQLCommand { get; set; }
		public string JsonPrefix { get; set; }
		public string JsonTitle { get; set; }
		public string JsonTimeline { get; set; }
		public string CanonicalModelVersion { get; set; }
		public object GetColumns { get; set; }
		public object GetInitiateData { get; set; }
		public double JsonRecNum { get; set; }

	}

	public class CreateInitiateJsonFile : InputRequestParam
	{
		//, string timeline = "yyyyMMdd_yyyyMMddHHmmss", string orgUnit = "054"
		public CreateInitiateJsonFile(InputRequestParam p) : base()
		{
			var strFN = p.JsonPrefix + "_" + p.JsonTitle + "_" + p.JsonTimeline + ".json";
			string path = Directory.GetCurrentDirectory();
			p.FileNamePath = ConfigurationManager.AppSettings["JsonDir"] + p.JsonTimeline + @"\" + strFN;
			p.DbFileNamePath = ConfigurationManager.AppSettings["JsonDir"] + p.JsonTimeline + @"\" + strFN;

			StructureOutput n = new StructureOutput();

			n.fileName = "{" + p.JsonTitle + "}";
			n.lastRecNo = p.JsonRecNum.ToString();
			n.NbErrorLIL = "0";
			if (p.CanonicalModelVersion != "")
			{
				n.CanonicalModel_Version = p.CanonicalModelVersion;
			}
			else
			{
				n.CanonicalModel_Version = "4.0";
			}

			n.columns = p.GetColumns;
			n.data = p.GetInitiateData;
			string json = JsonConvert.SerializeObject(n);
			System.IO.File.WriteAllText(p.FileNamePath, json);
		}
	}

	public class StructureOutput
	{
		public string fileName { get; set; }
		public string lastRecNo { get; set; }
		public string NbErrorLIL { get; set; }
		public string CanonicalModel_Version { get; set; }
		public object columns { get; set; }
		public object data { get; set; }
	}

	public class ExtractionLog
	{
		public string ID { get; set; }
		public string TaskName { get; set; }
		public string TaskGroup { get; set; }
		public string ClienIp { get; set; }
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
		public string CountPath { get; set; }
		public static List<ExtractionLog> GetAllLog()
		{
			List<ExtractionLog> lgs = new List<ExtractionLog>();
			int rCntr = 0;
			var sqlcom = "select * from [WebApp].[TaskProcessLog] ;";
			using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(sqlcom, SharedUtils.GetDSN()))
			{
				while (results.Read())
				{
					rCntr++;
					ExtractionLog lg = new ExtractionLog();
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
		public static List<ExtractionLog> GetAllLog(string range)
		{
			#region ParserString
			string condition = AF.DAL.SharedUtils.GetRangeCondition(range);
			#endregion
			List<ExtractionLog> lgs = new List<ExtractionLog>();
			int rCntr = 0;
			var sqlcom = "select * from [WebApp].[TaskProcessLog] " + condition;
			var constring = AF.DAL.SharedUtils.GetDSN();
			using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(sqlcom, constring))
			{
				while (results.Read())
				{
					rCntr++;
					ExtractionLog lg = new ExtractionLog();
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
		public async Task<int> LogItAsync(ExtractionLog l)
		{
			string vsql = "";
			vsql += "INSERT INTO WebApp.TaskProcessLog ";
			vsql += "(TaskName,TaskGroup,FromDate,ToDate,ExeTime,EndTime,SourceRows, ";
			vsql += "ExtractedRows,ExtractionStatus,FileTransferStatus,ExceptionThrown,FilePath) ";
			vsql += " VALUES (@TaskName,@TaskGroup,@FromDate,@ToDate,@ExeTime,@EndTime,@SourceRows,@ExtractedRows,@ExtractionStatus,@FileTransferStatus,@ExceptionThrown,@FilePath) ";

			vsql += "INSERT INTO LIL.FileMonitoring ";
			vsql += "(requestID ,txnLogID,clientID,nodeIP,contentType,urlPath,trxRq_DT,fileSizeRq,fileRq,generateDT) ";
			vsql += " VALUES (@requestID,@txnLogID,@clientID,@clientIP,@nodeIP,@contentType,@urlPath,@trxRq_DT,@fileSizeRq,@fileRq,@generateDT)";

			using (var newConnection = new SqlConnection(SharedUtils.GetDSN()))
			using (var newCommand = new SqlCommand(vsql, newConnection))
			{
				newCommand.CommandType = CommandType.Text;
				//if (parameters != null) newCommand.Parameters.AddRange(parameters);
				newCommand.Parameters.AddWithValue("@TaskName", l.TaskName);
				newCommand.Parameters.AddWithValue("@TaskGroup", "CANONICAL");
				newCommand.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(String.Format("{0: yyyy-MM-dd}", l.FromDate)));
				newCommand.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(String.Format("{0: yyyy-MM-dd}", l.ToDate)));
				newCommand.Parameters.AddWithValue("@ExeTime", Convert.ToDateTime(String.Format("{0: yyyy-MM-dd H:mm:ss }", l.ExeTime)));
				newCommand.Parameters.AddWithValue("@EndTime", Convert.ToDateTime(String.Format("{0: yyyy-MM-dd H:mm:ss }", l.EndTime)));
				newCommand.Parameters.AddWithValue("@SourceRows", Convert.ToInt64(l.SourceRows));
				newCommand.Parameters.AddWithValue("@ExtractedRows", Convert.ToInt64(l.ExtractedRows));
				newCommand.Parameters.AddWithValue("@ExtractionStatus", l.ExtractionStatus);
				newCommand.Parameters.AddWithValue("@FileTransferStatus", l.FileTransferStatus);
				newCommand.Parameters.AddWithValue("@ExceptionThrown", l.ExceptionThrown);
				newCommand.Parameters.AddWithValue("@FilePath", l.FilePath);
				newCommand.Parameters.AddWithValue("@requestID", "Batch (UDM)");
				newCommand.Parameters.AddWithValue("@txnLogID", l.TaskName);
				newCommand.Parameters.AddWithValue("@clientID", l.TaskName);
				//jadikan id
                newCommand.Parameters.AddWithValue("@clientIP", l.TaskName);
                newCommand.Parameters.AddWithValue("@nodeIP", "192.168.1.1");
				newCommand.Parameters.AddWithValue("@contentType", l.TaskName);
				newCommand.Parameters.AddWithValue("@urlPath", l.FilePath);
				newCommand.Parameters.AddWithValue("@trxRq_DT", DateTime.Now);
				newCommand.Parameters.AddWithValue("@fileSizeRq", l.FilePath.Length);
				newCommand.Parameters.AddWithValue("@fileRq", Path.GetFileName(l.FilePath));
				newCommand.Parameters.AddWithValue("@generateDT", DateTime.Now);
			

				await newConnection.OpenAsync().ConfigureAwait(false);
				return await newCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
			}
		}
		public static void LogIt(ExtractionLog l)
		{
			string vsql = "";
			vsql += "INSERT INTO WebApp.TaskProcessLog ";
			vsql += "(TaskName,TaskGroup,FromDate,ToDate,ExeTime,EndTime,SourceRows, ";
			vsql += "ExtractedRows,ExtractionStatus,FileTransferStatus,ExceptionThrown,FilePath) ";
			vsql += " VALUES (@TaskName,@TaskGroup,@FromDate,@ToDate,@ExeTime,@EndTime,@SourceRows,@ExtractedRows,@ExtractionStatus,@FileTransferStatus,@ExceptionThrown,@FilePath) ";

			vsql += "INSERT INTO LIL.FileMonitoring ";
			vsql += "(requestID ,txnLogID,clientID,clientIP,nodeIP,contentType,urlPath,trxRq_DT,fileSizeRq,fileRq,generateDT,CountFile) ";
			vsql += " VALUES (@requestID,@txnLogID,@clientID,@clientIP,@nodeIP,@contentType,@urlPath,@trxRq_DT,@fileSizeRq,@fileRq,@generateDT,@CountFile)";
			

			using (SqlConnection con = new SqlConnection(SharedUtils.GetDSN()))
			{
				using (SqlCommand cmd = new SqlCommand(vsql, con))
				{
					cmd.Parameters.AddWithValue("@TaskName", l.TaskName);
					cmd.Parameters.AddWithValue("@TaskGroup", "CANONICAL");
					cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(String.Format("{0: yyyy/MM/dd}", l.FromDate)));
					cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(String.Format("{0: yyyy/MM/dd}", l.ToDate)));
					cmd.Parameters.AddWithValue("@ExeTime", Convert.ToDateTime(String.Format("{0: yyyy/MM/dd H:mm:ss }", l.ExeTime)));
					cmd.Parameters.AddWithValue("@EndTime", Convert.ToDateTime(String.Format("{0: yyyy/MM/dd H:mm:ss }", l.EndTime)));
					cmd.Parameters.AddWithValue("@SourceRows", Convert.ToInt64(l.SourceRows));
					cmd.Parameters.AddWithValue("@ExtractedRows", Convert.ToInt64(l.ExtractedRows));
					cmd.Parameters.AddWithValue("@ExtractionStatus", l.ExtractionStatus);
					cmd.Parameters.AddWithValue("@FileTransferStatus", l.FileTransferStatus);
					cmd.Parameters.AddWithValue("@ExceptionThrown", l.ExceptionThrown);
					cmd.Parameters.AddWithValue("@CountFile",l.CountPath);
					if (l.FilePath != null)
					{

						cmd.Parameters.AddWithValue("@FilePath", l.FilePath);

					}
					else {
						cmd.Parameters.AddWithValue("@FilePath", "");
					}
					
					//add file monitoring
					cmd.Parameters.AddWithValue("@requestID", "Batch (UDM)");
					cmd.Parameters.AddWithValue("@txnLogID", l.TaskName);
					cmd.Parameters.AddWithValue("@clientID", l.TaskName);
					//jadikan id
                    cmd.Parameters.AddWithValue("@clientIP", "192.168.1.1");
                    cmd.Parameters.AddWithValue("@nodeIP", "192.168.1.1");
					cmd.Parameters.AddWithValue("@contentType", l.TaskName);
					cmd.Parameters.AddWithValue("@urlPath", l.FilePath);
					cmd.Parameters.AddWithValue("@trxRq_DT", DateTime.Now);
					cmd.Parameters.AddWithValue("@fileSizeRq", l.FilePath.Length);
					cmd.Parameters.AddWithValue("@fileRq", Path.GetFileName(l.FilePath));
					cmd.Parameters.AddWithValue("@generateDT", DateTime.Now);






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


		public static void LogOk(string okFile,string pathRs,string lenght)
		{
            string vsql = "";
            vsql += "INSERT INTO LIL.FileMonitoring ";
            vsql += "(requestID ,txnLogID,clientID,clientIP,nodeIP,contentType,urlPath,trxRq_DT,fileSizeRq,fileRq,generateDT,CountFile) ";
            vsql += " VALUES (@requestID,@txnLogID,@clientID,@clientIP,@nodeIP,@contentType,@urlPath,@trxRq_DT,@fileSizeRq,@fileRq,@generateDT,@CountFile)";
			using (SqlConnection con = new SqlConnection(SharedUtils.GetDSN()))
			{
				using (SqlCommand cmd = new SqlCommand(vsql, con))
				{
                    cmd.Parameters.AddWithValue("@requestID", "Batch (UDM)");
                    cmd.Parameters.AddWithValue("@txnLogID", "OkFile");
                    cmd.Parameters.AddWithValue("@clientID", "OKFile");
                    //jadikan id
                    cmd.Parameters.AddWithValue("@clientIP", "192.168.1.1");
                    cmd.Parameters.AddWithValue("@nodeIP", "192.168.1.1");
                    cmd.Parameters.AddWithValue("@contentType", "OkFile");
                    cmd.Parameters.AddWithValue("@urlPath", pathRs);
                    cmd.Parameters.AddWithValue("@trxRq_DT", DateTime.Now);
                    cmd.Parameters.AddWithValue("@fileSizeRq", 1);
					cmd.Parameters.AddWithValue("@fileRq", okFile);
                    cmd.Parameters.AddWithValue("@generateDT", DateTime.Now);
					cmd.Parameters.AddWithValue("@CountFile", lenght);


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
