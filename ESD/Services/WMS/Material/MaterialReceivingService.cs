using Dapper;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Helpers;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Models.Dtos.PO;
using Newtonsoft.Json;
using System.Data;
using static ESD.Extensions.ServiceExtensions;

namespace ESD.Services.WMS.Material
{
    public interface IMaterialReceivingService
    {
        Task<ResponseModel<IEnumerable<dynamic>?>> GetPO(PageModel model, string? POOrderCode, string? MaterialCode);
        Task<ResponseModel<IEnumerable<MaterialReceivingDto>?>> GetLotNo(MaterialReceivingDto model);
        Task<ResponseModel<IEnumerable<MaterialReceivingDto>?>> GetAll(MaterialReceivingDto model);
        Task<ResponseModel<MaterialReceivingDto?>> Create(MaterialReceivingDto model);
        Task<ResponseModel<MaterialReceivingDto?>> GetById(long? MaterialReceiveId);
        Task<ResponseModel<MaterialReceivingDto?>> Modify(MaterialReceivingDto model);
        Task<ResponseModel<MaterialReceivingDto?>> Delete(MaterialReceivingDto model);
        Task<ResponseModel<MaterialReceivingDto?>> AddLot(MaterialReceivingDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetForSelectByIQCType(long? IQCType);
        Task<ResponseModel<IEnumerable<MaterialLotDto>?>> GetMaterialLotAll(MaterialLotDto model);
        Task<ResponseModel<MaterialLotDto?>> DeleteLot(MaterialLotDto model);
        Task<ResponseModel<IEnumerable<MaterialLotDto>?>> GetMaterialReceivingLotAll(MaterialLotDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetMaterialReceivingLotAllPrint(MaterialLotDto model);

        Task<ResponseModel<IEnumerable<QCIQCDetailMDto>?>> GetDetailMaterial(long? QCIQCMasterId, long? MaterialLotId);
        Task<ResponseModel<CheckMaterialLotDto?>> CreateFormMaterial(CheckMaterialLotDto model);
        Task<ResponseModel<CheckMaterialLotDto?>> CreateFormSUSMaterial(CheckMaterialLotDto model);

        Task<ResponseModel<IEnumerable<QCIQCDetailRMDto>>> GetDetailRawMaterial(long? QCIQCMasterId, long? MaterialLotId);
        Task<ResponseModel<CheckRawMaterialLotDto?>> CreateRawMaterial(CheckRawMaterialLotDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetAllDetailId(long? MaterialReceivingId);

        Task<ResponseModel<IEnumerable<dynamic>?>> GetListPrintQR(List<long>? listQR);
        Task<ResponseModel<IEnumerable<dynamic>?>> PrintMaterialLot(List<long>? listQR);
    }
    [ScopedRegistration]
    public class MaterialReceivingService : IMaterialReceivingService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public MaterialReceivingService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        #region  IQC AND Receiving
        public async Task<ResponseModel<IEnumerable<MaterialReceivingDto>?>> GetLotNo(MaterialReceivingDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialReceivingDto>?>();
                string proc = "Usp_MaterialReceiving_GetLotNo";
                var param = new DynamicParameters();
                param.Add("@LotNo", model.LotNo);
                param.Add("@MaterialCode", model.MaterialCode);
                param.Add("@StartDate", model.StartDate);
                param.Add("@EndDate", model.EndDate);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MaterialReceivingDto>(proc, param);
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
        public async Task<ResponseModel<IEnumerable<MaterialReceivingDto>?>> GetAll(MaterialReceivingDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialReceivingDto>?>();
                string proc = "Usp_MaterialReceiving_GetAll";
                var param = new DynamicParameters();
                param.Add("@ReceivedDate", model.ReceivedDate);
                param.Add("@MaterialId", model.MaterialId);
                param.Add("@LotNo", model.LotNo);
                param.Add("@MaterialCode", model.MaterialCode);
                param.Add("@StartDate", model.StartDate);
                param.Add("@EndDate", model.EndDate);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MaterialReceivingDto>(proc, param);
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
        public async Task<ResponseModel<MaterialReceivingDto?>> Create(MaterialReceivingDto model)
        {
            try
            {
                var returnData = new ResponseModel<MaterialReceivingDto?>();

                string proc = "Usp_MaterialReceiving_Create";
                var param = new DynamicParameters();
                param.Add("@MaterialId", model.MaterialId);
                param.Add("@POId", model.POId);
                param.Add("@LotNo", model.LotNo);
                param.Add("@MaterialType", model.MaterialType);
                param.Add("@IQCCheck", model.IQCCheck);
                param.Add("@QCIQCMasterId", model.QCIQCMasterId);
                param.Add("@QuantityBundle", model.QuantityBundle);
                param.Add("@QuantityInBundle", model.QuantityInBundle);
                param.Add("@ManufactureDate", model.ManufactureDate);
                param.Add("@ReceivedDate", model.ReceivedDate);
                param.Add("@ExportDate", model.ExportDate);
                param.Add("@ExpirationDate", model.ExpirationDate);
                param.Add("@Width", model.Width);
                param.Add("@Length", model.Length);
                param.Add("@Description", model.Description);
                param.Add("@CutSlit", model.CutSlit);
                param.Add("@CuttingDate", model.CuttingDate);
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
                        //returnData = await GetById(model.MaterialReceiveId);
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
        public async Task<ResponseModel<MaterialReceivingDto?>> GetById(long? MaterialReceiveId)
        {
            var returnData = new ResponseModel<MaterialReceivingDto?>();
            string proc = "Usp_MaterialReceiving_GetById";
            var param = new DynamicParameters();
            param.Add("@MaterialReceiveId", MaterialReceiveId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MaterialReceivingDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }
        public async Task<ResponseModel<MaterialReceivingDto?>> Modify(MaterialReceivingDto model)
        {
            var returnData = new ResponseModel<MaterialReceivingDto?>();

            string proc = "Usp_MaterialReceiving_Modify";
            var param = new DynamicParameters();
            param.Add("@MaterialReceiveId", model.MaterialReceiveId);
            param.Add("@MaterialId", model.MaterialId);
            param.Add("@MaterialType", model.MaterialType);
            param.Add("@IQCCheck", model.IQCCheck);
            param.Add("@QCIQCMasterId", model.QCIQCMasterId);
            param.Add("@ManufactureDate", model.ManufactureDate);
            param.Add("@ReceivedDate", model.ReceivedDate);
            param.Add("@ExportDate", model.ExportDate);
            param.Add("@ExpirationDate", model.ExpirationDate);
            param.Add("@Width", model.Width);
            param.Add("@Length", model.Length);
            param.Add("@Description", model.Description);
            //.Add("@CuttingDate", model.CuttingDate);
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
                    returnData = await GetById(model.MaterialReceiveId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    returnData.ResponseMessage = result;
                    break;
            }
            return returnData;
        }
        public async Task<ResponseModel<MaterialReceivingDto?>> Delete(MaterialReceivingDto model)
        {
            string proc = "Usp_MaterialReceiving_Delete";
            var param = new DynamicParameters();
            param.Add("@MaterialReceiveId", model.MaterialReceiveId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<MaterialReceivingDto?>();
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
        public async Task<ResponseModel<MaterialLotDto?>> DeleteLot(MaterialLotDto model)
        {
            string proc = "Usp_MaterialReceiving_DeleteLot";
            var param = new DynamicParameters();
            param.Add("@MaterialLotId", model.MaterialLotId);
            param.Add("@MaterialLotCode", model.MaterialLotCode);
            param.Add("@MaterialReceiveId", model.MaterialReceiveId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<MaterialLotDto?>();
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
        public async Task<ResponseModel<MaterialReceivingDto?>> AddLot(MaterialReceivingDto model)
        {
            try
            {
                var returnData = new ResponseModel<MaterialReceivingDto?>();

                string proc = "Usp_MaterialReceiving_AddLot";
                var param = new DynamicParameters();
                param.Add("@MaterialReceiveId", model.MaterialReceiveId);
                param.Add("@QuantityInBundle", model.QuantityInBundle);
                param.Add("@Length", model.Length);
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
                        returnData = await GetById(model.MaterialReceiveId);
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
        public async Task<ResponseModel<IEnumerable<MaterialLotDto>?>> GetMaterialLotAll(MaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialLotDto>?>();
                string proc = "Usp_MaterialLot_GetAll"; var param = new DynamicParameters();
                param.Add("@MaterialReceiveId", model.MaterialReceiveId);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MaterialLotDto>(proc, param);
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
        public async Task<ResponseModel<IEnumerable<MaterialLotDto>?>> GetMaterialReceivingLotAll(MaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialLotDto>?>();
                string proc = "Usp_MaterialReceivingLot_GetAll"; var param = new DynamicParameters();
                param.Add("@MaterialReceiveId", model.MaterialReceiveId);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<MaterialLotDto>(proc, param);
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
        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetMaterialReceivingLotAllPrint(MaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_MaterialReceiving_PrintAllLot"; var param = new DynamicParameters();
                param.Add("@ReceivedDate", model.ReceivedDate);
                param.Add("@MaterialId", model.MaterialId);
                param.Add("@LotNo", model.LotNo);
                param.Add("@isActived", model.isActived);

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
        #endregion

        #region Check Material
        public async Task<ResponseModel<IEnumerable<QCIQCDetailMDto>?>> GetDetailMaterial(long? QCIQCMasterId, long? MaterialLotId)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<QCIQCDetailMDto>?>();
                string proc = "Usp_MaterialReceivingLot_GetCheck";
                var param = new DynamicParameters();
                param.Add("@QCIQCMasterId", QCIQCMasterId);
                param.Add("@MaterialLotId", MaterialLotId);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCIQCDetailMDto>(proc, param);
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
        public async Task<ResponseModel<CheckMaterialLotDto?>> CreateFormMaterial(CheckMaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<CheckMaterialLotDto?>();

                var jsonLotList = JsonConvert.SerializeObject(model.CheckValue);

                string proc = "Usp_MaterialReceivingLot_Check";
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

        public async Task<ResponseModel<CheckMaterialLotDto?>> CreateFormSUSMaterial(CheckMaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<CheckMaterialLotDto?>();

                var jsonLotList = JsonConvert.SerializeObject(model.CheckValue);

                string proc = "Usp_MaterialReceivingLot_CheckSUS";
                var param = new DynamicParameters();
                param.Add("@MaterialLotId", model.MaterialLotId);
                param.Add("@QCIQCMasterId", model.QCIQCMasterId);
                param.Add("@StaffId", model.StaffId);
                param.Add("@CheckDate", model.CheckDate);
                param.Add("@Jsonlist", jsonLotList);
                param.Add("@TotalQty", model.TotalQty);
                param.Add("@NGQty", model.NGQty);
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

        #region Check Raw Material
        public async Task<ResponseModel<IEnumerable<QCIQCDetailRMDto>>> GetDetailRawMaterial(long? QCIQCMasterId, long? MaterialLotId)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<QCIQCDetailRMDto>>();
                string proc = "Usp_MaterialReceivingLot_GetCheckRaw";
                var param = new DynamicParameters();
                param.Add("@QCIQCMasterId", QCIQCMasterId);
                param.Add("@MaterialLotId", MaterialLotId);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCIQCDetailRMDto>(proc, param);
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
        public async Task<ResponseModel<CheckRawMaterialLotDto?>> CreateRawMaterial(CheckRawMaterialLotDto model)
        {
            var returnData = new ResponseModel<CheckRawMaterialLotDto?>();

            var jsonLotList = JsonConvert.SerializeObject(model.CheckValue);
            string proc = "Usp_MaterialReceivingLot_CheckRaw";
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


        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetPO(PageModel model, string? POOrderCode, string? MaterialCode)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_MaterialReceiving_GetPO"; 
                var param = new DynamicParameters();
                param.Add("@POOrderCode", POOrderCode);
                param.Add("@MaterialCode", MaterialCode);
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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetForSelectByIQCType(long? IQCType)
        {
            var returnData = new ResponseModel<IEnumerable<dynamic>?>();
            var proc = $"Usp_QCIQCMaster_GetByIQCType";
            var param = new DynamicParameters();
            param.Add("@IQCType", IQCType);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);

            returnData.Data = data;
            if (!data.Any())
            {
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                returnData.HttpResponseCode = 204;
            }

            return returnData;
        }
        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetAllDetailId(long? MaterialReceivingId)
        {
            var returnData = new ResponseModel<IEnumerable<dynamic>?>();
            var proc = $"Usp_MaterialReceiving_GetAllDetailId";
            var param = new DynamicParameters();
            param.Add("@MaterialReceivingId", MaterialReceivingId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);

            returnData.Data = data;
            if (!data.Any())
            {
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                returnData.HttpResponseCode = 204;
            }

            return returnData;
        }
        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetListPrintQR(List<long>? listQR)
        {
            var returnData = new ResponseModel<IEnumerable<dynamic>?>();
            var proc = $"Usp_MaterialReceiving_PrintLabels";
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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> PrintMaterialLot(List<long>? listQR)
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
