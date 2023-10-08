using ESD.Models.Dtos.Common;
using ESD.Models.Dtos;
using static ESD.Extensions.ServiceExtensions;
using Dapper;
using ESD.DbAccess;
using ESD.Extensions;
using System.Data;

namespace ESD.Services.WMS.Material
{
    public interface IMaterialPutAwayService
    {
        Task<ResponseModel<IEnumerable<MaterialLotDto>?>> GetAll(PageModel model, DateTime? searchStartDay, DateTime? searchEndDay);
        Task<ResponseModel<MaterialLotDto?>> ScanLot(MaterialLotDto model);
        Task<ResponseModel<MaterialLotDto?>> DeleteLot(MaterialLotDto model);
    }
    [ScopedRegistration]
    public class MaterialPutAwayService : IMaterialPutAwayService
    {
        private readonly ISqlDataAccess _sqlDataAccess;
        public MaterialPutAwayService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<ResponseModel<IEnumerable<MaterialLotDto>?>> GetAll(PageModel model, DateTime? searchStartDay, DateTime? searchEndDay)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialLotDto>?>();
                string proc = "Usp_MaterialPutAway_GetAll"; var param = new DynamicParameters();
                param.Add("@StartDate", searchStartDay);
                param.Add("@EndDate", searchEndDay);
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

        public async Task<ResponseModel<MaterialLotDto?>> ScanLot(MaterialLotDto model)
        {
            var returnData = new ResponseModel<MaterialLotDto?>();

            string proc = "Usp_MaterialPutAway_Scan";
            var param = new DynamicParameters();
            param.Add("@LocationShelfId", model.LocationShelfId);
            param.Add("@MaterialLotCode", model.MaterialLotCode);
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
                    returnData = await GetByCode(model.MaterialLotCode);
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<MaterialLotDto?>> DeleteLot(MaterialLotDto model)
        {
            string proc = "Usp_MaterialPutAway_Delete";
            var param = new DynamicParameters();
            param.Add("@MaterialLotId", model.MaterialLotId);
            param.Add("@row_version", model.row_version);
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

        public async Task<ResponseModel<MaterialLotDto?>> GetByCode(string? MaterialLotCode)
        {
            var returnData = new ResponseModel<MaterialLotDto?>();
            string proc = "Usp_MaterialPutAway_GetByCode";
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
    }
}
