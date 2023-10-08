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
using ESD.Models.Dtos.FQC;

namespace ESD.Services.QMS.Holding
{
    public interface IHoldLogService
    {
        Task<ResponseModel<IEnumerable<HoldLogRawMaterialDto>?>> GetLogRawMaterial(HoldLogRawMaterialDto model);
        Task<ResponseModel<IEnumerable<HoldDto>?>> GetLogMaterial(HoldDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetFQCLog(WOSemiLotFQCDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetMMSLog(SemiMMSDto model);
        Task<ResponseModel<IEnumerable<HoldLogFGDto>?>> GetFGLog(HoldLogFGDto model);

    }
    [ScopedRegistration]
    public class HoldLogService : IHoldLogService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public HoldLogService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<ResponseModel<IEnumerable<HoldLogRawMaterialDto>?>> GetLogRawMaterial(HoldLogRawMaterialDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<HoldLogRawMaterialDto>?>();
                string proc = "Usp_HoldLogRawMaterial_GetAll"; 
                var param = new DynamicParameters();
                param.Add("@MaterialLotCode", model.MaterialLotCode);
                param.Add("@LotNo", model.LotNo);
                param.Add("@HoldStatus", model.HoldStatus);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<HoldLogRawMaterialDto>(proc, param);
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
        public async Task<ResponseModel<IEnumerable<HoldDto>?>> GetLogMaterial(HoldDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<HoldDto>?>();
                string proc = "Usp_HoldLogMaterial_GetAll"; var param = new DynamicParameters();
                param.Add("@MaterialLotCode", model.MaterialLotCode);
                param.Add("@LotNo", model.LotNo);
                param.Add("@HoldStatus", model.HoldStatus);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<HoldDto>(proc, param);
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
        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetFQCLog(WOSemiLotFQCDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_HoldLogSemiLotFQC_GetAll";
                var param = new DynamicParameters();
                param.Add("@SemiLotCode", model.SemiLotCode);
                param.Add("@WorkOrder", model.WOCode);
                param.Add("@HoldStatus", model.LotStatus);
                param.Add("@FQCSOName", model.LotStatusName);
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
        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetMMSLog(SemiMMSDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_HoldLogSemiLotMMS_GetAll";
                var param = new DynamicParameters();
                param.Add("@SemiLotCode", model.SemiLotCode);
                param.Add("@WorkOrder", model.WOCode);
                param.Add("@HoldStatus", model.LotStatus);
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
        public async Task<ResponseModel<IEnumerable<HoldLogFGDto>?>> GetFGLog(HoldLogFGDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<HoldLogFGDto>?>();
                string proc = "Usp_HoldLogFinishGood_GetAll";
                var param = new DynamicParameters();
                param.Add("@BuyerQR", model.BuyerQR);
                param.Add("@LotNo", model.LotNo);
                param.Add("@FQCSOName", model.FQCSOName);
                param.Add("@HoldStatus", model.HoldStatus);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<HoldLogFGDto>(proc, param);
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
