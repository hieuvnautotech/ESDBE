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

namespace ESD.Services.Common.Standard.Information
{
    public interface IQCMoldService
    {
        Task<ResponseModel<IEnumerable<QCMoldMasterDto>?>> GetAll(QCMoldMasterDto model);
        Task<ResponseModel<QCMoldMasterDto?>> GetById(long? QCMoldMasterId);
        Task<ResponseModel<QCMoldMasterDto?>> Create(QCMoldMasterDto model);
        Task<ResponseModel<QCMoldMasterDto?>> Modify(QCMoldMasterDto model);
        Task<ResponseModel<QCMoldMasterDto?>> Delete(QCMoldMasterDto model);
        Task<ResponseModel<QCMoldMasterDto?>> Confirm(QCMoldMasterDto model);
        Task<ResponseModel<QCMoldMasterDto?>> Copy(QCMoldMasterDto model);

        Task<ResponseModel<IEnumerable<QCMoldDetailDto>?>> GetDetail(QCMoldDetailDto model);
        Task<ResponseModel<QCMoldDetailDto?>> CreateDetail(QCMoldDetailDto model);
        Task<ResponseModel<QCMoldDetailDto?>> DeleteDetail(QCMoldDetailDto model);
        Task<ResponseModel<QCMoldDetailDto?>> ClearDetail(long? QCMoldMasterId);
    }
    [ScopedRegistration]
    public class QCMoldService : IQCMoldService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public QCMoldService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        #region Master
        public async Task<ResponseModel<IEnumerable<QCMoldMasterDto>?>> GetAll(QCMoldMasterDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<QCMoldMasterDto>?>();
                string proc = "Usp_QCMoldMaster_GetAll"; var param = new DynamicParameters();
                param.Add("@QCMoldMasterName", model.QCMoldMasterName);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCMoldMasterDto>(proc, param);
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

        public async Task<ResponseModel<QCMoldMasterDto?>> Create(QCMoldMasterDto model)
        {
            var returnData = new ResponseModel<QCMoldMasterDto?>();

            string proc = "Usp_QCMoldMaster_Create";
            var param = new DynamicParameters();
            param.Add("@QCMoldMasterId", model.QCMoldMasterId);
            param.Add("@QCMoldMasterName", model.QCMoldMasterName);
            param.Add("@Explain", model.Explain);
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
                    returnData = await GetById(model.QCMoldMasterId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<QCMoldMasterDto?>> Modify(QCMoldMasterDto model)
        {
            var returnData = new ResponseModel<QCMoldMasterDto?>();

            string proc = "Usp_QCMoldMaster_Modify";
            var param = new DynamicParameters();
            param.Add("@QCMoldMasterId", model.QCMoldMasterId);
            param.Add("@QCMoldMasterName", model.QCMoldMasterName);
            param.Add("@Explain", model.Explain);
            param.Add("@createdBy", model.createdBy);
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
                    returnData = await GetById(model.QCMoldMasterId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<QCMoldMasterDto?>> GetById(long? QCMoldMasterId)
        {
            var returnData = new ResponseModel<QCMoldMasterDto?>();
            string proc = "Usp_QCMoldMaster_GetById";
            var param = new DynamicParameters();
            param.Add("@QCMoldMasterId", QCMoldMasterId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCMoldMasterDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }

        public async Task<ResponseModel<QCMoldMasterDto?>> Delete(QCMoldMasterDto model)
        {
            string proc = "Usp_QCMoldMaster_Delete";
            var param = new DynamicParameters();
            param.Add("@QCMoldMasterId", model.QCMoldMasterId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<QCMoldMasterDto?>();
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

        public async Task<ResponseModel<QCMoldMasterDto?>> Confirm(QCMoldMasterDto model)
        {
            string proc = "Usp_QCMoldMaster_Confirm";
            var param = new DynamicParameters();
            param.Add("@QCMoldMasterId", model.QCMoldMasterId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<QCMoldMasterDto?>();
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
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }

            return returnData;
        }

        public async Task<ResponseModel<QCMoldMasterDto?>> Copy(QCMoldMasterDto model)
        {
            var returnData = new ResponseModel<QCMoldMasterDto?>();
            string proc = "Usp_QCMoldMaster_Copy";
            var NewId = AutoId.AutoGenerate();
            var param = new DynamicParameters();
            param.Add("@QCMoldMasterId", model.QCMoldMasterId);
            param.Add("@NewId", NewId);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);

            var result = await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);

            returnData.ResponseMessage = result;
            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.SUCCESS:
                    returnData = await GetById(NewId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }
        #endregion

        #region Detail
        public async Task<ResponseModel<IEnumerable<QCMoldDetailDto>?>> GetDetail(QCMoldDetailDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<QCMoldDetailDto>?>();
                string proc = "Usp_QCMoldDetail_GetAll";
                var param = new DynamicParameters();
                param.Add("@QCMoldMasterId", model.QCMoldMasterId);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCMoldDetailDto>(proc, param);
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

        public async Task<ResponseModel<QCMoldDetailDto?>> CreateDetail(QCMoldDetailDto model)
        {
            var returnData = new ResponseModel<QCMoldDetailDto?>();

            string proc = "Usp_QCMoldDetail_Create";
            var param = new DynamicParameters();
            param.Add("@QCMoldDetailId", model.QCMoldDetailId);
            param.Add("@QCMoldMasterId", model.QCMoldMasterId);
            param.Add("@QCItemId", model.QCItemId);
            param.Add("@QCTypeId", model.QCTypeId);
            param.Add("@QCStandardId", model.QCStandardId);
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

        public async Task<ResponseModel<QCMoldDetailDto?>> DeleteDetail(QCMoldDetailDto model)
        {
            string proc = "Usp_QCMoldDetail_Delete";
            var param = new DynamicParameters();
            param.Add("@QCMoldDetailId", model.QCMoldDetailId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<QCMoldDetailDto?>();
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
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }

            return returnData;
        }

        public async Task<ResponseModel<QCMoldDetailDto?>> ClearDetail(long? QCMoldMasterId)
        {
            string proc = "Usp_QCMoldDetail_Clear";
            var param = new DynamicParameters();
            param.Add("@QCMoldMasterId", QCMoldMasterId);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
            var returnData = new ResponseModel<QCMoldDetailDto?>();
            var result = await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);

            returnData.ResponseMessage = result;
            switch (result)
            {
                case StaticReturnValue.SUCCESS:
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
