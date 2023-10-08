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

namespace ESD.Services.WMS.Material
{
    public interface IHistoryReplacementService
    {
        Task<ResponseModel<IEnumerable<HistoryReplacementDto>?>> Get(HistoryReplacementDto model);
        Task<ResponseModel<IEnumerable<HistoryReplacementDetailDto>?>> GetDetail(HistoryReplacementDetailDto model);
    }
    [ScopedRegistration]
    public class HistoryReplacementService : IHistoryReplacementService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public HistoryReplacementService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<ResponseModel<IEnumerable<HistoryReplacementDto>?>> Get(HistoryReplacementDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<HistoryReplacementDto>?>();
                string proc = "Usp_HistoryAddHangBu_GetAll";
                var param = new DynamicParameters();
                param.Add("@BuyerQR", model.BuyerQR);
                param.Add("@SemiLotCode", model.SemiLotCode);
                param.Add("@WOCode", model.WOCode);
                param.Add("@StartDate", model.StartDate);
                param.Add("@EndDate", model.EndDate);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<HistoryReplacementDto>(proc, param);
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

        public async Task<ResponseModel<IEnumerable<HistoryReplacementDetailDto>?>> GetDetail(HistoryReplacementDetailDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<HistoryReplacementDetailDto>?>();
                string proc = "Usp_HistoryAddHangBuDetail_GetAll";
                var param = new DynamicParameters();
                param.Add("@SemiLotCode", model.SemiLotCode);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<HistoryReplacementDetailDto>(proc, param);
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
