using ESD.DbAccess;
using ESD.Extensions;
using ESD.Models.Dtos.Common;
using ESD.Models.Dtos.WMS.FG;
using Dapper;
using System.Data;
using static ESD.Extensions.ServiceExtensions;

namespace ESD.Services.WMS.FG
{
    public interface IFGShippingOrderService
    {
        Task<ResponseModel<IEnumerable<FGShippingOrderDto>?>> GetAll(FGShippingOrderDto model);
        Task<ResponseModel<FGShippingOrderDto?>> GetById(long? QCPQCMasterId);
        Task<ResponseModel<FGShippingOrderDto?>> Create(FGShippingOrderDto model);
        Task<ResponseModel<FGShippingOrderDto?>> Modify(FGShippingOrderDto model);
        Task<ResponseModel<FGShippingOrderDto?>> Delete(FGShippingOrderDto model);
        Task<ResponseModel<IEnumerable<BoxQRDto>?>> GetAllBuyerQR(BoxQRDto model);
        Task<ResponseModel<IEnumerable<BoxQRDto>?>> GetBoxQR(BoxQRDto model);
        Task<ResponseModel<IEnumerable<BoxQRDto>?>> GetBuyerQR(BoxQRDto model);
        Task<ResponseModel<BoxQRDto?>> ScanBoxQR(BoxQRDto model);
        Task<ResponseModel<BoxQRDto?>> DeleteBoxQR(BoxQRDto model);
    }
    [ScopedRegistration]
    public class FGShippingOrderService : IFGShippingOrderService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public FGShippingOrderService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        #region Master
        public async Task<ResponseModel<IEnumerable<FGShippingOrderDto>?>> GetAll(FGShippingOrderDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<FGShippingOrderDto>?>();
                string proc = "Usp_FGShippingOrder_GetAll";
                var param = new DynamicParameters();
                param.Add("@BuyerId", model.BuyerId);
                param.Add("@FGSOCode", model.FGSOCode);
                param.Add("@BuyerCode", model.BuyerCode);
                param.Add("@ProductCode", model.ProductCode);
                param.Add("@BuyerQR", model.BuyerQR);
                param.Add("@DeliveryDate", model.DeliveryDate);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<FGShippingOrderDto>(proc, param);
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

        public async Task<ResponseModel<FGShippingOrderDto?>> Create(FGShippingOrderDto model)
        {
            var returnData = new ResponseModel<FGShippingOrderDto?>();

            string proc = "Usp_FGShippingOrder_Create";
            var param = new DynamicParameters();
            param.Add("@FGSOId", model.FGSOId);
            param.Add("@BuyerId", model.BuyerId);
            param.Add("@DeliveryDate", model.DeliveryDate);
            param.Add("@FGSOCode", model.FGSOCode);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var result = await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);

            returnData.ResponseMessage = result;
            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.SUCCESS:
                    returnData = await GetById(model.FGSOId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<FGShippingOrderDto?>> Modify(FGShippingOrderDto model)
        {
            var returnData = new ResponseModel<FGShippingOrderDto?>();

            string proc = "Usp_FGShippingOrder_Modify";
            var param = new DynamicParameters();
            param.Add("@FGSOId", model.FGSOId);
            param.Add("@BuyerId", model.BuyerId);
            param.Add("@DeliveryDate", model.DeliveryDate);
            param.Add("@FGSOCode", model.FGSOCode);
            param.Add("@createdBy", model.createdBy);
            param.Add("@row_version", model.row_version);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var result = await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);

            returnData.ResponseMessage = result;
            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.REFRESH_REQUIRED:
                    returnData.HttpResponseCode = 500;
                    returnData.ResponseMessage = result;
                    break;
                case StaticReturnValue.SUCCESS:
                    returnData = await GetById(model.FGSOId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<FGShippingOrderDto?>> GetById(long? FGSOId)
        {
            var returnData = new ResponseModel<FGShippingOrderDto?>();
            string proc = "Usp_FGShippingOrder_GetById";
            var param = new DynamicParameters();
            param.Add("@FGSOId", FGSOId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<FGShippingOrderDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }

        public async Task<ResponseModel<FGShippingOrderDto?>> Delete(FGShippingOrderDto model)
        {
            string proc = "Usp_FGShippingOrder_Delete";
            var param = new DynamicParameters();
            param.Add("@FGSOId", model.FGSOId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<FGShippingOrderDto?>();
            var result = await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.REFRESH_REQUIRED:
                    returnData.HttpResponseCode = 500;
                    returnData.ResponseMessage = result;
                    break;
                case StaticReturnValue.SUCCESS:
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    returnData.ResponseMessage = result;
                    break;
            }

            return returnData;
        }

        public async Task<ResponseModel<IEnumerable<BoxQRDto>?>> GetAllBuyerQR(BoxQRDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<BoxQRDto>?>();
                string proc = "Usp_FGShippingOrder_GetAllBuyerQr";
                var param = new DynamicParameters();
                param.Add("@FGSOId", model.FGSOId);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<BoxQRDto>(proc, param);
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
        #endregion
        public async Task<ResponseModel<IEnumerable<BoxQRDto>?>> GetBoxQR(BoxQRDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<BoxQRDto>?>();
                string proc = "Usp_FGShippingOrderBoxQR_Get";
                var param = new DynamicParameters();
                param.Add("@FGSOId", model.FGSOId);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<BoxQRDto>(proc, param);
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
        public async Task<ResponseModel<IEnumerable<BoxQRDto>?>> GetBuyerQR(BoxQRDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<BoxQRDto>?>();
                string proc = "Usp_FGMapping_GetByBoxQR";
                var param = new DynamicParameters();
                param.Add("@BoxQR", model.BoxQR);
                param.Add("@BuyerQR", model.BuyerQR);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<BoxQRDto>(proc, param);
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
        public async Task<ResponseModel<BoxQRDto?>> ScanBoxQR(BoxQRDto model)
        {
            var returnData = new ResponseModel<BoxQRDto?>();

            string proc = "Usp_FGShippingOrderBoxQR_Scan";
            var param = new DynamicParameters();
            param.Add("@FGSOId", model.FGSOId);
            param.Add("@BoxQR", model.BoxQR);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<BoxQRDto?>(proc, param);
            returnData.ResponseMessage = param.Get<string>("output");

            switch (returnData.ResponseMessage)
            {
                case StaticReturnValue.SUCCESS:
                    returnData.Data = data.FirstOrDefault();
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }

            return returnData;
        }
        public async Task<ResponseModel<BoxQRDto?>> DeleteBoxQR(BoxQRDto model)
        {
            var returnData = new ResponseModel<BoxQRDto?>();

            string proc = "Usp_FGShippingOrderBoxQR_Delete";
            var param = new DynamicParameters();
            param.Add("@FGSOId", model.FGSOId);
            param.Add("@BoxQR", model.BoxQR);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);

            var result = await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);

            returnData.ResponseMessage = result;
            switch (result)
            {
                case StaticReturnValue.SUCCESS:
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    returnData.ResponseMessage = result;
                    break;
            }

            return returnData;
        }

    }
}
