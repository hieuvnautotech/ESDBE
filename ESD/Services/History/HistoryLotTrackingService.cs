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
    public interface IHistoryLotTrackingService
    {
        Task<ResponseModel<IEnumerable<HistoryLotTrackingDto>?>> Get(HistoryLotTrackingDto model);
        Task<ResponseModel<IEnumerable<HistoryLotTrackingDetailDto>?>> GetDetail(HistoryLotTrackingDetailDto model);
        Task<ResponseModel<IEnumerable<HistoryLotTrackingSlitDto>?>> GetDetailSlit(HistoryLotTrackingSlitDto model);
    }
    [ScopedRegistration]
    public class HistoryLotTrackingService : IHistoryLotTrackingService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public HistoryLotTrackingService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<ResponseModel<IEnumerable<HistoryLotTrackingDto>?>> Get(HistoryLotTrackingDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<HistoryLotTrackingDto>?>();
                string proc = "Usp_HistoryLotTracking_BySemiLot";
                var param = new DynamicParameters();
                param.Add("@BuyerQR", model.BuyerQR);
                param.Add("@SemiLotCode", model.SemiLotCode);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<HistoryLotTrackingDto>(proc, param);
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
        public async Task<ResponseModel<IEnumerable<HistoryLotTrackingDetailDto>?>> GetDetail(HistoryLotTrackingDetailDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<HistoryLotTrackingDetailDto>?>();
                string proc = "Usp_HistoryLotTracking_BySemiLotDetail";
                var param = new DynamicParameters();
                param.Add("@SemiLotCode", model.SemiLotCode);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<HistoryLotTrackingDetailDto>(proc, param);
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
        public async Task<ResponseModel<IEnumerable<HistoryLotTrackingSlitDto>?>> GetDetailSlit(HistoryLotTrackingSlitDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<HistoryLotTrackingSlitDto>?>();
                string proc = "Usp_HistoryLotTracking_Slit";
                var param = new DynamicParameters();
                param.Add("@MaterialLotCode", model.MaterialLotCode);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<HistoryLotTrackingSlitDto>(proc, param);
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
