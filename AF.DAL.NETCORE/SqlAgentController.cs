using System;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace AF.DAL
{

	public class SqlAgentController
	{

		public static void GetServer()
		{
			//ServerConnection connection = new ServerConnection("WRDSQL09\\MSSQLSERVER", "gfcc", "Ax@t0wer");
			ServerConnection connection = new ServerConnection("WRDSQL09");

			Server server = new Server(connection);
			var job = server.JobServer.Jobs["JOB_GFCC_00_MAIN_CANONICAL"];
			Console.WriteLine(job.CurrentRunStatus.ToString()); // Current running status
			Console.WriteLine(job.CurrentRunStep);
			Console.WriteLine(job.LastRunOutcome.ToString()); // Last run result
			Console.WriteLine(job.LastRunDate.ToString());
			Console.WriteLine(job.NextRunDate.ToString());
			Console.WriteLine(job.OwnerLoginName);
			Console.WriteLine(job.DateCreated.ToString());
			Console.WriteLine(job.DateLastModified.ToString());
		}

		public static void GetServer2()
		{
			Server server = new Server("WRDSQL09");
			server.ConnectionContext.LoginSecure = false;
			server.ConnectionContext.Login = "gfcc";
			server.ConnectionContext.Password = "Ax@t0wer";

			var job = server.JobServer.Jobs["JOB_GFCC_00_MAIN_CANONICAL"];
			Console.WriteLine(job.CurrentRunStatus.ToString()); // Current running status
			Console.WriteLine(job.CurrentRunStep);
			Console.WriteLine(job.LastRunOutcome.ToString()); // Last run result
			Console.WriteLine(job.LastRunDate.ToString());
			Console.WriteLine(job.NextRunDate.ToString());
			Console.WriteLine(job.OwnerLoginName);
			Console.WriteLine(job.DateCreated.ToString());
			Console.WriteLine(job.DateLastModified.ToString());
		}

	}

}
