using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Dynamic;
using AF.DAL.Model;

namespace AF.DAL
{
	public class ProductRequestParam
	{
		
		public string productCode { get; set; }
		public string salesEff_DT { get; set; }
		public string salesExp_DT { get; set; }
	}
	public class ProductDAO
	{

		#region Final Product Filter by Condition(search Product)

		public static dynamic GetViewProductByCondition(ProductRequestParam p)
		{
			dynamic vRet = null;
			try
			{
				dynamic prd = new ExpandoObject();
				prd.Products = GetDynProductByCondition(p);
				vRet = new ExpandoObject();
				vRet.data = prd;
			}
			catch (Exception e)
			{ Console.WriteLine(e.ToString()); }

			return vRet;
		}

		public static dynamic GetDynProductByCondition(ProductRequestParam p)
		{
			dynamic vRet = null;

			int rCntr = 0;

			if (p.productCode is null) { return vRet; throw new ArgumentException("Parameter cannot be null ", nameof(p.productCode)); }
			string strCom = @"select 
							[Product_HK]
							,[SystemSRC_CD]
							,[ProductCode]
							,[Product_NM]
							,[Currency_CD]
							,[SalesEff_DT]
							,[SalesExp_DT]
							from [GFCC_DM].[dbo].[Product] where 1=1 ";
			if (p.productCode != null)
			{
				strCom += " and lower(ProductCode) ='" + p.productCode.ToLower() + "' ";
			}
			if (p.salesEff_DT != null)
			{
				strCom += " and CAST(SalesEff_DT as date) = CAST('" + p.salesEff_DT + "',as date) ";
			}
			if (p.salesExp_DT != null)
			{
				strCom += " and CAST(SalesExp_DT as date) = CAST('" + p.salesExp_DT + "',as date) ";
			}

			ArrayList listOfProduct = new ArrayList();
			try
			{
				using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(strCom, SharedUtils.GetDSNApp09()))
				{
					while (results.Read())
					{
						rCntr++;
						var Product_HK = SharedUtils.SafeGetString(results, 0);
						var SystemSRC_CD = SharedUtils.SafeGetString(results, 1);
						var ProductCode = SharedUtils.SafeGetString(results, 2);
						var Product_NM = SharedUtils.SafeGetString(results, 3);
						var Currency_CD = SharedUtils.SafeGetString(results, 4);
						var SalesEff_DT = SharedUtils.SafeGetString(results, 5);
						var SalesExp_DT = SharedUtils.SafeGetString(results, 6);
						dynamic op = new ExpandoObject();
						op.Product_HK = Product_HK;
						op.SystemSRC_CD = SystemSRC_CD;
						op.ProductCode = ProductCode;
						op.Product_NM = Product_NM;
						op.Currency_CD = Currency_CD;
						op.SalesEff_DT = SalesEff_DT;
						op.SalesExp_DT = SalesExp_DT;

						listOfProduct.Add(op);
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			vRet = listOfProduct;

			return vRet;
		}


		#endregion

		#region Final Party by ID(Detail of the Product)

		public static dynamic GetViewProductByProductHK(string productHK)
		{
			dynamic vRet = null;
			try
			{
				dynamic p = new ExpandoObject();
				p.products = GetDynProductByID(productHK);
				vRet = new ExpandoObject();
				vRet.data = p;
			}
			catch (Exception e)
			{ Console.WriteLine(e.ToString()); }

			return vRet;
		}

		public static dynamic GetDynProductByID(string productHK)
		{
			dynamic vRet = null;

			int rCntr = 0;
			string strCom = @"select 
							[Product_HK]
							,[SystemSRC_CD]
							,[ProductCode]
							,[Product_NM]
							,[Currency_CD]
							,convert(varchar(10), [SalesEff_DT], 102)
							,convert(varchar(10), [SalesExp_DT], 102)
							from [GFCC_DM].[dbo].[Product] ";
			strCom += " where Product_HK='" + productHK + "' ";

			ArrayList listOfProducts = new ArrayList();
			try
			{
				using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(strCom, SharedUtils.GetDSNApp09()))
				{
					while (results.Read())
					{
						rCntr++;
						var Product_HK = SharedUtils.SafeGetString(results, 0);
						var SystemSRC_CD = SharedUtils.SafeGetString(results, 1);
						var ProductCode = SharedUtils.SafeGetString(results, 2);
						var Product_NM = SharedUtils.SafeGetString(results, 3);
						var Currency_CD = SharedUtils.SafeGetString(results, 4);
						var SalesEff_DT = SharedUtils.SafeGetString(results, 5);
						var SalesExp_DT = SharedUtils.SafeGetString(results, 6);

                        dynamic pr = new ExpandoObject();
						pr.Product_HK = Product_HK;
						pr.SystemSRC_CD = SystemSRC_CD;
						pr.ProductCode = ProductCode;
						pr.Product_NM = Product_NM;
						pr.Currency_CD = Currency_CD;
						pr.SalesEff_DT = SalesEff_DT;
						pr.SalesExp_DT = SalesExp_DT;
						
						DataTable productCoverages = new DataTable();
						productCoverages = GetListofProductScopeByProductIdDT(productHK);
						pr.productCoverages = productCoverages;
						listOfProducts.Add(pr);
					}
				}

			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			vRet = listOfProducts;
			return vRet;
		}
		public static DataTable GetListofProductScopeByProductIdDT(string pk)
		{
			DataTable vRet = null;
			string sqlcom = @" SELECT 
							t1.[Coverage_ID]
							,t1.[CoverageCode]
							,t1.[Coverage_NM]
							,t1.[Description]
							,t1.[CoverageType_CD]
							,t1.[CoverageCategory_CD]
							,t1.[CoverageUnitType_CD]
							,t1.[Value]
							from [GFCC_DM].[dbo].[ProductCoverage] t1
							where t1.Product_HK='" + pk + "'";
			vRet = new DataTable();
			vRet = SharedUtils.ExecuteSqlCommandReturnDt(sqlcom, SharedUtils.GetDSNApp09());
			return vRet;
		}

		#endregion


	}
}
