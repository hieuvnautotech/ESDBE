using Dapper;
using Newtonsoft.Json;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Helpers;
using ESD.Models;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Models.Validators;
using ESD.Services.Base;
using System.Data;
using ESD.Models.Dtos.WMS.Material;
using static ESD.Extensions.ServiceExtensions;

namespace ESD.Services.WMS.Material
{
    public interface ISlitReturnService
    {
        Task<ResponseModel<IEnumerable<ReturnMaterialDto>?>> GetAll(ReturnMaterialDto model);
        Task<ResponseModel<ReturnMaterialDto?>> GetById(long? QCPQCMasterId);
        Task<ResponseModel<ReturnMaterialDto?>> Create(ReturnMaterialDto model);
        Task<ResponseModel<ReturnMaterialDto?>> Modify(ReturnMaterialDto model);
        Task<ResponseModel<ReturnMaterialDto?>> Delete(ReturnMaterialDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetMaterial(BaseModel model, string MaterialLotCode, string MaterialCode, string ProductCode, long RMId);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetMaterialWIP(BaseModel model, string MaterialLotCode, string MaterialCode, string ProductCode, long RMId);

        Task<ResponseModel<IEnumerable<ReturnMaterialLotDto>?>> GetDetail(ReturnMaterialLotDto model);
        Task<ResponseModel<IEnumerable<ReturnMaterialLotDto>?>> GetDetailHistory(ReturnMaterialLotDto model);
        Task<ResponseModel<ReturnMaterialLotDto?>> CreateDetail(List<long> model, long RMId, long UserCreate);
        Task<ResponseModel<ReturnMaterialLotDto?>> DeleteDetail(ReturnMaterialLotDto model);

        Task<ResponseModel<IEnumerable<ReturnMaterialLotDto>?>> GetDetailLot(ReturnMaterialLotDto model);
        Task<ResponseModel<IEnumerable<ReturnMaterialLotDto>?>> GetDetailLotByRMId(ReturnMaterialLotDto model);
        Task<ResponseModel<ReturnMaterialLotDto?>> ScanLot(ReturnMaterialLotDto model);
        Task<ResponseModel<ReturnMaterialLotDto?>> ScanLotInWIP(ReturnMaterialLotDto model);
        Task<ResponseModel<ReturnMaterialLotDto?>> ScanLotReturnWMS(ReturnMaterialLotDto model);
        Task<ResponseModel<ReturnMaterialLotDto?>> ScanLotLineToWIP(ReturnMaterialLotDto model);
        Task<ResponseModel<ReturnMaterialLotDto?>> DeleteLot(ReturnMaterialLotDto model);
    }

