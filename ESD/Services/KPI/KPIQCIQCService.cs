using ESD.Models.Dtos.Common;
using ESD.Models.Dtos;
using static ESD.Extensions.ServiceExtensions;
using ESD.DbAccess;
using Dapper;
using ESD.Extensions;

namespace ESD.Services.KPI
{
    public interface IKPIQCIQCService
    {
        Task<ResponseModel<IEnumerable<dynamic>?>> GetForChart(string? MaterialCode, string LotNo);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetDataGrid(MaterialReceivingDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetForChartSlitCut(string? MaterialCode, string LotNo, long ProductId);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetDataGridSlitCut(string? MaterialCode, string LotNo, long ProductId);
    }
    [ScopedRegistration]
    public class KPIQCIQCService : IKPIQCIQCService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public KPIQCIQCService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetForChart(string? MaterialCode, string LotNo)
        {
            var returnData = new ResponseModel<IEnumerable<dynamic>?>();
            var proc = $"Usp_KPIQCIQCRaw_Get6Days";
            var param = new DynamicParameters();
            param.Add("@MaterialCode", MaterialCode);
            param.Add("@LotNo", LotNo);
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);

            returnData.Data = data;
            if (!data.Any())
            {
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                returnData.HttpResponseCode = 204;
            }

            return returnData;
        }

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetDataGrid(MaterialReceivingDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_KPIQCIQCRaw_GetCurrentDay";
                var param = new DynamicParameters();
                param.Add("@LotNo", model.LotNo);
                param.Add("@MaterialCode", model.MaterialCode);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);
                returnData.Data = data;
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
        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetForChartSlitCut(string? MaterialCode, string LotNo, long ProductId)
        {
            var returnData = new ResponseModel<IEnumerable<dynamic>?>();
            var proc = $"Usp_KPIQCIQCSlitCut_Get6Days";
            var param = new DynamicParameters();
            param.Add("@MaterialCode", MaterialCode);
            param.Add("@LotNo", LotNo);
            param.Add("@ProductId", ProductId);
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);

            returnData.Data = data;
            if (!data.Any())
            {
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                returnData.HttpResponseCode = 204;
            }

            return returnData;
        }

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetDataGridSlitCut(string? MaterialCode, string LotNo, long ProductId)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_KPIQCIQCSlitCut_GetCurrentDay";
                var param = new DynamicParameters();
                param.Add("@MaterialCode", MaterialCode);
                param.Add("@LotNo", LotNo);
                param.Add("@ProductId", ProductId);
                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);
                returnData.Data = data;
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
