using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Models;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Services.Base;
using System.Data;
using static ESD.Extensions.ServiceExtensions;

namespace ESD.Services.EDI
{
    public interface IQ2MgtService
    {
        Task<ResponseModel<IEnumerable<pportal_qual02_infoDto>?>> GetAll(pportal_qual02_infoDto pageInfo);

    }
    [ScopedRegistration]
    public class Q2MgtService : IQ2MgtService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public Q2MgtService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public async Task<ResponseModel<IEnumerable<pportal_qual02_infoDto>?>> GetAll(pportal_qual02_infoDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<pportal_qual02_infoDto>?>();
                var param = new DynamicParameters();
                param.Add("@ITEM_CODE", model.ITEM_CODE);
                param.Add("@StartDate", model.StartDate);
                param.Add("@EndDate", model.EndDate);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);

                var sql = @" IF(@page <= 0)
		                        BEGIN
			                        SET @page = 1;
		                        END

                            IF(@pageSize <= 0 )
		                        BEGIN
			                        SET @pageSize = 2147483647;
		                        END
                            DECLARE @skipRows int = (@page - 1) * @pageSize; 
                        SELECT  ROW_NUMBER() OVER(ORDER BY p.YYYYMMDDHH  desc) AS id,
                                [BUYER_COMPANY]
                                      ,[BUYER_DIVISION]
                                      ,[SELLER_COMPANY]
                                      ,[GBM]
                                      ,[PPORTAL_ITEM_GROUP]
                                      ,[QMS_ITEM_GROUP]
                                      ,p.[ITEM_CODE]
                                      ,p.[CTQ_NO]
                                     
                                      ,[YYYYMMDDHH]
                                      ,[TRANSACTION_ID]
                                      ,[STATUS]
                                      ,[ERR_FLAG]
                                      ,[SUP_DATE_ADDED]
                                      ,[SUP_TIME_ADDED]
                                      ,[SUP_CREATE_DATE]
                                      ,[SUP_CREATE_TIME]
                                      ,[SUP_SEND_DATE]
                                      ,[SUP_SEND_TIME]
                                      ,[PRC_QUAL_INFO01]
                                      ,[PRC_QUAL_INFO02]
                                      ,[PRC_QUAL_INFO03]
                                      ,[PRC_QUAL_INFO04]
                                      ,[PRC_QUAL_INFO05]
                                      ,[PRC_QUAL_INFO06]
                                      ,[PRC_QUAL_INFO07]
                                      ,[PRC_QUAL_INFO08]
                                      ,[PRC_QUAL_INFO09]
                                      ,[PRC_QUAL_INFO10]
                                      ,[PRC_QUAL_INFO11]
                                      ,[PRC_QUAL_INFO12]
                                      ,[PRC_QUAL_INFO13]
                                      ,[PRC_QUAL_INFO14]
                                      ,[PRC_QUAL_INFO15]
                                    ,cd.commonDetailNameVi TRAND_TP_NAME
	                                from [dbo].[pportal_qual02_info] p
                                   	join pportal_qual02_policy po on p.ITEM_CODE = po.ITEM_CODE   AND P.CTQ_NO = PO.ctq_no
                          join sysTbl_CommonDetail cd on po.TRAND_TP = cd.commonDetailCode and cd.commonMasterCode = '001'
	                                where
	                                p.ITEM_CODE like CONCAT('%',(@ITEM_CODE),'%')
	                                AND (@StartDate = '' OR p.[YYYYMMDDHH] like concat('%', @StartDate , '%') )
	                                AND (@EndDate = '' OR p.[YYYYMMDDHH] like concat('%', @EndDate , '%') )		
	                                order by p.YYYYMMDDHH desc , p.ITEM_CODE , cast(P.CTQ_NO as int)  
	                                OFFSET @skipRows ROWS FETCH NEXT @pageSize ROWS ONLY;";

                var data = await _sqlDataAccess.LoadDataUsingRawQueryEDI<pportal_qual02_infoDto>(sql, param);
                returnData.Data = data;
                returnData.TotalRow = 10000;
                if (!data.Any())
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                }
                return returnData;
                //var returnData = new ResponseModel<IEnumerable<pportal_qual02_infoDto>?>();
                //string proc = "Usp_Q2Mgt_GetAll"; 
                //var param = new DynamicParameters();
                //param.Add("@page", model.page);
                //param.Add("@pageSize", model.pageSize);
                //param.Add("@ITEM_CODE", model.ITEM_CODE);
                //param.Add("@TRAND_TP", model.TRAND_TP);
                //param.Add("@StartDate", model.StartDate);
                //param.Add("@EndDate", model.EndDate);
                //param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                //var data = await _sqlDataAccess.LoadDataUsingStoredProcedureEDI<pportal_qual02_infoDto>(proc, param);
                //returnData.Data = data;
                //returnData.TotalRow = param.Get<int>("totalRow");
                //if (!data.Any())
                //{
                //    returnData.HttpResponseCode = 204;
                //    returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                //}
                //return returnData;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        
    }
}
