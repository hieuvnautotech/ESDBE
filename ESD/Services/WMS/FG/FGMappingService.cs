using Dapper;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Models.Dtos.Common;
using ESD.Models.Dtos;
using System.Data;
using static ESD.Extensions.ServiceExtensions;
using ESD.Models.Dtos.Slitting;
using ESD.Helpers;
using System.Collections.Generic;
using ESD.Models.Dtos.WMS.FG;

namespace ESD.Services.APP
{
    public interface IFGMappingService
    {
        Task<ResponseModel<IEnumerable<BoxQRDto>?>> GetBoxQR(BoxQRDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetPrintBoxQR(List<string>? listQR);
        Task<ResponseModel<IEnumerable<BoxQRDto>?>> GetBuyerQR(BoxQRDto model);
        Task<ResponseModel<BoxQRDto?>> ScanLot(BoxQRDto model);
        Task<ResponseModel<BoxQRDto?>> UnMaping(BoxQRDto model);
        Task<ResponseModel<BoxQRDto?>> CreateBoxQR(List<BoxQRDto> model, long createdBy);
    }
    [ScopedRegistration]
    public class FGMappingService : IFGMappingService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public FGMappingService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<ResponseModel<IEnumerable<BoxQRDto>?>> GetBoxQR(BoxQRDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<BoxQRDto>?>();
                string proc = "Usp_FGMapping_GetAll";
                var param = new DynamicParameters();
                param.Add("@ProductId", model.ProductId);
                param.Add("@BoxQR", model.BoxQR);
                param.Add("@BuyerQR", model.BuyerQR);
                param.Add("@createdDate", model.SearchDate);
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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetPrintBoxQR(List<string>? listQR)
        {
            var returnData = new ResponseModel<IEnumerable<dynamic>?>();
            var proc = $"Usp_FGMapping_GetForPrint";
            var param = new DynamicParameters();
            param.Add("@listQR", ParameterTvp.GetTableValuedParameter_Varchar(listQR));
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);
            returnData.Data = data;
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
            }
            return returnData;
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

        public async Task<ResponseModel<BoxQRDto?>> ScanLot(BoxQRDto model)
        {
            var returnData = new ResponseModel<BoxQRDto?>();

            string proc = "Usp_FGMapping_ScanBuyerQR";
            var param = new DynamicParameters();
            param.Add("@BuyerQR", model.BuyerQR);
            param.Add("@FirstBuyerQR", model.FirstBuyerQR);
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

        public async Task<ResponseModel<BoxQRDto?>> UnMaping(BoxQRDto model)
        {
            var returnData = new ResponseModel<BoxQRDto?>();

            string proc = "Usp_FGMapping_UnMapping";
            var param = new DynamicParameters();
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

        public async Task<ResponseModel<BoxQRDto?>> CreateBoxQR(List<BoxQRDto> model, long createdBy)
        {
            var returnData = new ResponseModel<BoxQRDto?>();

            List<string> BuyerQRs = new List<string>();

            foreach (var item in model)
            {
                BuyerQRs.Add(item.BuyerQR);
            }

            string proc = "Usp_FGMapping_CreateBoxQR";
            var param = new DynamicParameters();
            param.Add("@listQR", ParameterTvp.GetTableValuedParameter_Varchar(BuyerQRs));
            param.Add("@createdBy", createdBy);
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
    }
}
