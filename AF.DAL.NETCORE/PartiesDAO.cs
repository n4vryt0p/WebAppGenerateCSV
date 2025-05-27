using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Dynamic;
using AF.DAL.Model;
//using AF.DAL.Model.PartyModel;

namespace AF.DAL
{
	public class PartiesDAO
	{

		//Final Dynamic Parties embedOBject
		#region Final Party Detil with Embeded Object
		public static dynamic GetViewPartyByPartyHK(string partyHK, string embedObject)
		{
			dynamic vRet = null;
			try {
				dynamic p = new ExpandoObject();
				p.Party = GetDynPartyByID(partyHK, embedObject);
				vRet = new ExpandoObject();
				vRet.data = p;
			}
			catch (Exception e)
			{ Console.WriteLine(e.ToString()); }

			return vRet;
		}

		public static dynamic GetDynPartyByID(string partyHK, string embedObject)
		{
			dynamic vRet = null;

			int rCntr = 0;
			string strCom = @"select 
							p.party_HK,p.systemsrc_cd,p.partysrc_ID,p.PartyType_CD
							from [dbo].[Party] p ";
			strCom += " where p.Party_HK='" + partyHK + "' ";
			try
			{
				using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(strCom, SharedUtils.GetDSNApp09()))
				{
					while (results.Read())
					{
						rCntr++;
						var party_HK = SharedUtils.SafeGetString(results, 0);
						var systemsrc_cd = SharedUtils.SafeGetString(results, 1);
						var partysrc_ID = SharedUtils.SafeGetString(results, 2);
						var PartyType_CD = SharedUtils.SafeGetString(results, 3);
						vRet = new ExpandoObject();
						vRet.Party_HK = party_HK;
						vRet.SystemSRC_CD = systemsrc_cd;
						vRet.PartySRC_ID = partysrc_ID;
						vRet.PartyType_CD = PartyType_CD;

						if (PartyType_CD == "P")
						{
							PartyModel.Person Person = new PartyModel.Person();
							Person = GetPersonByPartyID(partyHK);
							vRet.Person = Person;
						}

						if (PartyType_CD == "C")
						{
							object Organization = new object();
							Organization = GetOrganizationByPartyID(partyHK);
							vRet.Organization = Organization;
						}

						char[] delimiterChars = { ',' };
						string[] ws = embedObject.Split(delimiterChars);
						foreach (string w in ws)
						{
							switch (w)
							{
								case "address":
									DataTable Addresses = new DataTable();
									Addresses = GetListofAddressByPartyIdDT(partyHK);
									vRet.Addresses = Addresses;
									Console.WriteLine("List of Address!");
									break;
								case "email":
									DataTable Emails = new DataTable();
									Emails = GetListofEmailByPartyIdDT(partyHK);
									vRet.Emails = Emails;
									Console.WriteLine("List Of Email!");
									break;
								case "iddoc":
									DataTable Identities = new DataTable();
									Identities = GetListofIdentityByPartyIdDT(partyHK);
									vRet.Identities = Identities;
									Console.WriteLine("List of ID Doc!");
									break;
								case "phone":
									DataTable Phones = new DataTable();
									Phones = GetListofPhoneByPartyIdDT(partyHK);
									vRet.Phones = Phones;
									Console.WriteLine("List of phones registered!");
									break;
							}

						}
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

		#region FinalPartyClaims
		public static dynamic GetViewPartyClaimsByPartyHK(string partyHK)
		{
			dynamic vRet = null;
			try
			{
				dynamic p = new ExpandoObject();
				p.Party = GetDynPartyForClaimsByID(partyHK);
				vRet = new ExpandoObject();
				vRet.data = p;
			}
			catch (Exception e)
			{ Console.WriteLine(e.ToString()); }

			return vRet;
		}
		public static dynamic GetDynPartyForClaimsByID(string partyHK)
		{
			dynamic vRet = null;

			int rCntr = 0;
			string strCom = @"select 
							p.party_HK,p.systemsrc_cd,p.partysrc_ID,p.PartyType_CD
							from [dbo].[Party] p ";
			strCom += " where p.Party_HK='" + partyHK + "' ";
			try
			{
				using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(strCom, SharedUtils.GetDSNApp09()))
				{
					while (results.Read())
					{
						rCntr++;
						var party_HK = SharedUtils.SafeGetString(results, 0);
						var systemsrc_cd = SharedUtils.SafeGetString(results, 1);
						var partysrc_ID = SharedUtils.SafeGetString(results, 2);
						var PartyType_CD = SharedUtils.SafeGetString(results, 3);
						vRet = new ExpandoObject();
						vRet.Party_HK = party_HK;
						vRet.SystemSRC_CD = systemsrc_cd;
						vRet.PartySRC_ID = partysrc_ID;
						vRet.PartyType_CD = PartyType_CD;

						if (PartyType_CD == "P")
						{
							PartyModel.Person Person = new PartyModel.Person();
							Person = GetPersonByPartyID(partyHK);
							vRet.Person = Person;
						}

						if (PartyType_CD == "C")
						{
							object Organization = new object();
							Organization = GetOrganizationByPartyID(partyHK);
							vRet.Organization = Organization;
						}
						DataTable Claims = new DataTable();
						Claims = GetListofClaimsByPartyIdDT(partyHK);
						vRet.Claims = Claims;

					}
				}

			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			return vRet;
		}
		public static DataTable GetListofClaimsByPartyIdDT(string pk)
		{
			DataTable vRet = null;
			string sqlcom = @" SELECT 
							p1.policy_hk,p2.PolicyNumber,
							p1.RelationRole_CD, p2.ProductCode,
							p3.claimnumber, p3.claimstatus_cd,p3.claimcurrency_cd,
							p3.ProductPlan_CD, p3.claimedamt, p3.eligibleamt,p3.paidcurrency_cd,p3.PaidAmt
							from LNK_Policy2Party p1
							inner join Policy p2 on p1.policy_hk=p2.policy_hk
							inner join claims p3 on p2.PolicyNumber=p3.PolicyNumber
							where p1.Party_HK='" + pk + "'";
			vRet = new DataTable();
			vRet = SharedUtils.ExecuteSqlCommandReturnDt(sqlcom, SharedUtils.GetDSNApp09());
			return vRet;
		}

		#endregion

		#region FinalPartyPolicies
		//Final Parties Policies
		public static dynamic GetViewPartyPoliciesByPartyHK(string partyHK)
		{
			dynamic vRet = null;
			try
			{
				dynamic p = new ExpandoObject();
				p.Party = GetDynPartyForPoliciesByID(partyHK);
				vRet = new ExpandoObject();
				vRet.data = p;
			}
			catch (Exception e)
			{ Console.WriteLine(e.ToString()); }

			return vRet;
		}
		public static dynamic GetDynPartyForPoliciesByID(string partyHK)
		{
			dynamic vRet = null;

			int rCntr = 0;
			string strCom = @"select 
							p.party_HK,p.systemsrc_cd,p.partysrc_ID,p.PartyType_CD
							from [dbo].[Party] p ";
			strCom += " where p.Party_HK='" + partyHK + "' ";
			try
			{
				using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(strCom, SharedUtils.GetDSNApp09()))
				{
					while (results.Read())
					{
						rCntr++;
						var party_HK = SharedUtils.SafeGetString(results, 0);
						var systemsrc_cd = SharedUtils.SafeGetString(results, 1);
						var partysrc_ID = SharedUtils.SafeGetString(results, 2);
						var PartyType_CD = SharedUtils.SafeGetString(results, 3);
						vRet = new ExpandoObject();
						vRet.Party_HK = party_HK;
						vRet.SystemSRC_CD = systemsrc_cd;
						vRet.PartySRC_ID = partysrc_ID;
						vRet.PartyType_CD = PartyType_CD;

						if (PartyType_CD == "P")
						{
							PartyModel.Person Person = new PartyModel.Person();
							Person = GetPersonByPartyID(partyHK);
							vRet.Person = Person;
						}

						if (PartyType_CD == "C")
						{
							object Organization = new object();
							Organization = GetOrganizationByPartyID(partyHK);
							vRet.Organization = Organization;
						}
						DataTable Policies = new DataTable();
						Policies = GetListofPoliciesByPartyIdDT(partyHK);
						vRet.Policies = Policies;

					}
				}

			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			return vRet;
		}
		public static DataTable GetListofPoliciesByPartyIdDT(string pk)
		{
			DataTable vRet = null;
			string sqlcom = @" SELECT 
							p1.policy_hk,p2.PolicyNumber,
							p1.RelationRole_CD, p2.ProductCode
							from LNK_Policy2Party p1
							inner join Policy p2 on p1.policy_hk=p2.policy_hk
							where p1.Party_HK='" + pk + "'";
			vRet = new DataTable();
			vRet = SharedUtils.ExecuteSqlCommandReturnDt(sqlcom, SharedUtils.GetDSNApp09());
			return vRet;
		}

		#endregion

		#region Final Data Party(email,addresses,phones,identities)
		public static DataTable GetListofEmailByPartyIdDT(string pk)
		{
			DataTable vRet = null;
			string sqlcom = @" SELECT 
							  [EmailType_CD] as emailtype_CD
							  ,[AddrLine] as address
						  FROM [GFCC_DM].[dbo].[PartyEmail]
						  where Party_HK='" + pk + "'";
			vRet = new DataTable();
			vRet = SharedUtils.ExecuteSqlCommandReturnDt(sqlcom, SharedUtils.GetDSNApp09());
			return vRet;
		}

		public static DataTable GetListofAddressByPartyIdDT(string pk)
		{
			DataTable vRet = null;
			string sqlcom = @" SELECT 
							  [AddressType_CD] 
							  ,[StreetNM]
							  ,[City]
							  ,[Province]
							  ,[Country_CD]
							  ,[Zip]
						  FROM [GFCC_DM].[dbo].[PartyAddress]
						  where Party_HK='" + pk + "'";
			vRet = new DataTable();
			vRet = SharedUtils.ExecuteSqlCommandReturnDt(sqlcom, SharedUtils.GetDSNApp09());
			return vRet;
		}

		public static DataTable GetListofIdentityByPartyIdDT(string pk)
		{
			DataTable vRet = null;
			string sqlcom = @" SELECT 
							  [IDType_CD]
							  ,[IDNumber]
						  FROM [GFCC_DM].[dbo].[PartyIDDoc]
						  where Party_HK='" + pk + "'";
			vRet = new DataTable();
			vRet = SharedUtils.ExecuteSqlCommandReturnDt(sqlcom, SharedUtils.GetDSNApp09());
			return vRet;
		}

		public static DataTable GetListofPhoneByPartyIdDT(string pk)
		{
			DataTable vRet = null;
			string sqlcom = @" SELECT 
							  [PhoneType_CD]
							  ,[CountryCallingCode]
							  ,[AreaCode]
							  ,[DialNumber]
						  FROM [GFCC_DM].[dbo].[PartyPhone]
						  where Party_HK='" + pk + "'";
			vRet = new DataTable();
			vRet = SharedUtils.ExecuteSqlCommandReturnDt(sqlcom, SharedUtils.GetDSNApp09());
			return vRet;
		}

		#endregion

		#region Final Party Filter by Condition 
		public static dynamic GetViewPartyByCondition(PartyRequestParam p)
		{
			dynamic vRet = null;
			try
			{
				dynamic pty = new ExpandoObject();
				pty.Parties = GetDynPartyByCondition(p);
				vRet = new ExpandoObject();
				vRet.data = pty;
			}
			catch (Exception e)
			{ Console.WriteLine(e.ToString()); }

			return vRet;
		}

		public static dynamic GetDynPartyByCondition(PartyRequestParam p)
		{
			dynamic vRet = null;

			int rCntr = 0;
			
			if (p.PartyType_CD is null) { return vRet; throw new ArgumentException("Parameter cannot be null", nameof(p.PartyType_CD)); }
			if (p.Name is null) { return vRet; throw new ArgumentException("Parameter cannot be null", nameof(p.Name)); }
			string strCom = @"select p.party_HK,p.systemsrc_cd,p.partysrc_ID,p.PartyType_CD from [dbo].[Party] p  ";
			if (p.PartyType_CD.ToUpper() == "C")
			{
				strCom += " inner join dbo.PartyOrganization po on p.party_HK = po.Party_HK ";
				strCom += " where lower(po.FullName) ='" + p.Name.ToLower() + "'";
			}
			else if (p.PartyType_CD.ToUpper() == "P")
			{
				strCom += " inner join dbo.PartyPerson pp on p.party_HK = pp.Party_HK ";
				strCom += " where lower(concat(pp.firstname,pp.MiddleName,pp.lastname)) = '" + p.Name.ToLower() + "' ";
				if (p.Gender != null || p.Gender != "")
				{
					strCom += " and lower(pp.gender) = '" + p.Gender.ToLower() + "' ";
				}
				if (p.DOB != null || p.DOB != "")
				{
					strCom += " and FORMAT (pp.Birth_DT,  'yyyy-MM-dd', 'en-us')  = '" + p.DOB + "' ";
				}
			}else { return vRet; }

			ArrayList listOfParty = new ArrayList();
			try
			{
				using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(strCom, SharedUtils.GetDSNApp09()))
				{
					while (results.Read())
					{
						rCntr++;
						var party_HK = SharedUtils.SafeGetString(results, 0);
						var systemsrc_cd = SharedUtils.SafeGetString(results, 1);
						var partysrc_ID = SharedUtils.SafeGetString(results, 2);
						var PartyType_CD = SharedUtils.SafeGetString(results, 3);
						dynamic op = new ExpandoObject();
						op.Party_HK = party_HK;
						op.SystemSRC_CD = systemsrc_cd;
						op.PartySRC_ID = partysrc_ID;
						op.PartyType_CD = PartyType_CD;
						
						if (PartyType_CD == "P")
						{
							PartyModel.Person Person = new PartyModel.Person();
							Person = GetPersonByPartyID(party_HK);
							op.Person = Person;
						}

						if (PartyType_CD == "C")
						{
							object Organization = new object();
							Organization = GetOrganizationByPartyID(party_HK);
							op.Organization = Organization;
						}
						listOfParty.Add(op);
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			vRet = listOfParty;
			//listOfParty.Clear();

			return vRet;
		}

		#endregion

		#region GetFinal data Detail party

		public static dynamic GetDetailPartyByPartyHK(string partyHK)
		{
			dynamic vRet = null;

			int rCntr = 0;
			string strCom = @"select 
							p.party_HK,p.systemsrc_cd,p.partysrc_ID,p.PartyType_CD
							from [dbo].[Party] p ";
			strCom += " where p.Party_HK='" + partyHK + "' ";
			try
			{
				using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(strCom, SharedUtils.GetDSNApp09()))
				{
					while (results.Read())
					{
						rCntr++;
						var party_HK = SharedUtils.SafeGetString(results, 0);
						var systemsrc_cd = SharedUtils.SafeGetString(results, 1);
						var partysrc_ID = SharedUtils.SafeGetString(results, 2);
						var PartyType_CD = SharedUtils.SafeGetString(results, 3);
						vRet = new ExpandoObject();
						vRet.Party_HK = party_HK;
						vRet.SystemSRC_CD = systemsrc_cd;
						vRet.PartySRC_ID = partysrc_ID;
						vRet.PartyType_CD = PartyType_CD;

						if (PartyType_CD == "P")
						{
							PartyModel.Person Person = new PartyModel.Person();
							Person = GetPersonByPartyID(partyHK);
							vRet.Person = Person;
						}

						if (PartyType_CD == "C")
						{
							object Organization = new object();
							Organization = GetOrganizationByPartyID(partyHK);
							vRet.Organization = Organization;
						}

						DataTable Addresses = new DataTable();
						Addresses = GetListofAddressByPartyIdDT(partyHK);
						vRet.Addresses = Addresses;
						DataTable Emails = new DataTable();
						Emails = GetListofEmailByPartyIdDT(partyHK);
						vRet.Emails = Emails;
						DataTable Identities = new DataTable();
						Identities = GetListofIdentityByPartyIdDT(partyHK);
						vRet.Identities = Identities;
						DataTable Phones = new DataTable();
						Phones = GetListofPhoneByPartyIdDT(partyHK);
						vRet.Phones = Phones;
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			return vRet;
		}
		public static PartyModel.Person GetPersonByPartyID(string partyHK)
		{
			PartyModel.Person vRet = null;

			int rCntr = 0;
			string strCom = @"select 
							pp.FirstName,pp.LastName,pp.gender,pp.birth_DT,pp.BirthPlace,pp.citizenship
							from [dbo].[PartyPerson] pp
							where pp.Party_HK='" + partyHK + "'  ";
			try
			{
				using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(strCom, SharedUtils.GetDSNApp09()))
				{
					while (results.Read())
					{
						rCntr++;
						var firstName = SharedUtils.SafeGetString(results, 0);
						var lastName = SharedUtils.SafeGetString(results, 1);
						var gender = SharedUtils.SafeGetString(results, 2);
						var birthDate = (!results.IsDBNull(3)) ? Convert.ToDateTime(results["birth_DT"]).ToString("yyyy-MM-dd") : "00/00/00";
						var birthPlace = SharedUtils.SafeGetString(results, 4);
						var citizenship = SharedUtils.SafeGetString(results, 5);
						vRet = new PartyModel.Person();
						vRet.FirstName = firstName;
						vRet.LastName = lastName;
						vRet.Gender = gender;
						vRet.Birth_DT = birthDate;
						vRet.BirthPlace = birthPlace;
						vRet.Citizenship = citizenship;
					}
				}

			}
			catch (Exception e)
			{
                Console.WriteLine(e.ToString());
            }

			return vRet;
		}

		public static PartyModel.Organization GetOrganizationByPartyID(string partyHK)
		{
			PartyModel.Organization vRet = null;

			int rCntr = 0;
			string strCom = @"select 
							[FullName],[OrgForm],[Establish_DT],[NatureCategory]
							from [dbo].[PartyOrganization]
							where Party_HK='" + partyHK + "'  ";

			using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(strCom, SharedUtils.GetDSNApp09()))
			{
				while (results.Read())
				{
					rCntr++;
					var fullname = SharedUtils.SafeGetString(results, 0);
					var orgForm = SharedUtils.SafeGetString(results, 1);
					var establishDate = (!results.IsDBNull(2)) ? Convert.ToDateTime(results["Establish_DT"]).ToString("yyyy-MM-dd") : "00/00/00";
					var natureCat = SharedUtils.SafeGetString(results, 3);
					vRet = new PartyModel.Organization();
					vRet.FullName = fullname;
					vRet.OrgForm = orgForm;
					vRet.Establish_DT = establishDate;
					vRet.NatureCategory = natureCat;
				}
			}

			return vRet;
		}

		#endregion


		public static dynamic GetPartyByPartyHK(string partyHK, string embedObject)
		{
			dynamic vRet = null;

			dynamic expando = new ExpandoObject();

			PartyModel.Party p = new PartyModel.Party();
			p = GetPartyByID(partyHK);
			expando.Party = p;
			if (p.PartyType_CD == "P") {
				PartyModel.Person Person = new PartyModel.Person();
				Person = GetPersonByPartyID(partyHK);
				expando.Party.Person = Person;
			}

			if (p.PartyType_CD == "C")
			{
				object Organization = new object();
				Organization = GetOrganizationByPartyID(partyHK);
				expando.Party.Organization = Organization;
			}
			char[] delimiterChars = { ',' };
			string[] ws = embedObject.Split(delimiterChars);
			foreach (string w in ws)
			{
				switch (w)
				{
					case "address":
						DataTable Addresses = new DataTable();
						Addresses = GetListofAddressByPartyIdDT(partyHK);
						expando.Party.Addresses = Addresses;
						Console.WriteLine("List of Address!");
						break;
					case "email":
						DataTable Emails = new DataTable();
						Emails = GetListofEmailByPartyIdDT(partyHK);
						expando.Party.Emails = Emails;
						Console.WriteLine("List Of Email!");
						break;
					case "iddoc":
						DataTable Identities = new DataTable();
						Identities = GetListofIdentityByPartyIdDT(partyHK);
						expando.Party.Identities = Identities;
						Console.WriteLine("List of ID Doc!");
						break;
					case "phone":
						DataTable Phones = new DataTable();
						Phones = GetListofPhoneByPartyIdDT(partyHK);
						expando.Party.Phones = Phones;
						Console.WriteLine("List of phones registered!");
						break;
				}

			}
			vRet = expando;

			return vRet;
		}
		public static string SqlGetPersonByPartHK(string partyHK, string embedAttributes)
		{
			string vRret = "";

			PartyModel.PartyViewModel pv = new PartyModel.PartyViewModel();

			PartyModel.Party p = new PartyModel.Party();
			
			object Addresses = null;
			object Emails = null;
			object Identities = null;
			object Phones = null;

			char[] delimiterChars = { ',' };
			string[] ws = embedAttributes.Split(delimiterChars);
			
			return vRret;
		}
		public static string PartyPersonSqlBuilder(string partyHK,string embedAttributes)
		{
			string vRet = " ";
			/* 
SELECT TOP 1000 [Party_HK]
    ,[Diff_HK]
    ,[LDTS]
    ,[UDTS]
    ,[FirstName]
    ,[MiddleName]
    ,[LastName]
    ,[Prefix]
    ,[Suffix]
    ,[Gender]
    ,[Title]
    ,[Birth_DT]
    ,[BirthPlace]
    ,[Death_DT]
    ,[Citizenship]
    ,[MaritalStat]
    ,[Occupation]
    ,[OccupationClass_CD]
    ,[EstSalary]
    ,[EstAnnualSalary]
FROM [GFCC_DM].[dbo].[PartyPerson]
			 
			 */
			var sql = new System.Text.StringBuilder();
			string sqlcom = @" select 
			FirstName,MiddleName,LastName ";
			sql.Append(sqlcom);
			if (embedAttributes != "")
			{
				sql.Append("," + embedAttributes + "  ");
			}
			sql.Append(" FROM [GFCC_DM].[dbo].[PartyPerson] ");
			if (partyHK != "")
			{
				sql.Append(" where 1=1 and [Party_HK]='" + partyHK + "' ");
			}

			vRet = sql.ToString();
			return vRet;
		}
		public static List<PartyModel.Party> GetListOfPartyByCondition(PartyRequestParam p)
		{
			List<PartyModel.Party> vRet= new List<PartyModel.Party>();

			string strCom = @"select 
							p.party_HK,p.systemsrc_cd,p.partysrc_ID,p.PartyType_CD,
							pp.FirstName,pp.LastName,pp.gender,pp.birth_DT,pp.BirthPlace,pp.citizenship
							from [dbo].[Party] p
							inner join dbo.PartyPerson pp on p.party_HK = pp.Party_HK
							where pp.FirstName is not null ";
			strCom += " and p.PartyType_CD='" + p.PartyType_CD + "' ";
			strCom += " and pp.gender='" + p.Gender + "' and pp.birth_DT ='" + p.DOB + "'  ";

			using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(strCom, SharedUtils.GetDSNApp09()))
			{
				while (results.Read())
				{
					PartyModel.Party party = new PartyModel.Party();
					var partyhk = SharedUtils.SafeGetString(results,0);
					var syssrcCode = SharedUtils.SafeGetString(results, 1);
					var partysrcID = SharedUtils.SafeGetString(results, 2);
					var partytypeCode = SharedUtils.SafeGetString(results, 3);
					party.Party_HK = partyhk;
					party.SystemSRC_CD = syssrcCode;
					party.PartySRC_ID = partysrcID;
					party.PartyType_CD = partytypeCode;
					vRet.Add(party);
					party = null;
				}
			}

				return vRet;
		}

		public static PartyModel.Party GetPartyByID(string partyHK)
		{
			PartyModel.Party vRet = null;

			int rCntr = 0;
			string strCom = @"select 
							p.party_HK,p.systemsrc_cd,p.partysrc_ID,p.PartyType_CD
							from [dbo].[Party] p
							where p.Party_HK='" + partyHK + "'  ";
			//strCom += " and p.PartyType_CD='" + partyHK + "' ";
			try {
			using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(strCom, SharedUtils.GetDSNApp09()))
			{
				//var columns = new List<string>();
				//for (int i = 0; i < results.FieldCount; i++)
				//{
				//	columns.Add(results.GetName(i));
				//}

				while (results.Read())
				{
					rCntr++;
					var party_HK = SharedUtils.SafeGetString(results, 0);
					var systemsrc_cd = SharedUtils.SafeGetString(results, 1);
					var partysrc_ID = SharedUtils.SafeGetString(results, 2);
					var PartyType_CD = SharedUtils.SafeGetString(results, 3);
					vRet = new PartyModel.Party
					{
						Party_HK = party_HK,
						SystemSRC_CD = systemsrc_cd,
						PartySRC_ID = partysrc_ID,
						PartyType_CD = PartyType_CD
					};
				}
			}

			} catch (Exception e) {
				Console.WriteLine(e.ToString());
			}
			return vRet;
		}
		public static PartyModel.Party GetPartyByID(PartyRequestParam p)
		{
			PartyModel.Party vRet = null;

			int rCntr = 0;
			string strCom = @"select 
							p.party_HK,p.systemsrc_cd,p.partysrc_ID,p.PartyType_CD,
							pp.FirstName,pp.LastName,pp.gender,pp.birth_DT,pp.BirthPlace,pp.citizenship
							from [dbo].[Party] p
							inner join dbo.PartyPerson pp on p.party_HK = pp.Party_HK
							where pp.FirstName is not null ";
			strCom += " and p.PartyType_CD='" + p.PartyType_CD + "' " ;
			//strCom += "	and pp.FirstName='" + p.FirstName + "' and pp.middlename='" + p.MiddleName + "' and pp.lastname='" + p.LastName + "'  ";
			strCom += " and pp.gender='" + p.Gender + "' and pp.dob ='" + p.DOB + "'  ";

			using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(strCom, SharedUtils.GetDSNApp09()))
			{
				var columns = new List<string>();
				for (int i = 0; i < results.FieldCount; i++)
				{
					columns.Add(results.GetName(i));
				}

				while (results.Read())
				{
					rCntr++;
					//		var polno = Helpers.SharedUtils.SafeGetString(results, 1);
					//		var productplan = Helpers.SharedUtils.SafeGetString(results, 2);
					//		var claimno = Helpers.SharedUtils.SafeGetString(results, 3);
					//		var claimtype = Helpers.SharedUtils.SafeGetString(results, 4);
					//		var claimstatus = Helpers.SharedUtils.SafeGetString(results, 5);
				}
			}
			

			return vRet;
		}
		public static DataTable GetPersonByPartyIdRetDT(string partyHK, string embedAttributes)
		{
			DataTable vRet = null;
			string sqlcom = PartyPersonSqlBuilder(partyHK, embedAttributes);
			vRet = new DataTable();
			vRet = SharedUtils.ExecuteSqlCommandReturnDt(sqlcom, SharedUtils.GetDSNApp09());

			return vRet;
		}
		public static List<PartyDetail.Email> GetListofEmailByPartyID()
		{
			List<PartyDetail.Email> vRet = null;

			return vRet;
		}
		public static List<PartyDetail.Address> GetListofAddressByPersonID()
		{
			List<PartyDetail.Address> vRet = null;

			return vRet;
		}
		public static List<PartyDetail.Identity> GetListofIdentityByPersonID()
		{
			List<PartyDetail.Identity> vRet = null;

			return vRet;
		}
		public static List<PartyDetail.Phone> GetListofPhonrByPersonID()
		{
			List<PartyDetail.Phone> vRet = null;

			return vRet;
		}

	}
	public class PartyRequestParam
	{
		
		public string PartyType_CD { get; set; }
		public string Name { get; set; }
		//public string MiddleName { get; set; }
		//public string LastName { get; set; }
		public string Gender { get; set; }
		public string DOB { get; set; }
	}
	public class PartyDetilRequestParam
	{
		public string PartyHK { get; set; }
		public string[] Embed { get; set; }
	}
}
