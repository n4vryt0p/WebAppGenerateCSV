using System.Collections;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Dynamic;
using static AF.DAL.Model.ClaimModel;


namespace AF.DAL
{
	public class ClaimDAO
	{

		#region Financial Transaction list search
		public static dynamic GetViewListClaimByCondition(SearchParametera s)
		{
			dynamic vRet = null;
			try
			{
				dynamic p = new ExpandoObject();
				p.Claims = GetDyntClaimsByCondition(s);
				vRet = new ExpandoObject();
				vRet.data = p;
			}
			catch (Exception e)
			{ Console.WriteLine(e.ToString()); }

			return vRet;
		}

		public static DataTable GetDyntClaimsByCondition(SearchParametera s)
		{
			DataTable vRet = null;

			string strCom = @"select top 20 
				[Claim_HK] ,[SystemSRC_CD]
				,[ClaimNumber],[ClaimType_CD],[ClaimStatus_CD]
				,convert(varchar(10), [Report_DT], 121) as [Report_DT]
				,convert(varchar(10), [Finalized_DT], 121) as [Finalized_DT]
				,convert(varchar(10), [Admission_DT], 121) as [Admission_DT]
				,convert(varchar(10), [Discharge_DT], 121) as [Discharge_DT]
				,[ProductPlan_CD],[ClaimCurrency_CD],[ClaimedAmt]
				,[EligibleAmt],[PaidCurrency_CD],[PaidAmt],[Entity_CD]
				from GFCC_DM.dbo.claims where 1=1 ";
			if (s.search.claim.policyNumber != null)
			{
				if (s.search.claim.policyNumber.Trim() != "")
				{
					strCom += " and lower(PolicyNumber) ='" + s.search.claim.policyNumber.ToLower() + "'";
				}
			}
			if (s.search.claim.claimNumber != null)
			{
				if (s.search.claim.claimNumber.Trim() != "")
				{
					strCom += " and lower(ClaimNumber) ='" + s.search.claim.claimNumber.ToLower() + "'";
				}
			}

			if (s.search.claim.claimType_CD != null)
			{
				if (s.search.claim.claimType_CD.Trim() != "")
				{
					strCom += " and lower(ClaimType_CD) ='" + s.search.claim.claimType_CD.ToLower() + "'";
				}
			}
			if (s.search.claim.claimStatus_CD != null)
			{
				if (s.search.claim.claimStatus_CD.Trim() != "")
				{
					strCom += " and lower(ClaimStatus_CD) ='" + s.search.claim.claimStatus_CD.ToLower() + "'";
				}
			}

			if (s.search.claim.start_Report_DT != null && s.search.claim.end_Report_DT != null)
			{
				if (s.search.claim.start_Report_DT.Trim() != "" && s.search.claim.end_Report_DT.Trim() != "")
				{
					strCom += " and [Report_DT] between cast('" + s.search.claim.start_Report_DT + "' as date) and cast('" + s.search.claim.end_Report_DT + "'as date) ";
				}
			}
			else if (s.search.claim.start_Report_DT != null && s.search.claim.end_Report_DT is null)
			{
				if (s.search.claim.start_Report_DT.Trim() != "")
				{
					strCom += " and [Report_DT] between cast('" + s.search.claim.start_Report_DT + "' as date) and cast('" + s.search.claim.start_Report_DT + "'as date) ";
				}
			}
			else if (s.search.claim.start_Report_DT is null && s.search.claim.end_Report_DT != null)
			{
				if (s.search.claim.end_Report_DT.Trim() != "")
				{
					strCom += " and [Report_DT] between cast('" + s.search.claim.end_Report_DT + "' as date) and cast('" + s.search.claim.end_Report_DT + "'as date) ";
				}
			}


			vRet = new DataTable();
			vRet = SharedUtils.ExecuteSqlCommandReturnDt(strCom, SharedUtils.GetDSNApp09());

			return vRet;
		}

		#endregion

		#region Claims Transaction By ID and dynamic EmbedObjects

		public static dynamic GetViewLisClaimByID(string ClaimHK, string attributesObject)
		{
			dynamic vRet = null;
			try
			{
				dynamic p = new ExpandoObject();
				p.Claims = GetDyntClaimsByID(ClaimHK, attributesObject);
				vRet = new ExpandoObject();
				vRet.data = p;
			}
			catch (Exception e)
			{ Console.WriteLine(e.ToString()); }

			return vRet;
		}

