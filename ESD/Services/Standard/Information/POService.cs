using ESD.Models.Dtos.Common;
using ESD.Models.Dtos;
using ESD.Models.Dtos.PO;
using static ESD.Extensions.ServiceExtensions;
using ESD.DbAccess;
using Dapper;
using ESD.Extensions;
using System.Data;

namespace ESD.Services.Standard.Information
{
    public interface IPOService
    {
        Task<ResponseModel<IEnumerable<PODto>?>> GetAll(PageModel model, string? POOrderCode, DateTime? searchStartDay, DateTime? searchEndDay);
    }
    [ScopedRegistration]
    public class POService : IPOService
    {
        private readonly ISqlDataAccess _sqlDataAccess;
        public POService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public async Task<ResponseModel<IEnumerable<PODto>?>> GetAll(PageModel model,string? POOrderCode, DateTime? searchStartDay, DateTime? searchEndDay)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<PODto>?>();
                string proc = "Usp_PO_GetAll"; var param = new DynamicParameters();
                param.Add("@POOrderCode", POOrderCode);
                param.Add("@StartDate", searchStartDay);
                param.Add("@EndDate", searchEndDay);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<PODto>(proc, param);
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
