using Dapper;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using System.Data;
using static ESD.Extensions.ServiceExtensions;

namespace ESD.Services.Slit
{
    public interface ISlitReceivingService
    {
        Task<ResponseModel<IEnumerable<WIPReturnMaterialDto>?>> GetAll(WIPReturnMaterialDto model);
        Task<ResponseModel<IEnumerable<WIPReturnMaterialLotDto>?>> GetDetailTabWIP(WIPReturnMaterialLotDto model);
        Task<ResponseModel<WIPReturnMaterialLotDto?>> ScanLotReturnTabWIP(WIPReturnMaterialLotDto model);
    }
    [ScopedRegistration]
    public class SlitReceivingService : ISlitReceivingService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public SlitReceivingService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public async Task<ResponseModel<IEnumerable<WIPReturnMaterialDto>?>> GetAll(WIPReturnMaterialDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WIPReturnMaterialDto>?>();
                string proc = "Usp_WIPReturnMaterial_GetAll";
                var param = new DynamicParameters();
                param.Add("@Keyword", model.RMName);
                param.Add("@Description", model.Description);
                param.Add("@Status", model.RMStatus);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WIPReturnMaterialDto>(proc, param);
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

        public async Task<ResponseModel<IEnumerable<WIPReturnMaterialLotDto>?>> GetDetailTabWIP(WIPReturnMaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WIPReturnMaterialLotDto>?>();
                string proc = "Usp_WIPReturnMaterialLot_GetDetailWIP";
                var param = new DynamicParameters();
                param.Add("@WIPRMId", model.WIPRMId);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WIPReturnMaterialLotDto>(proc, param);
                if (!data.Any())
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                }
                else
                {
                    returnData.Data = data;
                }

                return returnData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseModel<WIPReturnMaterialLotDto?>> ScanLotReturnTabWIP(WIPReturnMaterialLotDto model)
        {
            var returnData = new ResponseModel<WIPReturnMaterialLotDto?>();

            string proc = "Usp_WIPReturnMaterialLot_ScanReturnWIP";
            var param = new DynamicParameters();
            param.Add("@WIPRMId", model.WIPRMId);
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
                    returnData.HttpResponseCode = 200;
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }
    }
}
