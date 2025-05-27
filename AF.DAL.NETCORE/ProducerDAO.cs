using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Dynamic;
using static AF.DAL.Model.ProducerModel;

namespace AF.DAL
{
	public class ProducerDAO
	{

		#region Financial Transaction list search
		public static dynamic GetViewListPoducerByCondition(SearchParameterab s)
		{
			dynamic vRet = null;
			try
			{
				dynamic p = new ExpandoObject();
				p.Producers = GetDynPoducerByCondition(s);
				vRet = new ExpandoObject();
				vRet.data = p;
			}
			catch (Exception e)
			{ Console.WriteLine(e.ToString()); }

			return vRet;
		}

		public static DataTable GetDynPoducerByCondition(SearchParameterab s)
		{
			DataTable vRet = null;

			string strCom = @"select top 20 
				t1.[Producer_HK]
				,t1.[Entity_CD]
				,t1.[SystemSRC_CD]
				,t1.[ProducerSRC_ID]
				,t1.[ProducerType_CD]
				,t1.[ProducerStatus_CD]
				,t1.[Country_CD]
				,t1.[Join_DT]
				,t1.[Term_DR]
			FROM [GFCC_DM].[dbo].[Producer] t1
			inner join gfcc_dm.dbo.producerperson t2 on t1.producer_hk=t2.producer_hk where 1=1 ";
			if (s.search.producer.name != null)
			{
				if (s.search.producer.name.Trim() != "")
				{
					strCom += " and lower(LTRIM(RTRIM(concat(t2.[FirstName],t2.[MiddleName],t2.[LastName])))) = lower('" + s.search.producer.name + "')";
				}
			}
			if (s.search.producer.gender != null)
			{
				if (s.search.producer.gender.Trim() != "")
				{
					strCom += " and lower(t2.[Gender])  =  lower('" + s.search.producer.gender + "')";
				}
			}
			if (s.search.producer.dob != null)
			{
				if (s.search.producer.dob.Trim() != "")
				{
					strCom += " and FORMAT(t2.[BirthDT], 'yyyy-MM-dd', 'en-us')  =  '" + s.search.producer.dob + "'";
				}
			}
			
			vRet = new DataTable();
			vRet = SharedUtils.ExecuteSqlCommandReturnDt(strCom, SharedUtils.GetDSNApp09());

			return vRet;
		}

		#endregion

		#region Producer,ProducerPerson Addresses, Phones,Emails, IdDocs

		public static dynamic GetProducerByProducerHK(string ProducerHK)
		{
			dynamic vRet = null;

			int rCntr = 0;
			string strCom = @"select 
							[Producer_HK],[Entity_CD],[SystemSRC_CD],[ProducerSRC_ID]
							,[ProducerType_CD],[ProducerStatus_CD],[Country_CD]
							,convert(varchar(10), [Join_DT], 121) as [Join_DT]
							,convert(varchar(10), [Term_DR], 121) as [Term_DR]
							from [GFCC_DM].[dbo].[Producer]
							where [Producer_HK]='" + ProducerHK + "'  ";
			try
			{
				using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(strCom, SharedUtils.GetDSNApp09()))
				{
					while (results.Read())
					{
						rCntr++;
						var Producer_HK = SharedUtils.SafeGetString(results, 0);
						var Entity_CD = SharedUtils.SafeGetString(results, 0);
						var SystemSRC_CD = SharedUtils.SafeGetString(results, 1);
						var ProducerSRC_ID = SharedUtils.SafeGetString(results, 2);
						var ProducerType_CD = SharedUtils.SafeGetString(results, 3);
						var ProducerStatus_CD = SharedUtils.SafeGetString(results, 4);
						var Country_CD = SharedUtils.SafeGetString(results, 5);
						var Join_DT = SharedUtils.SafeGetString(results, 6);
						var Term_DT = SharedUtils.SafeGetString(results, 6);
						vRet = new ExpandoObject();
						vRet.Producer_HK = Producer_HK;
						vRet.Entity_CD = Entity_CD;
						vRet.SystemSRC_CD = SystemSRC_CD;
						vRet.ProducerSRC_ID = ProducerSRC_ID;
						vRet.ProducerType_CD = ProducerType_CD;
						vRet.ProducerStatus_CD = ProducerStatus_CD;
						vRet.Country_CD = Country_CD;
						vRet.Join_DT = Join_DT;
						vRet.Term_DT = Term_DT;
					}
				}

			}
			catch (Exception e)
			{
                Console.WriteLine(e.ToString());
            }

