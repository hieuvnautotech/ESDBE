using Dapper;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Models.Dtos.Slit;
using Newtonsoft.Json;
using System.Data;
using static ESD.Extensions.ServiceExtensions;

namespace ESD.Services.Slit
{
    public interface ISlitOrderService
    {
        Task<ResponseModel<IEnumerable<SlitOrderDto>?>> GetAll(SlitOrderDto model);
        Task<ResponseModel<SlitOrderDto?>> GetById(long? QCPQCMasterId);
        Task<ResponseModel<SlitOrderDto?>> Create(SlitOrderDto model);
        Task<ResponseModel<SlitOrderDto?>> Modify(SlitOrderDto model);
        Task<ResponseModel<SlitOrderDto?>> Delete(SlitOrderDto model);

        Task<ResponseModel<IEnumerable<SlitOrderDetailDto>?>> GetSlitOrderDetail(SlitOrderDetailDto model);
        Task<ResponseModel<SlitOrderDetailDto?>> CreateSlitOrderDetail(SlitOrderDetailDto model);
        Task<ResponseModel<SlitOrderDetailDto?>> ModifySlitOrderDetail(SlitOrderDetailDto model);
        Task<ResponseModel<SlitOrderDetailDto?>> DeleteSlitOrderDetail(SlitOrderDetailDto model);

        Task<ResponseModel<SlitTurnDto?>> Slit(SlitTurnDto model);
        Task<ResponseModel<IEnumerable<SlitTurnDto>?>> GetSlitRaw(SlitTurnDto model);
        Task<ResponseModel<IEnumerable<SlitTurnDto>?>> GetSlitDetailRaw(SlitTurnDto model);
        Task<ResponseModel<IEnumerable<SlitTurnDto>?>> GetSlit(SlitTurnDto model);
        Task<ResponseModel<IEnumerable<SlitTurnDto>?>> GetSlitDetail(SlitTurnDto model);
        Task<ResponseModel<SlitTurnDto?>> ResetSlitTurn(SlitTurnDto model);
        Task<ResponseModel<SlitTurnDto?>> FinishSlitTurn(SlitTurnDto model);
        Task<ResponseModel<SlitTurnDto?>> EditSlitTurn(SlitTurnDto model);
        Task<ResponseModel<SlitTurnDto?>> DeleteSlitTurn(SlitTurnDto model);
        Task<ResponseModel<CheckMaterialLotDto?>> CheckIQC(CheckMaterialLotDto model);

