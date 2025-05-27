using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF.DAL.Model
{
	public class PolicyModela
	{
		#region Search Parameter 
		public class SearchParameteraq
		{
			public Searchq search { get; set; }
		}

		public class Searchq
		{
			public Policya policy { get; set; }
		}

		public class Policya
		{
			public string policyStatus_CD { get; set; }
			public string policyNumber { get; set; }
			public string productCode { get; set; }
			public string start_Eff_DT { get; set; }
			public string end_Eff_DT { get; set; }
			public string start_Issue_DT { get; set; }
			public string end_Issue_DT { get; set; }
		}

		#endregion

	}
}
