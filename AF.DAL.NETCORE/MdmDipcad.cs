using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Dynamic;

namespace AF.DAL
{
	public class DipCadParam
	{
		public string[] PolicyNo { get; set; }
		public static string NotExist(string p, string[] polnos)
		{
			string value = Array.Find(polnos,
					   element => element.StartsWith(p,
					   StringComparison.Ordinal));
			return value;
		}
	}
	public class ResMdmIDs
	{
		public List<string> MdmIDs { get; set; }
	}
	
}
