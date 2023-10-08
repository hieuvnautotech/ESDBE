using Dapper;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using static ESD.Extensions.ServiceExtensions;

namespace ESD.Services.QMS.QMSReport
{
    public interface IQCReportService
    {
        Task<ResponseModel<IEnumerable<dynamic>?>> GetPQCGeneral(QCReportDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetPQCGeneralChart(QCReportDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetPQCDetail(QCReportDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetPQCDetailExcel(QCReportDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetPQCGeneralView(QCReportDto model);

        Task<ResponseModel<IEnumerable<dynamic>?>> GetOQCGeneral(QCReportDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetOQCGeneralChart(QCReportDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetOQCDetail(QCReportDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetOQCDetailExcel(QCReportDto model);

        Task<ResponseModel<IEnumerable<dynamic>?>> GetMaterialGeneral(QCReportDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetMaterialGeneralChart(QCReportDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetMaterialDetail(QCReportDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetMaterialDetailExcel(QCReportDto model);
    }
    [ScopedRegistration]
    public class QCReportService : IQCReportService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public QCReportService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetPQCGeneral(QCReportDto model)
        {
            try
            {
                List<long> Products = new List<long>();

                if (!string.IsNullOrEmpty(model.Products))
                    Products = model.Products.Split('|').Select(long.Parse).ToList();

                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_QMSReport_PQCGeneral";
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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetPQCGeneralView(QCReportDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_QMSReport_PQCGeneralViewWO";
                var param = new DynamicParameters();
                param.Add("@ProductCode", model.Products);
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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetPQCGeneralChart(QCReportDto model)
        {
            try
            {
                List<long> Products = new List<long>();

                if (!string.IsNullOrEmpty(model.Products))
                    Products = model.Products.Split('|').Select(long.Parse).ToList();

                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_QMSReport_PQCGeneralChart";
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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetPQCDetail(QCReportDto model)
        {
            try
            {
                List<long> Products = new List<long>();

                if (!string.IsNullOrEmpty(model.Products))
                    Products = model.Products.Split('|').Select(long.Parse).ToList();

                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_QMSReport_PQCDetail";
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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetPQCDetailExcel(QCReportDto model)
        {
            try
            {
                List<long> Products = new List<long>();

                if (!string.IsNullOrEmpty(model.Products))
                    Products = model.Products.Split('|').Select(long.Parse).ToList();

                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_QMSReport_PQCDetailExcel";
                var param = new DynamicParameters();
                param.Add("@ProjectId", model.ProjectId);
                param.Add("@ModelId", model.ModelId);
                param.Add("@ProductIds", Helpers.ParameterTvp.GetTableValuedParameter_BigInt(Products));
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


        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetOQCGeneral(QCReportDto model)
        {
            try
            {
                List<long> Products = new List<long>();

                if (!string.IsNullOrEmpty(model.Products))
                    Products = model.Products.Split('|').Select(long.Parse).ToList();

                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_QMSReport_OQCGeneral";
                var param = new DynamicParameters();
                param.Add("@ProjectId", model.ProjectId);
                param.Add("@ModelId", model.ModelId);
                param.Add("@ProductIds", Helpers.ParameterTvp.GetTableValuedParameter_BigInt(Products));
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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetOQCGeneralChart(QCReportDto model)
        {
            try
            {
                List<long> Products = new List<long>();

                if (!string.IsNullOrEmpty(model.Products))
                    Products = model.Products.Split('|').Select(long.Parse).ToList();

                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_QMSReport_OQCGeneralChart";
                var param = new DynamicParameters();
                param.Add("@ProjectId", model.ProjectId);
                param.Add("@ModelId", model.ModelId);
                param.Add("@ProductIds", Helpers.ParameterTvp.GetTableValuedParameter_BigInt(Products));
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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetOQCDetail(QCReportDto model)
        {
            try
            {
                List<long> Products = new List<long>();

                if (!string.IsNullOrEmpty(model.Products))
                    Products = model.Products.Split('|').Select(long.Parse).ToList();

                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_QMSReport_OQCDetail";
                var param = new DynamicParameters();
                param.Add("@ProjectId", model.ProjectId);
                param.Add("@ModelId", model.ModelId);
                param.Add("@ProductIds", Helpers.ParameterTvp.GetTableValuedParameter_BigInt(Products));
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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetOQCDetailExcel(QCReportDto model)
        {
            try
            {
                List<long> Products = new List<long>();

                if (!string.IsNullOrEmpty(model.Products))
                    Products = model.Products.Split('|').Select(long.Parse).ToList();

                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_QMSReport_OQCDetailExcel";
                var param = new DynamicParameters();
                param.Add("@ProjectId", model.ProjectId);
                param.Add("@ModelId", model.ModelId);
                param.Add("@ProductIds", Helpers.ParameterTvp.GetTableValuedParameter_BigInt(Products));
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


        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetMaterialGeneral(QCReportDto model)
        {
            try
            {

                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_QMSReport_MaterialGeneral";
                var param = new DynamicParameters();
                param.Add("@MaterialId", model.MaterialId);
                param.Add("@StartDate", model.StartDate);
                param.Add("@EndDate", model.EndDate);
                param.Add("@Type", model.Type);
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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetMaterialGeneralChart(QCReportDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_QMSReport_MaterialGeneralChart";
                var param = new DynamicParameters();
                param.Add("@MaterialId", model.MaterialId);
                param.Add("@StartDate", model.StartDate);
                param.Add("@EndDate", model.EndDate);
                param.Add("@Type", model.Type);
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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetMaterialDetail(QCReportDto model)
        {
            try
            {
                List<long> Products = new List<long>();

                if (!string.IsNullOrEmpty(model.Products))
                    Products = model.Products.Split('|').Select(long.Parse).ToList();

                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_QMSReport_MaterialDetail";
                var param = new DynamicParameters();
                param.Add("@MaterialId", model.MaterialId);
                param.Add("@StartDate", model.StartDate);
                param.Add("@EndDate", model.EndDate);
                param.Add("@Type", model.Type);
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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetMaterialDetailExcel(QCReportDto model)
        {
            try
            {
                List<long> Products = new List<long>();

                if (!string.IsNullOrEmpty(model.Products))
                    Products = model.Products.Split('|').Select(long.Parse).ToList();

                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_QMSReport_MaterialDetailExcel";
                var param = new DynamicParameters();
                param.Add("@MaterialId", model.MaterialId);
                param.Add("@StartDate", model.StartDate);
                param.Add("@EndDate", model.EndDate);
                param.Add("@Type", model.Type);
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
