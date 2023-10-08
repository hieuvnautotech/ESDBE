using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Models;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Services.Base;
using System.Data;
using static ESD.Extensions.ServiceExtensions;
using ESD.Models.Dtos.FQC;

namespace ESD.Services.EDI
{
    public interface IKPIPQCService
    {
        Task<ResponseModel<IEnumerable<dynamic>?>> GetPQC(KPIQCDto pageInfo);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetPQCCurrent(KPIQCDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetFQC(KPIQCDto pageInfo);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetFQCCurrent(KPIQCDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetOQC(KPIQCDto pageInfo);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetOQCCurrent(KPIQCDto model);

    }
    [ScopedRegistration]
    public class KPIPQCService : IKPIPQCService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public KPIPQCService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetPQC(KPIQCDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_KPIQCPQC_Get6Days"; 
                var param = new DynamicParameters();
                param.Add("@ProductCode", model.ProductCode);
                param.Add("@WOCode", model.WOCode);
                param.Add("@ProcessCode", model.ProcessCode);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);
                returnData.Data = data;
                if (!data.Any())
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                }
                return returnData;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetPQCCurrent(KPIQCDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_KPIQCPQC_GetCurrentDate";
                var param = new DynamicParameters();
                param.Add("@ProductCode", model.ProductCode);
                param.Add("@WOCode", model.WOCode);
                param.Add("@ProcessCode", model.ProcessCode);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);
                returnData.Data = data;
                if (!data.Any())
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                }
                return returnData;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetFQC(KPIQCDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_KPIQCFQC_Get6Days";
                var param = new DynamicParameters();
                param.Add("@ProductCode", model.ProductCode);
                param.Add("@WOCode", model.WOCode);
                param.Add("@ProcessCode", model.ProcessCode);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);
                returnData.Data = data;
                if (!data.Any())
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                }
                return returnData;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetFQCCurrent(KPIQCDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_KPIQCFQC_GetCurrentDate";
                var param = new DynamicParameters();
                param.Add("@ProductCode", model.ProductCode);
                param.Add("@WOCode", model.WOCode);
                param.Add("@ProcessCode", model.ProcessCode);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);
                returnData.Data = data;
                if (!data.Any())
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                }
                return returnData;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetOQC(KPIQCDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_KPIQCOQC_Get6Days";
                var param = new DynamicParameters();
                param.Add("@ProductCode", model.ProductCode);
                param.Add("@WOCode", model.WOCode);
                param.Add("@ProcessCode", model.ProcessCode);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);
                returnData.Data = data;
                if (!data.Any())
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                }
                return returnData;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetOQCCurrent(KPIQCDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_KPIQCOQC_GetCurrentDate";
                var param = new DynamicParameters();
                param.Add("@ProductCode", model.ProductCode);
                param.Add("@WOCode", model.WOCode);
                param.Add("@ProcessCode", model.ProcessCode);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);
                returnData.Data = data;
                if (!data.Any())
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                }
                return returnData;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
