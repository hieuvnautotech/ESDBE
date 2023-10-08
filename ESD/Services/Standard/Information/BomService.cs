using Dapper;
using Newtonsoft.Json;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Models;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Models.Validators;
using ESD.Services.Base;
using System.Data;
using static ESD.Extensions.ServiceExtensions;

namespace ESD.Services.Common.Standard.Information
{
    public interface IBomService
    {
        Task<ResponseModel<IEnumerable<BomDto>?>> GetAll(BomDto model);
        Task<ResponseModel<BomDto?>> Create(BomDto model);
        Task<ResponseModel<BomDto?>> Modify(BomDto model);
        Task<ResponseModel<BomDto?>> Delete(BomDto model);

        Task<ResponseModel<IEnumerable<BomProcessDto>?>> GetProcess(BomDto model);
        Task<ResponseModel<BomProcessDto?>> CreateProcess(BomProcessDto model);
        Task<ResponseModel<BomProcessDto?>> ModifyProcess(BomProcessDto model);
        Task<ResponseModel<BomProcessDto?>> DeleteProcess(BomProcessDto model);

        Task<ResponseModel<IEnumerable<BomProcessMaterialDto>?>> GetProcessMaterial(BomDto model);
        Task<ResponseModel<BomProcessMaterialDto?>> CreateProcessMaterial(BomProcessMaterialDto model);
        Task<ResponseModel<BomProcessMaterialDto?>> CreateProcessMaterialByExcel(List<BomProcessMaterialExcelDto> model, long userCreate);
        Task<ResponseModel<BomProcessMaterialDto?>> ModifyProcessMaterial(BomProcessMaterialDto model);
        Task<ResponseModel<BomProcessDto?>> DeleteProcessMaterial(BomProcessMaterialDto model);

