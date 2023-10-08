using ESD.Models.Dtos.Common;
using ESD.Models.Dtos;
using ESD.DbAccess;
using static ESD.Extensions.ServiceExtensions;
using Dapper;
using ESD.Extensions;
using System.Data;
using ESD.Models.Dtos.Slitting;
using ESD.Helpers;
using Newtonsoft.Json;
using ESD.Models.Dtos.FQC;

namespace ESD.Services.APP
{
    public interface IBuyerQRService
    {
        Task<ResponseModel<IEnumerable<BuyerQRDto>?>> GetAll(BuyerQRDto model);
        Task<ResponseModel<IEnumerable<BuyerQRDto>?>> CreateBuyerQR(BuyerQRDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetListPrintQR(List<long>? listQR);
        Task<ResponseModel<BuyerQRDto?>> Delete(BuyerQRDto model);


        Task<ResponseModel<WOSemiLotFQCDto?>> ChangeBuyerQR(WOSemiLotFQCDto model);
        Task<ResponseModel<WOSemiLotFQCDto?>> GetForChangeBuyerQR(string? BuyerQR);


        Task<ResponseModel<IEnumerable<WOSemiLotFQCDto>?>> GetAppMapping(WOSemiLotFQCDto model);
        Task<ResponseModel<WOSemiLotFQCDto?>> MappingBuyerQR(WOSemiLotFQCDto model);
    }
    [ScopedRegistration]
    public class BuyerQRService : IBuyerQRService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public BuyerQRService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        #region Buyer QR
        public async Task<ResponseModel<IEnumerable<BuyerQRDto>?>> GetAll(BuyerQRDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<BuyerQRDto>?>();
                string proc = "Usp_BuyerQR_GetAll"; 
                var param = new DynamicParameters();
                param.Add("@BuyerQR", model.BuyerQR);
                param.Add("@ProductId", model.ProductId);
                param.Add("@createdDate", model.createdDate);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<BuyerQRDto>(proc, param);
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

        public async Task<ResponseModel<IEnumerable<BuyerQRDto>?>> CreateBuyerQR(BuyerQRDto model)
        {
            var returnData = new ResponseModel<IEnumerable<BuyerQRDto>?> ();

            if (string.IsNullOrEmpty(model.Stamps))
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "BuyerQR.NoStamps";
                return returnData;
            }

            string proc = "Usp_BuyerQR_Create";
            var param = new DynamicParameters();
            param.Add("@ProductCode", model.ProductCode);
            param.Add("@VendorLine", model.VendorLine);
            param.Add("@LabelPrinter", model.LabelPrinter);
            param.Add("@IsSample", model.IsSample);
            param.Add("@PCN", model.PCN);
            param.Add("@LabelQty", model.LabelQty);
            param.Add("@QuantityFormat", model.QuantityFormat);
            param.Add("@LotNo", model.LotNo);
            param.Add("@MachineLine", model.MachineLine);
            param.Add("@Shift", model.Shift);
            param.Add("@RemarkBuyer", model.RemarkBuyer);
            param.Add("@PackingAmount", model.PackingAmount);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<BuyerQRDto>(proc, param);
            returnData.ResponseMessage = param.Get<string>("output");

            switch (returnData.ResponseMessage)
            {
                case StaticReturnValue.SUCCESS:
                    returnData.Data = data;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }

            return returnData;
        }

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetListPrintQR(List<long>? listQR)
        {
            var returnData = new ResponseModel<IEnumerable<dynamic>?>();
            var proc = $"Usp_BuyerQR_GetPrint";
            var param = new DynamicParameters();
            param.Add("@listQR", ParameterTvp.GetTableValuedParameter_BigInt(listQR));
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);
            returnData.Data = data;
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
            }
            return returnData;
        }

        public async Task<ResponseModel<BuyerQRDto?>> Delete(BuyerQRDto model)
        {
            string proc = "Usp_BuyerQR_Delete";
            var param = new DynamicParameters();
            param.Add("@BuyerQRId", model.BuyerQRId);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<BuyerQRDto?>();
            var result = await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
            returnData.ResponseMessage = result;
            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.REFRESH_REQUIRED:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.SUCCESS:
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }

            return returnData;
        }
        #endregion

        #region Buyer Mapping
        public async Task<ResponseModel<IEnumerable<WOSemiLotFQCDto>?>> GetAppMapping(WOSemiLotFQCDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WOSemiLotFQCDto>?>();
                string proc = "Usp_MappingBuyer_GetAll";
                var param = new DynamicParameters();
                param.Add("@SemiLotCode", model.SemiLotCode);
                param.Add("@BuyerQR", model.BuyerQR);
                param.Add("@ProductId", model.ProductId);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOSemiLotFQCDto>(proc, param);
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

        public async Task<ResponseModel<WOSemiLotFQCDto?>> MappingBuyerQR(WOSemiLotFQCDto model)
        {
            var returnData = new ResponseModel<WOSemiLotFQCDto?>();

            string proc = "Usp_MappingBuyer_MappingQR";
            var param = new DynamicParameters();
            param.Add("@SemiLotCode", model.SemiLotCode);
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
                    returnData = await GetByCode(model.SemiLotCode);
                    returnData.ResponseMessage = StaticReturnValue.SUCCESS;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }

            return returnData;
        }

        public async Task<ResponseModel<WOSemiLotFQCDto?>> GetByCode(string? SemiLotCode)
        {
            var returnData = new ResponseModel<WOSemiLotFQCDto?>();
            string proc = "Usp_WOSemiLotFQC_GetByCode";
            var param = new DynamicParameters();
            param.Add("@SemiLotCode", SemiLotCode);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOSemiLotFQCDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }

        public async Task<ResponseModel<WOSemiLotFQCDto?>> GetForChangeBuyerQR(string? BuyerQR)
        {
            var returnData = new ResponseModel<WOSemiLotFQCDto?>();
            string proc = "Usp_WOSemiLotFQC_GetByBuyerQR";
            var param = new DynamicParameters();
            param.Add("@BuyerQR", BuyerQR);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOSemiLotFQCDto>(proc, param);
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

        public async Task<ResponseModel<WOSemiLotFQCDto?>> ChangeBuyerQR(WOSemiLotFQCDto model)
        {
            var returnData = new ResponseModel<WOSemiLotFQCDto?>();

            string proc = "Usp_MappingBuyer_ChangeBuyerQR";
            var param = new DynamicParameters();
            param.Add("@BuyerQROld", model.SemiLotCode);
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
                    returnData.ResponseMessage = StaticReturnValue.SUCCESS;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }

            return returnData;
        }

        #endregion
    }
}
