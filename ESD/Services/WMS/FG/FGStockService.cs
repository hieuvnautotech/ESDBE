using ESD.Models.Dtos.Common;
using ESD.Models.Dtos;
using static ESD.Extensions.ServiceExtensions;
using ESD.Models.Dtos.FQC;
using Dapper;
using ESD.DbAccess;
using ESD.Extensions;
using System.Data;

namespace ESD.Services.WMS.FG
{
    public interface IFGStockService
    {
        Task<ResponseModel<IEnumerable<dynamic>?>> GetStock(SemiMMSDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetLotDetail(WOSemiLotFQCDto model);
    }
    [ScopedRegistration]
    public class FGStockService : IFGStockService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public FGStockService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetStock(SemiMMSDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_FGStock_Get";
                var param = new DynamicParameters();
                param.Add("@WorkOrder", model.WorkOrder);
                param.Add("@ProductId", model.ProductId);
                param.Add("@ModelId", model.ModelId);
                param.Add("@BuyerQR", model.BuyerQR);
                param.Add("@createdDate", model.ReceivedDate);
                param.Add("@ProductType", model.ProductType);
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
                string proc = "Usp_FGStock_GetDetail";
                var param = new DynamicParameters();
                param.Add("@ProductId", model.ProductId);
                param.Add("@BuyerQR", model.BuyerQR);
                param.Add("@WorkOrder", model.WorkOrder);
                param.Add("@createdDate", model.ReceivedDate);
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
    }
}
