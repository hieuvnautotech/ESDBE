using Dapper;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Models.Dtos.Common;
using ESD.Models.Dtos.FQC;
using System.Data;
using static ESD.Extensions.ServiceExtensions;

namespace ESD.Services.WMS
{
    public interface IFGReceivingService
    {
        Task<ResponseModel<IEnumerable<FQCShippingDto>?>> GetAll(FQCShippingDto model);
        Task<ResponseModel<IEnumerable<FQCShippingLotDto>?>> GetDetailLot(FQCShippingLotDto model);
        Task<ResponseModel<FQCShippingLotDto?>> ScanReceivingLot(FQCShippingLotDto model);
        Task<ResponseModel<FQCShippingLotDto?>> DeleteLot(FQCShippingLotDto model);
    }
    [ScopedRegistration]
    public class FGReceivingService: IFGReceivingService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public FGReceivingService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        #region Master
        public async Task<ResponseModel<IEnumerable<FQCShippingDto>?>> GetAll(FQCShippingDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<FQCShippingDto>?>();
                string proc = "Usp_FGReceiving_GetAll";
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
        #endregion

        public async Task<ResponseModel<IEnumerable<FQCShippingLotDto>?>> GetDetailLot(FQCShippingLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<FQCShippingLotDto>?>();
                string proc = "Usp_FGReceiving_GetDetail";
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

        public async Task<ResponseModel<FQCShippingLotDto?>> ScanReceivingLot(FQCShippingLotDto model)
        {
            var returnData = new ResponseModel<FQCShippingLotDto?>();

            string proc = "Usp_FGReceiving_ScanBuyer";
            var param = new DynamicParameters();
            param.Add("@FQCSOId", model.FQCSOId);
            param.Add("@BuyerQR", model.BuyerQR);
            param.Add("@LocationId", model.LocationId);
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
                    //returnData = await GetLotById(model.SemiLotCode);
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
            string proc = "Usp_FGReceiving_DeleteLot";
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
    }
}
