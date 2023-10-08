using Dapper;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Models.Dtos.Common;
using ESD.Models.Dtos.Slit;
using ESD.Models.Dtos.WMS.Material;
using System.Data;
using static ESD.Extensions.ServiceExtensions;

namespace ESD.Services.WMS.WIP
{
    public interface IWIPReceivingService
    {
        Task<ResponseModel<IEnumerable<SlitShippingOrderDto>?>> GetAll(SlitShippingOrderDto model);
        Task<ResponseModel<SlitShippingOrderLotDto?>> WIPScanLot(SlitShippingOrderLotDto model);
        Task<ResponseModel<IEnumerable<SlitShippingOrderDetailDto>?>> GetDetailHistory(SlitShippingOrderDetailDto model);
        Task<ResponseModel<IEnumerable<SlitShippingOrderDetailDto>?>> GetDetail(SlitShippingOrderDetailDto model);
        Task<string> DeleteScan(SlitShippingOrderLotDto model);
        //WMS
        Task<ResponseModel<IEnumerable<MaterialShippingOrderDto>?>> GetAllWMS(MaterialShippingOrderDto model);
        Task<ResponseModel<IEnumerable<MaterialShippingOrderDetailDto>?>> GetDetailWMS(MaterialShippingOrderDetailDto model);
        Task<ResponseModel<IEnumerable<MaterialShippingOrderDetailDto>?>> GetDetailHistoryWMS(MaterialShippingOrderDetailDto model);
        Task<ResponseModel<IEnumerable<MaterialShippingOrderLotDto>?>> GetDetailLotWMS(MaterialShippingOrderLotDto model);
        Task<ResponseModel<MaterialShippingOrderLotDto?>> WIPScanLotWMS(MaterialShippingOrderLotDto model);
        Task<string> DeleteScanWMS(MaterialShippingOrderLotDto model);
        Task<ResponseModel<IEnumerable<MaterialShippingOrderLotDto>?>> GetDetailLotByMSOId(long MSOId);
    }
    [ScopedRegistration]
    public class WIPReceivingService : IWIPReceivingService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public WIPReceivingService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        // SLIT CUT
        public async Task<ResponseModel<IEnumerable<SlitShippingOrderDto>?>> GetAll(SlitShippingOrderDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<SlitShippingOrderDto>?>();
                string proc = "Usp_WIPReceiving_GetAll";
                var param = new DynamicParameters();
                param.Add("@keyWord", model.SlitSOName);
                param.Add("@Description", model.Description);
                param.Add("@ProductId", model.ProductId);
                param.Add("@LocationName", model.LocationName);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<SlitShippingOrderDto>(proc, param);
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
        public async Task<ResponseModel<IEnumerable<SlitShippingOrderDetailDto>?>> GetDetailHistory(SlitShippingOrderDetailDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<SlitShippingOrderDetailDto>?>();
                string proc = "Usp_WIPReceivingDetail_GetHistory";
                var param = new DynamicParameters();
                param.Add("@SlitSOId", model.SlitSOId);
                param.Add("@MaterialId", model.MaterialId);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<SlitShippingOrderDetailDto>(proc, param);
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
        public async Task<ResponseModel<IEnumerable<SlitShippingOrderDetailDto>?>> GetDetail(SlitShippingOrderDetailDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<SlitShippingOrderDetailDto>?>();
                string proc = "Usp_WIPReceivingDetail_Get";
                var param = new DynamicParameters();
                param.Add("@SlitSOId", model.SlitSOId);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<SlitShippingOrderDetailDto>(proc, param);
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
        public async Task<ResponseModel<SlitShippingOrderLotDto?>> WIPScanLot(SlitShippingOrderLotDto model)
        {
            var returnData = new ResponseModel<SlitShippingOrderLotDto?>();

            string proc = "Usp_WIPReceiving_ScanReceivingWIP";
            var param = new DynamicParameters();
            param.Add("@SlitSODetailId", model.SlitSODetailId);
            param.Add("@MaterialLotCode", model.MaterialLotCode);
            param.Add("@createdBy", model.createdBy);
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
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }
        public async Task<string> DeleteScan(SlitShippingOrderLotDto model)
        {
            string proc = "Usp_WIPReceivingDetail_DeleteScan";
            var param = new DynamicParameters();
            param.Add("@SlitSODetailId", model.SlitSODetailId);
            param.Add("@MaterialLotId", model.MaterialLotId);
            param.Add("@modifiedBy", model.modifiedBy);
            param.Add("@row_version", model.row_version);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            return await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
        }
        // WMS
        public async Task<ResponseModel<IEnumerable<MaterialShippingOrderDto>?>> GetAllWMS(MaterialShippingOrderDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialShippingOrderDto>?>();
                string proc = "Usp_WIPReceiving_GetAllWMS";
                var param = new DynamicParameters();
                param.Add("@Keyword", model.MSOName);
                param.Add("@Description", model.Description);
                param.Add("@ProductId", model.ProductId);
                //param.Add("@AreaCode", model.AreaCode);
                param.Add("@Status", model.MSOStatus);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MaterialShippingOrderDto>(proc, param);
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
        public async Task<ResponseModel<IEnumerable<MaterialShippingOrderDetailDto>?>> GetDetailWMS(MaterialShippingOrderDetailDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialShippingOrderDetailDto>?>();
                string proc = "Usp_WIPReceiving_GetDetailWMS";
                var param = new DynamicParameters();
                param.Add("@MSOId", model.MSOId);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MaterialShippingOrderDetailDto>(proc, param);
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
        public async Task<ResponseModel<IEnumerable<MaterialShippingOrderDetailDto>?>> GetDetailHistoryWMS(MaterialShippingOrderDetailDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialShippingOrderDetailDto>?>();
                string proc = "Usp_WIPReceiving_GetHistoryWMS";
                var param = new DynamicParameters();
                param.Add("@MSOId", model.MSOId);
                param.Add("@MaterialId", model.MaterialId);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MaterialShippingOrderDetailDto>(proc, param);
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
        public async Task<ResponseModel<IEnumerable<MaterialShippingOrderLotDto>?>> GetDetailLotWMS(MaterialShippingOrderLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialShippingOrderLotDto>?>();
                string proc = "Usp_WIPReceiving_GetLotWMS";
                var param = new DynamicParameters();
                param.Add("@MSODetailId", model.MSODetailId);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MaterialShippingOrderLotDto>(proc, param);
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
        public async Task<ResponseModel<MaterialShippingOrderLotDto?>> WIPScanLotWMS(MaterialShippingOrderLotDto model)
        {
            var returnData = new ResponseModel<MaterialShippingOrderLotDto?>();

            string proc = "Usp_WIPReceiving_ScanReceivingWIPWMS";
            var param = new DynamicParameters();
            param.Add("@MSODetailId", model.MSODetailId);
            param.Add("@MaterialLotCode", model.MaterialLotCode);
            param.Add("@createdBy", model.createdBy);
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
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }
        public async Task<string> DeleteScanWMS(MaterialShippingOrderLotDto model)
        {
            string proc = "Usp_WIPReceivingDetail_DeleteScanWMS";
            var param = new DynamicParameters();
            param.Add("@MSODetailId", model.MSODetailId);
            param.Add("@MaterialLotId", model.MaterialLotId);
            param.Add("@modifiedBy", model.modifiedBy);
            param.Add("@row_version", model.row_version);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            return await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
        }
        public async Task<ResponseModel<IEnumerable<MaterialShippingOrderLotDto>?>> GetDetailLotByMSOId(long MSOId)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialShippingOrderLotDto>?>();
                string proc = "Usp_WIPReceiving_GetByMSOIdWMS";
                var param = new DynamicParameters();
                param.Add("@MSOId", MSOId);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MaterialShippingOrderLotDto>(proc, param);
                if (!data.Any())
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                }
                else
                {
                    returnData.Data = data;
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
