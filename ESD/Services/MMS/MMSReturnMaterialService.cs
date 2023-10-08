using ESD.Models.Dtos.Common;
using ESD.Models.Dtos;
using ESD.DbAccess;
using ESD.Services.WMS.Material;
using static ESD.Extensions.ServiceExtensions;
using Dapper;
using ESD.Extensions;
using System.Data;
using ESD.Helpers;
using Newtonsoft.Json;

namespace ESD.Services.MMS
{
    public interface IMMSReturnMaterialService
    {
        Task<ResponseModel<IEnumerable<dynamic>?>> GetAll(MaterialLotDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetAllF4(MaterialLotDto model);
        Task<ResponseModel<IEnumerable<dynamic?>>> ConfirmMMS(List<MMSMaterialDto> model, long userCreate);
        Task<ResponseModel<IEnumerable<dynamic?>>> ConfirmMMSF4(List<MMSMaterialDto> model, long userCreate);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetListPrintQR(List<long>? listQR);
        Task<ResponseModel<MaterialLotDto?>> UpdateLength(MaterialLotDto model);
    }
    [ScopedRegistration]
    public class MMSReturnMaterialService : IMMSReturnMaterialService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public MMSReturnMaterialService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetAll(MaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_MMSReturnMaterial_GetAll";
                var param = new DynamicParameters();
                param.Add("@MaterialLotCode", model.MaterialLotCode);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
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
        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetAllF4(MaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_MMSReturnMaterial_GetAllF4";
                var param = new DynamicParameters();
                param.Add("@MaterialLotCode", model.MaterialLotCode);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
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
        public async Task<ResponseModel<IEnumerable<dynamic?>>> ConfirmMMS(List<MMSMaterialDto> model, long userCreate)
        {
            var returnData = new ResponseModel<IEnumerable<dynamic?>>();
            string proc = $"Usp_MMSReturnMaterial_Confirm";
            var param = new DynamicParameters();
            var jsonLotList = JsonConvert.SerializeObject(model);
            param.Add("@Jsonlist", jsonLotList);
            param.Add("@createdBy", userCreate);
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
        public async Task<ResponseModel<IEnumerable<dynamic?>>> ConfirmMMSF4(List<MMSMaterialDto> model, long userCreate)
        {
            var returnData = new ResponseModel<IEnumerable<dynamic?>>();
            string proc = $"Usp_MMSReturnMaterial_ConfirmF4";
            var param = new DynamicParameters();
            var jsonLotList = JsonConvert.SerializeObject(model);
            param.Add("@Jsonlist", jsonLotList);
            param.Add("@createdBy", userCreate);
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
        public async Task<ResponseModel<MaterialLotDto?>> UpdateLength(MaterialLotDto model)
        {
            var returnData = new ResponseModel<MaterialLotDto?>();

            string proc = "Usp_MMSReturnMaterial_UpdateLength";
            var param = new DynamicParameters();
            param.Add("@MaterialLotId", model.MaterialLotId);
            param.Add("@Length", model.Length);
            param.Add("@modifiedBy", model.createdBy);
            param.Add("@row_version", model.row_version);
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
        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetListPrintQR(List<long>? listQR)
        {
            var returnData = new ResponseModel<IEnumerable<dynamic>?>();
            var proc = $"Usp_MaterialLot_Print";
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
    }
}
