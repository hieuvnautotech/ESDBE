using Dapper;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Models.Dtos.Common;
using ESD.Models.Dtos.FQC;
using System.Data;
using static ESD.Extensions.ServiceExtensions;

namespace ESD.Services.FQC
{
    public interface IFQCRoutingService
    {
        Task<ResponseModel<IEnumerable<ProductRoutingDto>?>> GetRoutingDetail(ProductRoutingDto model);
        Task<ResponseModel<ProductRoutingDto?>> CreateRoutingDetail(ProductRoutingDto model);
        Task<ResponseModel<ProductRoutingDto?>> ModifyDetail(ProductRoutingDto model);
        Task<ResponseModel<ProductRoutingDto?>> DeleteDetail(ProductRoutingDto model);
    }
    [ScopedRegistration]
    public class FQCRoutingService : IFQCRoutingService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public FQCRoutingService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<ResponseModel<IEnumerable<ProductRoutingDto>?>> GetRoutingDetail(ProductRoutingDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<ProductRoutingDto>?>();
                string proc = "Usp_FQCRouting_GetDetail"; var param = new DynamicParameters();
                param.Add("@ProductCode", model.ProductCode);
                param.Add("@ProcessName", model.ProcessName);
                param.Add("@RoutingLevel", model.RoutingLevel);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<ProductRoutingDto>(proc, param);
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

        public async Task<ResponseModel<ProductRoutingDto?>> CreateRoutingDetail(ProductRoutingDto model)
        {
            var returnData = new ResponseModel<ProductRoutingDto?>();

            string proc = "Usp_FQCRouting_CreateDetail";
            var param = new DynamicParameters();
            param.Add("@RoutingId", model.RoutingId);
            param.Add("@ProductCode", model.ProductCode);
            param.Add("@ProcessCode", model.ProcessCode);
            param.Add("@RoutingLevel", model.RoutingLevel);
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
                    returnData = await GetByIdAPPRoutingDetail(model.RoutingId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<ProductRoutingDto?>> GetByIdAPPRoutingDetail(long? RoutingId)
        {
            var returnData = new ResponseModel<ProductRoutingDto?>();
            string proc = "Usp_FQCRouting_GetDetailById";
            var param = new DynamicParameters();
            param.Add("@RoutingId", RoutingId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<ProductRoutingDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }

        public async Task<ResponseModel<ProductRoutingDto?>> ModifyDetail(ProductRoutingDto model)
        {
            var returnData = new ResponseModel<ProductRoutingDto?>();

            string proc = "Usp_FQCRouting_ModifyDetail";
            var param = new DynamicParameters();
            param.Add("@RoutingId", model.RoutingId);
            param.Add("@ProductCode", model.ProductCode);
            param.Add("@ProcessCode", model.ProcessCode);
            param.Add("@RoutingLevel", model.RoutingLevel);
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
                    returnData = await GetByIdAPPRoutingDetail(model.RoutingId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<ProductRoutingDto?>> DeleteDetail(ProductRoutingDto model)
        {
            string proc = "Usp_FQCRouting_DeleteDetail";
            var param = new DynamicParameters();
            param.Add("@RoutingId", model.RoutingId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<ProductRoutingDto?>();
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
