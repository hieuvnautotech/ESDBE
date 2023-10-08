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
    public interface IRoutingService
    {
        Task<ResponseModel<IEnumerable<RoutingDto>?>> GetAll(RoutingDto model);
        //Task<ResponseModel<RoutingDto?>> GetById(long? RoutingId);
        //Task<ResponseModel<RoutingDto?>> Create(RoutingDto model);
        //Task<ResponseModel<RoutingDto?>> Modify(RoutingDto model);
        //Task<ResponseModel<RoutingDto?>> Delete(RoutingDto model);
    }
    [ScopedRegistration]
    public class RoutingService : IRoutingService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public RoutingService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<ResponseModel<IEnumerable<RoutingDto>?>> GetAll(RoutingDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<RoutingDto>?>();
                string proc = "Usp_Routing_GetAll"; 
                var param = new DynamicParameters();
                //param.Add("@Keyword", model.RoutingCode);
                //param.Add("@SupplierId", model.SupplierId);
                //param.Add("@StartDate", model.StartDate);
                //param.Add("@EndDate", model.EndDate);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<RoutingDto>(proc, param);
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

        //public async Task<ResponseModel<RoutingDto?>> Create(RoutingDto model)
        //{
        //    var returnData = new ResponseModel<RoutingDto?>();

        //    var validator = new RoutingValidator();
        //    var validateResults = validator.Validate(model);
        //    if (!validateResults.IsValid)
        //    {
        //        returnData.HttpResponseCode = 400;
        //        returnData.ResponseMessage = validateResults.Errors[0].ToString();
        //        return returnData;
        //    }

        //    string proc = "Usp_Routing_Create";
        //    var param = new DynamicParameters();
        //    param.Add("@RoutingId", model.RoutingId);
        //    param.Add("@RoutingCode", model.RoutingCode);
        //    param.Add("@RoutingName", model.RoutingName);
        //    param.Add("@Description", model.Description);
        //    param.Add("@RoutingUnit", model.RoutingUnit);
        //    param.Add("@Width", model.Width);
        //    param.Add("@Length", model.Length);
        //    param.Add("@IQCRoutingId", model.IQCRoutingId);
        //    param.Add("@IQCRawRoutingId", model.IQCRawRoutingId);
        //    param.Add("@SupplierId", model.SupplierId);
        //    param.Add("@createdBy", model.createdBy);
        //    param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

        //    var result = await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);

        //    returnData.ResponseMessage = result;
        //    switch (result)
        //    {
        //        case StaticReturnValue.SYSTEM_ERROR:
        //            returnData.HttpResponseCode = 500;
        //            break;
        //        case StaticReturnValue.SUCCESS:
        //            returnData = await GetById(model.RoutingId);
        //            returnData.ResponseMessage = result;
        //            break;
        //        default:
        //            returnData.HttpResponseCode = 400;
        //            break;
        //    }
        //    return returnData;
        //}

        //public async Task<ResponseModel<RoutingDto?>> Modify(RoutingDto model)
        //{
        //    var returnData = new ResponseModel<RoutingDto?>();

        //    var validator = new RoutingValidator();
        //    var validateResults = validator.Validate(model);
        //    if (!validateResults.IsValid)
        //    {
        //        returnData.HttpResponseCode = 400;
        //        returnData.ResponseMessage = validateResults.Errors[0].ToString();
        //        return returnData;
        //    }

        //    string proc = "Usp_Routing_Modify";
        //    var param = new DynamicParameters();
        //    param.Add("@RoutingId", model.RoutingId);
        //    param.Add("@RoutingCode", model.RoutingCode);
        //    param.Add("@RoutingName", model.RoutingName);
        //    param.Add("@Description", model.Description);
        //    param.Add("@RoutingUnit", model.RoutingUnit);
        //    param.Add("@Width", model.Width);
        //    param.Add("@Length", model.Length);
        //    param.Add("@IQCRoutingId", model.IQCRoutingId);
        //    param.Add("@IQCRawRoutingId", model.IQCRawRoutingId);
        //    param.Add("@SupplierId", model.SupplierId);
        //    param.Add("@createdBy", model.createdBy);
        //    param.Add("@row_version", model.row_version);
        //    param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

        //    var result = await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);

        //    returnData.ResponseMessage = result;
        //    switch (result)
        //    {
        //        case StaticReturnValue.SYSTEM_ERROR:
        //            returnData.HttpResponseCode = 500;
        //            break;
        //        case StaticReturnValue.REFRESH_REQUIRED:
        //            returnData.HttpResponseCode = 500;
        //            returnData.ResponseMessage = result;
        //            break;
        //        case StaticReturnValue.SUCCESS:
        //            returnData = await GetById(model.RoutingId);
        //            returnData.ResponseMessage = result;
        //            break;
        //        default:
        //            returnData.HttpResponseCode = 400;
        //            break;
        //    }
        //    return returnData;
        //}

        //public async Task<ResponseModel<RoutingDto?>> GetById(long? RoutingId)
        //{
        //    var returnData = new ResponseModel<RoutingDto?>();
        //    string proc = "Usp_Routing_GetById";
        //    var param = new DynamicParameters();
        //    param.Add("@RoutingId", RoutingId);

        //    var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<RoutingDto>(proc, param);
        //    returnData.Data = data.FirstOrDefault();
        //    if (!data.Any())
        //    {
        //        returnData.HttpResponseCode = 204;
        //        returnData.ResponseMessage = "NO DATA";
        //    }
        //    return returnData;
        //}

        //public async Task<ResponseModel<RoutingDto?>> Delete(RoutingDto model)
        //{
        //    string proc = "Usp_Routing_Delete";
        //    var param = new DynamicParameters();
        //    param.Add("@RoutingId", model.RoutingId);
        //    param.Add("@row_version", model.row_version);
        //    param.Add("@createdBy", model.createdBy);
        //    param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

        //    var returnData = new ResponseModel<RoutingDto?>();
        //    var result = await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
        //    switch (result)
        //    {
        //        case StaticReturnValue.SYSTEM_ERROR:
        //            returnData.HttpResponseCode = 500;
        //            break;
        //        case StaticReturnValue.REFRESH_REQUIRED:
        //            returnData.HttpResponseCode = 500;
        //            returnData.ResponseMessage = result;
        //            break;
        //        case StaticReturnValue.SUCCESS:
        //            break;
        //        default:
        //            returnData.HttpResponseCode = 400;
        //            break;
        //    }

        //    return returnData;
        //}
    }
}
