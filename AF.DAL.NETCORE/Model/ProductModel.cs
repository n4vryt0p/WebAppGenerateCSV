using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF.DAL.Model
{
	public class ProductModel
	{
		

		public class ProductViewModel
		{
			public Header header { get; set; }
			public Data data { get; set; }
		}

		public class Header
		{
			public string txnLogId { get; set; }
			public int processingTime { get; set; }
		}

		public class Data
		{
			public Product[] products { get; set; }
		}

		public class Product
		{
			public string Product_HK { get; set; }
			public string SystemSRC_CD { get; set; }
			public string ProductCode { get; set; }
			public string Product_NM { get; set; }
			public string Currency_CD { get; set; }
			public string SalesEff_DT { get; set; }
			public string SalesExp_DT { get; set; }
			public Productcoverage[] productCoverages { get; set; }
		}

		public class Productcoverage
		{
			public int Coverage_ID { get; set; }
			public string CoverageCode { get; set; }
			public string Coverage_NM { get; set; }
			public string Description { get; set; }
			public string CoverageType_CD { get; set; }
			public string CoverageCategory_CD { get; set; }
			public string CoverageUnitType_CD { get; set; }
			public int Value { get; set; }
		}
	}
}
