using Dapper;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Models.Dtos.Common;
using ESD.Models.Dtos.Slit;
using Newtonsoft.Json;
using System.Data;
using static ESD.Extensions.ServiceExtensions;

namespace ESD.Services.Slit
{
    public interface ISlitSOService
    {
        Task<ResponseModel<IEnumerable<SlitShippingOrderDto>?>> GetAll(SlitShippingOrderDto model);
        Task<ResponseModel<SlitShippingOrderDto?>> GetById(long? QCPQCMasterId);
        Task<ResponseModel<SlitShippingOrderDto?>> Create(SlitShippingOrderDto model);
        Task<ResponseModel<SlitShippingOrderDto?>> Modify(SlitShippingOrderDto model);
        Task<ResponseModel<SlitShippingOrderDto?>> Delete(SlitShippingOrderDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetMaterial(BaseModel model, string MaterialCode, string ProductCode, long SlitSOId);

        Task<ResponseModel<IEnumerable<SlitShippingOrderDetailDto>?>> GetDetail(SlitShippingOrderDetailDto model);
        Task<ResponseModel<IEnumerable<SlitShippingOrderDetailDto>?>> GetDetailHistory(SlitShippingOrderDetailDto model);
        Task<ResponseModel<SlitShippingOrderDetailDto?>> CreateDetail(List<SlitShippingOrderDetailDto> model, long SlitSOId, long UserCreate);
        Task<ResponseModel<SlitShippingOrderDetailDto?>> DeleteDetail(SlitShippingOrderDetailDto model);

        Task<ResponseModel<IEnumerable<SlitShippingOrderLotDto>?>> GetDetailLot(SlitShippingOrderLotDto model);
        Task<ResponseModel<IEnumerable<SlitShippingOrderLotDto>?>> GetDetailLotBySlitSOId(long SlitSOId);
        Task<ResponseModel<SlitShippingOrderLotDto?>> ScanLot(SlitShippingOrderLotDto model);
        //Task<ResponseModel<SlitShippingOrderLotDto?>> ScanReceivingLot(SlitShippingOrderLotDto model);
        Task<ResponseModel<SlitShippingOrderLotDto?>> DeleteLot(SlitShippingOrderLotDto model);
    }
    [ScopedRegistration]
    public class SlitSOService : ISlitSOService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public SlitSOService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        #region Master
        public async Task<ResponseModel<IEnumerable<SlitShippingOrderDto>?>> GetAll(SlitShippingOrderDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<SlitShippingOrderDto>?>();
                string proc = "Usp_SlitShippingOrder_GetAll";
                var param = new DynamicParameters();
                param.Add("@Keyword", model.SlitSOName);
                param.Add("@Description", model.Description);
                param.Add("@ProductId", model.ProductId);
                param.Add("@LocationName", model.LocationName);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<SlitShippingOrderDto>(proc, param);
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

        public async Task<ResponseModel<SlitShippingOrderDto?>> Create(SlitShippingOrderDto model)
        {
            var returnData = new ResponseModel<SlitShippingOrderDto?>();

            string proc = "Usp_SlitShippingOrder_Create";
            var param = new DynamicParameters();
            param.Add("@SlitSOId", model.SlitSOId);
            param.Add("@SlitSOName", model.SlitSOName);
            param.Add("@ProductId", model.ProductId);
            param.Add("@WOId", model.WOId);
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
                    returnData = await GetById(model.SlitSOId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<SlitShippingOrderDto?>> Modify(SlitShippingOrderDto model)
        {
            var returnData = new ResponseModel<SlitShippingOrderDto?>();

            string proc = "Usp_SlitShippingOrder_Modify";
            var param = new DynamicParameters();
            param.Add("@SlitSOId", model.SlitSOId);
            param.Add("@SlitSOName", model.SlitSOName);
            param.Add("@ProductId", model.ProductId);
            param.Add("@WOId", model.WOId);
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
                    returnData = await GetById(model.SlitSOId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<SlitShippingOrderDto?>> GetById(long? SlitSOId)
        {
            var returnData = new ResponseModel<SlitShippingOrderDto?>();
            string proc = "Usp_SlitShippingOrder_GetById";
            var param = new DynamicParameters();
            param.Add("@SlitSOId", SlitSOId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<SlitShippingOrderDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }

        public async Task<ResponseModel<SlitShippingOrderDto?>> Delete(SlitShippingOrderDto model)
        {
            string proc = "Usp_SlitShippingOrder_Delete";
            var param = new DynamicParameters();
            param.Add("@SlitSOId", model.SlitSOId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<SlitShippingOrderDto?>();
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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetMaterial(BaseModel model, string MaterialCode, string ProductCode, long SlitSOId)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_SlitShippingOrder_GetMaterial";
                var param = new DynamicParameters();
                param.Add("@SlitSOId", SlitSOId);
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
        public async Task<ResponseModel<IEnumerable<SlitShippingOrderDetailDto>?>> GetDetail(SlitShippingOrderDetailDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<SlitShippingOrderDetailDto>?>();
                string proc = "Usp_SlitShippingOrderDetail_Get";
                var param = new DynamicParameters();
                param.Add("@SlitSOId", model.SlitSOId);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<SlitShippingOrderDetailDto>(proc, param);
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

        public async Task<ResponseModel<IEnumerable<SlitShippingOrderDetailDto>?>> GetDetailHistory(SlitShippingOrderDetailDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<SlitShippingOrderDetailDto>?>();
                string proc = "Usp_SlitShippingOrderDetail_GetHistory";
                var param = new DynamicParameters();
                param.Add("@SlitSOId", model.SlitSOId);
                param.Add("@MaterialId", model.MaterialId);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<SlitShippingOrderDetailDto>(proc, param);
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

        //public async Task<ResponseModel<SlitShippingOrderDetailDto?>> GetDetailSingle(long? SlitSOId, long? MaterialId)
        //{
        //    var returnData = new ResponseModel<SlitShippingOrderDetailDto?>();
        //    string proc = "Usp_SlitShippingOrderDetail_GetSingleMaterial";
        //    var param = new DynamicParameters();
        //    param.Add("@SlitSOId", SlitSOId);
        //    param.Add("@MaterialId", MaterialId);

        //    var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<SlitShippingOrderDetailDto>(proc, param);
        //    returnData.Data = data.FirstOrDefault();
        //    if (!data.Any())
        //    {
        //        returnData.HttpResponseCode = 204;
        //        returnData.ResponseMessage = "NO DATA";
        //    }
        //    return returnData;
        //}

        public async Task<ResponseModel<SlitShippingOrderDetailDto?>> CreateDetail(List<SlitShippingOrderDetailDto> model, long SlitSOId, long UserCreate)
        {
            var returnData = new ResponseModel<SlitShippingOrderDetailDto?>();


            var jsonLotList = JsonConvert.SerializeObject(model);

            string proc = "Usp_SlitShippingOrderDetail_Create";
            var param = new DynamicParameters();
            param.Add("@SlitSOId", SlitSOId);
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

        public async Task<ResponseModel<SlitShippingOrderDetailDto?>> DeleteDetail(SlitShippingOrderDetailDto model)
        {
            string proc = "Usp_SlitShippingOrderDetail_Delete";
            var param = new DynamicParameters();
            param.Add("@SlitSODetailId", model.SlitSODetailId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<SlitShippingOrderDetailDto?>();
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
        public async Task<ResponseModel<IEnumerable<SlitShippingOrderLotDto>?>> GetDetailLot(SlitShippingOrderLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<SlitShippingOrderLotDto>?>();
                string proc = "Usp_SlitShippingOrderLot_Get";
                var param = new DynamicParameters();
                param.Add("@SlitSODetailId", model.SlitSODetailId);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<SlitShippingOrderLotDto>(proc, param);
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
        public async Task<ResponseModel<IEnumerable<SlitShippingOrderLotDto>?>> GetDetailLotBySlitSOId(long SlitSOId)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<SlitShippingOrderLotDto>?>();
                string proc = "Usp_SlitShippingOrderLot_GetBySlitSOId";
                var param = new DynamicParameters();
                param.Add("@SlitSOId", SlitSOId);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<SlitShippingOrderLotDto>(proc, param);
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
        public async Task<ResponseModel<SlitShippingOrderLotDto?>> ScanLot(SlitShippingOrderLotDto model)
        {
            var returnData = new ResponseModel<SlitShippingOrderLotDto?>();

            string proc = "Usp_SlitShippingOrderLot_Scan";
            var param = new DynamicParameters();
            param.Add("@SlitSODetailId", model.SlitSODetailId);
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
        //public async Task<ResponseModel<SlitShippingOrderLotDto?>> ScanReceivingLot(SlitShippingOrderLotDto model)
        //{
        //    var returnData = new ResponseModel<SlitShippingOrderLotDto?>();

        //    string proc = "Usp_SlitShippingOrderLot_ScanReceivingSlit";
        //    var param = new DynamicParameters();
        //    param.Add("@SlitSODetailId", model.SlitSODetailId);
        //    param.Add("@MaterialLotCode", model.MaterialLotCode);
        //    param.Add("@createdBy", model.createdBy);
        //    param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);

        //    var result = await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);

        //    returnData.ResponseMessage = result;
        //    switch (result)
        //    {
        //        case StaticReturnValue.SYSTEM_ERROR:
        //            returnData.HttpResponseCode = 500;
        //            break;
        //        case StaticReturnValue.SUCCESS:
        //            returnData.HttpResponseCode = 200;
        //            returnData.ResponseMessage = result;
        //            break;
        //        default:
        //            returnData.HttpResponseCode = 400;
        //            break;
        //    }
        //    return returnData;
        //}
        public async Task<ResponseModel<SlitShippingOrderLotDto?>> DeleteLot(SlitShippingOrderLotDto model)
        {
            string proc = "Usp_SlitShippingOrderLot_Delete";
            var param = new DynamicParameters();
            param.Add("@SlitSODetailId", model.SlitSODetailId);
            param.Add("@MaterialLotId", model.MaterialLotId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<SlitShippingOrderLotDto?>();
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
