using ESD.Models.Dtos.Common;
using ESD.Models.Dtos;
using static ESD.Extensions.ServiceExtensions;
using ESD.DbAccess;
using ESD.Models.Dtos.FQC;
using ESD.Extensions;
using System.Data;
using Dapper;

namespace ESD.Services.FQC
{
    public interface IFQCShippingService
    {
        Task<ResponseModel<IEnumerable<FQCShippingDto>?>> GetAll(FQCShippingDto model);
        Task<ResponseModel<FQCShippingDto?>> GetById(long? QCPQCMasterId);
        Task<ResponseModel<FQCShippingDto?>> Create(FQCShippingDto model);
        Task<ResponseModel<FQCShippingDto?>> Modify(FQCShippingDto model);
        Task<ResponseModel<FQCShippingDto?>> Delete(FQCShippingDto model);

        Task<ResponseModel<IEnumerable<FQCShippingLotDto>?>> GetDetailLot(FQCShippingLotDto model);
        Task<ResponseModel<FQCShippingLotDto?>> ScanLot(FQCShippingLotDto model);
        Task<ResponseModel<FQCShippingLotDto?>> ScanReceivingLot(FQCShippingLotDto model);
        Task<ResponseModel<FQCShippingLotDto?>> DeleteLot(FQCShippingLotDto model);
    }
    [ScopedRegistration]
    public class FQCShippingService : IFQCShippingService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public FQCShippingService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        #region Master
        public async Task<ResponseModel<IEnumerable<FQCShippingDto>?>> GetAll(FQCShippingDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<FQCShippingDto>?>();
                string proc = "Usp_FQCShipping_GetAll";
                var param = new DynamicParameters();
                param.Add("@FQCSOName", model.FQCSOName);
                param.Add("@Description", model.Description);
                param.Add("@ProductId", model.ProductId);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<FQCShippingDto>(proc, param);
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

        public async Task<ResponseModel<FQCShippingDto?>> Create(FQCShippingDto model)
        {
            var returnData = new ResponseModel<FQCShippingDto?>();

            string proc = "Usp_FQCShipping_Create";
            var param = new DynamicParameters();
            param.Add("@FQCSOId", model.FQCSOId);
            param.Add("@FQCSOName", model.FQCSOName);
            param.Add("@ProductId", model.ProductId);
            param.Add("@OrderQty", model.OrderQty);
            param.Add("@EAQty", model.EAQty);
            param.Add("@ShippingDate", model.ShippingDate);
            param.Add("@Description", model.Description);
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
                    returnData = await GetById(model.FQCSOId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<FQCShippingDto?>> Modify(FQCShippingDto model)
        {
            var returnData = new ResponseModel<FQCShippingDto?>();

            string proc = "Usp_FQCShipping_Modify";
            var param = new DynamicParameters();
            param.Add("@FQCSOId", model.FQCSOId);
            param.Add("@FQCSOName", model.FQCSOName);
            param.Add("@ProductId", model.ProductId);
            param.Add("@OrderQty", model.OrderQty);
            param.Add("@EAQty", model.EAQty);
            param.Add("@ShippingDate", model.ShippingDate);
            param.Add("@Description", model.Description);
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
                    returnData = await GetById(model.FQCSOId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<FQCShippingDto?>> GetById(long? FQCSOId)
        {
            var returnData = new ResponseModel<FQCShippingDto?>();
            string proc = "Usp_FQCShipping_GetById";
            var param = new DynamicParameters();
            param.Add("@FQCSOId", FQCSOId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<FQCShippingDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }

        public async Task<ResponseModel<FQCShippingDto?>> Delete(FQCShippingDto model)
        {
            string proc = "Usp_FQCShipping_Delete";
            var param = new DynamicParameters();
            param.Add("@FQCSOId", model.FQCSOId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<FQCShippingDto?>();
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
        #endregion

        #region Detail lot
        public async Task<ResponseModel<IEnumerable<FQCShippingLotDto>?>> GetDetailLot(FQCShippingLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<FQCShippingLotDto>?>();
                string proc = "Usp_FQCShippingLot_Get";
                var param = new DynamicParameters();
                param.Add("@FQCSOId", model.FQCSOId);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<FQCShippingLotDto>(proc, param);
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

        public async Task<ResponseModel<FQCShippingLotDto?>> ScanLot(FQCShippingLotDto model)
        {
            var returnData = new ResponseModel<FQCShippingLotDto?>();

            string proc = "Usp_FQCShippingLot_Scan";
            var param = new DynamicParameters();
            param.Add("@FQCSOId", model.FQCSOId);
            param.Add("@BuyerQR", model.BuyerQR);
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
                    returnData = await GetLotById(model.BuyerQR);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }
        public async Task<ResponseModel<FQCShippingLotDto?>> ScanReceivingLot(FQCShippingLotDto model)
        {
            var returnData = new ResponseModel<FQCShippingLotDto?>();

            string proc = "Usp_FQCShippingOrderLot_ScanReceiving";
            var param = new DynamicParameters();
            param.Add("@FQCSOId", model.FQCSOId);
            param.Add("@SemiLotCode", model.SemiLotCode);
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
                    returnData = await GetLotById(model.BuyerQR);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }
        public async Task<ResponseModel<FQCShippingLotDto?>> DeleteLot(FQCShippingLotDto model)
        {
            string proc = "Usp_FQCShippingLot_Delete";
            var param = new DynamicParameters();
            param.Add("@FQCSOId", model.FQCSOId);
            param.Add("@WOSemiLotFQCId", model.WOSemiLotFQCId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<FQCShippingLotDto?>();
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
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }

            return returnData;
        }
        public async Task<ResponseModel<FQCShippingLotDto?>> GetLotById(string? BuyerQR)
        {
            var returnData = new ResponseModel<FQCShippingLotDto?>();
            string proc = "Usp_FQCShippingLot_GetById";
            var param = new DynamicParameters();
            param.Add("@BuyerQR", BuyerQR);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<FQCShippingLotDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }

        #endregion
    }
}
