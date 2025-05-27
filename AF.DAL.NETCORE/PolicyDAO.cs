using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Dynamic;
using AF.DAL.Model;

namespace AF.DAL
{
	public class PolicyDAO
	{

		/*
		 mapping methods : 
		 Get Policy By Policy_HK
		 Get Coverage By Policy_HK
		 Get LNK Policy to Party by Policy_HK
		 Get object Policy Owner 
		 Get List of Object Insured 
		 Get List of Object Beneficiary
			 */

		#region Final GetPolicy Detail Data 
		public static dynamic GetViewPolicydetailByPolicyHK(string policyHK)
		{
			dynamic vRet = null;
			try
			{
				dynamic p = new ExpandoObject();
				p.Policies = GetPolicybyPolicyHK(policyHK);
				vRet = new ExpandoObject();
				vRet.data = p;
			}
			catch (Exception e)
			{ Console.WriteLine(e.ToString()); }

			return vRet;
		}

		public static dynamic GetPolicybyPolicyHK(string policyHK)
		{
			dynamic vRet = null;

			int rCntr = 0;
			string strCom = @"select 
							t1.[Policy_HK],t1.[Entity_CD],t1.[SystemSRC_CD],t1.[PolicyNumber],t1.[ProductCode]
							,t1.[ProductDesc],t1.[Currency_CD],t1.[PaymentMode_CD],t1.[PremiumAmt],t1.[AnnualPremiumToDate]
							,t1.[PolicyStatus_CD],convert(varchar(10), t1.[Eff_DT], 121),convert(varchar(10), t1.[Issue_DT], 121)
							,convert(varchar(10), t1.[Term_DT], 121),convert(varchar(10), t1.[ActualTerm_DT], 121) 
						  FROM [GFCC_DM].[dbo].[Policy] t1 ";
			strCom += " where t1.Policy_HK='" + policyHK + "' ";
			try
			{
				using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(strCom, SharedUtils.GetDSNApp09()))
				{
					while (results.Read())
					{
						rCntr++;
						var Policy_HK = SharedUtils.SafeGetString(results, 0);
						var Entity_CD = SharedUtils.SafeGetString(results, 1);
						var SystemSRC_CD = SharedUtils.SafeGetString(results, 2);
						var PolicyNumber = SharedUtils.SafeGetString(results, 3);
						var ProductCode = SharedUtils.SafeGetString(results, 4);
						var ProductDesc = SharedUtils.SafeGetString(results, 5);
						var Currency_CD = SharedUtils.SafeGetString(results, 6);
						var PaymentMode_CD = SharedUtils.SafeGetString(results, 7);
						var PremiumAmt = (!results.IsDBNull(8)) ? results.GetDecimal(8): 0; 
						var AnnualPremiumToDate = (!results.IsDBNull(9)) ? results.GetDecimal(9) : 0; 
						var PolicyStatus_CD = SharedUtils.SafeGetString(results, 10);
						var Eff_DT = SharedUtils.SafeGetString(results, 11);
						var Issue_DT = SharedUtils.SafeGetString(results, 12);
						var Term_DT = SharedUtils.SafeGetString(results, 13);
						var ActualTerm_DT = SharedUtils.SafeGetString(results, 14);
						vRet = new ExpandoObject();
						vRet.Policy_HK = Policy_HK;
						vRet.Entity_CD = Entity_CD;
						vRet.SystemSRC_CD = SystemSRC_CD;
						vRet.PolicyNumber = PolicyNumber;
						vRet.ProductCode = ProductCode;
						vRet.ProductDesc = ProductDesc;
						vRet.Currency_CD = Currency_CD;
						vRet.PaymentMode_CD = PaymentMode_CD;
						vRet.PremiumAmt= PremiumAmt;
						vRet.AnnualPremiumToDate = AnnualPremiumToDate;
						vRet.PolicyStatus_CD = PolicyStatus_CD;
						vRet.Eff_DT = Eff_DT;
						vRet.Issue_DT = Issue_DT;
						vRet.Term_DT = Term_DT;
						vRet.ActualTerm_DT = ActualTerm_DT;

						vRet.Coverages = GetListofCoverageByPolicyIdDT(Policy_HK);
						vRet.Owner = GetPolicyOwnerPartyDetailByPolHK(Policy_HK);
						if (SystemSRC_CD.ToUpper().Trim() == "G4IAM")
						{
							vRet.InsuredsList = GetPolicyPartiesDetailByPolHK(Policy_HK, "INS", true);
							vRet.BeneficiaryList = GetPolicyPartiesDetailByPolHK(Policy_HK, "BFY", true);
							vRet.DependantsList = GetPolicyPartiesDetailByPolHK(Policy_HK, "DEP", true);
							vRet.Producers = GetDynPolicyProducerByPolicyHK(Policy_HK);
						}
						else {
							vRet.Insureds = GetPolicyPartiesDetailByPolHK(Policy_HK, "INS", false);
							vRet.Beneficiary = GetPolicyPartiesDetailByPolHK(Policy_HK, "BFY", false);
							vRet.Dependants = GetPolicyPartiesDetailByPolHK(Policy_HK, "DEP", false);
							vRet.Producers = GetDynPolicyProducerByPolicyHK(Policy_HK);
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

		public static DataTable GetListofCoverageByPolicyIdDT(string policyHK)
		{
			DataTable vRet = null;
			string sqlcom = @" SELECT 
							[CoverageCode]
							,[CoverageOptCode]
							,[CoverageType]
							,[CoverageCategory_CD]
							,[Name]
							,[Description]
							,[PremiumAmt]
						FROM [GFCC_DM].[dbo].[PolicyCoverages]
							where [Policy_HK]='" + policyHK + "'";
			vRet = new DataTable();
			vRet = SharedUtils.ExecuteSqlCommandReturnDt(sqlcom, SharedUtils.GetDSNApp09());
			return vRet;
		}

		public static dynamic GetPolicyOwnerPartyDetailByPolHK(string policyHK)
		{
			dynamic vRet = null;

			int rCntr = 0;
			string strCom = @"select 
							 t1.[Party_HK]
							,t1.[ProductCode]
							,t1.[CoverageCode]
							,t1.[CoverageOptCode]
						FROM [GFCC_DM].[dbo].[LNK_Policy2Party] t1 ";
			strCom += " where t1.Policy_HK='" + policyHK + "' and t1.RelationRole_CD='OWN' ";
			try
			{
				using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(strCom, SharedUtils.GetDSNApp09()))
				{
					while (results.Read())
					{
						rCntr++;
						var Party_HK = SharedUtils.SafeGetString(results, 0);
						//var ProductCode = SharedUtils.SafeGetString(results, 1);
						//var CoverageCode = SharedUtils.SafeGetString(results, 2);
						//var CoverageOptCode = SharedUtils.SafeGetString(results, 3);
						vRet = new ExpandoObject();
						//vRet.ProductCode = ProductCode;
						//vRet.CoverageCode = CoverageCode;
						//vRet.CoverageOptCode = CoverageOptCode;

						vRet.Party = PartiesDAO.GetDetailPartyByPartyHK(Party_HK);
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}

			return vRet;
		}

		public static dynamic GetPolicyPartiesDetailByPolHK(string policyHK, string relationCD,bool isCorp)
		{
			dynamic vRet = null;

			int rCntr = 0;
			string strCom = "SELECT ";
			if (isCorp)
			{
				//strCom += @"  STUFF((SELECT  ',' + [Party_HK]
				//			FROM [GFCC_DM].[dbo].[LNK_Policy2Party] EE
				//			WHERE  EE.Policy_HK=E.Policy_HK
				//			ORDER BY [Party_HK]
				//			FOR XML PATH('')), 1, 1, '') AS listStr
				//			FROM [GFCC_DM].[dbo].[LNK_Policy2Party] E ";
				//strCom += " where E.Policy_HK='" + policyHK + "' and E.RelationRole_CD='" + relationCD + "'  GROUP BY E.RelationRole_CD";

				strCom += @"  STUFF(( SELECT  ', ' + [Party_HK] FROM    (
							SELECT  [Party_HK] FROM [GFCC_DM].[dbo].[LNK_Policy2Party] EE ";
				strCom += "WHERE  EE.Policy_HK='" + policyHK + "' and EE.RelationRole_CD='" + relationCD + "' ) x FOR XML PATH('')), 1, 2, '') ListOfParties ";
				vRet = SharedUtils.GetDataSingleValue(strCom, SharedUtils.GetDSNApp09());
			}
			else {

				strCom +=@" t1.[Party_HK]
						,t1.[ProductCode]
						,t1.[CoverageCode]
						,t1.[CoverageOptCode]
					FROM [GFCC_DM].[dbo].[LNK_Policy2Party] t1 ";
				strCom += " where t1.Policy_HK='" + policyHK + "' and t1.RelationRole_CD='" + relationCD +"'";

				ArrayList listOfParty = new ArrayList();
				try
				{
					using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(strCom, SharedUtils.GetDSNApp09()))
					{
						while (results.Read())
						{
							rCntr++;
							var Party_HK = SharedUtils.SafeGetString(results, 0);
							var ProductCode = SharedUtils.SafeGetString(results, 1);
							var CoverageCode = SharedUtils.SafeGetString(results, 2);
							var CoverageOptCode = SharedUtils.SafeGetString(results, 3);
							dynamic op = new ExpandoObject();
							op.ProductCode = ProductCode;
							op.CoverageCode = CoverageCode;
							op.CoverageOptCode = CoverageOptCode;

							op.Party = PartiesDAO.GetDetailPartyByPartyHK(Party_HK);
							listOfParty.Add(op);
						}
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e.ToString());
				}
				vRet = listOfParty;
			}
			
			return vRet;
		}

		public static dynamic GetDynPolicyProducerByPolicyHK(string policyHK)
		{
			dynamic vRet = null;

			string strCom = @"select 
							[Producer_HK],[relationrole_cd]
							from gfcc_dm.dbo.lnk_policy2producer ";
			strCom += " where [Policy_HK] ='" + policyHK + "' ";

			ArrayList listOfProducer = new ArrayList();
			try
			{
				using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(strCom, SharedUtils.GetDSNApp09()))
				{
					while (results.Read())
					{
						var Producer_HK = SharedUtils.SafeGetString(results, 0);
						var relationrolecd = SharedUtils.SafeGetString(results, 1);
						dynamic op = new ExpandoObject();
						
						op.Producer_HK = Producer_HK;
						op.RelationRole_CD = relationrolecd;

						op.Producer = ProducerDAO.GetDyntProducerByProducerHK(Producer_HK, "email,address,phone,iddoc");
						listOfProducer.Add(op);
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			vRet = listOfProducer;

			return vRet;
		}

			#endregion

			#region Policies list search 

			public static dynamic GetViewListPolicyByCondition(PolicyModela.SearchParameteraq s)
		{
			dynamic vRet = null;
			try
			{
				dynamic p = new ExpandoObject();
				p.Policies = GetDynPoliciesByCondition(s);
				vRet = new ExpandoObject();
				vRet.data = p;
			}
			catch (Exception e)
			{ Console.WriteLine(e.ToString()); }

			return vRet;
		}

		public static DataTable GetDynPoliciesByCondition(PolicyModela.SearchParameteraq s)
		{
			DataTable vRet = null;

			//if (s.search.policy is null) { return vRet; throw new ArgumentException("Parameter cannot be null", nameof(s.search.policy)); }
			//if (s.search.policy.policyStatus_CD is null) { return vRet; throw new ArgumentException("Parameter cannot be null", nameof(s.search.policy.policyStatus_CD)); }
			//if (s.search.policy.productCode is null) { return vRet; throw new ArgumentException("Parameter cannot be null", nameof(s.search.policy.productCode)); }
			string strCom = @"select top 20 [Policy_HK],[Entity_CD],[SystemSRC_CD],[PolicyNumber],[ProductCode]
							,[ProductDesc],[Currency_CD],[PaymentMode_CD],[PremiumAmt],[AnnualPremiumToDate]
							,[PolicyStatus_CD],convert(varchar(10), [Eff_DT], 121) as Eff_DT,convert(varchar(10), [Issue_DT], 121) as Issue_DT
							,convert(varchar(10), [Term_DT], 121) as Term_DT,convert(varchar(10), [ActualTerm_DT], 121) as ActualTerm_DT
						  FROM [GFCC_DM].[dbo].[Policy]  where 1=1 ";
			//strCom += " where lower(policyStatus_CD) ='" + s.search.policy.policyStatus_CD.ToLower() + "'";
			//strCom += " and lower(ProductCode) ='" + s.search.policy.productCode.ToLower() + "'";
			if (s.search.policy.policyStatus_CD != null)
			{
				if (s.search.policy.policyStatus_CD.Trim() != "")
				{
					strCom += " and lower(policyStatus_CD) ='" + s.search.policy.policyStatus_CD.ToLower() + "'";
				}
			}
			if (s.search.policy.productCode != null)
			{
				if (s.search.policy.productCode.Trim() != "")
				{
					strCom += " and lower(ProductCode) ='" + s.search.policy.productCode.ToLower() + "'";
				}
			}
			if (s.search.policy.policyNumber != null )
			{
				if (s.search.policy.policyNumber.Trim() != "" )
				{
					strCom += " and PolicyNumber ='" + s.search.policy.policyNumber + "' ";
				}
			}
			if (s.search.policy.start_Eff_DT != null && s.search.policy.end_Eff_DT != null)
			{
				if (s.search.policy.start_Eff_DT.Trim() != "" && s.search.policy.end_Eff_DT.Trim() != "")
				{
					strCom += " and Eff_DT between cast('" + s.search.policy.start_Eff_DT + "' as date) and cast('" + s.search.policy.end_Eff_DT + "'as date) ";
				}
			} else if (s.search.policy.start_Eff_DT != null && s.search.policy.end_Eff_DT is null)
			{
				if (s.search.policy.start_Eff_DT.Trim() != "" )
				{
					strCom += " and Eff_DT between cast('" + s.search.policy.start_Eff_DT + "' as date) and cast('" + s.search.policy.start_Eff_DT + "'as date) ";
				}
			}
			else if (s.search.policy.start_Eff_DT is null && s.search.policy.end_Eff_DT != null)
			{
				if (s.search.policy.end_Eff_DT.Trim() != "")
				{
					strCom += " and Eff_DT between cast('" + s.search.policy.end_Eff_DT + "' as date) and cast('" + s.search.policy.end_Eff_DT + "'as date) ";
				}
			}

			if (s.search.policy.start_Issue_DT != null && s.search.policy.end_Issue_DT != null)
			{
				if (s.search.policy.start_Issue_DT != "" && s.search.policy.end_Issue_DT != "")
				{
					strCom += " and  Issue_DT between cast('" + s.search.policy.start_Issue_DT + "' as date) and cast('" + s.search.policy.end_Issue_DT + "' as date) ";

				}
			}
			else if (s.search.policy.start_Issue_DT != null && s.search.policy.end_Issue_DT is null)
			{
				if (s.search.policy.start_Issue_DT.Trim() != "")
				{
					strCom += " and Issue_DT between cast('" + s.search.policy.start_Issue_DT + "' as date) and cast('" + s.search.policy.start_Issue_DT + "'as date) ";
				}
			}
			else if (s.search.policy.start_Issue_DT is null && s.search.policy.end_Issue_DT != null)
			{
				if (s.search.policy.end_Issue_DT.Trim() != "")
				{
					strCom += " and Issue_DT between cast('" + s.search.policy.end_Issue_DT + "' as date) and cast('" + s.search.policy.end_Issue_DT + "'as date) ";
				}
			}

			vRet = new DataTable();
			vRet = SharedUtils.ExecuteSqlCommandReturnDt(strCom, SharedUtils.GetDSNApp09());

			return vRet;
		}

		#endregion

		#region Policies with list of Claims by PolicyHK
		public static dynamic GetViewListClaimsByPolicyHK(string policyHK)
		{
			dynamic vRet = null;
			try
			{
				dynamic p = new ExpandoObject();
				p.Claims = GetDynPolicyClaimsByPolicyHK(policyHK);
				vRet = new ExpandoObject();
				vRet.data = p;
			}
			catch (Exception e)
			{ Console.WriteLine(e.ToString()); }

			return vRet;
		}
		public static dynamic GetDynPolicyClaimsByPolicyHK(string policyHK)
		{
			dynamic vRet = null;

			string strCom = @"select 
							t2.[Claim_HK] ,t2.[SystemSRC_CD],t1.PolicyNumber,
							t2.[ClaimNumber],t2.[ClaimType_CD],t2.[ClaimStatus_CD]
							,convert(varchar(10), t2.[Report_DT], 121) as [Report_DT]
							,convert(varchar(10), t2.[Finalized_DT], 121) as [Finalized_DT]
							,convert(varchar(10), t2.[Admission_DT], 121) as [Admission_DT]
							,convert(varchar(10), t2.[Discharge_DT], 121) as [Discharge_DT]
							,t2.[ProductPlan_CD],t2.[ClaimCurrency_CD],t2.[ClaimedAmt]
							,t2.[EligibleAmt],t2.[PaidCurrency_CD],t2.[PaidAmt],t2.[Entity_CD]
							from GFCC_DM.dbo.Policy t1
							inner join gfcc_dm.dbo.claims t2 on t1.policynumber=t2.policynumber   ";
			strCom += " where t1.[Policy_HK] ='" + policyHK + "' ";
			//DataTable dt = new DataTable();
			//dt = SharedUtils.ExecuteSqlCommandReturnDt(strCom, SharedUtils.GetDSNApp09());
			//string claimHK = dt.Rows[0][2].ToString();

			ArrayList listOfClaims = new ArrayList();
			try
			{
				using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(strCom, SharedUtils.GetDSNApp09()))
				{
					while (results.Read())
					{
						var Claim_HK = SharedUtils.SafeGetString(results, 0);
						var SystemSRC_CD = SharedUtils.SafeGetString(results, 1);
						var PolicyNumber = SharedUtils.SafeGetString(results, 2);
						var ClaimNumber = SharedUtils.SafeGetString(results, 3);
						var ClaimType_CD = SharedUtils.SafeGetString(results, 4);
						var ClaimStatus_CD = SharedUtils.SafeGetString(results, 5);
						var Report_DT = SharedUtils.SafeGetString(results, 6); 
						var Finalized_DT = SharedUtils.SafeGetString(results, 7);
						var Admission_DT = SharedUtils.SafeGetString(results, 8);
						var Discharge_DT = SharedUtils.SafeGetString(results, 9);
						var ProductPlan_CD = SharedUtils.SafeGetString(results, 10);
						var ClaimCurrency_CD = SharedUtils.SafeGetString(results, 11);
						var claimedamt = (!results.IsDBNull(12)) ? results.GetDecimal(12) : 0;
						var EligibleAmt = (!results.IsDBNull(13)) ? results.GetDecimal(13) : 0;
						var PaidCurrency_CD = SharedUtils.SafeGetString(results, 14);
						var PaidAmt = (!results.IsDBNull(15)) ? results.GetDecimal(15) : 0;
						var Entity_CD = SharedUtils.SafeGetString(results, 16);
						dynamic op = new ExpandoObject();
						op.Claim_HK = Claim_HK;
						op.SystemSRC_CD = SystemSRC_CD;
						op.PolicyNumber = PolicyNumber;
						op.ClaimNumber = ClaimNumber;
						op.ClaimType_CD = ClaimType_CD;
						op.ClaimStatus_CD = ClaimStatus_CD;
						op.Report_DT = Report_DT;
						op.Finalized_DT = Finalized_DT;
						op.Admission_DT = Admission_DT;
						op.Discharge_DT = Discharge_DT;
						op.ProductPlan_CD = ProductPlan_CD;
						op.ClaimCurrency_CD = ClaimCurrency_CD;
						op.claimedamt = claimedamt;
						op.EligibleAmt = EligibleAmt;
						op.PaidCurrency_CD = PaidCurrency_CD;
						op.PaidAmt = PaidAmt;
						op.Entity_CD = Entity_CD;

						op.Banks = GetBank(Claim_HK);
						op.Participants = GetParticipants(Claim_HK);
						op.Providers = GetProvider(Claim_HK);
						op.StatusLogs = GetStatusLog(Claim_HK);
						listOfClaims.Add(op);
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			vRet = listOfClaims;

			return vRet;
		}
		public static DataTable GetBank(string claimHK)
		{
			DataTable vRet = null;
			string sqlcom = @" select 
				cb.BankName,cb.Branch,cb.AccountNumber,'' AccountName
				from ClaimBank cb
				where cb.claim_hk='" + claimHK + "'";
			vRet = new DataTable();
			vRet = SharedUtils.ExecuteSqlCommandReturnDt(sqlcom, SharedUtils.GetDSNApp09());
			return vRet;
		}
		public static DataTable GetParticipants(string claimHK)
		{
			DataTable vRet = null;
			string sqlcom = @" select 
				cp.RelationRole_CD,cp.Name,cp.Birth_DT,cp.Gender,cp.Email,cp.Phone,cp.Address
				from ClaimHParticipant cp
				where cp.claim_hk='" + claimHK + "'";
			vRet = new DataTable();
			vRet = SharedUtils.ExecuteSqlCommandReturnDt(sqlcom, SharedUtils.GetDSNApp09());
			return vRet;
		}
		public static DataTable GetProvider(string claimHK)
		{
			DataTable vRet = null;
			string sqlcom = @" select 
				cpv.Name,cpv.ProviderType_CD,cpv.TreatmentType_CD,cpv.Remark
				from ClaimProvider cpv
				where cpv.claim_hk='" + claimHK + "'";
			vRet = new DataTable();
			vRet = SharedUtils.ExecuteSqlCommandReturnDt(sqlcom, SharedUtils.GetDSNApp09());
			return vRet;
		}
		public static DataTable GetStatusLog(string claimHK)
		{
			DataTable vRet = null;
			string sqlcom = @" select 
				cs.ClaimStatus_CD,cs.Remark,cs.Start_DT,cs.End_DT
				from [GFCC_DM].[dbo].[ClaimStatusLog] cs
				where cs.claim_hk='" + claimHK + "'";
			vRet = new DataTable();
			vRet = SharedUtils.ExecuteSqlCommandReturnDt(sqlcom, SharedUtils.GetDSNApp09());
			return vRet;
		}

		#endregion

		#region Policy with Dynamic attributes object 

		public static dynamic GetViewPolicydetailDynObjByPolicyHK(string policyHK,string attributesObject)
		{
			dynamic vRet = null;
			try
			{
				dynamic p = new ExpandoObject();
				p.Policies = GetPolicyDynObjbyPolicyHK(policyHK, attributesObject);
				vRet = new ExpandoObject();
				vRet.data = p;
			}
			catch (Exception e)
			{ Console.WriteLine(e.ToString()); }

			return vRet;
		}

		public static dynamic GetPolicyDynObjbyPolicyHK(string policyHK, string attributesObject)
		{
			dynamic vRet = null;

			int rCntr = 0;
			string strCom = @"select 
							t1.[Policy_HK],t1.[Entity_CD],t1.[SystemSRC_CD],t1.[PolicyNumber],t1.[ProductCode]
							,t1.[ProductDesc],t1.[Currency_CD],t1.[PaymentMode_CD],t1.[PremiumAmt],t1.[AnnualPremiumToDate]
							,t1.[PolicyStatus_CD],convert(varchar(10), t1.[Eff_DT], 121),convert(varchar(10), t1.[Issue_DT], 121)
							,convert(varchar(10), t1.[Term_DT], 121),convert(varchar(10), t1.[ActualTerm_DT], 121) 
						  FROM [GFCC_DM].[dbo].[Policy] t1 ";
			strCom += " where t1.Policy_HK='" + policyHK + "' ";
			try
			{
				using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(strCom, SharedUtils.GetDSNApp09()))
				{
					while (results.Read())
					{
						rCntr++;
						var Policy_HK = SharedUtils.SafeGetString(results, 0);
						var Entity_CD = SharedUtils.SafeGetString(results, 1);
						var SystemSRC_CD = SharedUtils.SafeGetString(results, 2);
						var PolicyNumber = SharedUtils.SafeGetString(results, 3);
						var ProductCode = SharedUtils.SafeGetString(results, 4);
						var ProductDesc = SharedUtils.SafeGetString(results, 5);
						var Currency_CD = SharedUtils.SafeGetString(results, 6);
						var PaymentMode_CD = SharedUtils.SafeGetString(results, 7);
						var PremiumAmt = (!results.IsDBNull(8)) ? results.GetDecimal(8) : 0;
						var AnnualPremiumToDate = (!results.IsDBNull(9)) ? results.GetDecimal(9) : 0;
						var PolicyStatus_CD = SharedUtils.SafeGetString(results, 10);
						var Eff_DT = SharedUtils.SafeGetString(results, 11);
						var Issue_DT = SharedUtils.SafeGetString(results, 12);
						var Term_DT = SharedUtils.SafeGetString(results, 13);
						var ActualTerm_DT = SharedUtils.SafeGetString(results, 14);
						vRet = new ExpandoObject();
						vRet.Policy_HK = Policy_HK;
						vRet.Entity_CD = Entity_CD;
						vRet.SystemSRC_CD = SystemSRC_CD;
						vRet.PolicyNumber = PolicyNumber;
						vRet.ProductCode = ProductCode;
						vRet.ProductDesc = ProductDesc;
						vRet.Currency_CD = Currency_CD;
						vRet.PaymentMode_CD = PaymentMode_CD;
						vRet.PremiumAmt = PremiumAmt;
						vRet.AnnualPremiumToDate = AnnualPremiumToDate;
						vRet.PolicyStatus_CD = PolicyStatus_CD;
						vRet.Eff_DT = Eff_DT;
						vRet.Issue_DT = Issue_DT;
						vRet.Term_DT = Term_DT;
						vRet.ActualTerm_DT = ActualTerm_DT;

						char[] delimiterChars = { ',' };
						string[] ws = attributesObject.Split(delimiterChars);
						foreach (string w in ws)
						{
							switch (w)
							{
								case "coverages":
									vRet.Coverages = GetListofCoverageByPolicyIdDT(Policy_HK);
									break;
								case "owner":
									vRet.Owner = GetPolicyOwnerPartyDetailByPolHK(Policy_HK); ;
									break;
								case "insureds":
									if (SystemSRC_CD.ToUpper().Trim() == "G4IAM")
									{
										vRet.InsuredList = GetPolicyPartiesDetailByPolHK(Policy_HK, "INS", true);
									}
									else {
										vRet.Insureds = GetPolicyPartiesDetailByPolHK(Policy_HK, "INS", false);
									}
									break;
								case "beneficiaries":
									if (SystemSRC_CD.ToUpper().Trim() == "G4IAM")
									{
										vRet.BeneficiaryList = GetPolicyPartiesDetailByPolHK(Policy_HK, "BFY",true);
									}
									else {
										vRet.Beneficiary = GetPolicyPartiesDetailByPolHK(Policy_HK, "BFY",false);
									}
									break;
								case "dependants":
									if (SystemSRC_CD.ToUpper().Trim() == "G4IAM")
									{
										vRet.DependantList = GetPolicyPartiesDetailByPolHK(Policy_HK, "DEP", true);
									}
									else
									{
										vRet.Dependants = GetPolicyPartiesDetailByPolHK(Policy_HK, "DEP", false);
									}
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

		#region Policies with list of Financial Transaction 

		public static dynamic GetViewListPoliciesByPolicyHK(string policyHK)
		{
			dynamic vRet = null;
			try
			{
				dynamic p = new ExpandoObject();
				p.Policies = GetDynPolicyByPolicyHK(policyHK);
				vRet = new ExpandoObject();
				vRet.data = p;
			}
			catch (Exception e)
			{ Console.WriteLine(e.ToString()); }

			return vRet;
		}
		
		public static dynamic GetDynPolicyByPolicyHK(string policyHK)
		{
			dynamic vRet = null;

			int rCntr = 0;
			string strCom = @"select 
							t1.[Policy_HK],t1.[Entity_CD],t1.[SystemSRC_CD],t1.[PolicyNumber],t1.[ProductCode]
							,t1.[ProductDesc],t1.[Currency_CD],t1.[PaymentMode_CD],t1.[PremiumAmt],t1.[AnnualPremiumToDate]
							,t1.[PolicyStatus_CD],convert(varchar(10), t1.[Eff_DT], 121),convert(varchar(10), t1.[Issue_DT], 121)
							,convert(varchar(10), t1.[Term_DT], 121),convert(varchar(10), t1.[ActualTerm_DT], 121) 
						  FROM [GFCC_DM].[dbo].[Policy] t1 ";
			strCom += " where t1.Policy_HK='" + policyHK + "' ";
			ArrayList listOfPolicies = new ArrayList();
			try
			{
				using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(strCom, SharedUtils.GetDSNApp09()))
				{
					while (results.Read())
					{
						rCntr++;
						var Policy_HK = SharedUtils.SafeGetString(results, 0);
						var Entity_CD = SharedUtils.SafeGetString(results, 1);
						var SystemSRC_CD = SharedUtils.SafeGetString(results, 2);
						var PolicyNumber = SharedUtils.SafeGetString(results, 3);
						var ProductCode = SharedUtils.SafeGetString(results, 4);
						var ProductDesc = SharedUtils.SafeGetString(results, 5);
						var Currency_CD = SharedUtils.SafeGetString(results, 6);
						var PaymentMode_CD = SharedUtils.SafeGetString(results, 7);
						var PremiumAmt = (!results.IsDBNull(8)) ? results.GetDecimal(8) : 0;
						var AnnualPremiumToDate = (!results.IsDBNull(9)) ? results.GetDecimal(9) : 0;
						var PolicyStatus_CD = SharedUtils.SafeGetString(results, 10);
						var Eff_DT = SharedUtils.SafeGetString(results, 11);
						var Issue_DT = SharedUtils.SafeGetString(results, 12);
						var Term_DT = SharedUtils.SafeGetString(results, 13);
						var ActualTerm_DT = SharedUtils.SafeGetString(results, 14);
						dynamic op = new ExpandoObject();
						op.Policy_HK = Policy_HK;
						op.Entity_CD = Entity_CD;
						op.SystemSRC_CD = SystemSRC_CD;
						op.PolicyNumber = PolicyNumber;
						op.ProductCode = ProductCode;
						op.ProductDesc = ProductDesc;
						op.Currency_CD = Currency_CD;
						op.PaymentMode_CD = PaymentMode_CD;
						op.PremiumAmt = PremiumAmt;
						op.AnnualPremiumToDate = AnnualPremiumToDate;
						op.PolicyStatus_CD = PolicyStatus_CD;
						op.Eff_DT = Eff_DT;
						op.Issue_DT = Issue_DT;
						op.Term_DT = Term_DT;
						op.ActualTerm_DT = ActualTerm_DT;

						op.FinTrx = GetFinTracts(PolicyNumber);
						listOfPolicies.Add(op);
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}

			vRet = listOfPolicies;
			return vRet;
		}

		public static DataTable GetFinTracts(string policyNumber)
		{
			DataTable vRet = null;
			string sqlcom = @" select 
				[SystemSRC_CD] ,[TrxFinAct_ID],convert(varchar(10), [FinAct_DT], 121) as [FinAct_DT]
				,convert(varchar(10), [FinActEff_DT], 121) as [FinActEff_DT],[FinActType_CD],[DebitCredit_Ind]
				,[FinActTypeDesc],[Channel_CD],[PolicyNumber],[Party_HK],[PaymentMode_CD],[PaymentMethod_CD]
				,[TrxCurrency_CD],[IntPostingRate],convert(varchar(10), [IntPosting_DT], 121) as [IntPosting_DT]
				,[TrxGrossAmt],[TrxNetAmt],[TrxFeeAmt],[OriginCurrency_CD],[OriginGrossAmt]
				from [GFCC_DM].[dbo].[TRX_FinAct]
				where [PolicyNumber]='" + policyNumber + "'";
			vRet = new DataTable();
			vRet = SharedUtils.ExecuteSqlCommandReturnDt(sqlcom, SharedUtils.GetDSNApp09());
			return vRet;
		}

		#endregion
	}
}
