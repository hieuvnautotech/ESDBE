using Dapper;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Models.Dtos.Common;
using ESD.Models.Dtos;
using ESD.Models.Validators;
using System.Data;
using static ESD.Extensions.ServiceExtensions;
using Newtonsoft.Json;

namespace ESD.Services.Standard.Information
{
    public interface IBladeService
    {
        Task<ResponseModel<IEnumerable<BladeDto>?>> GetAll(BladeDto model);
        Task<ResponseModel<BladeDto?>> GetById(long? BladeId);
        Task<ResponseModel<BladeDto?>> Create(BladeDto model);
        Task<ResponseModel<BladeDto?>> Modify(BladeDto model);
        Task<ResponseModel<BladeDto?>> Delete(BladeDto model);
        Task<ResponseModel<IEnumerable<BladeDto>?>> GetActive(BladeDto model);
        Task<ResponseModel<IEnumerable<QCMoldMasterDto>>> GetQCMasters();
        Task<ResponseModel<IEnumerable<BladeCheckMasterDto>?>> GetBladeCheckFormMapping(BladeDto model);
        //Task<string> CreateBladeCheckFormMapping(BladeCheckMasterDto model);
        //Task<ResponseModel<BladeCheckMasterDto>> GetBladeCheckFormHistory(BladeDto model);
        Task<ResponseModel<IEnumerable<BladeHistoryDto>>> GetBladeHistory(BladeHistoryDto model);