        Task<ResponseModel<IEnumerable<SlitTurnDto>?>> GetRawLotMaterial(long? SlitOrderId);
        Task<ResponseModel<IEnumerable<ProductDto>?>> GetProductForLot(long? SlitOrderId, long? MaterialId);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetTurn(long? SlitOrderId, long? MaterialLotId);
    }
    [ScopedRegistration]
    public class SlitOrderService : ISlitOrderService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public SlitOrderService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        #region Slit order master
        public async Task<ResponseModel<IEnumerable<SlitOrderDto>?>> GetAll(SlitOrderDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<SlitOrderDto>?>();
                string proc = "Usp_SlitOrder_GetAll";
                var param = new DynamicParameters();
                param.Add("@StartDate", model.StartDate);
                param.Add("@EndDate", model.EndDate);
                param.Add("@ProductId", model.ProductId);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<SlitOrderDto>(proc, param);
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
        public async Task<ResponseModel<SlitOrderDto?>> GetById(long? SlitOrderId)
        {
            var returnData = new ResponseModel<SlitOrderDto?>();
            string proc = "Usp_SlitOrder_GetById";
            var param = new DynamicParameters();
            param.Add("@SlitOrderId", SlitOrderId);
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<SlitOrderDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }
        public async Task<ResponseModel<SlitOrderDto?>> Create(SlitOrderDto model)
        {
            var returnData = new ResponseModel<SlitOrderDto?>();

            string proc = "Usp_SlitOrder_Create";
            var param = new DynamicParameters();
            param.Add("@SlitOrderId", model.SlitOrderId);
            param.Add("@OrderDate", model.OrderDate);
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
                    returnData = await GetById(model.SlitOrderId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    returnData.ResponseMessage = result;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<SlitOrderDto?>> Modify(SlitOrderDto model)
        {
            var returnData = new ResponseModel<SlitOrderDto?>();

            string proc = "Usp_SlitOrder_Modify";
            var param = new DynamicParameters();
            param.Add("@SlitOrderId", model.SlitOrderId);
            param.Add("@OrderDate", model.OrderDate);
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
                    returnData = await GetById(model.SlitOrderId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }
        public async Task<ResponseModel<SlitOrderDto?>> Delete(SlitOrderDto model)
        {
            string proc = "Usp_SlitOrder_Delete";
            var param = new DynamicParameters();
            param.Add("@SlitOrderId", model.SlitOrderId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<SlitOrderDto?>();
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
        #endregion

        #region Slit Order Detail
        public async Task<ResponseModel<IEnumerable<SlitOrderDetailDto>?>> GetSlitOrderDetail(SlitOrderDetailDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<SlitOrderDetailDto>?>();
                string proc = "Usp_SlitOrderDetail_GetAll"; var param = new DynamicParameters();
                param.Add("@SlitOrderId", model.SlitOrderId);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<SlitOrderDetailDto>(proc, param);
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

        public async Task<ResponseModel<SlitOrderDetailDto?>> CreateSlitOrderDetail(SlitOrderDetailDto model)
        {
            var returnData = new ResponseModel<SlitOrderDetailDto?>();

            string proc = "Usp_SlitOrderDetail_Create";
            var param = new DynamicParameters();
            param.Add("@SlitOrderDetailId", model.SlitOrderDetailId);
            param.Add("@SlitOrderId", model.SlitOrderId);
            param.Add("@ProductId", model.ProductId);
            param.Add("@MaterialId", model.MaterialId);
            param.Add("@Width", model.Width);
            param.Add("@Length", model.Length);
            param.Add("@OrderQty", model.OrderQty);
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
                    returnData = await GetByIdSlitOrderDetail(model.SlitOrderDetailId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<SlitOrderDetailDto?>> ModifySlitOrderDetail(SlitOrderDetailDto model)
        {
            var returnData = new ResponseModel<SlitOrderDetailDto?>();

            string proc = "Usp_SlitOrderDetail_Modify";
            var param = new DynamicParameters();
            param.Add("@SlitOrderDetailId", model.SlitOrderDetailId);
            param.Add("@SlitOrderId", model.SlitOrderId);
            param.Add("@ProductId", model.ProductId);
            param.Add("@MaterialId", model.MaterialId);
            param.Add("@Width", model.Width);
            param.Add("@Length", model.Length);
            param.Add("@OrderQty", model.OrderQty);
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
                    returnData = await GetByIdSlitOrderDetail(model.SlitOrderDetailId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<SlitOrderDetailDto?>> GetByIdSlitOrderDetail(long? SlitOrderDetailId)
        {
            var returnData = new ResponseModel<SlitOrderDetailDto?>();
            string proc = "Usp_SlitOrderDetail_GetById";
            var param = new DynamicParameters();
            param.Add("@SlitOrderDetailId", SlitOrderDetailId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<SlitOrderDetailDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }

        public async Task<ResponseModel<SlitOrderDetailDto?>> DeleteSlitOrderDetail(SlitOrderDetailDto model)
        {
            string proc = "Usp_SlitOrderDetail_Delete";
            var param = new DynamicParameters();
            param.Add("@SlitOrderDetailId", model.SlitOrderDetailId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<SlitOrderDetailDto?>();
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

        #region Slit Turn
        public async Task<ResponseModel<SlitTurnDto?>> Slit(SlitTurnDto model)
        {
            var returnData = new ResponseModel<SlitTurnDto?>();

            string staffs = "";
            foreach (var item in model.StaffIds)
            {
                if (item != model.StaffIds[0])
                    staffs += ",";
                staffs += item.StaffId;
            }

            string proc = "Usp_SlitTurn_Slit";
            var param = new DynamicParameters();
            param.Add("@SlitOrderId", model.SlitOrderId);
            param.Add("@MaterialLotId", model.MaterialLotId);
            param.Add("@ProductId", model.ProductId);
            param.Add("@StaffId", staffs);
            param.Add("@LineId", model.LineId);
            param.Add("@BladeId", model.BladeId);
            param.Add("@Turn", model.Turn);
            param.Add("@Width", model.Width);
            param.Add("@Length", model.Length);
            param.Add("@Quantity", model.SlitQty);
            param.Add("@LossWidth", model.LossWidth);
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
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    returnData.ResponseMessage = result;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<IEnumerable<SlitTurnDto>?>> GetSlitRaw(SlitTurnDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<SlitTurnDto>?>();
                string proc = "Usp_SlitTurn_GetTurnRaw";
                var param = new DynamicParameters();
                param.Add("@SlitOrderId", model.SlitOrderId);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<SlitTurnDto>(proc, param);
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

        public async Task<ResponseModel<IEnumerable<SlitTurnDto>?>> GetSlitDetailRaw(SlitTurnDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<SlitTurnDto>?>();
                string proc = "Usp_SlitTurn_GetTurnSlitRaw";
                var param = new DynamicParameters();
                param.Add("@SlitOrderId", model.SlitOrderId);
                param.Add("@MaterialLotId", model.MaterialLotId);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<SlitTurnDto>(proc, param);
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

        public async Task<ResponseModel<IEnumerable<SlitTurnDto>?>> GetSlit(SlitTurnDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<SlitTurnDto>?>();
                string proc = "Usp_SlitTurn_GetTurn";
                var param = new DynamicParameters();
                param.Add("@SlitOrderId", model.SlitOrderId);
                param.Add("@ParentId", model.ParentId);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<SlitTurnDto>(proc, param);
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

        public async Task<ResponseModel<IEnumerable<SlitTurnDto>?>> GetSlitDetail(SlitTurnDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<SlitTurnDto>?>();
                string proc = "Usp_SlitTurn_GetTurnSlit";
                var param = new DynamicParameters();
                param.Add("@SlitOrderId", model.SlitOrderId);
                param.Add("@Turn", model.Turn);
                param.Add("@ParentId", model.ParentId);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<SlitTurnDto>(proc, param);
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

        public async Task<ResponseModel<SlitTurnDto?>> FinishSlitTurn(SlitTurnDto model)
        {
            string proc = "Usp_SlitTurn_Finish";
            var param = new DynamicParameters();
            param.Add("@SlitOrderId", model.SlitOrderId);
            param.Add("@MaterialLotId", model.MaterialLotId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<SlitTurnDto?>();
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

        public async Task<ResponseModel<SlitTurnDto?>> ResetSlitTurn(SlitTurnDto model)
        {
            string proc = "Usp_SlitTurn_Reset";
            var param = new DynamicParameters();
            param.Add("@SlitOrderId", model.SlitOrderId);
            param.Add("@MaterialLotId", model.MaterialLotId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<SlitTurnDto?>();
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

        public async Task<ResponseModel<SlitTurnDto?>> EditSlitTurn(SlitTurnDto model)
        {
            string proc = "Usp_SlitTurn_EditLength";
            var param = new DynamicParameters();
            param.Add("@MaterialLotId", model.MaterialLotId);
            param.Add("@Length", model.Length);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<SlitTurnDto?>();
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

        public async Task<ResponseModel<SlitTurnDto?>> DeleteSlitTurn(SlitTurnDto model)
        {
            string proc = "Usp_SlitTurn_Delete";
            var param = new DynamicParameters();
            param.Add("@SlitTurnId", model.SlitTurnId);
            param.Add("@MaterialLotId", model.MaterialLotId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<SlitTurnDto?>();
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

        public async Task<ResponseModel<CheckMaterialLotDto?>> CheckIQC(CheckMaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<CheckMaterialLotDto?>();

                var jsonLotList = JsonConvert.SerializeObject(model.CheckValue);

                string proc = "Usp_SlitTurn_CheckIQC";
                var param = new DynamicParameters();
                param.Add("@MaterialLotId", model.MaterialLotId);
                param.Add("@QCIQCMasterId", model.QCIQCMasterId);
                param.Add("@StaffId", model.StaffId);
                param.Add("@CheckDate", model.CheckDate);
                param.Add("@CheckResult", model.CheckResult);
                param.Add("@Jsonlist", jsonLotList);
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
                        //returnData = await GetById(model.LotCheckMasterId);
                        returnData.ResponseMessage = result;
                        break;
                    default:
                        returnData.HttpResponseCode = 400;
                        returnData.ResponseMessage = result;

                        break;
                }
                return returnData;
            }
            catch (Exception e)
            {

                throw;
            }

        }
        #endregion
        public async Task<ResponseModel<IEnumerable<SlitTurnDto>?>> GetRawLotMaterial(long? SlitOrderId)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<SlitTurnDto>?>();
                string proc = "Usp_SlitOrder_GetLotMaterial";
                var param = new DynamicParameters();
                param.Add("@SlitOrderId", SlitOrderId);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<SlitTurnDto>(proc, param);
                returnData.Data = data;
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
        public async Task<ResponseModel<IEnumerable<ProductDto>?>> GetProductForLot(long? SlitOrderId, long? MaterialId)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<ProductDto>?>();
                string proc = "Usp_SlitOrder_GetProduct";
                var param = new DynamicParameters();
                param.Add("@SlitOrderId", SlitOrderId);
                param.Add("@MaterialId", MaterialId);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<ProductDto>(proc, param);
                returnData.Data = data;
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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetTurn(long? SlitOrderId, long? MaterialLotId)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_SlitOrder_GetTurn";
                var param = new DynamicParameters();
                param.Add("@SlitOrderId", SlitOrderId);
                param.Add("@MaterialLotId", MaterialLotId);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);
                returnData.Data = data;
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
    }
}
