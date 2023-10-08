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
using System.Collections.Generic;
using System.Data;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using static ESD.Extensions.ServiceExtensions;

namespace ESD.Services.QMS.Holding
{
    public interface IHoldRawMaterialService
    {
        Task<ResponseModel<IEnumerable<MaterialLotDto>?>> GetAll(MaterialLotDto model);

        Task<ResponseModel<HoldLogRawMaterialDto?>> Hold(HoldLogRawMaterialDto model);
        Task<ResponseModel<HoldLogRawMaterialDto?>> UnHold(HoldLogRawMaterialDto model);
        Task<ResponseModel<HoldLogRawMaterialDto?>> Scrap(HoldLogRawMaterialDto model);
        Task<ResponseModel<IEnumerable<QCIQCDetailRMDto>>> GetDetailRawMaterial(long? QCIQCMasterId, long? MaterialLotId);
        Task<ResponseModel<CheckRawMaterialLotDto?>> CreateRawMaterial(CheckRawMaterialLotDto model);
    }
    [ScopedRegistration]
    public class HoldRawMaterialService : IHoldRawMaterialService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public HoldRawMaterialService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        #region Master
        public async Task<ResponseModel<IEnumerable<MaterialLotDto>?>> GetAll(MaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialLotDto>?>();
                string proc = "Usp_HoldRawMaterial_GetAll"; var param = new DynamicParameters();
                param.Add("@MaterialLotCode", model.MaterialLotCode);
                param.Add("@LotNo", model.LotNo);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MaterialLotDto>(proc, param);
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
        #endregion

        #region RawMaterial
        public async Task<ResponseModel<HoldLogRawMaterialDto?>> Hold(HoldLogRawMaterialDto model)
        {
            try
            {
                var returnData = new ResponseModel<HoldLogRawMaterialDto?>();

                string proc = "Usp_HoldRawMaterial_Hold";
                var param = new DynamicParameters();
                //param.Add("@HoldLogId", model.HoldLogId);
                //param.Add("@MaterialLotId", model.MaterialLotId);
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

        public async Task<ResponseModel<HoldLogRawMaterialDto?>> UnHold(HoldLogRawMaterialDto model)
        {

            try
            {
                var returnData = new ResponseModel<HoldLogRawMaterialDto?>();

                string proc = "Usp_HoldRawMaterial_UnHold";
                var param = new DynamicParameters();
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

        public async Task<ResponseModel<HoldLogRawMaterialDto?>> Scrap(HoldLogRawMaterialDto model)
        {

            try
            {
                var returnData = new ResponseModel<HoldLogRawMaterialDto?>();

                string proc = "Usp_HoldRawMaterial_Scrap";
                var param = new DynamicParameters();
                //param.Add("@HoldLogId", model.HoldLogId);
                //param.Add("@MaterialLotId", model.MaterialLotId);
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

        public async Task<ResponseModel<IEnumerable<QCIQCDetailRMDto>>> GetDetailRawMaterial(long? QCIQCMasterId, long? MaterialLotId)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<QCIQCDetailRMDto>>();
                string proc = "Usp_HoldRawMaterial_GetReCheck";
                var param = new DynamicParameters();
                param.Add("@QCIQCMasterId", QCIQCMasterId);
                param.Add("@MaterialLotId", MaterialLotId);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCIQCDetailRMDto>(proc, param);
                returnData.Data = data;
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

        public async Task<ResponseModel<CheckRawMaterialLotDto?>> CreateRawMaterial(CheckRawMaterialLotDto model)
        {
            var returnData = new ResponseModel<CheckRawMaterialLotDto?>();

            var jsonLotList = JsonConvert.SerializeObject(model.CheckValue);
            string proc = "Usp_HoldRawMaterial_ReCheck";
            var param = new DynamicParameters();
            param.Add("@MaterialLotId", model.MaterialLotId);
            param.Add("@QCIQCMasterId", model.QCIQCMasterId);
            param.Add("@StaffId", model.StaffId);
            param.Add("@CheckDate", model.CheckDate);
            param.Add("@CheckResult", model.CheckResult);
            param.Add("@Jsonlist", jsonLotList);
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
                    returnData.HttpResponseCode = 200;
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }
        #endregion
    }
}