		public static dynamic GetDyntClaimsByID(string ClaimHK, string attributesObject)
		{
			dynamic vRet = null;

			string strCom = @"select
				[Claim_HK] ,[SystemSRC_CD],[ClaimNumber],[ClaimType_CD]
				,PolicyNumber,[ClaimStatus_CD]
				,convert(varchar(10), [Report_DT], 121) as [Report_DT]
				,convert(varchar(10), [Finalized_DT], 121) as [Finalized_DT]
				,convert(varchar(10), [Admission_DT], 121) as [Admission_DT]
				,convert(varchar(10), [Discharge_DT], 121) as [Discharge_DT]
				,[ProductPlan_CD],[ClaimCurrency_CD],[ClaimedAmt]
				,[EligibleAmt],[PaidCurrency_CD],[PaidAmt],[Entity_CD]
				from GFCC_DM.dbo.claims where 1=1 ";
			if (ClaimHK != null)
			{
				if (ClaimHK.Trim() != "")
				{
					strCom += " and Claim_HK ='" + ClaimHK + "'";
				}
			}
			else
			{
				return vRet;
			}

			//vRet = new DataTable();
			//vRet = SharedUtils.ExecuteSqlCommandReturnDt(strCom, SharedUtils.GetDSNApp09());

			ArrayList listOfClaims = new ArrayList();
			try
			{
				using (SqlDataReader results = SharedUtils.ExecuteSqlCommand(strCom, SharedUtils.GetDSNApp09()))
				{
					while (results.Read())
					{
						var Claim_HK = SharedUtils.SafeGetString(results, 0);
						var SystemSRC_CD = SharedUtils.SafeGetString(results, 1);
						var ClaimNumber = SharedUtils.SafeGetString(results, 2);
						var ClaimType_CD = SharedUtils.SafeGetString(results, 3);
						var PolicyNumber = SharedUtils.SafeGetString(results, 4);
						var ClaimStatus_CD = SharedUtils.SafeGetString(results, 5);
						var Report_DT = SharedUtils.SafeGetString(results, 6);
						var Finalized_DT = SharedUtils.SafeGetString(results, 7);
						var Admission_DT = SharedUtils.SafeGetString(results,8);
						var Discharge_DT = SharedUtils.SafeGetString(results, 9);
						var ProductPlan_CD = SharedUtils.SafeGetString(results, 10);
						var ClaimCurrency_CD = SharedUtils.SafeGetString(results, 11);
						var ClaimedAmt = (!results.IsDBNull(12)) ? results.GetDecimal(12) : 0 ;
						var EligibleAmt = (!results.IsDBNull(13)) ? results.GetDecimal(13) : 0;
						var PaidCurrency_CD = SharedUtils.SafeGetString(results, 14);
						var PaidAmt = (!results.IsDBNull(15)) ? results.GetDecimal(15) : 0;
						var Entity_CD = SharedUtils.SafeGetString(results, 16);
						dynamic op = new ExpandoObject();
						op.Claim_HK = Claim_HK;
						op.SystemSRC_CD = SystemSRC_CD;
						op.ClaimNumber = ClaimNumber;
						op.ClaimType_CD = ClaimType_CD;
						op.PolicyNumber = PolicyNumber;
						op.ClaimStatus_CD = ClaimStatus_CD;
						op.Report_DT = Report_DT;
						op.Finalized_DT = Finalized_DT;
						op.Admission_DT = Admission_DT;
						op.Discharge_DT = Discharge_DT;
						op.ProductPlan_CD = ProductPlan_CD;
						op.ClaimCurrency_CD = ClaimCurrency_CD;
						op.ClaimedAmt = ClaimedAmt;
						op.EligibleAmt = EligibleAmt;
						op.PaidCurrency_CD = PaidCurrency_CD;
						op.PaidAmt = PaidAmt;
						op.Entity_CD = Entity_CD;

						char[] delimiterChars = { ',' };
						string[] ws = attributesObject.Split(delimiterChars);
						foreach (string w in ws)
						{
							switch (w)
							{
								case "participants":
									op.Participants = PolicyDAO.GetParticipants(ClaimHK);
									break;
								case "banks":
									op.Banks = PolicyDAO.GetBank(ClaimHK);
									break;
								case "providers":
									op.Providers = PolicyDAO.GetProvider(ClaimHK); ;
									break;
								case "statuslogs":
									op.StatusLogs = PolicyDAO.GetStatusLog(ClaimHK);
									break;
							}

						}
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

		#endregion

	}
}
