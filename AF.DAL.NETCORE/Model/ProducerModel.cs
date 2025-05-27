using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF.DAL.Model
{
	public class ProducerModel
	{

		#region Search Parameter 
		public class SearchParameterab
		{
			public Searchab search { get; set; }
		}

		public class Searchab
		{
			public Claimab producer { get; set; }
		}

		public class Claimab
		{
			public string name { get; set; }
			public string gender { get; set; }
			public string dob { get; set; }
		}

		#endregion


	}
}
