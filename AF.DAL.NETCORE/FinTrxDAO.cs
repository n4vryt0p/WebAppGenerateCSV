using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Dynamic;
using static AF.DAL.Model.FinTrxModel;

namespace AF.DAL
{
	public class FinTrxDAO
	{
		#region Financial Transaction list search
		public static dynamic GetViewListFinTrxByCondition(SearchParameterac s)
		{
			dynamic vRet = null;
			try
			{
				dynamic p = new ExpandoObject();
				p.FinTrx = GetDyntFinTrxsByCondition(s);
				vRet = new ExpandoObject();
				vRet.data = p;
			}
			catch (Exception e)
			{ Console.WriteLine(e.ToString()); }

			return vRet;
		}

		public static DataTable GetDyntFinTrxsByCondition(SearchParameterac s)
		{
			DataTable vRet = null;

			string strCom = @"select top 20 
				[SystemSRC_CD] ,[TrxSRC_ID],[TrxFinAct_ID],convert(varchar(10), [FinAct_DT], 121) as [FinAct_DT]
				,convert(varchar(10), [FinActEff_DT], 121) as [FinActEff_DT],[FinActType_CD],[DebitCredit_Ind]
				,[FinActTypeDesc],[Channel_CD],[PolicyNumber],[Party_HK],[PaymentMode_CD],[PaymentMethod_CD]
				,[TrxCurrency_CD],[IntPostingRate],convert(varchar(10), [IntPosting_DT], 121) as [IntPosting_DT]
				,[TrxGrossAmt],[TrxNetAmt],[TrxFeeAmt],[OriginCurrency_CD],[OriginGrossAmt]
						  FROM [GFCC_DM].[dbo].[TRX_FinAct] where 1=1 ";
			if (s.search.finTrx.DebitCredit_Ind != null)
			{
				if (s.search.finTrx.DebitCredit_Ind.Trim() != "")
				{
					strCom += " and lower(DebitCredit_Ind) ='" + s.search.finTrx.DebitCredit_Ind.ToLower() + "'";
				}
			}
			if (s.search.finTrx.PolicyNumber != null)
			{
				if (s.search.finTrx.PolicyNumber.Trim() != "")
				{
					strCom += " and PolicyNumber ='" + s.search.finTrx.PolicyNumber + "'";
				}
			}
			if (s.search.finTrx.TrxSRC_ID != null)
			{
				if (s.search.finTrx.TrxSRC_ID.Trim() != "")
				{
					strCom += " and TrxSRC_ID ='" + s.search.finTrx.TrxSRC_ID + "'";
				}
			}
			if (s.search.finTrx.trxType_CD!= null)
			{
				if (s.search.finTrx.trxType_CD.Trim() != "")
				{
					strCom += " and lower(FinActType_CD) ='" + s.search.finTrx.trxType_CD.ToLower() + "'";
				}
			}
			if (s.search.finTrx.trxStatus_CD != null)
			{
				if (s.search.finTrx.trxStatus_CD.Trim() != "")
				{
					strCom += " and lower(FinActStatus_CD) ='" + s.search.finTrx.trxStatus_CD.ToLower() + "'";
				}
			}
			
			if (s.search.finTrx.start_DT != null && s.search.finTrx.end_DT != null)
			{
				if (s.search.finTrx.start_DT.Trim() != "" && s.search.finTrx.end_DT.Trim() != "")
				{
					strCom += " and FinAct_DT between cast('" + s.search.finTrx.start_DT + "' as date) and cast('" + s.search.finTrx.end_DT + "'as date) ";
				}
			}
			else if (s.search.finTrx.start_DT != null && s.search.finTrx.end_DT is null)
			{
				if (s.search.finTrx.start_DT.Trim() != "")
				{
					strCom += " and FinAct_DT between cast('" + s.search.finTrx.start_DT + "' as date) and cast('" + s.search.finTrx.start_DT + "'as date) ";
				}
			}
			else if (s.search.finTrx.start_DT is null && s.search.finTrx.end_DT != null)
			{
				if (s.search.finTrx.end_DT.Trim() != "")
				{
					strCom += " and FinAct_DT between cast('" + s.search.finTrx.end_DT + "' as date) and cast('" + s.search.finTrx.end_DT + "'as date) ";
				}
			}

			
			vRet = new DataTable();
			vRet = SharedUtils.ExecuteSqlCommandReturnDt(strCom, SharedUtils.GetDSNApp09());

			return vRet;
		}

		#endregion

		#region Financial Transaction By ID 

		public static dynamic GetViewListFinTrxByID(string fintrx_id)
		{
			dynamic vRet = null;
			try
			{
				dynamic p = new ExpandoObject();
				p.FinTrx = GetDyntFinTrxsByID(fintrx_id);
				vRet = new ExpandoObject();
				vRet.data = p;
			}
			catch (Exception e)
			{ Console.WriteLine(e.ToString()); }

			return vRet;
		}

		public static DataTable GetDyntFinTrxsByID(string fintrx_id)
		{
			DataTable vRet = null;

			string strCom = @"select top 20 
				[SystemSRC_CD],[TrxSRC_ID],[TrxFinAct_ID],convert(varchar(10), [FinAct_DT], 121) as [FinAct_DT]
				,convert(varchar(10), [FinActEff_DT], 121) as [FinActEff_DT],[FinActType_CD],[DebitCredit_Ind]
				,[FinActTypeDesc],[Channel_CD],[PolicyNumber],[Party_HK],[PaymentMode_CD],[PaymentMethod_CD]
				,[TrxCurrency_CD],[IntPostingRate],convert(varchar(10), [IntPosting_DT], 121) as [IntPosting_DT]
				,[TrxGrossAmt],[TrxNetAmt],[TrxFeeAmt],[OriginCurrency_CD],[OriginGrossAmt]
				FROM [GFCC_DM].[dbo].[TRX_FinAct] where 1=1 ";

			if (fintrx_id != null)
			{
				if (fintrx_id.Trim() != "")
				{
					//strCom += " and TrxSRC_ID ='" + TrxSRC_ID + "'"; //[TrxFinAct_ID]
					strCom += " and TrxFinAct_ID ='" + fintrx_id + "'"; //[TrxFinAct_ID]
				}
			}
			else {
				return vRet;
			}

			vRet = new DataTable();
			vRet = SharedUtils.ExecuteSqlCommandReturnDt(strCom, SharedUtils.GetDSNApp09());

			return vRet;
		}

		#endregion
	}
}
