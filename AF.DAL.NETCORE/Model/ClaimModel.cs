using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF.DAL.Model
{
	public class ClaimModel
	{
		#region Search Parameter 
		public class SearchParametera
		{
			public Searcha search { get; set; }
		}

		public class Searcha
		{
			public Claima claim { get; set; }
		}

		public class Claima
		{
			public string policyNumber { get; set; }
			public string claimNumber { get; set; }
			public string claimType_CD { get; set; }
			public string claimStatus_CD { get; set; }
			public string start_Report_DT { get; set; }
			public string end_Report_DT { get; set; }
		}

		#endregion

	}
}
