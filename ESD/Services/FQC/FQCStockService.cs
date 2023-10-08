using ESD.Models.Dtos.Common;
using ESD.Models.Dtos;
using static ESD.Extensions.ServiceExtensions;
using ESD.DbAccess;
using ESD.Extensions;
using System.Data;
using ESD.Models.Dtos.FQC;
using Dapper;

namespace ESD.Services.FQC
{
    public interface IFQCStockService
    {
        Task<ResponseModel<IEnumerable<dynamic>?>> GetStock(SemiMMSDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetLotDetail(WOSemiLotFQCDto model);
    }
    [ScopedRegistration]
    public class FQCStockService : IFQCStockService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public FQCStockService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetStock(SemiMMSDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_FQCStock_Get";
                var param = new DynamicParameters();
                param.Add("@WorkOrder", model.WorkOrder);
                param.Add("@ProductId", model.ProductId);
                param.Add("@ModelId", model.ModelId);
                param.Add("@SemiLotCode", model.SemiLotCode);
                param.Add("@createdDate", model.ReceivedDate);
                param.Add("@LotStatus", model.LotStatus);
                param.Add("@ProductType", model.ProductType);
                param.Add("@BuyerQR", model.BuyerQR);
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
        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetLotDetail(WOSemiLotFQCDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_FQCStock_GetDetail";
                var param = new DynamicParameters();
                param.Add("@ProductId", model.ProductId);
                param.Add("@WorkOrder", model.WorkOrder);
                param.Add("@SemiLotCode", model.SemiLotCode);
                param.Add("@BuyerQR", model.BuyerQR);
                param.Add("@LotStatus", model.LotStatus);
                param.Add("@createdDate", model.createdDateSearch);
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
    }
}
