using Dapper;
using Newtonsoft.Json;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Helpers;
using ESD.Models;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Models.Validators;
using ESD.Services.Base;
using System.Data;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using static ESD.Extensions.ServiceExtensions;

namespace ESD.Services.QMS.Holding
{
    public interface IHoldFinishGoodService
    {
        Task<ResponseModel<IEnumerable<dynamic>?>> GetAll(HoldLogFGDto model);
        Task<ResponseModel<HoldLogFGDto?>> Hold(HoldLogFGDto model);
        Task<ResponseModel<HoldLogFGDto?>> UnHold(HoldLogFGDto model);
        Task<ResponseModel<HoldLogFGDto?>> Scrap(HoldLogFGDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetPrintFG(List<long>? listQR);
    }
    [ScopedRegistration]
    public class HoldFinishGoodService : IHoldFinishGoodService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public HoldFinishGoodService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetAll(HoldLogFGDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_HoldFinishGood_GetAll"; 
                var param = new DynamicParameters();
                param.Add("@BuyerQR", model.BuyerQR);
                param.Add("@LotNo", model.LotNo);
                param.Add("@FQCSOName", model.FQCSOName);
                param.Add("@BoxQR", model.BoxQR);
                param.Add("@SemiLotCode", model.SemiLotCode);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);
                returnData.Data = data;
                returnData.TotalRow = param.Get<int>("totalRow");
                if (!data.Any())
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                }
                return returnData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseModel<HoldLogFGDto?>> Hold(HoldLogFGDto model)
        {

            try
            {
                var returnData = new ResponseModel<HoldLogFGDto?>();

                string proc = "Usp_HoldFinishGood_Hold";
                var param = new DynamicParameters();
                //param.Add("@HoldLogId", model.HoldLogId);
                //param.Add("@FGInventoryId", model.FGInventoryId);
                param.Add("@ListId", ParameterTvp.GetTableValuedParameter_BigInt(model.ListId));
                param.Add("@Reason", model.Reason);
                param.Add("@IsPicture", model.IsPicture);
                param.Add("@FileName", model.FileName);
                param.Add("@createdBy", model.createdBy);
                param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

                var result = await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);

                returnData.ResponseMessage = result;
                switch (result)
                {
                    case StaticReturnValue.SYSTEM_ERROR:
                        returnData.HttpResponseCode = 500;
                        break;
                    case StaticReturnValue.SUCCESS:
                        returnData.ResponseMessage = result;
                        break;
                    default:
                        returnData.HttpResponseCode = 400;
                        break;
                }
                return returnData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseModel<HoldLogFGDto?>> UnHold(HoldLogFGDto model)
        {

            try
            {
                var returnData = new ResponseModel<HoldLogFGDto?>();

                string proc = "Usp_HoldFinishGood_UnHold";
                var param = new DynamicParameters();
                //param.Add("@HoldLogId", model.HoldLogId);
                //param.Add("@FGInventoryId", model.FGInventoryId);
                param.Add("@ListId", ParameterTvp.GetTableValuedParameter_BigInt(model.ListId));
                param.Add("@Reason", model.Reason);
                param.Add("@IsPicture", model.IsPicture);
                param.Add("@FileName", model.FileName);
                param.Add("@createdBy", model.createdBy);
                param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

                var result = await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);

                returnData.ResponseMessage = result;
                switch (result)
                {
                    case StaticReturnValue.SYSTEM_ERROR:
                        returnData.HttpResponseCode = 500;
                        break;
                    case StaticReturnValue.SUCCESS:
                        returnData.ResponseMessage = result;
                        break;
                    default:
                        returnData.HttpResponseCode = 400;
                        break;
                }
                return returnData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseModel<HoldLogFGDto?>> Scrap(HoldLogFGDto model)
        {

            try
            {
                var returnData = new ResponseModel<HoldLogFGDto?>();

                string proc = "Usp_HoldFinishGood_Scrap";
                var param = new DynamicParameters();
                //param.Add("@HoldLogId", model.HoldLogId);
                //param.Add("@FGInventoryId", model.FGInventoryId);
                param.Add("@ListId", ParameterTvp.GetTableValuedParameter_BigInt(model.ListId));
                param.Add("@createdBy", model.createdBy);
                param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

                var result = await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);

                returnData.ResponseMessage = result;
                switch (result)
                {
                    case StaticReturnValue.SYSTEM_ERROR:
                        returnData.HttpResponseCode = 500;
                        break;
                    case StaticReturnValue.SUCCESS:
                        returnData.ResponseMessage = result;
                        break;
                    default:
                        returnData.HttpResponseCode = 400;
                        break;
                }
                return returnData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetPrintFG(List<long>? listQR)
        {
            var returnData = new ResponseModel<IEnumerable<dynamic>?>();
            var proc = $"Usp_HoldFinishGood_GetPrint";
            var param = new DynamicParameters();
            param.Add("@listQR", ParameterTvp.GetTableValuedParameter_BigInt(listQR));
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);
            returnData.Data = data;
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
            }
            return returnData;
        }
    }
}
