using ESD.Models.Dtos.Common;
using ESD.Models.Dtos;
using ESD.DbAccess;
using Dapper;
using ESD.Extensions;
using System.Data;
using static ESD.Extensions.ServiceExtensions;

namespace ESD.Services.FQC
{
    public interface IFQCReceivingService
    {
        Task<ResponseModel<IEnumerable<dynamic>?>> GetAll(PageModel pageInfo, string WONo, string ProductCode, string SemiLotCode,string Shift, DateTime? StartDate, DateTime? EndDate);
        Task<ResponseModel<SemiMMSDto?>> ScanLot(SemiMMSDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetAllHistory(PageModel pageInfo, string SemiLotCode, string ProductCode, string Shift, DateTime? StartDate, DateTime? EndDate);
    }
    [ScopedRegistration]
    public class FQCReceivingService : IFQCReceivingService
    {
        private readonly ISqlDataAccess _sqlDataAccess;
        public FQCReceivingService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetAll(PageModel pageInfo, string WONo, string ProductCode, string SemiLotCode,string Shift, DateTime? StartDate, DateTime? EndDate)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_FQCReceiving_GetAll"; var param = new DynamicParameters();
                param.Add("@WONo", WONo);
                param.Add("@ProductCode", ProductCode);
                param.Add("@SemiLotCode", SemiLotCode);
                param.Add("@StartDate", StartDate?.ToString("yyyy-MM-dd"));
                param.Add("@EndDate", EndDate?.ToString("yyyy-MM-dd"));
                param.Add("@Shift", Shift);
                param.Add("@page", pageInfo.page);
                param.Add("@pageSize", pageInfo.pageSize);
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
        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetAllHistory(PageModel pageInfo, string SemiLotCode, string ProductCode, string Shift, DateTime? StartDate, DateTime? EndDate)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_FQCReceivingHistory_GetAll"; var param = new DynamicParameters();
                param.Add("@SemiLotCode", SemiLotCode);
                param.Add("@ProductCode", ProductCode);
                param.Add("@StartDate", StartDate);
                param.Add("@EndDate", EndDate);
                param.Add("@Shift", Shift);
                param.Add("@page", pageInfo.page);
                param.Add("@pageSize", pageInfo.pageSize);
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
            catch (Exception e)
            {
                throw;
            }
        }
        public async Task<ResponseModel<SemiMMSDto?>> GetWOSemiLotByCode(string? SemiLotCode)
        {
            var returnData = new ResponseModel<SemiMMSDto?>();

            if (SemiLotCode == null || SemiLotCode=="")
            {
                returnData.ResponseMessage = StaticReturnValue.FIELD_REQUIRED;
                returnData.HttpResponseCode = 400;
                return returnData;
            }

            var proc = $"Usp_FQCReceiving_GetBySemiCode";
            var param = new DynamicParameters();
            param.Add("@SemiLotCode", SemiLotCode);
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<SemiMMSDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            returnData.ResponseMessage = StaticReturnValue.SUCCESS;
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
            }
            return returnData;
        }
        public async Task<ResponseModel<SemiMMSDto?>> ScanLot(SemiMMSDto model)
        {
            var returnData = new ResponseModel<SemiMMSDto?>();

            string proc = "Usp_FQCReceiving_Scan";
            var param = new DynamicParameters();
            param.Add("@SemiLotCode", model.SemiLotCode);
            //param.Add("@FactoryName", model.FactoryName);
            //param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);

            var result = await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);

            returnData.ResponseMessage = result;
            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.SUCCESS:
                    returnData.HttpResponseCode = 200;
                    returnData = await GetWOSemiLotByCode(model.SemiLotCode);
                    break;
                default:
                    returnData.ResponseMessage = result;
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }
    }
}
