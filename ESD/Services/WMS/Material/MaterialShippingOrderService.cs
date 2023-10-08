using Dapper;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Models.Dtos.Common;
using ESD.Models.Dtos.WMS.Material;
using Newtonsoft.Json;
using System.Data;
using static ESD.Extensions.ServiceExtensions;

namespace ESD.Services.WMS.Material
{
    public interface IMaterialShippingOrderService
    {
        Task<ResponseModel<IEnumerable<MaterialShippingOrderDto>?>> GetAll(MaterialShippingOrderDto model);
        Task<ResponseModel<MaterialShippingOrderDto?>> GetById(long? QCPQCMasterId);
        Task<ResponseModel<MaterialShippingOrderDto?>> Create(MaterialShippingOrderDto model);
        Task<ResponseModel<MaterialShippingOrderDto?>> Modify(MaterialShippingOrderDto model);
        Task<ResponseModel<MaterialShippingOrderDto?>> Delete(MaterialShippingOrderDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetMaterial(BaseModel model, string MaterialCode, string ProductCode, long MsoId);

        Task<ResponseModel<IEnumerable<MaterialShippingOrderDetailDto>?>> GetDetail(MaterialShippingOrderDetailDto model);
        Task<ResponseModel<IEnumerable<MaterialShippingOrderDetailDto>?>> GetDetailHistory(MaterialShippingOrderDetailDto model);
        Task<ResponseModel<MaterialShippingOrderDetailDto?>> CreateDetail(List<MaterialShippingOrderDetailDto> model, long MSOId, long UserCreate);
        Task<ResponseModel<MaterialShippingOrderDetailDto?>> DeleteDetail(MaterialShippingOrderDetailDto model);

        Task<ResponseModel<IEnumerable<MaterialShippingOrderLotDto>?>> GetDetailLot(MaterialShippingOrderLotDto model);
        Task<ResponseModel<IEnumerable<MaterialShippingOrderLotDto>?>> GetDetailLotByMSOId(long MSOId);
        Task<ResponseModel<MaterialShippingOrderLotDto?>> ScanLot(MaterialShippingOrderLotDto model);
        Task<ResponseModel<MaterialShippingOrderLotDto?>> ScanReceivingLot(MaterialShippingOrderLotDto model);
        Task<ResponseModel<MaterialShippingOrderLotDto?>> DeleteLot(MaterialShippingOrderLotDto model);
    }
    [ScopedRegistration]
    public class MaterialShippingOrderService : IMaterialShippingOrderService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public MaterialShippingOrderService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        #region Master
        public async Task<ResponseModel<IEnumerable<MaterialShippingOrderDto>?>> GetAll(MaterialShippingOrderDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialShippingOrderDto>?>();
                string proc = "Usp_MaterialShippingOrder_GetAll";
                var param = new DynamicParameters();
                param.Add("@Keyword", model.MSOName);
                param.Add("@Description", model.Description);
                param.Add("@ProductId", model.ProductId);
                param.Add("@AreaCode", model.AreaCode);
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

        public async Task<ResponseModel<MaterialShippingOrderDto?>> Create(MaterialShippingOrderDto model)
        {
            var returnData = new ResponseModel<MaterialShippingOrderDto?>();

            string proc = "Usp_MaterialShippingOrder_Create";
            var param = new DynamicParameters();
            param.Add("@MSOId", model.MSOId);
            param.Add("@MSOName", model.MSOName);
            param.Add("@ProductId", model.ProductId);
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
                    returnData = await GetById(model.MSOId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<MaterialShippingOrderDto?>> Modify(MaterialShippingOrderDto model)
        {
            var returnData = new ResponseModel<MaterialShippingOrderDto?>();

            string proc = "Usp_MaterialShippingOrder_Modify";
            var param = new DynamicParameters();
            param.Add("@MSOId", model.MSOId);
            param.Add("@MSOName", model.MSOName);
            param.Add("@ProductId", model.ProductId);
            param.Add("@AreaCode", model.AreaCode);
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
                    returnData = await GetById(model.MSOId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }


        public async Task<ResponseModel<MaterialShippingOrderDto?>> GetById(long? MSOId)
        {
            var returnData = new ResponseModel<MaterialShippingOrderDto?>();
            string proc = "Usp_MaterialShippingOrder_GetById";
            var param = new DynamicParameters();
            param.Add("@MSOId", MSOId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MaterialShippingOrderDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }

        public async Task<ResponseModel<MaterialShippingOrderDto?>> Delete(MaterialShippingOrderDto model)
        {
            string proc = "Usp_MaterialShippingOrder_Delete";
            var param = new DynamicParameters();
            param.Add("@MSOId", model.MSOId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<MaterialShippingOrderDto?>();
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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetMaterial(BaseModel model, string MaterialCode, string ProductCode, long MsoId)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_MaterialShippingOrder_GetMaterial";
                var param = new DynamicParameters();
                param.Add("@MsoId", MsoId);
                param.Add("@MaterialCode", MaterialCode);
                param.Add("@ProductCode", ProductCode);
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
        public async Task<ResponseModel<IEnumerable<MaterialShippingOrderDetailDto>?>> GetDetail(MaterialShippingOrderDetailDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialShippingOrderDetailDto>?>();
                string proc = "Usp_MaterialShippingOrderDetail_Get";
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

        public async Task<ResponseModel<IEnumerable<MaterialShippingOrderDetailDto>?>> GetDetailHistory(MaterialShippingOrderDetailDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialShippingOrderDetailDto>?>();
                string proc = "Usp_MaterialShippingOrderDetail_GetHistory";
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

        public async Task<ResponseModel<MaterialShippingOrderDetailDto?>> GetDetailSingle(long? MSOId, long? MaterialId)
        {
            var returnData = new ResponseModel<MaterialShippingOrderDetailDto?>();
            string proc = "Usp_MaterialShippingOrderDetail_GetSingleMaterial";
            var param = new DynamicParameters();
            param.Add("@MSOId", MSOId);
            param.Add("@MaterialId", MaterialId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MaterialShippingOrderDetailDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }

        public async Task<ResponseModel<MaterialShippingOrderDetailDto?>> CreateDetail(List<MaterialShippingOrderDetailDto> model, long MSOId, long UserCreate)
        {
            var returnData = new ResponseModel<MaterialShippingOrderDetailDto?>();


            var jsonLotList = JsonConvert.SerializeObject(model);

            string proc = "Usp_MaterialShippingOrderDetail_Create";
            var param = new DynamicParameters();
            param.Add("@MSOId", MSOId);
            param.Add("@Jsonlist", jsonLotList);
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

        public async Task<ResponseModel<MaterialShippingOrderDetailDto?>> DeleteDetail(MaterialShippingOrderDetailDto model)
        {
            string proc = "Usp_MaterialShippingOrderDetail_Delete";
            var param = new DynamicParameters();
            param.Add("@MSODetailId", model.MSODetailId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<MaterialShippingOrderDetailDto?>();
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
        public async Task<ResponseModel<IEnumerable<MaterialShippingOrderLotDto>?>> GetDetailLot(MaterialShippingOrderLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialShippingOrderLotDto>?>();
                string proc = "Usp_MaterialShippingOrderLot_Get";
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
        public async Task<ResponseModel<IEnumerable<MaterialShippingOrderLotDto>?>> GetDetailLotByMSOId(long MSOId)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialShippingOrderLotDto>?>();
                string proc = "Usp_MaterialShippingOrderLot_GetByMSOId";
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
        public async Task<ResponseModel<MaterialShippingOrderLotDto?>> ScanLot(MaterialShippingOrderLotDto model)
        {
            var returnData = new ResponseModel<MaterialShippingOrderLotDto?>();

            string proc = "Usp_MaterialShippingOrderLot_Scan";
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
        public async Task<ResponseModel<MaterialShippingOrderLotDto?>> ScanReceivingLot(MaterialShippingOrderLotDto model)
        {
            var returnData = new ResponseModel<MaterialShippingOrderLotDto?>();

            string proc = "Usp_MaterialShippingOrderLot_ScanReceivingSlit";
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
        public async Task<ResponseModel<MaterialShippingOrderLotDto?>> DeleteLot(MaterialShippingOrderLotDto model)
        {
            string proc = "Usp_MaterialShippingOrderLot_Delete";
            var param = new DynamicParameters();
            param.Add("@MSODetailId", model.MSODetailId);
            param.Add("@MaterialLotId", model.MaterialLotId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<MaterialShippingOrderLotDto?>();
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
