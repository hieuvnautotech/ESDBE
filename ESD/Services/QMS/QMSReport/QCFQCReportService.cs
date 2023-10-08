using ESD.Models.Dtos.Common;
using ESD.Models.Dtos;
using static ESD.Extensions.ServiceExtensions;
using ESD.DbAccess;
using Dapper;
using ESD.Extensions;

namespace ESD.Services.QMS.QMSReport
{
    public interface IQCFQCReportService
    {
        Task<ResponseModel<IEnumerable<dynamic>?>> GetFQCGeneral(QCReportDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetFQCGeneralChart(QCReportDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetFQCDetail(QCReportDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetFQCDetailChart(QCReportDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetFQCDetailExcel(QCReportDto model);
    }
    [ScopedRegistration]
    public class QCFQCReportService : IQCFQCReportService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public QCFQCReportService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetFQCGeneral(QCReportDto model)
        {
            try
            {
                List<long> Products = new List<long>();

                if (!string.IsNullOrEmpty(model.Products))
                    Products = model.Products.Split('|').Select(long.Parse).ToList();

                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_QMSReport_FQCGeneral";
                var param = new DynamicParameters();
                param.Add("@ModelId", model.ModelId);
                param.Add("@ProductIds", Helpers.ParameterTvp.GetTableValuedParameter_BigInt(Products));
                //param.Add("@ProductId", model.ProductId);
                param.Add("@StartDate", model.StartDate);
                param.Add("@EndDate", model.EndDate);

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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetFQCGeneralChart(QCReportDto model)
        {
            try
            {
                List<long> Products = new List<long>();

                if (!string.IsNullOrEmpty(model.Products))
                    Products = model.Products.Split('|').Select(long.Parse).ToList();

                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_QMSReport_FQCGeneralChart";
                var param = new DynamicParameters();
                param.Add("@ModelId", model.ModelId);
                param.Add("@ProductIds", Helpers.ParameterTvp.GetTableValuedParameter_BigInt(Products));
                //param.Add("@ProductId", model.ProductId);
                param.Add("@StartDate", model.StartDate);
                param.Add("@EndDate", model.EndDate);

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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetFQCDetail(QCReportDto model)
        {
            try
            {
                List<long> Products = new List<long>();

                if (!string.IsNullOrEmpty(model.Products))
                    Products = model.Products.Split('|').Select(long.Parse).ToList();

                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_QMSReport_FQCDetail";
                var param = new DynamicParameters();
                param.Add("@ModelId", model.ModelId);
                param.Add("@ProductIds", Helpers.ParameterTvp.GetTableValuedParameter_BigInt(Products));
                //param.Add("@ProductId", model.ProductId);
                param.Add("@StartDate", model.StartDate);
                param.Add("@EndDate", model.EndDate);

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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetFQCDetailExcel(QCReportDto model)
        {
            try
            {
                List<long> Products = new List<long>();

                if (!string.IsNullOrEmpty(model.Products))
                    Products = model.Products.Split('|').Select(long.Parse).ToList();

                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_QMSReport_FQCDetailExcel";
                var param = new DynamicParameters();
                param.Add("@ProjectId", model.ProjectId);
                param.Add("@ModelId", model.ModelId);
                param.Add("@ProductIds", Helpers.ParameterTvp.GetTableValuedParameter_BigInt(Products));
                param.Add("@StartDate", model.StartDate);
                param.Add("@EndDate", model.EndDate);

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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetFQCDetailChart(QCReportDto model)
        {
            try
            {
                List<long> Products = new List<long>();

                if (!string.IsNullOrEmpty(model.Products))
                    Products = model.Products.Split('|').Select(long.Parse).ToList();

                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_QMSReport_FQCDetailChart";
                var param = new DynamicParameters();
                param.Add("@ProjectId", model.ProjectId);
                param.Add("@ModelId", model.ModelId);
                param.Add("@ProductIds", Helpers.ParameterTvp.GetTableValuedParameter_BigInt(Products));
                //param.Add("@ProductId", model.ProductId);
                param.Add("@StartDate", model.StartDate);
                param.Add("@EndDate", model.EndDate);
                param.Add("@LotorQty", model.LotorQty);

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
