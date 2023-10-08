using Dapper;
using Newtonsoft.Json;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Models;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Models.Validators;
using ESD.Services.Base;
using System.Data;
using static ESD.Extensions.ServiceExtensions;

namespace ESD.Services.Common.Standard.Information
{
    public interface IMoldService
    {
        Task<ResponseModel<IEnumerable<MoldDto>>> Get(MoldDto model);
        Task<ResponseModel<IEnumerable<WOMoldPressingTimesDto>>> GetMoldHistory(WOMoldPressingTimesDto model);
        Task<ResponseModel<MoldDto>> GetById(long? MoldId);
        Task<ResponseModel<MoldDto>> Create(MoldDto model);
        Task<ResponseModel<MoldDto>> Modify(MoldDto model);
        Task<ResponseModel<MoldDto>> DeleteReuse(long? MoldId, byte[] row_version, long user, bool? isActived);
        Task<ResponseModel<MoldDto>> CreateByExcel(List<MoldExcelDto> model, long userCreate);
        Task<ResponseModel<IEnumerable<ProductDto>>> GetProductMapping(long moldId);
        Task<ResponseModel<IEnumerable<CommonDetailDto>>> GetLineTypeMapping(long moldId);

        Task<ResponseModel<MoldDto?>> GetCheckMaster(long? MoldId, int? CheckTime);
        Task<ResponseModel<IEnumerable<QCMoldDetail>>> getCheckQC(long? QCMoldMasterId, long? MoldId, int? CheckTime);
        Task<ResponseModel<MoldDto>> CheckQC(CheckMoldDto model);
    }
    [ScopedRegistration]
    public class MoldService : IMoldService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public MoldService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<ResponseModel<IEnumerable<MoldDto>>> Get(MoldDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MoldDto>>();
                string proc = "Usp_Mold_Get"; var param = new DynamicParameters();
                param.Add("@MoldCode", model.MoldCode);
                param.Add("@MoldName", model.MoldName);
                param.Add("@MoldStatus", model.MoldStatus);
                param.Add("@ProductId", model.ProductId);

                param.Add("@isActive", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MoldDto>(proc, param);

                if (!data.Any())
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                }
                else
                {
                    returnData.Data = data;
                    returnData.TotalRow = param.Get<int>("totalRow");
                }
                return returnData;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<ResponseModel<MoldDto>> Create(MoldDto model)
        {
             var returnData = new ResponseModel<MoldDto>();

            var productIds = (from product in model.Products
                              select product.ProductId).ToList();

            //var lineTypes = (from lineType in model.LineTypes
            //                 select lineType.commonDetailId).ToList();

            string proc = "Usp_Mold_Create";
            var param = new DynamicParameters();
            param.Add("@MoldId", model.MoldId);
            param.Add("@MoldCode", model.MoldCode);
            param.Add("@MoldVersion", model.MoldVersion);
            param.Add("@MoldName", model.MoldName);
            param.Add("@MoldStatus", model.MoldStatus);
            param.Add("@ProductIds", Helpers.ParameterTvp.GetTableValuedParameter_BigInt(productIds));
            //param.Add("@LineTypes", Helpers.ParameterTvp.GetTableValuedParameter_BigInt(lineTypes));
            param.Add("@MoldType", model.MoldType);
            param.Add("@ProductionDate", model.ProductionDate);
            param.Add("@SupplierId", model.SupplierId);
            param.Add("@QCMasterId", model.QCMasterId);
            param.Add("@Remark", model.Remark);
            param.Add("@CurrentNumber", model.CurrentNumber);
            param.Add("@MaxNumber", model.MaxNumber);
            param.Add("@PeriodNumber", model.PeriodNumber);

            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var result = await _sqlDataAccess.SaveDataUsingStoredProcedure<string>(proc, param);

            returnData.ResponseMessage = result;
            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.SUCCESS:
                    returnData = await GetById(model.MoldId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<MoldDto>> Modify(MoldDto model)
        {
            var returnData = new ResponseModel<MoldDto>();

            var productIds = (from product in model.Products
                              select product.ProductId).ToList();

            //var lineTypes = (from lineType in model.LineTypes
            //                 select lineType.commonDetailId).ToList();

            string proc = "Usp_Mold_Modify";
            var param = new DynamicParameters();
            param.Add("@MoldId", model.MoldId);
            param.Add("@MoldCode", model.MoldCode);
            param.Add("@MoldVersion", model.MoldVersion);
            param.Add("@MoldName", model.MoldName);
            param.Add("@MoldStatus", model.MoldStatus);
            param.Add("@ProductIds", Helpers.ParameterTvp.GetTableValuedParameter_BigInt(productIds));
            //param.Add("@LineTypes", Helpers.ParameterTvp.GetTableValuedParameter_BigInt(lineTypes));
            param.Add("@MoldType", model.MoldType);
            param.Add("@ProductionDate", model.ProductionDate);
            param.Add("@SupplierId", model.SupplierId);
            param.Add("@QCMasterId", model.QCMasterId);
            param.Add("@Remark", model.Remark);
            param.Add("@CurrentNumber", model.CurrentNumber);
            param.Add("@MaxNumber", model.MaxNumber);
            param.Add("@PeriodNumber", model.PeriodNumber);

            param.Add("@modifiedBy", model.modifiedBy);
            param.Add("@row_version", model.row_version);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var result = await _sqlDataAccess.SaveDataUsingStoredProcedure<string>(proc, param);

            returnData.ResponseMessage = result;
            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.REFRESH_REQUIRED:
                    returnData.HttpResponseCode = 500;
                    returnData.ResponseMessage = result;
                    break;
                case StaticReturnValue.SUCCESS:
                    returnData = await GetById(model.MoldId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }
        public async Task<ResponseModel<MoldDto>> GetById(long? MoldId)
        {
            var returnData = new ResponseModel<MoldDto>();
            string proc = "Usp_Mold_GetById";
            var param = new DynamicParameters();
            param.Add("@MoldId", MoldId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MoldDto>(proc, param);

            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            else
            {
                returnData.Data = data.FirstOrDefault();
            }
            return returnData;
        }

        public async Task<ResponseModel<MoldDto>> DeleteReuse(long? MoldId, byte[] row_version, long user, bool? isActived)
        {
            string proc = "Usp_Mold_DeleteReUse";
            var param = new DynamicParameters();
            param.Add("@MoldId", MoldId);
            param.Add("@row_version", row_version);
            param.Add("@isActived", isActived);
            param.Add("@modifiedBy", user);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<MoldDto>();
            var result = await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.REFRESH_REQUIRED:
                    returnData.HttpResponseCode = 500;
                    returnData.ResponseMessage = result;
                    break;
                case StaticReturnValue.SUCCESS:
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }

            return returnData;
        }

        public async Task<ResponseModel<IEnumerable<WOMoldPressingTimesDto>>> GetMoldHistory(WOMoldPressingTimesDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WOMoldPressingTimesDto>>();
                string proc = "Usp_MoldHistory_Get"; var param = new DynamicParameters();
                param.Add("@MoldId", model.MoldId);

                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOMoldPressingTimesDto>(proc, param);

                if (!data.Any())
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                }
                else
                {
                    returnData.Data = data;
                    returnData.TotalRow = param.Get<int>("totalRow");
                }
                return returnData;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public async Task<ResponseModel<MoldDto>> CreateByExcel(List<MoldExcelDto> model, long userCreate)
        {
            var returnData = new ResponseModel<MoldDto>();

            var jsonLotList = JsonConvert.SerializeObject(model);

            string proc = "Usp_Mold_CreateByExcel";
            var param = new DynamicParameters();
            param.Add("@Jsonlist", jsonLotList);
            param.Add("@createdBy", userCreate);
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
        public async Task<ResponseModel<IEnumerable<ProductDto>>> GetProductMapping(long moldId)
        {
            var returnData = new ResponseModel<IEnumerable<ProductDto>>();

            string proc = "Usp_Mold_GetProductMapping";

            var param = new DynamicParameters();
            param.Add("@MoldId", moldId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<ProductDto>(proc, param);

            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
            }
            else
            {
                returnData.Data = data;
            }
            return returnData;
        }

        public async Task<ResponseModel<IEnumerable<CommonDetailDto>>> GetLineTypeMapping(long moldId)
        {
            var returnData = new ResponseModel<IEnumerable<CommonDetailDto>>();

            string proc = "Usp_Mold_GetLineTypeMapping";

            var param = new DynamicParameters();
            param.Add("@MoldId", moldId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<CommonDetailDto>(proc, param);

            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
            }
            else
            {
                returnData.Data = data;
            }
            return returnData;
        }

        #region Check qc
        public async Task<ResponseModel<IEnumerable<QCMoldDetail>>> getCheckQC(long? QCMoldMasterId, long? MoldId, int? CheckTime)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<QCMoldDetail>>();
                string proc = "Usp_Mold_GetCheckQC";
                var param = new DynamicParameters();
                param.Add("@MoldId", MoldId);
                param.Add("@QCMoldMasterId", QCMoldMasterId);
                param.Add("@CheckTime", CheckTime);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCMoldDetail>(proc, param);
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
        public async Task<ResponseModel<MoldDto>> CheckQC(CheckMoldDto model)
        {
            var returnData = new ResponseModel<MoldDto>();

            var jsonLotList = JsonConvert.SerializeObject(model.CheckValue);
            string proc = "Usp_Mold_CheckQC";
            var param = new DynamicParameters();
            param.Add("@MoldId", model.MoldId);
            param.Add("@QCMoldMasterId", model.QCMoldMasterId);
            param.Add("@StaffId", model.StaffId);
            param.Add("@CheckDate", model.CheckDate);
            param.Add("@CheckResult", model.CheckResult);
            param.Add("@Jsonlist", jsonLotList);
            param.Add("@CheckTime", model.CheckTime);
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
                    returnData = await GetById(model.MoldId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }
        public async Task<ResponseModel<MoldDto?>> GetCheckMaster(long? MoldId, int? CheckTime)
        {
            var returnData = new ResponseModel<MoldDto?>();
            string proc = "Usp_Mold_GetCheckMaster";
            var param = new DynamicParameters();
            param.Add("@MoldId", MoldId);
            param.Add("@CheckTime", CheckTime);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MoldDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }
        #endregion
    }
}
