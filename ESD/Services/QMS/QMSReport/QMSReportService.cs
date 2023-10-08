using ESD.Models.Dtos.Common;
using ESD.Models.Dtos;
using static ESD.Extensions.ServiceExtensions;
using ESD.DbAccess;
using Dapper;
using ESD.Extensions;

namespace ESD.Services.QMS.QMSReport
{
    public interface IQMSReportService
    {
        Task<ResponseModel<IEnumerable<dynamic>?>> GetIQCRawGeneral(MaterialReceivingDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetIQCRawDetail(MaterialReceivingDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetIQCSlitCutDetail(MaterialReceivingDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetChartIQCRaw(long? MaterialId, DateTime? StartDate, DateTime? EndDate, string Type);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetIQCSlitCutGeneral(MaterialReceivingDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetChartIQCSlitCut(long? MaterialId, DateTime? StartDate, DateTime? EndDate, string Type);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetIQCSlitCutDetailExcel(MaterialReceivingDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetIQCRawDetailExcel(MaterialReceivingDto model);
    }
    [ScopedRegistration]
    public class QMSReportService : IQMSReportService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public QMSReportService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetIQCRawGeneral(MaterialReceivingDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_QMSReport_IQCRawGeneral";
                var param = new DynamicParameters();
                param.Add("@MaterialId", model.MaterialId);
                param.Add("@StartDate", model.StartDate);
                param.Add("@EndDate", model.EndDate);
                param.Add("@Type", model.Type);

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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetIQCRawDetail(MaterialReceivingDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_QMSReport_IQCRawDetail";
                var param = new DynamicParameters();
                param.Add("@MaterialId", model.MaterialId);
                param.Add("@StartDate", model.StartDate);
                param.Add("@EndDate", model.EndDate);
                param.Add("@Type", model.Type);

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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetIQCSlitCutDetail(MaterialReceivingDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_QMSReport_IQCSlitCutDetail";
                var param = new DynamicParameters();
                param.Add("@MaterialId", model.MaterialId);
                param.Add("@StartDate", model.StartDate);
                param.Add("@EndDate", model.EndDate);
                param.Add("@Type", model.Type);

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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetIQCRawDetailExcel(MaterialReceivingDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_QMSReport_IQCRawDetailExcel";
                var param = new DynamicParameters();
                param.Add("@MaterialId", model.MaterialId);
                param.Add("@StartDate", model.StartDate);
                param.Add("@EndDate", model.EndDate);
                param.Add("@Type", model.Type);

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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetIQCSlitCutDetailExcel(MaterialReceivingDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_QMSReport_IQCSlitCutDetailExcel";
                var param = new DynamicParameters();
                param.Add("@MaterialId", model.MaterialId);
                param.Add("@StartDate", model.StartDate);
                param.Add("@EndDate", model.EndDate);
                param.Add("@Type", model.Type);

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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetIQCSlitCutGeneral(MaterialReceivingDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_QMSReport_IQCSlitCutGeneral";
                var param = new DynamicParameters();
                param.Add("@MaterialId", model.MaterialId);
                param.Add("@StartDate", model.StartDate);
                param.Add("@EndDate", model.EndDate);
                param.Add("@Type", model.Type);

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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetChartIQCRaw(long? MaterialId, DateTime? StartDate, DateTime? EndDate, string Type)
        {
            var returnData = new ResponseModel<IEnumerable<dynamic>?>();
            var proc = $"Usp_QMSReport_IQCRawGeneralGetChart";
            var param = new DynamicParameters();
            param.Add("@MaterialId", MaterialId);
            param.Add("@StartDate", StartDate);
            param.Add("@EndDate", EndDate);
            param.Add("@Type", Type);
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);

            returnData.Data = data;
            if (!data.Any())
            {
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                returnData.HttpResponseCode = 204;
            }

            return returnData;
        }

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetChartIQCSlitCut(long? MaterialId, DateTime? StartDate, DateTime? EndDate, string Type)
        {
            var returnData = new ResponseModel<IEnumerable<dynamic>?>();
            var proc = $"Usp_QMSReport_IQCSlitCutGeneralGetChart";
            var param = new DynamicParameters();
            param.Add("@MaterialId", MaterialId);
            param.Add("@StartDate", StartDate);
            param.Add("@EndDate", EndDate);
            param.Add("@Type", Type);
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);

            returnData.Data = data;
            if (!data.Any())
            {
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                returnData.HttpResponseCode = 204;
            }

            return returnData;
        }
    }
}
