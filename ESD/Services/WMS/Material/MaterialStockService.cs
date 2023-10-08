using ESD.Models.Dtos.Common;
using ESD.Models.Dtos;
using static ESD.Extensions.ServiceExtensions;
using ESD.DbAccess;
using Dapper;
using ESD.Extensions;
using System.Data;

namespace ESD.Services.WMS.Material
{
    public interface IMaterialStockService
    {
        Task<ResponseModel<IEnumerable<MaterialDto>?>> GetAll(MaterialLotDto model);
        Task<ResponseModel<IEnumerable<MaterialLotDto>?>> GetDetail(MaterialLotDto model);
        Task<ResponseModel<IEnumerable<MaterialDto>?>> GetSlit(MaterialLotDto model);
        Task<ResponseModel<IEnumerable<MaterialLotDto>?>> GetSlitDetail(MaterialLotDto model);
        Task<ResponseModel<IEnumerable<MaterialDto>?>> GetAllTabNG(MaterialLotDto model);
        Task<ResponseModel<IEnumerable<MaterialLotDto>?>> GetDetailTabNG(MaterialLotDto model);
    }
    [ScopedRegistration]
    public class MaterialStockService : IMaterialStockService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public MaterialStockService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public async Task<ResponseModel<IEnumerable<MaterialDto>?>> GetAll(MaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialDto>?>();
                string proc = "Usp_MaterialStock_Get";
                var param = new DynamicParameters();
                param.Add("@MaterialCode", model.MaterialCode);
                param.Add("@MaterialLotCode", model.MaterialLotCode);
                param.Add("@LotNo", model.LotNo);
                param.Add("@ReceivedDate", model.ReceivedDate?.ToString("yyyy-MM-dd"));
                param.Add("@Status", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MaterialDto>(proc, param);
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
        public async Task<ResponseModel<IEnumerable<MaterialDto>?>> GetAllTabNG(MaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialDto>?>();
                string proc = "Usp_MaterialStock_GetNG";
                var param = new DynamicParameters();
                param.Add("@MaterialCode", model.MaterialCode);
                param.Add("@MaterialLotCode", model.MaterialLotCode);
                param.Add("@ReceivedDate", model.ReceivedDate?.ToString("yyyy-MM-dd"));
                param.Add("@Status", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MaterialDto>(proc, param);
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
        public async Task<ResponseModel<IEnumerable<MaterialDto>?>> GetSlit(MaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialDto>?>();
                string proc = "Usp_SlitStock_Get";
                var param = new DynamicParameters();
                param.Add("@MaterialCode", model.MaterialCode);
                param.Add("@MaterialLotCode", model.MaterialLotCode);
                param.Add("@LotNo", model.LotNo);
                param.Add("@ReceivedDate", model.ReceivedDate?.ToString("yyyy-MM-dd"));
                param.Add("@Status", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MaterialDto>(proc, param);
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
        public async Task<ResponseModel<IEnumerable<MaterialLotDto>?>> GetSlitDetail(MaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialLotDto>?>();
                string proc = "Usp_SlitStock_GetLotDetail";
                var param = new DynamicParameters();
                param.Add("@MaterialId", model.MaterialId);
                param.Add("@MaterialLotCode", model.MaterialLotCode);
                param.Add("@LotNo", model.LotNo);
                param.Add("@ReceivedDate", model.ReceivedDate?.ToString("yyyy-MM-dd"));
                param.Add("@Status", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MaterialLotDto>(proc, param);
                if (!data.Any())
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                }
                else
                {
                    returnData.Data = data;
                    returnData.TotalRow = param.Get<int>("totalRow");
                }

                return returnData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ResponseModel<IEnumerable<MaterialLotDto>?>> GetDetail(MaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialLotDto>?>();
                string proc = "Usp_MaterialStock_GetLotDetail";
                var param = new DynamicParameters();
                param.Add("@MaterialId", model.MaterialId);
                param.Add("@MaterialLotCode", model.MaterialLotCode);
                param.Add("@LotNo", model.LotNo);
                param.Add("@ReceivedDate", model.ReceivedDate?.ToString("yyyy-MM-dd"));
                param.Add("@Status", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MaterialLotDto>(proc, param);
                if (!data.Any())
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                }
                else
                {
                    returnData.Data = data;
                    returnData.TotalRow = param.Get<int>("totalRow");
                }

                return returnData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseModel<IEnumerable<MaterialLotDto>?>> GetDetailTabNG(MaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialLotDto>?>();
                string proc = "Usp_MaterialStock_GetLotDetailNG";
                var param = new DynamicParameters();
                param.Add("@MaterialId", model.MaterialId);
                param.Add("@MaterialLotCode", model.MaterialLotCode);
                param.Add("@ReceivedDate", model.ReceivedDate?.ToString("yyyy-MM-dd"));
                param.Add("@Status", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MaterialLotDto>(proc, param);
                if (!data.Any())
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                }
                else
                {
                    returnData.Data = data;
                    returnData.TotalRow = param.Get<int>("totalRow");
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
