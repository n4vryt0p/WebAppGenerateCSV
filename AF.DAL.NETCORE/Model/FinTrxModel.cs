using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF.DAL.Model
{
	public class FinTrxModel
	{
		#region Search Parameter 
		public class SearchParameterac
		{
			public Searchac search { get; set; }
		}

		public class Searchac
		{
			public FinTrxac finTrx { get; set; }
		}

		public class FinTrxac
		{
			public string TrxSRC_ID { get; set; }
			public string PolicyNumber { get; set; }
			public string DebitCredit_Ind { get; set; }
			public string trxType_CD { get; set; }
			public string trxStatus_CD { get; set; }
			public string start_DT { get; set; }
			public string end_DT { get; set; }
		}

		#endregion

	}
}
