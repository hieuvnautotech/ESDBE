using ESD.Models.Dtos.Common;
using ESD.Models.Dtos;
using static ESD.Extensions.ServiceExtensions;
using ESD.DbAccess;
using Dapper;
using ESD.Extensions;
using System.Data;
using ESD.Models.Dtos.WIP;
using ESD.Helpers;

namespace ESD.Services.WMS.WIP
{
    public interface IWIPStockService
    {
        Task<ResponseModel<IEnumerable<MaterialDto>?>> GetAll(MaterialLotDto model);
        Task<ResponseModel<IEnumerable<MaterialLotDto>?>> GetDetail(MaterialLotDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetProduct(WOSemiMMSDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetSemiLotDetail(WOSemiMMSDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetListPrintQR(List<long>? listQR);
    }
    [ScopedRegistration]
    public class WIPStockService : IWIPStockService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public WIPStockService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public async Task<ResponseModel<IEnumerable<MaterialDto>?>> GetAll(MaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialDto>?>();
                string proc = "Usp_WIPStock_Get";
                var param = new DynamicParameters();
                param.Add("@MaterialCode", model.MaterialCode);
                param.Add("@MaterialLotCode", model.MaterialLotCode);
                param.Add("@ReceivedDate", model.ReceivedDate?.ToString("yyyy-MM-dd"));
                param.Add("@LotNo", model.LotNo);
                param.Add("@Status", model.isActived);
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
        public async Task<ResponseModel<IEnumerable<MaterialLotDto>?>> GetDetail(MaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialLotDto>?>();
                string proc = "Usp_WIPStock_GetLotDetail";
                var param = new DynamicParameters();
                param.Add("@MaterialId", model.MaterialId);
                param.Add("@MaterialLotCode", model.MaterialLotCode);
                param.Add("@ReceivedDate", model.ReceivedDate?.ToString("yyyy-MM-dd"));
                param.Add("@LotNo", model.LotNo);
                param.Add("@Status", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MaterialLotDto>(proc, param);
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
        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetProduct(WOSemiMMSDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_WIPStock_GetProduct";
                var param = new DynamicParameters();
                param.Add("@ProductType", model.ProductType);
                param.Add("@WorkOrder", model.WorkOrder);
                param.Add("@ModelName", model.ModelName);
                param.Add("@ProductCode", model.ProductCode);
                param.Add("@SemiLotCode", model.SemiLotCode);
                param.Add("@ReceivedDate", model.ReceivedDate?.ToString("yyyy-MM-dd"));
                //param.Add("@Status", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);
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
        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetSemiLotDetail(WOSemiMMSDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_WIPStock_GetSemiLotDetail";
                var param = new DynamicParameters();
                param.Add("@ProductId", model.ProductId);
                param.Add("@WorkOrder", model.WorkOrder);
                param.Add("@SemiLotCode", model.SemiLotCode);
                param.Add("@ReceivedDate", model.ReceivedDate?.ToString("yyyy-MM-dd"));
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);
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
        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetListPrintQR(List<long>? listQR)
        {
            var returnData = new ResponseModel<IEnumerable<dynamic>?>();
            var proc = $"Usp_WIPStock_PrintSemiLots";
            var param = new DynamicParameters();
            param.Add("@listQR", ParameterTvp.GetTableValuedParameter_BigInt(listQR));
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);
            returnData.Data = data;
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
            }
            return returnData;
        }
    }
}
