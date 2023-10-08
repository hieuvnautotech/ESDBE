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
    public interface IQCOQCService
    {
        Task<ResponseModel<IEnumerable<QCOQCMasterDto>?>> GetAll(QCOQCMasterDto model);
        Task<ResponseModel<QCOQCMasterDto?>> GetById(long? QCOQCMasterId);
        Task<ResponseModel<QCOQCMasterDto?>> Create(QCOQCMasterDto model);
        Task<ResponseModel<QCOQCMasterDto?>> Modify(QCOQCMasterDto model);
        Task<ResponseModel<QCOQCMasterDto?>> Delete(QCOQCMasterDto model);
        Task<ResponseModel<QCOQCMasterDto?>> Confirm(QCOQCMasterDto model);
        Task<ResponseModel<QCOQCMasterDto?>> Copy(QCOQCMasterDto model);

        Task<ResponseModel<IEnumerable<QCOQCDetailDto>?>> GetDetail(QCOQCDetailDto model);
        Task<ResponseModel<QCOQCDetailDto?>> CreateDetail(QCOQCDetailDto model);
        Task<ResponseModel<QCOQCDetailDto?>> DeleteDetail(QCOQCDetailDto model);
        Task<ResponseModel<QCOQCDetailDto?>> ClearDetail(long? QCOQCMasterId);
    }
    [ScopedRegistration]
    public class QCOQCService : IQCOQCService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public QCOQCService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        #region Master
        public async Task<ResponseModel<IEnumerable<QCOQCMasterDto>?>> GetAll(QCOQCMasterDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<QCOQCMasterDto>?>();
                string proc = "Usp_QCOQCMaster_GetAll"; var param = new DynamicParameters();
                param.Add("@QCOQCMasterName", model.QCOQCMasterName);
                param.Add("@ProductId", model.ProductId);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCOQCMasterDto>(proc, param);
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

        public async Task<ResponseModel<QCOQCMasterDto?>> Create(QCOQCMasterDto model)
        {
            var returnData = new ResponseModel<QCOQCMasterDto?>();

            var products = new List<long>();
            foreach (var item in model.Products)
            {
                products.Add(item.ProductId);
            }

            string proc = "Usp_QCOQCMaster_Create";
            var param = new DynamicParameters();
            param.Add("@QCOQCMasterId", model.QCOQCMasterId);
            param.Add("@QCOQCMasterName", model.QCOQCMasterName);
            param.Add("@OQCType", model.OQCType);
            param.Add("@ProductIds", Helpers.ParameterTvp.GetTableValuedParameter_BigInt(products));
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
                    returnData = await GetById(model.QCOQCMasterId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<QCOQCMasterDto?>> Modify(QCOQCMasterDto model)
        {
            var returnData = new ResponseModel<QCOQCMasterDto?>();

            var products = new List<long>();
            foreach (var item in model.Products)
            {
                products.Add(item.ProductId);
            }

            string proc = "Usp_QCOQCMaster_Modify";
            var param = new DynamicParameters();
            param.Add("@QCOQCMasterId", model.QCOQCMasterId);
            param.Add("@QCOQCMasterName", model.QCOQCMasterName);
            param.Add("@OQCType", model.OQCType);
            param.Add("@ProductIds", Helpers.ParameterTvp.GetTableValuedParameter_BigInt(products));
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
                    returnData = await GetById(model.QCOQCMasterId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<QCOQCMasterDto?>> GetById(long? QCOQCMasterId)
        {
            var returnData = new ResponseModel<QCOQCMasterDto?>();
            string proc = "Usp_QCOQCMaster_GetById";
            var param = new DynamicParameters();
            param.Add("@QCOQCMasterId", QCOQCMasterId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCOQCMasterDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }

        public async Task<ResponseModel<QCOQCMasterDto?>> Delete(QCOQCMasterDto model)
        {
            string proc = "Usp_QCOQCMaster_Delete";
            var param = new DynamicParameters();
            param.Add("@QCOQCMasterId", model.QCOQCMasterId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<QCOQCMasterDto?>();
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

        public async Task<ResponseModel<QCOQCMasterDto?>> Confirm(QCOQCMasterDto model)
        {
            string proc = "Usp_QCOQCMaster_Confirm";
            var param = new DynamicParameters();
            param.Add("@QCOQCMasterId", model.QCOQCMasterId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<QCOQCMasterDto?>();
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

        public async Task<ResponseModel<QCOQCMasterDto?>> Copy(QCOQCMasterDto model)
        {
            var returnData = new ResponseModel<QCOQCMasterDto?>();
            string proc = "Usp_QCOQCMaster_Copy";
            var NewId = AutoId.AutoGenerate();
            var param = new DynamicParameters();
            param.Add("@QCOQCMasterId", model.QCOQCMasterId);
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
        public async Task<ResponseModel<IEnumerable<QCOQCDetailDto>?>> GetDetail(QCOQCDetailDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<QCOQCDetailDto>?>();
                string proc = "Usp_QCOQCDetail_GetAll";
                var param = new DynamicParameters();
                param.Add("@QCOQCMasterId", model.QCOQCMasterId);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCOQCDetailDto>(proc, param);
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

        public async Task<ResponseModel<QCOQCDetailDto?>> CreateDetail(QCOQCDetailDto model)
        {
            var returnData = new ResponseModel<QCOQCDetailDto?>();

            string proc = "Usp_QCOQCDetail_Create";
            var param = new DynamicParameters();
            param.Add("@QCOQCDetailId", model.QCOQCDetailId);
            param.Add("@QCOQCMasterId", model.QCOQCMasterId);
            param.Add("@QCItemId", model.QCItemId);
            param.Add("@QCTypeId", model.QCTypeId);
            param.Add("@QCStandardId", model.QCStandardId);
            param.Add("@QCFrequencyId", model.QCFrequencyId);
            param.Add("@QCToolId", model.QCToolId);
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

        public async Task<ResponseModel<QCOQCDetailDto?>> DeleteDetail(QCOQCDetailDto model)
        {
            string proc = "Usp_QCOQCDetail_Delete";
            var param = new DynamicParameters();
            param.Add("@QCOQCDetailId", model.QCOQCDetailId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<QCOQCDetailDto?>();
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

        public async Task<ResponseModel<QCOQCDetailDto?>> ClearDetail(long? QCOQCMasterId)
        {
            string proc = "Usp_QCOQCDetail_Clear";
            var param = new DynamicParameters();
            param.Add("@QCOQCMasterId", QCOQCMasterId);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
            var returnData = new ResponseModel<QCOQCDetailDto?>();
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
