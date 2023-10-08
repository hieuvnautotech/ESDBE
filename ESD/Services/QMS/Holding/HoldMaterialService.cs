using ESD.Models.Dtos.Common;
using ESD.Models.Dtos;
using ESD.DbAccess;
using static ESD.Extensions.ServiceExtensions;
using Dapper;
using ESD.Extensions;
using System.Data;
using ESD.Helpers;
using Newtonsoft.Json;

namespace ESD.Services.QMS.Holding
{
    public interface IHoldMaterialService
    {
        Task<ResponseModel<IEnumerable<MaterialLotDto>?>> GetAll(MaterialLotDto model);
        Task<ResponseModel<HoldDto?>> Hold(HoldDto model);
        Task<ResponseModel<HoldDto?>> UnHold(HoldDto model);
        Task<ResponseModel<HoldDto?>> Scrap(HoldDto model);
        Task<ResponseModel<IEnumerable<QCIQCDetailMDto>?>> GetDetailMaterial(long? QCIQCMasterId, long? MaterialLotId);
        Task<ResponseModel<CheckMaterialLotDto?>> CreateFormMaterial(CheckMaterialLotDto model);
    }
    [ScopedRegistration]
    public class HoldMaterialService : IHoldMaterialService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public HoldMaterialService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        #region Master
        public async Task<ResponseModel<IEnumerable<MaterialLotDto>?>> GetAll(MaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialLotDto>?>();
                string proc = "Usp_HoldMaterial_GetAll"; var param = new DynamicParameters();
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
        public async Task<ResponseModel<HoldDto?>> Hold(HoldDto model)
        {
            var returnData = new ResponseModel<HoldDto?>();

            string proc = "Usp_HoldMaterial_Hold";
            var param = new DynamicParameters();
            //param.Add("@HoldLogId", model.HoldLogId);
            //param.Add("@MaterialLotId", model.MaterialLotId);
            param.Add("@ListId", ParameterTvp.GetTableValuedParameter_BigInt(model.ListId));
            param.Add("@Reason", model.Reason);
            param.Add("@FileName", model.FileName);
            param.Add("@IsPicture", model.IsPicture);
            //param.Add("@row_version", model.row_version);
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
                    //returnData = await GetById(model.QCPQCMasterId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }
        public async Task<ResponseModel<HoldDto?>> UnHold(HoldDto model)
        {
            var returnData = new ResponseModel<HoldDto?>();

            string proc = "Usp_HoldMaterial_UnHold";
            var param = new DynamicParameters();
            //param.Add("@HoldLogId", model.HoldLogId);
            //param.Add("@MaterialLotId", model.MaterialLotId);
            param.Add("@ListId", ParameterTvp.GetTableValuedParameter_BigInt(model.ListId));
            param.Add("@Reason", model.Reason);
            param.Add("@FileName", model.FileName);
            param.Add("@IsPicture", model.IsPicture);
            //param.Add("@row_version", model.row_version);
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
                    //returnData = await GetById(model.QCPQCMasterId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }
        public async Task<ResponseModel<HoldDto?>> Scrap(HoldDto model)
        {

            try
            {
                var returnData = new ResponseModel<HoldDto?>();

                string proc = "Usp_HoldMaterial_Scrap";
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


        #region Check Material
        public async Task<ResponseModel<IEnumerable<QCIQCDetailMDto>?>> GetDetailMaterial(long? QCIQCMasterId, long? MaterialLotId)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<QCIQCDetailMDto>?>();
                string proc = "Usp_HoldMaterial_GetReCheck";
                var param = new DynamicParameters();
                param.Add("@QCIQCMasterId", QCIQCMasterId);
                param.Add("@MaterialLotId", MaterialLotId);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCIQCDetailMDto>(proc, param);
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
        public async Task<ResponseModel<CheckMaterialLotDto?>> CreateFormMaterial(CheckMaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<CheckMaterialLotDto?>();

                var jsonLotList = JsonConvert.SerializeObject(model.CheckValue);

                string proc = "Usp_HoldMaterial_ReCheck";
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
                        //returnData = await GetById(model.LotCheckMasterId);
                        returnData.ResponseMessage = result;
                        break;
                    default:
                        returnData.HttpResponseCode = 400;
                        returnData.ResponseMessage = result;

                        break;
                }
                return returnData;
            }
            catch (Exception e)
            {

                throw;
            }

        }

        #endregion
        #endregion
    }

}