        Task<ResponseModel<BladeDto?>> GetCheckMaster(long? MoldId, int? CheckTime);
        Task<ResponseModel<IEnumerable<QCMoldDetail>>> getCheckQC(long? QCMoldMasterId, long? MoldId, int? CheckTime);
        Task<ResponseModel<BladeDto?>> CheckQC(CheckMoldDto model);

    }
    [ScopedRegistration]
    public class BladeService : IBladeService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public BladeService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<ResponseModel<IEnumerable<BladeDto>?>> GetAll(BladeDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<BladeDto>?>();
                string proc = "Usp_Blade_GetAll"; var param = new DynamicParameters();
                param.Add("@BladeName", model.BladeName);
                param.Add("@BladeStatus", model.BladeStatus);
                param.Add("@showDelete", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<BladeDto>(proc, param);
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

        public async Task<ResponseModel<BladeDto?>> Create(BladeDto model)
        {
            var returnData = new ResponseModel<BladeDto?>();

            string proc = "Usp_Blade_Create";
            var param = new DynamicParameters();
            param.Add("@BladeId", model.BladeId);
            param.Add("@BladeName", model.BladeName);
            param.Add("@BladeSize", model.BladeSize);
            param.Add("@BladeStatus", model.BladeStatus);
            param.Add("@SupplierId", model.SupplierId);
            param.Add("@ImportDate", model.ImportDate);
            param.Add("@QCMoldMasterId", model.QCMoldMasterId);
            param.Add("@CutMaxNumber", model.CutMaxNumber);
            param.Add("@PeriodicCheck", model.PeriodicCheck);
            param.Add("@CutCurrentNumber", model.CutCurrentNumber);
            param.Add("@BladeStatus", model.BladeStatus);
            param.Add("@LineId", model.LineId);
            param.Add("@Description", model.Description);
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
                    returnData = await GetById(model.BladeId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<BladeDto?>> GetById(long? BladeId)
        {
            var returnData = new ResponseModel<BladeDto?>();
            string proc = "Usp_Blade_GetById";
            var param = new DynamicParameters();
            param.Add("@BladeId", BladeId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<BladeDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }

        public async Task<ResponseModel<BladeDto?>> Modify(BladeDto model)
        {
            var returnData = new ResponseModel<BladeDto?>();


            string proc = "Usp_Blade_Modify";
            var param = new DynamicParameters();
            param.Add("@BladeId", model.BladeId);
            param.Add("@BladeName", model.BladeName);
            param.Add("@BladeSize", model.BladeSize);
            param.Add("@BladeStatus", model.BladeStatus);
            param.Add("@SupplierId", model.SupplierId);
            param.Add("@ImportDate", model.ImportDate);
            param.Add("@QCMoldMasterId", model.QCMoldMasterId);
            param.Add("@CutMaxNumber", model.CutMaxNumber);
            param.Add("@PeriodicCheck", model.PeriodicCheck);
            param.Add("@CutCurrentNumber", model.CutCurrentNumber);
            param.Add("@BladeStatus", model.BladeStatus);
            param.Add("@LineId", model.LineId);
            param.Add("@Description", model.Description);
            param.Add("@modifiedBy", model.modifiedBy);
            param.Add("@row_version", model.row_version);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var result = await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);

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
                    returnData = await GetById(model.BladeId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }
        public async Task<ResponseModel<BladeDto?>> Delete(BladeDto model)
        {
            string proc = "Usp_Blade_Delete";
            var param = new DynamicParameters();
            param.Add("@BladeId", model.BladeId);
            param.Add("@modifiedBy", model.modifiedBy);
            param.Add("@isActived", model.isActived);
            param.Add("@row_version", model.row_version);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<BladeDto?>();
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

        public async Task<ResponseModel<IEnumerable<BladeDto>?>> GetActive(BladeDto model)
        {
            var returnData = new ResponseModel<IEnumerable<BladeDto>?>();
            var proc = $"Usp_Blade_GetActive";
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<BladeDto>(proc);

            if (!data.Any())
            {
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                returnData.HttpResponseCode = 204;
            }
            else
            {
                returnData.Data = data;
            }

            return returnData;
        }
        public async Task<ResponseModel<IEnumerable<QCMoldMasterDto>>> GetQCMasters()
        {
            var returnData = new ResponseModel<IEnumerable<QCMoldMasterDto>>();

            string proc = "Usp_Mold_GetQCMasters";

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCMoldMasterDto>(proc);

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
        public async Task<ResponseModel<IEnumerable<BladeCheckMasterDto>?>> GetBladeCheckFormMapping(BladeDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<BladeCheckMasterDto>?>();
                string proc = "Usp_Blade_GetCheckForm";
                var param = new DynamicParameters();
                param.Add("@QCMoldMasterId", model.QCMoldMasterId);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<BladeCheckMasterDto>(proc, param);
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

        //public async Task<string> CreateBladeCheckFormMapping(BladeCheckMasterDto model)
        //{
        //    string proc = "Usp_Blade_CreateMoldCheckFormMapping";

        //    var param = new DynamicParameters();

        //    param.Add("@BladeCheckMasterId", model.BladeCheckMasterId);
        //    param.Add("@BladeId", model.BladeId);
        //    param.Add("@QCMoldMasterId", model.QCMoldMasterId);
        //    param.Add("@CheckNo", model.CheckNo + 1);
        //    param.Add("@UpdateAvailable", true);
        //    param.Add("@StaffId", model.StaffId);
        //    param.Add("@CheckDate", model.CheckDate);
        //    param.Add("@CheckResult", model.CheckResult);
        //    param.Add("@createdBy", model.createdBy);


        //    var jsonText = JsonConvert.SerializeObject(model.BladeCheckDetailTextDtos);
        //    var jsonValue = JsonConvert.SerializeObject(model.BladeCheckDetailValueDtos);

        //    param.Add("@jsonText", jsonText);
        //    param.Add("@jsonValue", jsonValue);

        //    param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);

        //    return await _sqlDataAccess.SaveDataUsingStoredProcedure<string>(proc, param);
        //}

        //public async Task<ResponseModel<BladeCheckMasterDto>> GetBladeCheckFormHistory(BladeDto model)
        //{
        //    var returnData = new ResponseModel<BladeCheckMasterDto>();

        //    string proc = "Usp_Blade_GetCheckFormHistory";
        //    var param = new DynamicParameters();
        //    param.Add("@BladeId", model.BladeId);
        //    param.Add("@CheckNo", model.CheckNo);

        //    var data = await _sqlDataAccess.LoadMultiDataSetUsingStoredProcedure<BladeCheckMasterDto, BladeCheckDetailTextDto, BladeCheckDetailValueDto>(proc, param);

        //    if (
        //        !data.Item1.Any()
        //       //&& !data.Item2.Any()
        //       //&& !data.Item3.Any()
        //       )
        //    {
        //        returnData.HttpResponseCode = 204;
        //        returnData.ResponseMessage = StaticReturnValue.NO_DATA;
        //    }
        //    else
        //    {
        //        returnData.Data = new BladeCheckMasterDto();
        //        returnData.Data = data.Item1.FirstOrDefault();

        //        returnData.Data.BladeCheckDetailTextDtos = data.Item2;
        //        returnData.Data.BladeCheckDetailValueDtos = data.Item3;
        //    }

        //    return returnData;
        //}
        public async Task<ResponseModel<IEnumerable<BladeHistoryDto>>> GetBladeHistory(BladeHistoryDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<BladeHistoryDto>>();
                string proc = "Usp_BladeHistory_Get"; var param = new DynamicParameters();
                param.Add("@BladeId", model.BladeId);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<BladeHistoryDto>(proc, param);

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
        public async Task<ResponseModel<BladeDto>> CheckQC(CheckMoldDto model)
        {
            var returnData = new ResponseModel<BladeDto>();

            var jsonLotList = JsonConvert.SerializeObject(model.CheckValue);
            string proc = "Usp_Blade_CheckQC";
            var param = new DynamicParameters();
            param.Add("@BladeId", model.BladeId);
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
                    returnData = await GetById(model.BladeId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }
        public async Task<ResponseModel<BladeDto?>> GetCheckMaster(long? BladeId, int? CheckTime)
        {
            var returnData = new ResponseModel<BladeDto?>();
            string proc = "Usp_Blade_GetCheckMaster";
            var param = new DynamicParameters();
            param.Add("@BladeId", BladeId);
            param.Add("@CheckTime", CheckTime);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<BladeDto>(proc, param);
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
