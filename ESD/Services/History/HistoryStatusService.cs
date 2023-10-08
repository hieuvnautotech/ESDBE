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
using ESD.Models.Dtos.MMS;
using ESD.Models.Dtos.FQC;

namespace ESD.Services.WMS.Material
{
    public interface IHistoryStatusService
    {
        Task<ResponseModel<IEnumerable<HistoryReplacementDto>?>> Get(string MaterialLotCode);
        Task<MaterialLotDto> checkMaterialLot(string MaterialLotCode);
        Task<WOSemiLotMMSDto> checkSemiLotMMS(string SemiLotCode);
        Task<WOSemiLotFQCDto> checkSemiLotAPP(string SemiLotCode);
        Task<string> GetDetailNameByDetailCode(string detailCode);
        Task<WOProcessDto> GetWOProcess(long? WOProcessId);
        Task<IEnumerable<WOSemiLotMMSDetailDto>> GetMaterialMappingByMaterialCode(string MaterialLotCode);
        Task<IEnumerable<WOSemiLotFQCDetailDto>> GetSemiLotMappingByMaterialCode(string MaterialLotCode);
    }
    [ScopedRegistration]
    public class HistoryStatusService : IHistoryStatusService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public HistoryStatusService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public async Task<ResponseModel<IEnumerable<HistoryReplacementDto>?>> Get(string MaterialLotCode)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<HistoryReplacementDto>?>();
                string proc = "Usp_HistoryStatusByMaterialLotCode_GetAll";
                var param = new DynamicParameters();
                param.Add("@MaterialLotCode", MaterialLotCode);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<HistoryReplacementDto>(proc, param);
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
        public async Task<MaterialLotDto> checkMaterialLot(string MaterialLotCode)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@MaterialLotCode", MaterialLotCode);
                var sql = "SELECT top 1 * FROM MaterialLot  where MaterialLotCode = @MaterialLotCode  and isActived = 1";
                var data = await _sqlDataAccess.LoadDataFirstOrDefaultAsync<MaterialLotDto>(sql, param);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<string> GetDetailNameByDetailCode(string detailCode)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@detailCode", detailCode);
                var query = @"select commonDetailName from sysTbl_CommonDetail where commonMasterCode= 'MLSTATUS' AND commonDetailCode = @detailCode";
                var data = await _sqlDataAccess.LoadDataFirstOrDefaultAsync<string>(query, param);
                return data;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<WOProcessDto> GetWOProcess(long? WOProcessId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@WOProcessId", WOProcessId);
                var query = @"select top 1 wp.ProcessLevel,wp.AreaCode,wp.ProcessCode,w.WOCode,p.ProductCode
                            from WOProcess wp
                            join WO w on wp.WOId = w.WOId 
							join Product p on w.ProductCode = p.ProductCode
                            where WOProcessId = @WOProcessId";
                var data = await _sqlDataAccess.LoadDataFirstOrDefaultAsync<WOProcessDto>(query, param);
                return data;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<WOSemiLotMMSDetailDto>> GetMaterialMappingByMaterialCode(string MaterialLotCode)
        {
            try
            {
                var returnData = new WOSemiLotMMSDetailDto();
                string proc = "SELECT * FROM WOSemiLotMMSDetail  where MaterialLotCode = @MaterialLotCode";
                var param = new DynamicParameters();
                param.Add("@MaterialLotCode", MaterialLotCode);

                var data = await _sqlDataAccess.LoadDataUsingRawQuery<WOSemiLotMMSDetailDto>(proc, param);
                return data;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<WOSemiLotMMSDto> checkSemiLotMMS(string SemiLotCode)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@SemiLotCode", SemiLotCode);
                var sql = "SELECT top 1 * FROM WOSemiLotMMS  where SemiLotCode = @SemiLotCode  and isActived = 1";
                var data = await _sqlDataAccess.LoadDataFirstOrDefaultAsync<WOSemiLotMMSDto>(sql, param);
                return data;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<WOSemiLotFQCDto> checkSemiLotAPP(string SemiLotCode)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@SemiLotCode", SemiLotCode);
                var sql = "SELECT top 1 * FROM WOSemiLotFQC  where SemiLotCode = @SemiLotCode ";
                var data = await _sqlDataAccess.LoadDataFirstOrDefaultAsync<WOSemiLotFQCDto>(sql, param);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<WOSemiLotFQCDetailDto>> GetSemiLotMappingByMaterialCode(string MaterialLotCode)
        {
            try
            {
                var returnData = new WOSemiLotFQCDetailDto();
                string proc = "SELECT * FROM WOSemiLotFQCDetail  where MaterialLotCode = @MaterialLotCode";
                var param = new DynamicParameters();
                param.Add("@MaterialLotCode", MaterialLotCode);

                var data = await _sqlDataAccess.LoadDataUsingRawQuery<WOSemiLotFQCDetailDto>(proc, param);
                return data;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
    }
