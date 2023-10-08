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
    public interface IMaterialService
    {
        Task<ResponseModel<IEnumerable<MaterialDto>?>> GetAll(MaterialDto model);
        Task<ResponseModel<MaterialDto?>> GetById(long? MaterialId);
        Task<ResponseModel<MaterialDto?>> Create(MaterialDto model);
        Task<ResponseModel<MaterialDto?>> Modify(MaterialDto model);
        Task<ResponseModel<MaterialDto?>> Delete(MaterialDto model);
    }
    [ScopedRegistration]
    public class MaterialService : IMaterialService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public MaterialService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<ResponseModel<IEnumerable<MaterialDto>?>> GetAll(MaterialDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialDto>?>();
                string proc = "Usp_Material_GetAll"; var param = new DynamicParameters();
                param.Add("@Keyword", model.MaterialCode);
                param.Add("@SupplierId", model.SupplierId);
                param.Add("@StartDate", model.StartDate);
                param.Add("@EndDate", model.EndDate);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MaterialDto>(proc, param);
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

        public async Task<ResponseModel<MaterialDto?>> Create(MaterialDto model)
        {
            var returnData = new ResponseModel<MaterialDto?>();

            var validator = new MaterialValidator();
            var validateResults = validator.Validate(model);
            if (!validateResults.IsValid)
            {
                returnData.HttpResponseCode = 400;
                returnData.ResponseMessage = validateResults.Errors[0].ToString();
                return returnData;
            }

            string proc = "Usp_Material_Create";
            var param = new DynamicParameters();
            param.Add("@MaterialId", model.MaterialId);
            param.Add("@MaterialCode", model.MaterialCode);
            param.Add("@MaterialName", model.MaterialName);
            param.Add("@Description", model.Description);
            param.Add("@MaterialUnit", model.MaterialUnit);
            param.Add("@Width", model.Width);
            param.Add("@Length", model.Length);
            param.Add("@IQCMaterialId", model.IQCMaterialId);
            param.Add("@IQCRawMaterialId", model.IQCRawMaterialId);
            param.Add("@SupplierId", model.SupplierId);
            param.Add("@ExpirationMonth", model.ExpirationMonth);
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
                    returnData = await GetById(model.MaterialId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<MaterialDto?>> Modify(MaterialDto model)
        {
            var returnData = new ResponseModel<MaterialDto?>();

            var validator = new MaterialValidator();
            var validateResults = validator.Validate(model);
            if (!validateResults.IsValid)
            {
                returnData.HttpResponseCode = 400;
                returnData.ResponseMessage = validateResults.Errors[0].ToString();
                return returnData;
            }

            string proc = "Usp_Material_Modify";
            var param = new DynamicParameters();
            param.Add("@MaterialId", model.MaterialId);
            param.Add("@MaterialCode", model.MaterialCode);
            param.Add("@MaterialName", model.MaterialName);
            param.Add("@Description", model.Description);
            param.Add("@MaterialUnit", model.MaterialUnit);
            param.Add("@Width", model.Width);
            param.Add("@Length", model.Length);
            param.Add("@IQCMaterialId", model.IQCMaterialId);
            param.Add("@IQCRawMaterialId", model.IQCRawMaterialId);
            param.Add("@SupplierId", model.SupplierId);
            param.Add("@ExpirationMonth", model.ExpirationMonth);
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
                    returnData = await GetById(model.MaterialId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<MaterialDto?>> GetById(long? MaterialId)
        {
            var returnData = new ResponseModel<MaterialDto?>();
            string proc = "Usp_Material_GetById";
            var param = new DynamicParameters();
            param.Add("@MaterialId", MaterialId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MaterialDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }

        public async Task<ResponseModel<MaterialDto?>> Delete(MaterialDto model)
        {
            string proc = "Usp_Material_Delete";
            var param = new DynamicParameters();
            param.Add("@MaterialId", model.MaterialId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<MaterialDto?>();
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
    }
}