        Task<ResponseModel<IEnumerable<dynamic>?>> GetAllModel();
    }
    [ScopedRegistration]
    public class BomService : IBomService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public BomService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        #region BOM
        public async Task<ResponseModel<IEnumerable<BomDto>?>> GetAll(BomDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<BomDto>?>();
                string proc = "Usp_Bom_GetAll"; var param = new DynamicParameters();
                param.Add("@ProductId", model.ProductId);
                param.Add("@isActived", model.isActived);
                param.Add("@StartDate", model.StartDate);
                param.Add("@EndDate", model.EndDate);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<BomDto>(proc, param);
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

        public async Task<ResponseModel<BomDto?>> Create(BomDto model)
        {
            var returnData = new ResponseModel<BomDto?>();

            var validator = new BomValidator();
            var validateResults = validator.Validate(model);
            if (!validateResults.IsValid)
            {
                returnData.HttpResponseCode = 400;
                returnData.ResponseMessage = validateResults.Errors[0].ToString();
                return returnData;
            }

            string proc = "Usp_Bom_Create";
            var param = new DynamicParameters();
            param.Add("@BomId", model.BomId);
            param.Add("@BuyerId", model.BuyerId);
            param.Add("@BomVersion", model.BomVersion);
            param.Add("@Ver", model.Ver);
            param.Add("@ProductId", model.ProductId);
            param.Add("@Description", model.Description);
            param.Add("@DateApply", model.DateApply);
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
                    returnData = await GetById(model.BomId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<BomDto?>> Modify(BomDto model)
        {
            var returnData = new ResponseModel<BomDto?>();

            var validator = new BomValidator();
            var validateResults = validator.Validate(model);
            if (!validateResults.IsValid)
            {
                returnData.HttpResponseCode = 400;
                returnData.ResponseMessage = validateResults.Errors[0].ToString();
                return returnData;
            }

            string proc = "Usp_Bom_Modify";
            var param = new DynamicParameters();
            param.Add("@BomId", model.BomId);
            param.Add("@BuyerId", model.BuyerId);
            param.Add("@BomVersion", model.BomVersion);
            param.Add("@Ver", model.Ver);
            param.Add("@ProductId", model.ProductId);
            param.Add("@Description", model.Description);
            param.Add("@DateApply", model.DateApply);
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
                    returnData = await GetById(model.BomId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<BomDto?>> GetById(long? BomId)
        {
            var returnData = new ResponseModel<BomDto?>();
            string proc = "Usp_Bom_GetById";
            var param = new DynamicParameters();
            param.Add("@BomId", BomId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<BomDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }

        public async Task<ResponseModel<BomDto?>> Delete(BomDto model)
        {
            string proc = "Usp_Bom_Delete";
            var param = new DynamicParameters();
            param.Add("@BomId", model.BomId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<BomDto?>();
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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetAllModel()
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_Bom_GetAllModel"; var param = new DynamicParameters();

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc);
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

        #region BOM Process
        public async Task<ResponseModel<IEnumerable<BomProcessDto>?>> GetProcess(BomDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<BomProcessDto>?>();
                string proc = "Usp_BomProcess_GetAll"; var param = new DynamicParameters();
                param.Add("@ProductCode", model.ProductCode);
                param.Add("@BuyerCode", model.BuyerCode);
                param.Add("@BomVersion", model.BomVersion);
                param.Add("@DateApply", model.DateApply);
                param.Add("@Ver", model.Ver);
                param.Add("@ProcessCode", model.ProcessCode);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<BomProcessDto>(proc, param);
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

        public async Task<ResponseModel<BomProcessDto?>> CreateProcess(BomProcessDto model)
        {
            var returnData = new ResponseModel<BomProcessDto?>();

            string proc = "Usp_BomProcess_Create";
            var param = new DynamicParameters();
            param.Add("@BomProcessId", model.BomProcessId);
            param.Add("@BomId", model.BomId);
            param.Add("@ProcessCode", model.ProcessCode);
            param.Add("@CHA", model.CHA);
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
                    returnData = await GetByIdProcess(model.BomProcessId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<BomProcessDto?>> ModifyProcess(BomProcessDto model)
        {
            var returnData = new ResponseModel<BomProcessDto?>();

            string proc = "Usp_BomProcess_Modify";
            var param = new DynamicParameters();
            param.Add("@BomProcessId", model.BomProcessId);
            param.Add("@BomId", model.BomId);
            param.Add("@ProcessCode", model.ProcessCode);
            param.Add("@CHA", model.CHA);
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
                    returnData = await GetByIdProcess(model.BomProcessId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<BomProcessDto?>> GetByIdProcess(long? BomProcessId)
        {
            var returnData = new ResponseModel<BomProcessDto?>();
            string proc = "Usp_BomProcess_GetById";
            var param = new DynamicParameters();
            param.Add("@BomProcessId", BomProcessId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<BomProcessDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }

        public async Task<ResponseModel<BomProcessDto?>> DeleteProcess(BomProcessDto model)
        {
            string proc = "Usp_BomProcess_Delete";
            var param = new DynamicParameters();
            param.Add("@BomProcessId", model.BomProcessId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<BomProcessDto?>();
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

        #region BOM Process Material
        public async Task<ResponseModel<IEnumerable<BomProcessMaterialDto>?>> GetProcessMaterial(BomDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<BomProcessMaterialDto>?>();
                string proc = "Usp_BomProcessMaterial_GetAll"; var param = new DynamicParameters();
                //param.Add("@BomProcessId", model.BomProcessId);
                param.Add("@ProductCode", model.ProductCode);
                param.Add("@BuyerCode", model.BuyerCode);
                param.Add("@BomVersion", model.BomVersion);
                param.Add("@DateApply", model.DateApply);
                param.Add("@Ver", model.Ver);
                param.Add("@Step", model.Step);
                param.Add("@BomProcessCode", model.BomProcessCode);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<BomProcessMaterialDto>(proc, param);
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

        public async Task<ResponseModel<BomProcessMaterialDto?>> CreateProcessMaterial(BomProcessMaterialDto model)
        {
            var returnData = new ResponseModel<BomProcessMaterialDto?>();

            string proc = "Usp_BomProcessMaterial_Create";
            var param = new DynamicParameters();
            param.Add("@ProcessMaterialId", model.ProcessMaterialId);
            param.Add("@BomProcessId", model.BomProcessId);
            param.Add("@MaterialCode", model.MaterialCode);
            param.Add("@CuttingSize", model.CuttingSize);
            param.Add("@BomProcessType", model.BomProcessType);
            param.Add("@Pitch", model.Pitch);
            param.Add("@Cavity", model.Cavity);
            param.Add("@RollUse", model.RollUse);
            param.Add("@Note", model.Note);

            param.Add("@ProductCode", model.ProductCode);
            param.Add("@BuyerCode", model.BuyerCode);
            param.Add("@BomVersion", model.BomVersion);
            param.Add("@DateApply", model.DateApply);
            param.Add("@Ver", model.Ver);
            param.Add("@Step", model.Step);
            param.Add("@BomProcessCode", model.BomProcessCode);

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
                    returnData = await GetByIdProcessMaterial(model.ProcessMaterialId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<BomProcessMaterialDto?>> CreateProcessMaterialByExcel(List<BomProcessMaterialExcelDto> model, long userCreate)
        {
            var returnData = new ResponseModel<BomProcessMaterialDto?>();

            var jsonLotList = JsonConvert.SerializeObject(model);

            string proc = "Usp_BomProcessMaterial_CreateByExcel";
            var param = new DynamicParameters();
            param.Add("@Jsonlist", jsonLotList);
            param.Add("@createdBy", userCreate);
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

        public async Task<ResponseModel<BomProcessMaterialDto?>> ModifyProcessMaterial(BomProcessMaterialDto model)
        {
            var returnData = new ResponseModel<BomProcessMaterialDto?>();

            string proc = "Usp_BomProcessMaterial_Modify";
            var param = new DynamicParameters();
            param.Add("@ProcessMaterialId", model.ProcessMaterialId);
            param.Add("@BomProcessId", model.BomProcessId);
            param.Add("@MaterialCode", model.MaterialCode);
            param.Add("@CuttingSize", model.CuttingSize);
            param.Add("@BomProcessType", model.BomProcessType);
            param.Add("@Pitch", model.Pitch);
            param.Add("@Cavity", model.Cavity);
            param.Add("@RollUse", model.RollUse);
            param.Add("@Note", model.Note);


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
                    returnData = await GetByIdProcessMaterial(model.ProcessMaterialId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<BomProcessMaterialDto?>> GetByIdProcessMaterial(long? ProcessMaterialId)
        {
            var returnData = new ResponseModel<BomProcessMaterialDto?>();
            string proc = "Usp_BomProcessMaterial_GetById";
            var param = new DynamicParameters();
            param.Add("@ProcessMaterialId", ProcessMaterialId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<BomProcessMaterialDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }

        public async Task<ResponseModel<BomProcessDto?>> DeleteProcessMaterial(BomProcessMaterialDto model)
        {
            string proc = "Usp_BomProcessMaterial_Delete";
            var param = new DynamicParameters();
            param.Add("@ProcessMaterialId", model.ProcessMaterialId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<BomProcessDto?>();
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
    }
}