			return vRet;
		}

		public static dynamic GetPersonByProducerHK(string ProducerHK)
		{
			dynamic vRet = null;

			int rCntr = 0;
			string strCom = @"select 
							[FirstName],[MiddleName],[LastName],convert(varchar(10), [BirthDT], 121) as [BirthDT],
							[BirthPlace],[Citizenship],[MaritalStat],[Occupation],[OccupationClass_CD]
							from [GFCC_DM].[dbo].[ProducerPerson]
							where [Producer_HK]='" + ProducerHK + "'  ";
			try
			{
				using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(strCom, SharedUtils.GetDSNApp09()))
				{
					while (results.Read())
					{
						rCntr++;
						var firstName = SharedUtils.SafeGetString(results, 0);
						var MiddleName = SharedUtils.SafeGetString(results, 1);
						var LastName = SharedUtils.SafeGetString(results, 2);
						var BirthDT = SharedUtils.SafeGetString(results, 3);
						var birthPlace = SharedUtils.SafeGetString(results, 4);
						var citizenship = SharedUtils.SafeGetString(results, 5);
						var MaritalStat = SharedUtils.SafeGetString(results, 6);
						var Occupation = SharedUtils.SafeGetString(results, 7);
						var OccupationClass_CD = SharedUtils.SafeGetString(results, 8);
						vRet = new ExpandoObject();
						vRet.FirstName = firstName;
						vRet.MiddleName = MiddleName;
						vRet.LastName = LastName;
						vRet.BirthDT = BirthDT;
						vRet.BirthPlace = birthPlace;
						vRet.Citizenship = citizenship;
						vRet.MaritalStat = MaritalStat;
						vRet.Occupation = Occupation;
						vRet.OccupationClass_CD = OccupationClass_CD;
					}
				}

			}
			catch (Exception e)
			{
                Console.WriteLine(e.ToString());
            }

