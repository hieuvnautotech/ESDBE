using ESD.Models.Dtos.Common;
using ESD.Models.Dtos;
using static ESD.Extensions.ServiceExtensions;
using ESD.DbAccess;
using System.Data;
using Dapper;
using ESD.Extensions;
using ESD.Helpers;

namespace ESD.Services.WMS.WIP
{
    public interface IWIPReturnService
    {
        Task<ResponseModel<IEnumerable<WIPReturnMaterialDto>?>> GetAll(WIPReturnMaterialDto model);
        Task<ResponseModel<WIPReturnMaterialDto?>> Create(WIPReturnMaterialDto model);
        Task<ResponseModel<WIPReturnMaterialDto?>> Modify(WIPReturnMaterialDto model);
        Task<ResponseModel<WIPReturnMaterialDto?>> Delete(WIPReturnMaterialDto model);
        Task<ResponseModel<IEnumerable<WIPReturnMaterialLotDto>?>> GetDetail(WIPReturnMaterialLotDto model);
        Task<ResponseModel<IEnumerable<WIPReturnMaterialLotDto>?>> GetDetailHistory(WIPReturnMaterialLotDto model);
        Task<ResponseModel<WIPReturnMaterialLotDto?>> CreateDetail(List<long> model, long WIPRMId, long UserCreate);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetMaterial(BaseModel model, string MaterialLotCode, string MaterialCode, string ProductCode, long WIPRMId);
        Task<ResponseModel<WIPReturnMaterialLotDto?>> ScanLot(WIPReturnMaterialLotDto model);
        Task<ResponseModel<IEnumerable<WIPReturnMaterialLotDto>?>> GetDetailLotByRMId(WIPReturnMaterialLotDto model);
        Task<ResponseModel<WIPReturnMaterialLotDto?>> DeleteLot(WIPReturnMaterialLotDto model);
        #region TAB WIP
        Task<ResponseModel<IEnumerable<WIPReturnMaterialLotDto>?>> GetDetailTabWIP(WIPReturnMaterialLotDto model);
        Task<ResponseModel<WIPReturnMaterialLotDto?>> ScanLotReturnTabWIP(WIPReturnMaterialLotDto model);
        #endregion
    }
    [ScopedRegistration]
    public class WIPReturnService : IWIPReturnService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public WIPReturnService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        #region Master
        public async Task<ResponseModel<IEnumerable<WIPReturnMaterialDto>?>> GetAll(WIPReturnMaterialDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WIPReturnMaterialDto>?>();
                string proc = "Usp_WIPReturnMaterial_GetAll";
                var param = new DynamicParameters();
                param.Add("@Keyword", model.RMName);
                param.Add("@Description", model.Description);
                param.Add("@Status", model.RMStatus);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WIPReturnMaterialDto>(proc, param);
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
        public async Task<ResponseModel<WIPReturnMaterialDto?>> Create(WIPReturnMaterialDto model)
        {
            var returnData = new ResponseModel<WIPReturnMaterialDto?>();

            string proc = "Usp_WIPReturnMaterial_Create";
            var param = new DynamicParameters();
            param.Add("@WIPRMId", model.WIPRMId);
            param.Add("@RMName", model.RMName);
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
                    returnData = await GetById(model.WIPRMId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }
        public async Task<ResponseModel<WIPReturnMaterialDto?>> Modify(WIPReturnMaterialDto model)
        {
            var returnData = new ResponseModel<WIPReturnMaterialDto?>();

            string proc = "Usp_WIPReturnMaterial_Modify";
            var param = new DynamicParameters();
            param.Add("@WIPRMId", model.WIPRMId);
            param.Add("@RMName", model.RMName);
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
                    returnData = await GetById(model.WIPRMId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }
        public async Task<ResponseModel<WIPReturnMaterialDto?>> Delete(WIPReturnMaterialDto model)
        {
            string proc = "Usp_WIPReturnMaterial_Delete";
            var param = new DynamicParameters();
            param.Add("@WIPRMId", model.WIPRMId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<WIPReturnMaterialDto?>();
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
                    break;
            }

            return returnData;
        }
        public async Task<ResponseModel<WIPReturnMaterialDto?>> GetById(long? WIPRMId)
        {
            var returnData = new ResponseModel<WIPReturnMaterialDto?>();
            string proc = "Usp_WIPReturnMaterial_GetById";
            var param = new DynamicParameters();
            param.Add("@WIPRMId", WIPRMId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WIPReturnMaterialDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }
        #endregion
        public async Task<ResponseModel<IEnumerable<WIPReturnMaterialLotDto>?>> GetDetail(WIPReturnMaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WIPReturnMaterialLotDto>?>();
                string proc = "Usp_WIPReturnMaterialLot_Get";
                var param = new DynamicParameters();
                param.Add("@WIPRMId", model.WIPRMId);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WIPReturnMaterialLotDto>(proc, param);
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
        public async Task<ResponseModel<WIPReturnMaterialLotDto?>> CreateDetail(List<long> model, long WIPRMId, long UserCreate)
        {
            var returnData = new ResponseModel<WIPReturnMaterialLotDto?>();

            string proc = "Usp_WIPReturnMaterialLot_Pick";
            var param = new DynamicParameters();
            param.Add("@WIPRMId", WIPRMId);
            param.Add("@LotIds", ParameterTvp.GetTableValuedParameter_BigInt(model));
            param.Add("@createdBy", UserCreate);
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
        public async Task<ResponseModel<IEnumerable<WIPReturnMaterialLotDto>?>> GetDetailHistory(WIPReturnMaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WIPReturnMaterialLotDto>?>();
                string proc = "Usp_WIPReturnMaterialLot_GetDetail";
                var param = new DynamicParameters();
                param.Add("@WIPRMId", model.WIPRMId);
                param.Add("@MaterialId", model.MaterialId);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WIPReturnMaterialLotDto>(proc, param);
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
        public async Task<ResponseModel<WIPReturnMaterialLotDto?>> ScanLot(WIPReturnMaterialLotDto model)
        {
            var returnData = new ResponseModel<WIPReturnMaterialLotDto?>();

            string proc = "Usp_WIPReturnMaterialLot_Scan";
            var param = new DynamicParameters();
            param.Add("@WIPRMId", model.WIPRMId);
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
        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetMaterial(BaseModel model, string MaterialLotCode, string MaterialCode, string ProductCode, long WIPRMId)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_WIPReturnMaterialLot_GetMaterial";
                var param = new DynamicParameters();
                param.Add("@WIPRMId", WIPRMId);
                param.Add("@MaterialCode", MaterialCode);
                param.Add("@ProductCode", ProductCode);
                param.Add("@MaterialLotCode", MaterialLotCode);
                param.Add("@isActived", model.isActived);
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
        public async Task<ResponseModel<IEnumerable<WIPReturnMaterialLotDto>?>> GetDetailLotByRMId(WIPReturnMaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WIPReturnMaterialLotDto>?>();
                string proc = "Usp_WIPReturnMaterialLot_GetByRMId";
                var param = new DynamicParameters();
                param.Add("@WIPRMId", model.WIPRMId);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WIPReturnMaterialLotDto>(proc, param);
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
        public async Task<ResponseModel<WIPReturnMaterialLotDto?>> DeleteLot(WIPReturnMaterialLotDto model)
        {
            string proc = "Usp_WIPReturnMaterialLot_Delete";
            var param = new DynamicParameters();
            param.Add("@WIPRMId", model.WIPRMId);
            param.Add("@MaterialLotId", model.MaterialLotId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<WIPReturnMaterialLotDto?>();
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

        #region TAB WIP
        public async Task<ResponseModel<IEnumerable<WIPReturnMaterialLotDto>?>> GetDetailTabWIP(WIPReturnMaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WIPReturnMaterialLotDto>?>();
                string proc = "Usp_WIPReturnMaterialLot_GetDetailWIP";
                var param = new DynamicParameters();
                param.Add("@WIPRMId", model.WIPRMId);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WIPReturnMaterialLotDto>(proc, param);
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
        public async Task<ResponseModel<WIPReturnMaterialLotDto?>> ScanLotReturnTabWIP(WIPReturnMaterialLotDto model)
        {
            var returnData = new ResponseModel<WIPReturnMaterialLotDto?>();

            string proc = "Usp_WIPReturnMaterialLot_ScanReturnWIP";
            var param = new DynamicParameters();
            param.Add("@WIPRMId", model.WIPRMId);
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
        #endregion
    }
}
