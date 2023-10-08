using ESD.Models.Dtos.Common;
using ESD.Models.Dtos;
using ESD.DbAccess;
using ESD.Services.WMS.Material;
using static ESD.Extensions.ServiceExtensions;
using ESD.Extensions;
using System.Data;
using ESD.Helpers;
using Newtonsoft.Json;
using ESD.Models.Dtos.MMS;
using Dapper;

namespace ESD.Services.MMS
{
    public interface IAnalyticsService
    {
        Task<ResponseModel<WODisplayDto>> GetDisplay();
        Task<ResponseModel<dynamic>> GetWOForDisplay(DateTime StartDate, DateTime EndDate);

        Task<ResponseModel<ProcessQty>> GetWOProcessMMS(EffectiveDto model);
        Task<ResponseModel<ProcessQty>> GetWOProcessFQC(EffectiveDto model);
    }

    [SingletonRegistration]
    public class AnalyticsService : IAnalyticsService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public AnalyticsService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<ResponseModel<WODisplayDto>> GetDisplay()
        {
            var returnData = new ResponseModel<WODisplayDto>();
            var proc = $"Usp_Display_CurrentDate";
            var param = new DynamicParameters();
            param.Add("@totalWO", 0, DbType.Int32, ParameterDirection.Output);
            param.Add("@totalTarget", 0, DbType.Int32, ParameterDirection.Output);
            param.Add("@totalOK", 0, DbType.Int32, ParameterDirection.Output);
            param.Add("@totalNG", 0, DbType.Int32, ParameterDirection.Output);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WODto>(proc, param);

            if (!data.Any())
            {
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                returnData.HttpResponseCode = 204;
            }
            else
            {
                returnData.Data = new WODisplayDto();
                returnData.Data.data = data.ToList();
                returnData.Data.totalWO = param.Get<int>("totalWO");
                returnData.Data.totalTarget = param.Get<int>("totalTarget");
                returnData.Data.totalOK = param.Get<int>("totalOK");
                returnData.Data.totalNG = param.Get<int>("totalNG");
            }

            return returnData;
        }

        public async Task<ResponseModel<dynamic>> GetWOForDisplay(DateTime StartDate, DateTime EndDate)
        {
            var returnData = new ResponseModel<dynamic>();
            var proc = $"Usp_Display_GetWO";
            var param = new DynamicParameters();
            param.Add("@StartDate", StartDate);
            param.Add("@EndDate", EndDate);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);

            if (!data.Any())
            {
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                returnData.HttpResponseCode = 204;
            }
            else
            {
                returnData.Data = data;
            }

            return returnData;
        }

        public async Task<ResponseModel<ProcessQty>> GetWOProcessMMS(EffectiveDto model)
        {
            if (model.ProcessCode == null)
                model.ProcessCode = model.MMSCode;

            var returnData = new ResponseModel<ProcessQty>();
            var proc = $"Usp_Display_GetProcessMMS";
            var param = new DynamicParameters();
            param.Add("@ProductCode", model.ProductCode);
            param.Add("@ProcessCode", model.ProcessCode == "FINISH GOOD" ? "1" : model.ProcessCode);
            param.Add("@StartDate", model.StartDate);
            param.Add("@EndDate", model.EndDate);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<ProcessQty>(proc, param);

            if (!data.Any())
            {
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                returnData.HttpResponseCode = 204;
            }
            else
            {
                returnData.Data = data.FirstOrDefault();
            }

            return returnData;
        }

        public async Task<ResponseModel<ProcessQty>> GetWOProcessFQC(EffectiveDto model)
        {
            if (model.ProcessCode == null)
                model.ProcessCode = model.FQCCode;

            var returnData = new ResponseModel<ProcessQty>();
            var proc = $"Usp_Display_GetProcessFQC";
            var param = new DynamicParameters();
            param.Add("@ProductCode", model.ProductCode);
            param.Add("@ProcessCode", model.ProcessCode == "FINISH GOOD" ? "1" : model.ProcessCode);
            param.Add("@StartDate", model.StartDate);
            param.Add("@EndDate", model.EndDate);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<ProcessQty>(proc, param);

            if (!data.Any())
            {
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                returnData.HttpResponseCode = 204;
            }
            else
            {
                returnData.Data = data.FirstOrDefault();
            }

            return returnData;
        }
    }
}
