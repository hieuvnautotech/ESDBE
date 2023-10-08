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
    public interface IQCPQCService
    {
        Task<ResponseModel<IEnumerable<QCPQCMasterDto>?>> GetAll(QCPQCMasterDto model);
        Task<ResponseModel<QCPQCMasterDto?>> GetById(long? QCPQCMasterId);
        Task<ResponseModel<QCPQCMasterDto?>> Create(QCPQCMasterDto model);
        Task<ResponseModel<QCPQCMasterDto?>> Modify(QCPQCMasterDto model);
        Task<ResponseModel<QCPQCMasterDto?>> Delete(QCPQCMasterDto model);
        Task<ResponseModel<QCPQCMasterDto?>> Confirm(QCPQCMasterDto model);
        Task<ResponseModel<QCPQCMasterDto?>> Copy(QCPQCMasterDto model);

        Task<ResponseModel<IEnumerable<QCPQCDetailASDto>?>> GetDetailAS(QCPQCDetailASDto model);
        Task<ResponseModel<QCPQCDetailASDto?>> CreateDetailAS(QCPQCDetailASDto model);
        Task<ResponseModel<QCPQCDetailASDto?>> DeleteDetailAS(QCPQCDetailASDto model);

        Task<ResponseModel<IEnumerable<QCPQCDetailSLDto>?>> GetDetailSL(QCPQCDetailSLDto model);
        Task<ResponseModel<QCPQCDetailSLDto?>> CreateDetailSL(QCPQCDetailSLDto model);
        Task<ResponseModel<QCPQCDetailSLDto?>> DeleteDetailSL(QCPQCDetailSLDto model);
        Task<ResponseModel<QCPQCDetailSLDto?>> ClearDetailSL(long? QCPQCMasterId);
    }
    [ScopedRegistration]
    public class QCPQCService : IQCPQCService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public QCPQCService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        #region Master
        public async Task<ResponseModel<IEnumerable<QCPQCMasterDto>?>> GetAll(QCPQCMasterDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<QCPQCMasterDto>?>();
                string proc = "Usp_QCPQCMaster_GetAll"; var param = new DynamicParameters();
                param.Add("@QCPQCMasterName", model.QCPQCMasterName);
                param.Add("@ProductId", model.ProductId);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCPQCMasterDto>(proc, param);
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

        public async Task<ResponseModel<QCPQCMasterDto?>> Create(QCPQCMasterDto model)
        {

            try
            {
                var returnData = new ResponseModel<QCPQCMasterDto?>();

            string proc = "Usp_QCPQCMaster_Create";
            var param = new DynamicParameters();
            param.Add("@QCPQCMasterId", model.QCPQCMasterId);
            param.Add("@QCPQCMasterName", model.QCPQCMasterName);
            param.Add("@REVDate", model.REVDate);
            param.Add("@ProductIds", Helpers.ParameterTvp.GetTableValuedParameter_BigInt(model.Products));
            param.Add("@ProcessCodes", Helpers.ParameterTvp.GetTableValuedParameter_Varchar(model.Process));
            //param.Add("@ProcessId", model.ProcessId);
            param.Add("@Description", model.Description);
            param.Add("@ImageFile", model.ImageFile);
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
                    returnData = await GetById(model.QCPQCMasterId);
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

        public async Task<ResponseModel<QCPQCMasterDto?>> Modify(QCPQCMasterDto model)
        {
            var returnData = new ResponseModel<QCPQCMasterDto?>();

            if (model.Products == null)
                model.Products = new List<long> { 0 };

            if (model.Process == null)
                model.Process = new List<string> { "" };

            string proc = "Usp_QCPQCMaster_Modify";
            var param = new DynamicParameters();
            param.Add("@QCPQCMasterId", model.QCPQCMasterId);
            param.Add("@QCPQCMasterName", model.QCPQCMasterName);
            param.Add("@REVDate", model.REVDate);
            param.Add("@ProductIds", Helpers.ParameterTvp.GetTableValuedParameter_BigInt(model.Products));
            param.Add("@ProcessCodes", Helpers.ParameterTvp.GetTableValuedParameter_Varchar(model.Process));
            //param.Add("@ProcessId", model.ProcessId);
            param.Add("@Description", model.Description);
            param.Add("@ImageFile", model.ImageFile);
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
                    returnData = await GetById(model.QCPQCMasterId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<QCPQCMasterDto?>> GetById(long? QCPQCMasterId)
        {
            var returnData = new ResponseModel<QCPQCMasterDto?>();
            string proc = "Usp_QCPQCMaster_GetById";
            var param = new DynamicParameters();
            param.Add("@QCPQCMasterId", QCPQCMasterId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCPQCMasterDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }

        public async Task<ResponseModel<QCPQCMasterDto?>> Delete(QCPQCMasterDto model)
        {
            string proc = "Usp_QCPQCMaster_Delete";
            var param = new DynamicParameters();
            param.Add("@QCPQCMasterId", model.QCPQCMasterId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<QCPQCMasterDto?>();
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

        public async Task<ResponseModel<QCPQCMasterDto?>> Confirm(QCPQCMasterDto model)
        {
            string proc = "Usp_QCPQCMaster_Confirm";
            var param = new DynamicParameters();
            param.Add("@QCPQCMasterId", model.QCPQCMasterId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<QCPQCMasterDto?>();
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

        public async Task<ResponseModel<QCPQCMasterDto?>> Copy(QCPQCMasterDto model)
        {
            var returnData = new ResponseModel<QCPQCMasterDto?>();
            string proc = "Usp_QCPQCMaster_Copy";
            var NewId = AutoId.AutoGenerate();
            var param = new DynamicParameters();
            param.Add("@QCPQCMasterId", model.QCPQCMasterId);
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

        #region Detail AS
        public async Task<ResponseModel<IEnumerable<QCPQCDetailASDto>?>> GetDetailAS(QCPQCDetailASDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<QCPQCDetailASDto>?>();
                string proc = "Usp_QCPQCDetailAS_GetAll"; 
                var param = new DynamicParameters();
                param.Add("@QCPQCMasterId", model.QCPQCMasterId);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCPQCDetailASDto>(proc, param);
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

        public async Task<ResponseModel<QCPQCDetailASDto?>> CreateDetailAS(QCPQCDetailASDto model)
        {
            var returnData = new ResponseModel<QCPQCDetailASDto?>();

            string proc = "Usp_QCPQCDetailAS_Create";
            var param = new DynamicParameters();
            param.Add("@QCPQCDetailASId", model.QCPQCDetailASId);
            param.Add("@QCPQCMasterId", model.QCPQCMasterId);
            param.Add("@QCItemId", model.QCItemId);
            param.Add("@QCTypeId", model.QCTypeId);
            param.Add("@QCStandardId", model.QCStandardId);
            param.Add("@QCToolId", model.QCToolId);
            param.Add("@QCFrequencyId", model.QCFrequencyId);
            param.Add("@Samples", model.Samples);
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

        public async Task<ResponseModel<QCPQCDetailASDto?>> DeleteDetailAS(QCPQCDetailASDto model)
        {
            string proc = "Usp_QCPQCDetailAS_Delete";
            var param = new DynamicParameters();
            param.Add("@QCPQCDetailASId", model.QCPQCDetailASId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<QCPQCDetailASDto?>();
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
        #endregion

        #region Detail SL
        public async Task<ResponseModel<IEnumerable<QCPQCDetailSLDto>?>> GetDetailSL(QCPQCDetailSLDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<QCPQCDetailSLDto>?>();
                string proc = "Usp_QCPQCDetailSL_GetAll";
                var param = new DynamicParameters();
                param.Add("@QCPQCMasterId", model.QCPQCMasterId);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCPQCDetailSLDto>(proc, param);
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

        public async Task<ResponseModel<QCPQCDetailSLDto?>> CreateDetailSL(QCPQCDetailSLDto model)
        {
            var returnData = new ResponseModel<QCPQCDetailSLDto?>();

            string proc = "Usp_QCPQCDetailSL_Create";
            var param = new DynamicParameters();
            param.Add("@QCPQCDetailSLId", model.QCPQCDetailSLId);
            param.Add("@QCPQCMasterId", model.QCPQCMasterId);
            param.Add("@QCItemId", model.QCItemId);
            param.Add("@QCTypeId", model.QCTypeId);
            param.Add("@QCStandardId", model.QCStandardId);
            param.Add("@QCToolId", model.QCToolId);
            param.Add("@QCFrequencyId", model.QCFrequencyId);
            param.Add("@LocationToCheck", model.LocationToCheck);
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

        public async Task<ResponseModel<QCPQCDetailSLDto?>> DeleteDetailSL(QCPQCDetailSLDto model)
        {
            string proc = "Usp_QCPQCDetailSL_Delete";
            var param = new DynamicParameters();
            param.Add("@QCPQCDetailSLId", model.QCPQCDetailSLId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<QCPQCDetailSLDto?>();
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

        public async Task<ResponseModel<QCPQCDetailSLDto?>> ClearDetailSL(long? QCPQCMasterId)
        {
            string proc = "Usp_QCPQCDetailSL_Clear";
            var param = new DynamicParameters();
            param.Add("@QCPQCMasterId", QCPQCMasterId);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
            var returnData = new ResponseModel<QCPQCDetailSLDto?>();
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
