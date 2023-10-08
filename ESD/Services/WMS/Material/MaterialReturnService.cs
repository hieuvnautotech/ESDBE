using Dapper;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Models.Dtos.Common;
using ESD.Models.Dtos.WMS.Material;
using System.Data;
using static ESD.Extensions.ServiceExtensions;

namespace ESD.Services.WMS.Material
{
    public interface IMaterialReturnService
    {
        Task<ResponseModel<IEnumerable<ReturnMaterialDto>?>> GetAll(ReturnMaterialDto model);
        Task<ResponseModel<IEnumerable<ReturnMaterialLotDto>?>> GetDetail(ReturnMaterialLotDto model);
        Task<ResponseModel<ReturnMaterialLotDto?>> ScanLotReturnWMS(ReturnMaterialLotDto model);
    }
    [ScopedRegistration]
    public class MaterialReturnService : IMaterialReturnService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public MaterialReturnService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        #region Master
        public async Task<ResponseModel<IEnumerable<ReturnMaterialDto>?>> GetAll(ReturnMaterialDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<ReturnMaterialDto>?>();
                string proc = "Usp_WMSReturnMaterial_GetAll";
                var param = new DynamicParameters();
                param.Add("@Keyword", model.RMName);
                param.Add("@Description", model.Description);
                param.Add("@AreaCode", model.AreaCode);
                param.Add("@Status", model.RMStatus);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<ReturnMaterialDto>(proc, param);
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
        public async Task<ResponseModel<IEnumerable<ReturnMaterialLotDto>?>> GetDetail(ReturnMaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<ReturnMaterialLotDto>?>();
                string proc = "Usp_WMSReturnMaterial_GetDetail";
                var param = new DynamicParameters();
                param.Add("@RMId", model.RMId);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<ReturnMaterialLotDto>(proc, param);
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
        public async Task<ResponseModel<ReturnMaterialLotDto?>> ScanLotReturnWMS(ReturnMaterialLotDto model)
        {
            var returnData = new ResponseModel<ReturnMaterialLotDto?>();

            string proc = "Usp_WMSReturnMaterialLot_ScanReturn";
            var param = new DynamicParameters();
            param.Add("@RMId", model.RMId);
            param.Add("@MaterialLotCode", model.MaterialLotCode);
            param.Add("@LocationShelfId", model.LocationShelfId);
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
        #endregion
    }
}