			return vRet;
		}

		public static DataTable GetListofEmailByProducerHK(string pk)
		{
			DataTable vRet = null;
			string sqlcom = @" SELECT 
							  [EmailType_CD] as emailtype_CD
							  ,[AddrLine] as address
						  FROM [GFCC_DM].[dbo].[ProducerEmail]
						  where [Producer_HK]='" + pk + "'";
			vRet = new DataTable();
			vRet = SharedUtils.ExecuteSqlCommandReturnDt(sqlcom, SharedUtils.GetDSNApp09());
			return vRet;
		}

		public static DataTable GetListofAddressByProducerHK(string pk)
		{
			DataTable vRet = null;
			string sqlcom = @" SELECT 
							  [AddressType_CD] 
							  ,concat([Line1],[Line2],[Line3],[Line4]) as [StreetNM]
							  ,[City]
							  ,[State]
							  ,[Country_CD]
							  ,[Zip]
						  FROM [GFCC_DM].[dbo].[ProducerAddress]
						  where [Producer_HK]='" + pk + "'";
			vRet = new DataTable();
			vRet = SharedUtils.ExecuteSqlCommandReturnDt(sqlcom, SharedUtils.GetDSNApp09());
			return vRet;
		}

		public static DataTable GetListofIdentityByProducerHK(string pk)
		{
			DataTable vRet = null;
			string sqlcom = @" SELECT 
							  [IDType_CD]
							  ,[IDNumber]
							  ,[IDStatus_CD]
							  ,[Issue_DT],[Exp_DT]
						  FROM [GFCC_DM].[dbo].[ProducerIDDoc]
						  where [Producer_HK]='" + pk + "'";
			vRet = new DataTable();
			vRet = SharedUtils.ExecuteSqlCommandReturnDt(sqlcom, SharedUtils.GetDSNApp09());
			return vRet;
		}

		public static DataTable GetListofPhoneByProducerHK(string pk)
		{
			DataTable vRet = null;
			string sqlcom = @" SELECT 
							  [PhoneType_CD]
							  ,[CountryCallingCD]
							  ,[AreaCode]
							  ,[DialNumber]
						  FROM [GFCC_DM].[dbo].[ProducerPhone]
						  where [Producer_HK]='" + pk + "'";
			vRet = new DataTable();
			vRet = SharedUtils.ExecuteSqlCommandReturnDt(sqlcom, SharedUtils.GetDSNApp09());
			return vRet;
		}


		#endregion

		#region Producer By ID and dynamic EmbedObjects

		public static dynamic GetViewProducerByProducerHK(string ProducerHK, string attributesObject)
		{
			dynamic vRet = null;
			try
			{
				dynamic p = new ExpandoObject();
				p.Producer = GetDyntProducerByProducerHK(ProducerHK, attributesObject);
				vRet = new ExpandoObject();
				vRet.data = p;
			}
			catch (Exception e)
			{ Console.WriteLine(e.ToString()); }

			return vRet;
		}

		public static dynamic GetDyntProducerByProducerHK(string ProducerHK, string attributesObject)
		{
			dynamic vRet = null;
			vRet = GetProducerByProducerHK(ProducerHK);
			vRet.Person = GetPersonByProducerHK(ProducerHK);
			try
			{
				char[] delimiterChars = { ',' };
				string[] ws = attributesObject.Split(delimiterChars);
				foreach (string w in ws)
				{
					switch (w)
					{
						case "email":
							vRet.Emails = ProducerDAO.GetListofEmailByProducerHK(ProducerHK);
							break;
						case "address":
							vRet.Addresses = ProducerDAO.GetListofAddressByProducerHK(ProducerHK);
							break;
						case "phone":
							vRet.Phones = ProducerDAO.GetListofPhoneByProducerHK(ProducerHK); ;
							break;
						case "iddoc":
							vRet.Identities = ProducerDAO.GetListofIdentityByProducerHK(ProducerHK);
							break;
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}

			return vRet;
		}

		#endregion


		#region Producer with list of Policies 

		public static dynamic GetViewProducerPolicesByProducerHK(string ProducerHK)
		{
			dynamic vRet = null;
			try
			{
				dynamic p = new ExpandoObject();
				p.Producer = GetProducerPolicesByProducerHK(ProducerHK);
				vRet = new ExpandoObject();
				vRet.data = p;
			}
			catch (Exception e)
			{ Console.WriteLine(e.ToString()); }

			return vRet;
		}

		public static dynamic GetProducerPolicesByProducerHK(string ProducerHK)
		{
			dynamic vRet = null;
			string sqlRc = @"SELECT count(p2.PolicyNumber)
							from [GFCC_DM].[dbo].lnk_policy2producer p1
							inner join [GFCC_DM].[dbo].Policy p2 on p1.policy_hk = p2.policy_hk
							where p1.producer_hk = '" + ProducerHK + "'" ;
			vRet = GetProducerByProducerHK(ProducerHK);
			vRet.RowsCount = ExtractionCanonicalDAO.RowsCount(sqlRc, SharedUtils.GetDSNApp09());
			vRet.Policies = GetListofPoliciesByProducerHK(ProducerHK);

			return vRet; 
		}

		public static DataTable GetListofPoliciesByProducerHK(string ProducerHK)
		{
			DataTable vRet = null;
			string sqlcom = @" SELECT top 20
							p1.policy_hk,p2.PolicyNumber,
							p1.RelationRole_CD, p2.ProductCode,
							p2.ProductDesc,p2.PolicyStatus_CD
							from [GFCC_DM].[dbo].lnk_policy2producer p1
							inner join [GFCC_DM].[dbo].Policy p2 on p1.policy_hk=p2.policy_hk
							where p1.Producer_HK='" + ProducerHK + "'";
			vRet = new DataTable();
			vRet = SharedUtils.ExecuteSqlCommandReturnDt(sqlcom, SharedUtils.GetDSNApp09());
			return vRet;
		}

		#endregion
	}
}
