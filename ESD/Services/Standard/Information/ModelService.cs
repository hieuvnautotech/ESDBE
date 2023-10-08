using ESD.Models.Dtos.Common;
using ESD.Models.Dtos;
using Dapper;
using ESD.DbAccess;
using ESD.Extensions;
using System.Data;
using static ESD.Extensions.ServiceExtensions;

namespace ESD.Services.Standard.Information
{
    public interface IModelService
    {
        Task<ResponseModel<IEnumerable<ModelDto>?>> GetAll(ModelDto model);
        Task<ResponseModel<ModelDto?>> GetById(long? ModelId);
        Task<ResponseModel<ModelDto?>> Create(ModelDto model);
        Task<ResponseModel<ModelDto?>> Modify(ModelDto model);
        Task<ResponseModel<ModelDto?>> Delete(ModelDto model);

    }
    [ScopedRegistration]
    public class ModelService : IModelService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public ModelService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<ResponseModel<IEnumerable<ModelDto>?>> GetAll(ModelDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<ModelDto>?>();
                string proc = "Usp_Model_GetAll"; var param = new DynamicParameters();
                param.Add("@ModelCode", model.ModelCode);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<ModelDto>(proc, param);
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

        public async Task<ResponseModel<ModelDto?>> Create(ModelDto model)
        {
            var returnData = new ResponseModel<ModelDto?>();

            string proc = "Usp_Model_Create";
            var param = new DynamicParameters();
            param.Add("@ModelId", model.ModelId);
            param.Add("@ModelCode", model.ModelCode);
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
                    returnData = await GetById(model.ModelId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<ModelDto?>> Modify(ModelDto model)
        {
            var returnData = new ResponseModel<ModelDto?>();

            string proc = "Usp_Model_Modify";
            var param = new DynamicParameters();
            param.Add("@ModelId", model.ModelId);
            param.Add("@ModelCode", model.ModelCode);
            param.Add("@modifiedBy", model.createdBy);
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
                    returnData = await GetById(model.ModelId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<ModelDto?>> GetById(long? ModelId)
        {
            var returnData = new ResponseModel<ModelDto?>();
            string proc = "Usp_Model_GetById";
            var param = new DynamicParameters();
            param.Add("@ModelId", ModelId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<ModelDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }

        public async Task<ResponseModel<ModelDto?>> Delete(ModelDto model)
        {
            string proc = "Usp_Model_Delete";
            var param = new DynamicParameters();
            param.Add("@ModelId", model.ModelId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<ModelDto?>();
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
