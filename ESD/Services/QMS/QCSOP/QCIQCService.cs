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
    public interface IQCIQCService
    {
        Task<ResponseModel<IEnumerable<QCIQCMasterDto>?>> GetAll(QCIQCMasterDto model);
        Task<ResponseModel<QCIQCMasterDto?>> GetById(long? QCIQCMasterId);
        Task<ResponseModel<QCIQCMasterDto?>> Create(QCIQCMasterDto model);
        Task<ResponseModel<QCIQCMasterDto?>> Modify(QCIQCMasterDto model);
        Task<ResponseModel<QCIQCMasterDto?>> Delete(QCIQCMasterDto model);
        Task<ResponseModel<QCIQCMasterDto?>> Confirm(QCIQCMasterDto model);
        Task<ResponseModel<QCIQCMasterDto?>> Copy(QCIQCMasterDto model);

        Task<ResponseModel<IEnumerable<QCIQCDetailRMDto?>>> GetDetailRawMaterial(QCIQCDetailRMDto model);
        Task<ResponseModel<QCIQCDetailRMDto?>> CreateDetailRM(QCIQCDetailRMDto model);
        Task<ResponseModel<QCIQCDetailRMDto?>> DeleteDetailRM(QCIQCDetailRMDto model);
        Task<ResponseModel<QCIQCDetailRMDto?>> ClearDetailRM(long? QCIQCMasterId);

        Task<ResponseModel<IEnumerable<QCIQCDetailMDto>?>> GetDetailMaterial(QCIQCDetailMDto model);
        Task<ResponseModel<QCIQCDetailMDto?>> CreateDetailMaterial(QCIQCDetailMDto model);
        Task<ResponseModel<QCIQCDetailMDto?>> DeleteDetailMaterial(QCIQCDetailMDto model);
        Task<ResponseModel<QCIQCDetailMDto?>> ClearDetailMaterial(long? QCIQCMasterId);
    }
    [ScopedRegistration]
    public class QCIQCService : IQCIQCService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public QCIQCService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        #region Master
        public async Task<ResponseModel<IEnumerable<QCIQCMasterDto>?>> GetAll(QCIQCMasterDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<QCIQCMasterDto>?>();
                string proc = "Usp_QCIQCMaster_GetAll"; var param = new DynamicParameters();
                param.Add("@QCIQCMasterName", model.QCIQCMasterName);
                param.Add("@IQCType", model.IQCType);
                param.Add("@Explain", model.Explain);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCIQCMasterDto>(proc, param);
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

        public async Task<ResponseModel<QCIQCMasterDto?>> Create(QCIQCMasterDto model)
        {
            var returnData = new ResponseModel<QCIQCMasterDto?>();

            string proc = "Usp_QCIQCMaster_Create";
            var param = new DynamicParameters();
            param.Add("@QCIQCMasterId", model.QCIQCMasterId);
            param.Add("@QCIQCMasterName", model.QCIQCMasterName);
            param.Add("@IQCType", model.IQCType);
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
                    returnData = await GetById(model.QCIQCMasterId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<QCIQCMasterDto?>> Modify(QCIQCMasterDto model)
        {
            var returnData = new ResponseModel<QCIQCMasterDto?>();
            string proc = "Usp_QCIQCMaster_Modify";
            var param = new DynamicParameters();
            param.Add("@QCIQCMasterId", model.QCIQCMasterId);
            param.Add("@QCIQCMasterName", model.QCIQCMasterName);
            param.Add("@IQCType", model.IQCType);
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
                    returnData = await GetById(model.QCIQCMasterId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<QCIQCMasterDto?>> GetById(long? QCIQCMasterId)
        {
            var returnData = new ResponseModel<QCIQCMasterDto?>();
            string proc = "Usp_QCIQCMaster_GetById";
            var param = new DynamicParameters();
            param.Add("@QCIQCMasterId", QCIQCMasterId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCIQCMasterDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }

        public async Task<ResponseModel<QCIQCMasterDto?>> Delete(QCIQCMasterDto model)
        {
            string proc = "Usp_QCIQCMaster_Delete";
            var param = new DynamicParameters();
            param.Add("@QCIQCMasterId", model.QCIQCMasterId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<QCIQCMasterDto?>();
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
                    returnData.ResponseMessage = result;
                    break;
            }

            return returnData;
        }

        public async Task<ResponseModel<QCIQCMasterDto?>> Confirm(QCIQCMasterDto model)
        {
            string proc = "Usp_QCIQCMaster_Confirm";
            var param = new DynamicParameters();
            param.Add("@QCIQCMasterId", model.QCIQCMasterId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<QCIQCMasterDto?>();
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
                    returnData.ResponseMessage = result;
                    break;
            }

            return returnData;
        }

        public async Task<ResponseModel<QCIQCMasterDto?>> Copy(QCIQCMasterDto model)
        {
            var returnData = new ResponseModel<QCIQCMasterDto?>();
            string proc = "Usp_QCIQCMaster_Copy";
            var NewId = AutoId.AutoGenerate();
            var param = new DynamicParameters();
            param.Add("@QCIQCMasterId", model.QCIQCMasterId);
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

        #region Detail Raw Material
        public async Task<ResponseModel<IEnumerable<QCIQCDetailRMDto?>>> GetDetailRawMaterial(QCIQCDetailRMDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<QCIQCDetailRMDto?>>();
                string proc = "Usp_QCIQCDetailRM_GetAll";
                var param = new DynamicParameters();
                param.Add("@QCIQCMasterId", model.QCIQCMasterId);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCIQCDetailRMDto>(proc, param);
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
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseModel<QCIQCDetailRMDto?>> CreateDetailRM(QCIQCDetailRMDto model)
        {
            var returnData = new ResponseModel<QCIQCDetailRMDto?>();

            string proc = "Usp_QCIQCDetailRM_Create";
            var param = new DynamicParameters();
            param.Add("@QCIQCDetailRMId", model.QCIQCDetailRMId);
            param.Add("@QCIQCMasterId", model.QCIQCMasterId);
            param.Add("@QCTypeId", model.QCTypeId);
            param.Add("@QCItemId", model.QCItemId);
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

        public async Task<ResponseModel<QCIQCDetailRMDto?>> DeleteDetailRM(QCIQCDetailRMDto model)
        {
            string proc = "Usp_QCIQCDetailRM_Delete";
            var param = new DynamicParameters();
            param.Add("@QCIQCDetailRMId", model.QCIQCDetailRMId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<QCIQCDetailRMDto?>();
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
                    returnData.HttpResponseCode = 200;
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    returnData.ResponseMessage = result;
                    break;
            }

            return returnData;
        }
        public async Task<ResponseModel<QCIQCDetailRMDto?>> ClearDetailRM(long? QCIQCMasterId)
        {
            string proc = "Usp_QCIQCDetailRM_Clear";
            var param = new DynamicParameters();
            param.Add("@QCIQCMasterId", QCIQCMasterId);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
            var returnData = new ResponseModel<QCIQCDetailRMDto?>();
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
                    returnData.HttpResponseCode = 200;
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    returnData.ResponseMessage = result;
                    break;
            }

            return returnData;
        }
        #endregion

        #region Detail Material
        public async Task<ResponseModel<IEnumerable<QCIQCDetailMDto>?>> GetDetailMaterial(QCIQCDetailMDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<QCIQCDetailMDto>?>();
                string proc = "Usp_QCIQCDetailMaterial_GetAll";
                var param = new DynamicParameters();
                param.Add("@QCIQCMasterId", model.QCIQCMasterId);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCIQCDetailMDto>(proc, param);
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

        public async Task<ResponseModel<QCIQCDetailMDto?>> CreateDetailMaterial(QCIQCDetailMDto model)
        {
            var returnData = new ResponseModel<QCIQCDetailMDto?>();

            string proc = "Usp_QCIQCDetailMaterial_Create";
            var param = new DynamicParameters();
            param.Add("@@QCIQCDetailMId", model.QCIQCDetailMId);
            param.Add("@QCIQCMasterId", model.QCIQCMasterId);
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

        public async Task<ResponseModel<QCIQCDetailMDto?>> DeleteDetailMaterial(QCIQCDetailMDto model)
        {
            string proc = "Usp_QCIQCDetailMaterial_Delete";
            var param = new DynamicParameters();
            param.Add("@QCIQCDetailMId", model.QCIQCDetailMId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<QCIQCDetailMDto?>();
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
                    returnData.HttpResponseCode = 200;
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    returnData.ResponseMessage = result;
                    break;
            }

            return returnData;
        }
        public async Task<ResponseModel<QCIQCDetailMDto?>> ClearDetailMaterial(long? QCIQCMasterId)
        {
            string proc = "Usp_QCIQCDetailMaterial_Clear";
            var param = new DynamicParameters();
            param.Add("@QCIQCMasterId", QCIQCMasterId);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
            var returnData = new ResponseModel<QCIQCDetailMDto?>();
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
                    returnData.HttpResponseCode = 200;
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    returnData.ResponseMessage = result;
                    break;
            }

            return returnData;
        }
        #endregion
    }
}
