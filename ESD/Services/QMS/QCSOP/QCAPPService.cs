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
    public interface IQCAPPService
    {
        Task<ResponseModel<IEnumerable<QCAPPMasterDto>?>> GetAll(QCAPPMasterDto model);
        Task<ResponseModel<QCAPPMasterDto?>> GetById(long? QCAPPMasterId);
        Task<ResponseModel<QCAPPMasterDto?>> Create(QCAPPMasterDto model);
        Task<ResponseModel<QCAPPMasterDto?>> Modify(QCAPPMasterDto model);
        Task<ResponseModel<QCAPPMasterDto?>> Delete(QCAPPMasterDto model);
        Task<ResponseModel<QCAPPMasterDto?>> Confirm(QCAPPMasterDto model);
        Task<ResponseModel<QCAPPMasterDto?>> Copy(QCAPPMasterDto model);

        Task<ResponseModel<IEnumerable<QCAPPDetailDto>?>> GetDetail(QCAPPDetailDto model);
        Task<ResponseModel<QCAPPDetailDto?>> CreateDetail(QCAPPDetailDto model);
        Task<ResponseModel<QCAPPDetailDto?>> DeleteDetail(QCAPPDetailDto model);
        Task<ResponseModel<QCAPPDetailDto?>> ClearDetail(long? QCAPPMasterId);
    }
    [ScopedRegistration]
    public class QCAPPService : IQCAPPService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public QCAPPService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        #region Master
        public async Task<ResponseModel<IEnumerable<QCAPPMasterDto>?>> GetAll(QCAPPMasterDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<QCAPPMasterDto>?>();
                string proc = "Usp_QCAPPMaster_GetAll"; var param = new DynamicParameters();
                param.Add("@QCAPPMasterName", model.QCAPPMasterName);
                param.Add("@ProductId", model.ProductId);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCAPPMasterDto>(proc, param);
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

        public async Task<ResponseModel<QCAPPMasterDto?>> Create(QCAPPMasterDto model)
        {
            var returnData = new ResponseModel<QCAPPMasterDto?>();

            var products = new List<long>();
            foreach (var item in model.Products)
            {
                products.Add(item.ProductId);
            }

            string proc = "Usp_QCAPPMaster_Create";
            var param = new DynamicParameters();
            param.Add("@QCAPPMasterId", model.QCAPPMasterId);
            param.Add("@QCAPPMasterName", model.QCAPPMasterName);
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
                    returnData = await GetById(model.QCAPPMasterId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<QCAPPMasterDto?>> Modify(QCAPPMasterDto model)
        {
            var returnData = new ResponseModel<QCAPPMasterDto?>();

            var products = new List<long>();
            foreach (var item in model.Products)
            {
                products.Add(item.ProductId);
            }

            string proc = "Usp_QCAPPMaster_Modify";
            var param = new DynamicParameters();
            param.Add("@QCAPPMasterId", model.QCAPPMasterId);
            param.Add("@QCAPPMasterName", model.QCAPPMasterName);
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
                    returnData = await GetById(model.QCAPPMasterId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<QCAPPMasterDto?>> GetById(long? QCAPPMasterId)
        {
            var returnData = new ResponseModel<QCAPPMasterDto?>();
            string proc = "Usp_QCAPPMaster_GetById";
            var param = new DynamicParameters();
            param.Add("@QCAPPMasterId", QCAPPMasterId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCAPPMasterDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }

        public async Task<ResponseModel<QCAPPMasterDto?>> Delete(QCAPPMasterDto model)
        {
            string proc = "Usp_QCAPPMaster_Delete";
            var param = new DynamicParameters();
            param.Add("@QCAPPMasterId", model.QCAPPMasterId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<QCAPPMasterDto?>();
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

        public async Task<ResponseModel<QCAPPMasterDto?>> Confirm(QCAPPMasterDto model)
        {
            string proc = "Usp_QCAPPMaster_Confirm";
            var param = new DynamicParameters();
            param.Add("@QCAPPMasterId", model.QCAPPMasterId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<QCAPPMasterDto?>();
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

        public async Task<ResponseModel<QCAPPMasterDto?>> Copy(QCAPPMasterDto model)
        {
            var returnData = new ResponseModel<QCAPPMasterDto?>();
            string proc = "Usp_QCAPPMaster_Copy";
            var NewId = AutoId.AutoGenerate();
            var param = new DynamicParameters();
            param.Add("@QCAPPMasterId", model.QCAPPMasterId);
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
        public async Task<ResponseModel<IEnumerable<QCAPPDetailDto>?>> GetDetail(QCAPPDetailDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<QCAPPDetailDto>?>();
                string proc = "Usp_QCAPPDetail_GetAll";
                var param = new DynamicParameters();
                param.Add("@QCAPPMasterId", model.QCAPPMasterId);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCAPPDetailDto>(proc, param);
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

        public async Task<ResponseModel<QCAPPDetailDto?>> CreateDetail(QCAPPDetailDto model)
        {
            var returnData = new ResponseModel<QCAPPDetailDto?>();

            string proc = "Usp_QCAPPDetail_Create";
            var param = new DynamicParameters();
            param.Add("@QCAPPDetailId", model.QCAPPDetailId);
            param.Add("@QCAPPMasterId", model.QCAPPMasterId);
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

        public async Task<ResponseModel<QCAPPDetailDto?>> DeleteDetail(QCAPPDetailDto model)
        {
            string proc = "Usp_QCAPPDetail_Delete";
            var param = new DynamicParameters();
            param.Add("@QCAPPDetailId", model.QCAPPDetailId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<QCAPPDetailDto?>();
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

        public async Task<ResponseModel<QCAPPDetailDto?>> ClearDetail(long? QCAPPMasterId)
        {
            string proc = "Usp_QCAPPDetail_Clear";
            var param = new DynamicParameters();
            param.Add("@QCAPPMasterId", QCAPPMasterId);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
            var returnData = new ResponseModel<QCAPPDetailDto?>();
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