    [ScopedRegistration]
    public class SlitReturnService : ISlitReturnService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public SlitReturnService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        #region Master
        public async Task<ResponseModel<IEnumerable<ReturnMaterialDto>?>> GetAll(ReturnMaterialDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<ReturnMaterialDto>?>();
                string proc = "Usp_ReturnMaterial_GetAll"; 
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

        public async Task<ResponseModel<ReturnMaterialDto?>> Create(ReturnMaterialDto model)
        {
            var returnData = new ResponseModel<ReturnMaterialDto?>();

            string proc = "Usp_ReturnMaterial_Create";
            var param = new DynamicParameters();
            param.Add("@RMId", model.RMId);
            param.Add("@RMName", model.RMName);
            param.Add("@AreaCode", model.AreaCode);
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
                    returnData = await GetById(model.RMId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<ReturnMaterialDto?>> Modify(ReturnMaterialDto model)
        {
            var returnData = new ResponseModel<ReturnMaterialDto?>();

            string proc = "Usp_ReturnMaterial_Modify";
            var param = new DynamicParameters();
            param.Add("@RMId", model.RMId);
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
                    returnData = await GetById(model.RMId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<ReturnMaterialDto?>> GetById(long? RMId)
        {
            var returnData = new ResponseModel<ReturnMaterialDto?>();
            string proc = "Usp_ReturnMaterial_GetById";
            var param = new DynamicParameters();
            param.Add("@RMId", RMId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<ReturnMaterialDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }

        public async Task<ResponseModel<ReturnMaterialDto?>> Delete(ReturnMaterialDto model)
        {
            string proc = "Usp_ReturnMaterial_Delete";
            var param = new DynamicParameters();
            param.Add("@RMId", model.RMId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<ReturnMaterialDto?>();
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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetMaterial(BaseModel model, string MaterialLotCode, string MaterialCode, string ProductCode, long RMId)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_ReturnMaterial_GetMaterial";
                var param = new DynamicParameters();
                param.Add("@RMId", RMId);
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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetMaterialWIP(BaseModel model, string MaterialLotCode, string MaterialCode, string ProductCode, long RMId)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_ReturnMaterial_GetMaterialWIP";
                var param = new DynamicParameters();
                param.Add("@RMId", RMId);
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
        #endregion

        #region Detail
        public async Task<ResponseModel<IEnumerable<ReturnMaterialLotDto>?>> GetDetail(ReturnMaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<ReturnMaterialLotDto>?>();
                string proc = "Usp_ReturnMaterialLot_Get"; 
                var param = new DynamicParameters();
                param.Add("@RMId", model.RMId);
                param.Add("@isActived", model.isActived);
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
                    returnData.TotalRow = param.Get<int>("totalRow");
                }

                return returnData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseModel<IEnumerable<ReturnMaterialLotDto>?>> GetDetailHistory(ReturnMaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<ReturnMaterialLotDto>?>();
                string proc = "Usp_ReturnMaterialLot_GetDetail";
                var param = new DynamicParameters();
                param.Add("@RMId", model.RMId);
                param.Add("@MaterialId", model.MaterialId);
                param.Add("@isActived", model.isActived);
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
                    returnData.TotalRow = param.Get<int>("totalRow");
                }

                return returnData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseModel<ReturnMaterialLotDto?>> GetDetailSingle(long? RMId, long? MaterialId)
        {
            var returnData = new ResponseModel<ReturnMaterialLotDto?>();
            string proc = "Usp_ReturnMaterialDetail_GetSingleMaterial";
            var param = new DynamicParameters();
            param.Add("@RMId", RMId);
            param.Add("@MaterialId", MaterialId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<ReturnMaterialLotDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }

        public async Task<ResponseModel<ReturnMaterialLotDto?>> CreateDetail(List<long> model, long RMId, long UserCreate)
        {
            var returnData = new ResponseModel<ReturnMaterialLotDto?>();

            string proc = "Usp_ReturnMaterialLot_Pick";
            var param = new DynamicParameters();
            param.Add("@RMId", RMId);
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

        public async Task<ResponseModel<ReturnMaterialLotDto?>> DeleteDetail(ReturnMaterialLotDto model)
        {
            string proc = "Usp_ReturnMaterialDetail_Delete";
            var param = new DynamicParameters();
            param.Add("@RMDetailId", model.RMId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<ReturnMaterialLotDto?>();
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
        #endregion

        #region Detail lot
        public async Task<ResponseModel<IEnumerable<ReturnMaterialLotDto>?>> GetDetailLot(ReturnMaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<ReturnMaterialLotDto>?>();
                string proc = "Usp_ReturnMaterialLot_Get";
                var param = new DynamicParameters();
                param.Add("@RMDetailId", model.RMId);
                param.Add("@isActived", model.isActived);
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
                    returnData.TotalRow = param.Get<int>("totalRow");
                }

                return returnData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ResponseModel<IEnumerable<ReturnMaterialLotDto>?>> GetDetailLotByRMId(ReturnMaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<ReturnMaterialLotDto>?>();
                string proc = "Usp_ReturnMaterialLot_GetByRMId";
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
        public async Task<ResponseModel<ReturnMaterialLotDto?>> ScanLot(ReturnMaterialLotDto model)
        {
            var returnData = new ResponseModel<ReturnMaterialLotDto?>();

            string proc = "Usp_ReturnMaterialLot_ScanSlitToWMS";
            var param = new DynamicParameters();
            param.Add("@RMId", model.RMId);
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
        public async Task<ResponseModel<ReturnMaterialLotDto?>> ScanLotInWIP(ReturnMaterialLotDto model)
        {
            var returnData = new ResponseModel<ReturnMaterialLotDto?>();

            string proc = "Usp_ReturnMaterialLot_ScanWIPToWMS";
            var param = new DynamicParameters();
            param.Add("@RMId", model.RMId);
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
        public async Task<ResponseModel<ReturnMaterialLotDto?>> ScanLotLineToWIP(ReturnMaterialLotDto model)
        {
            var returnData = new ResponseModel<ReturnMaterialLotDto?>();

            string proc = "Usp_ReturnMaterialLot_ScanLineToWIP";
            var param = new DynamicParameters();
            param.Add("@RMId", model.RMId);
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
        public async Task<ResponseModel<ReturnMaterialLotDto?>> ScanLotReturnWMS(ReturnMaterialLotDto model)
        {
            var returnData = new ResponseModel<ReturnMaterialLotDto?>();

            string proc = "Usp_ReturnMaterialLot_ScanReturnWMS";
            var param = new DynamicParameters();
            param.Add("@RMId", model.RMId);
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
        public async Task<ResponseModel<ReturnMaterialLotDto?>> DeleteLot(ReturnMaterialLotDto model)
        {
            string proc = "Usp_ReturnMaterialLot_Delete";
            var param = new DynamicParameters();
            param.Add("@RMId", model.RMId);
            param.Add("@MaterialLotId", model.MaterialLotId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<ReturnMaterialLotDto?>();
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
        #endregion
    }
}
