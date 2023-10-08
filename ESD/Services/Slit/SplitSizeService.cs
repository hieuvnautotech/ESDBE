using ESD.Models.Dtos.Common;
using ESD.Models.Dtos;
using static ESD.Extensions.ServiceExtensions;
using ESD.DbAccess;
using Dapper;
using ESD.Extensions;
using System.Data;
using ESD.Models.Dtos.Slit;
using Newtonsoft.Json;

namespace ESD.Services.Slit
{
    public interface ISplitSizeService
    {
        Task<ResponseModel<IEnumerable<MaterialLotDto>?>> GetAll(MaterialLotDto model);
        Task<ResponseModel<IEnumerable<SlitTurnDto>?>> GetDetail(MaterialLotDto model);
        Task<ResponseModel<MaterialLotDto?>> ScanLotMaterial(string MaterialLotCode);
        Task<ResponseModel<MaterialLotDto?>> SplitLot(long MaterialLotId, List<SlitSplitDto> List, long createdBy);
        Task<ResponseModel<MaterialLotDto?>> Reset(MaterialLotDto model);
    }
    [ScopedRegistration]
    public class SplitSizeService : ISplitSizeService
    {
        private readonly ISqlDataAccess _sqlDataAccess;
        public SplitSizeService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public async Task<ResponseModel<IEnumerable<MaterialLotDto>?>> GetAll(MaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialLotDto>?>();
                string proc = "Usp_SplitSize_GetAll"; 
                var param = new DynamicParameters();
                param.Add("@MaterialLotCode", model.MaterialLotCode);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MaterialLotDto>(proc, param);
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
        public async Task<ResponseModel<IEnumerable<SlitTurnDto>?>> GetDetail(MaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<SlitTurnDto>?>();
                string proc = "Usp_SplitSize_GetDetail";
                var param = new DynamicParameters();
                param.Add("@MaterialLotId", model.MaterialLotId);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<SlitTurnDto>(proc, param);
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
        public async Task<ResponseModel<MaterialLotDto?>> ScanLotMaterial(string MaterialLotCode)
        {
            var returnData = new ResponseModel<MaterialLotDto?>();
            string proc = "Usp_SplitSize_GetLotCode";
            var param = new DynamicParameters();
            param.Add("@MaterialLotCode", MaterialLotCode);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MaterialLotDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            returnData.ResponseMessage = StaticReturnValue.SUCCESS;
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }
        public async Task<ResponseModel<MaterialLotDto?>> SplitLot(long MaterialLotId, List<SlitSplitDto> List, long createdBy)
        {
            var jsonLotList = JsonConvert.SerializeObject(List);

            string proc = "Usp_SplitSize_Split";
            var param = new DynamicParameters();
            param.Add("@MaterialLotId", MaterialLotId);
            param.Add("@Jsonlist", jsonLotList);
            param.Add("@createdBy", createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<MaterialLotDto?>();
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

        public async Task<ResponseModel<MaterialLotDto?>> Reset(MaterialLotDto model)
        {
            string proc = "Usp_SplitSize_Reset";
            var param = new DynamicParameters();
            param.Add("@MaterialLotId", model.MaterialLotId);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<MaterialLotDto?>();
            var result = await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
            returnData.ResponseMessage = result;
            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.REFRESH_REQUIRED:
                    returnData.HttpResponseCode = 500;
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
