using ESD.Models.Dtos.Common;
using ESD.Models.Dtos;
using static ESD.Extensions.ServiceExtensions;
using ESD.DbAccess;
using Dapper;
using ESD.Extensions;
using System.Data;
using ESD.Models.Dtos.MMS;
using Newtonsoft.Json;
using System.Collections.Generic;
using ESD.Helpers;

namespace ESD.Services.MMS
{
    public interface IWOService
    {
        #region WO and WO PROCESS (CRUD)
        Task<ResponseModel<IEnumerable<WODto>?>> GetAll(WODto model);
        Task<ResponseModel<WODto?>> Create(WODto model);
        Task<ResponseModel<WODto?>> Modify(WODto model);
        Task<ResponseModel<WODto?>> Delete(WODto model);
        Task<ResponseModel<WODto?>> Finish(WODto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetSelectBOM(long? ProductId);
        Task<ResponseModel<IEnumerable<WOProcessDto>?>> GetProcess(WOProcessDto model);
        Task<ResponseModel<WOProcessDto?>> CreateProcess(WOProcessDto model);
        Task<ResponseModel<WOProcessDto?>> ModifyProcess(WOProcessDto model);
        Task<ResponseModel<WOProcessDto?>> DeleteProcess(WOProcessDto model);
        #endregion

        #region checkPQC
        Task<ResponseModel<IEnumerable<PQCWOProcessDetailASDto>?>> GetValueCheck(long? QCPQCMasterId, long? WOProcessId);
        Task<ResponseModel<PQCWOProcessCheckMasterASDto?>> CheckProcessPQC(PQCWOProcessCheckMasterASDto model);
        #endregion

        #region ProcessMold
        Task<ResponseModel<IEnumerable<WOProcessMoldDto>?>> GetProcessMold(WOProcessMoldDto model);
        Task<ResponseModel<WOProcessMoldDto?>> CreateProcessMold(WOProcessMoldDto model);
        Task<ResponseModel<WOProcessMoldDto?>> ModifyProcessMold(WOProcessMoldDto model);
        Task<ResponseModel<WOProcessMoldDto?>> DeleteProcessMold(WOProcessMoldDto model);
        Task<ResponseModel<IEnumerable<WOMoldPressingTimesDto>?>> GetWOProcessMold(WOMoldPressingTimesDto model);
        #endregion

        #region ProcessStaff
        Task<ResponseModel<IEnumerable<WOProcessStaffDto>?>> GetProcessStaff(WOProcessStaffDto model);
        Task<ResponseModel<WOProcessStaffDto?>> CreateProcessStaff(WOProcessStaffDto model);
        Task<ResponseModel<WOProcessStaffDto?>> ModifyProcessStaff(WOProcessStaffDto model);
        Task<ResponseModel<WOProcessStaffDto?>> DeleteProcessStaff(WOProcessStaffDto model);
        #endregion

        #region ProcessLine
        Task<ResponseModel<IEnumerable<WOProcessLineDto>?>> GetProcessLine(WOProcessLineDto model);
        Task<ResponseModel<WOProcessLineDto?>> CreateProcessLine(WOProcessLineDto model);
        Task<ResponseModel<WOProcessLineDto?>> ModifyProcessLine(WOProcessLineDto model);
        Task<ResponseModel<WOProcessLineDto?>> DeleteProcessLine(WOProcessLineDto model);
        Task<ResponseModel<WOProcessLineDto?>> CreateProcessLineDup(WOProcessLineDto model);
        #endregion

        #region ProcessMoldStaffLine
        Task<ResponseModel<IEnumerable<WOProcessMoldStaffLineDto>?>> GetProcessMoldStaffLine(WOProcessMoldStaffLineDto model);
        #endregion

        #region WO SemiLot MMS
        Task<ResponseModel<int>> CheckNewMoldWorkerMachine(long? WOProcessId);
        Task<ResponseModel<IEnumerable<WOSemiLotMMSDto>?>> GetWoSemiLotMMS(WOSemiLotMMSDto model);
        Task<bool> CheckMoldStaffMachineShift(long? WOProcessId);
        Task<ResponseModel<WOSemiLotMMSDto?>> CreateSemiLot(WOSemiLotMMSDto model);
        Task<ResponseModel<WOSemiLotMMSDto?>> GetByIdWOSemiLotMMS(long? WOSemiLotMMSId);
        Task<bool> CheckSemiLotShiftOfStaff(long? WOProcessId, string SemiLotCode);
        Task<bool> CheckMaterialMapping(string MaterialLotCode);
        Task<bool> IsMaterialFinished(string MaterialLotCode);
        Task<bool> CheckMaxSemilot(long? WOSemiLotMMSId, long? WOProcessId);
        Task<IEnumerable<WOSemiLotMMSDetailDto>> GetMaterialMappingByMaterialCode(string SemiLotCode);
        Task<MaterialLotDto> GetMaterialLotCode(string MaterialLotCode);
        Task<int> GetCountMaterialMapping(string SemiLotCode, string MaterialLotCode);
        Task<int> UpdateMaterialLot(MaterialLotDto model);
        Task<ResponseModel<WOSemiLotMMSDto?>> DeleteSemiLot(WOSemiLotMMSDto model);
        Task<ResponseModel<WOSemiLotMMSDto?>> ModifyWOSemiLotQuantity(WOSemiLotMMSDto model);
        Task<ResponseModel<WOMoldPressingTimesDto?>> CreateWOMoldPressingTimes(WOMoldPressingTimesDto model);
        Task<ResponseModel<IEnumerable<QCPQCDetailSLDto>?>> GetListPQCSL(long? QCIQCMasterId, long? MaterialLotId);
        Task<ResponseModel<IEnumerable<WOSemiLotMMSDetailSLDto>?>> GetValuePQCSL(long? WOSemiLotMMSId);
        Task<ResponseModel<WOSemiLotMMSCheckMasterSLDto?>> CheckPQCSL(WOSemiLotMMSCheckMasterSLDto model);
        Task<WOProcessDto> GetWOProcessName(long? WOId, int? ProcessLevel);
        Task<ResponseModel<WOSemiLotMMSDetailDto?>> CreateSemiLotDetailSemiLot(WOSemiLotMMSDetailDto model);
        Task<ResponseModel<dynamic?>> GetByIdWOSemiLotMMSPrint(long? WOSemiLotMMSId);
        #endregion

        #region WO SemiLot MMS Detail
        Task<ResponseModel<IEnumerable<WOSemiLotMMSDetailDto>?>> GetWoSemiLotMMSDetail(WOSemiLotMMSDetailDto model);
        Task<ResponseModel<IEnumerable<MaterialLotDto>?>> GetWoMaterialLot(MaterialLotDto model);
        Task<ResponseModel<IEnumerable<WOSemiLotMMSDto>?>> GetWoSemilot(WOSemiLotMMSDto model);
        Task<WOSemiLotMMSDto> GetSemiLotCode(long? WOSemiLotMMSId);
        Task<bool> IsMaterialInfoExistByProcess(string MaterialLotCode, long? WOProcessId);
        Task<ResponseModel<WOSemiLotMMSDetailDto?>> CreateSemiLotDetail(WOSemiLotMMSDetailDto model);
        Task<WOSemiLotMMSDetailDto> GetFistWOSemiLotMMSDetail(long? WOSemiLotDetailId);
        Task<int> checkMaterialOrSemiLot(string MaterialLotCode);
        Task<ResponseModel<WOSemiLotMMSDetailDto?>> DeleteSemiLotDetail(WOSemiLotMMSDetailDto model);
        Task<ResponseModel<WOSemiLotMMSDetailDto?>> FinishSemiLotDetail(WOSemiLotMMSDetailDto model);
        Task<ResponseModel<WOSemiLotMMSDetailDto?>> FinishSemiLotDetailSemi(WOSemiLotMMSDetailDto model);
        Task<ResponseModel<WOSemiLotMMSDetailDto?>> ReturnSemiLotDetail(WOSemiLotMMSDetailDto model);
        Task<WOSemiLotMMSDto> GetSemiLotCodeByCode(string SemiLotCode);

        #endregion
        #region List material lot code From BOM
        Task<ResponseModel<IEnumerable<BomProcessMaterialDto>?>> GetListMaterialLotFromBom(BomProcessMaterialDto model);
        #endregion
        #region General
        Task<WOProcessDto> GetWOProcessById(long? WOProcessId);
        Task<bool> IsMaterialReturn(string MaterialLotCode);
        #endregion
        #region PressLot
        Task<ResponseModel<IEnumerable<WOPressLotMMSDto>?>> GetSemiPressLotMMS(WOPressLotMMSDto model);
        Task<ResponseModel<WOPressLotMMSDto?>> CreatePressLot(WOPressLotMMSDto model);
        Task<ResponseModel<IEnumerable<WOPressLotMMSDto>?>> GetPressLotMMS(WOPressLotMMSDto model);
        Task<ResponseModel<WOPressLotMMSDto?>> UnMaping(WOPressLotMMSDto model);
        Task<ResponseModel<WOPressLotMMSDto?>> GetByCodeWOPressLotMMS(long WOSemiLotMMSId);

        #endregion
        Task<ResponseModel<IEnumerable<dynamic>?>> GetSelectMold(long? WOProcessId);
    }
    [ScopedRegistration]
    public class WOService : IWOService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public WOService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        #region WO
        public async Task<ResponseModel<IEnumerable<WODto>?>> GetAll(WODto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WODto>?>();
                string proc = "Usp_WO_GetAll";
                var param = new DynamicParameters();
                param.Add("@WOCode", model.WOCode);
                param.Add("@ProductCode", model.ProductCode);
                param.Add("@ModelId", model.ModelId);
                param.Add("@StartDate", model.StartDate);
                param.Add("@EndDate", model.EndDate);
                param.Add("@isFinish", model.isFinish);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WODto>(proc, param);
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

        public async Task<ResponseModel<WODto?>> Create(WODto model)
        {
            var returnData = new ResponseModel<WODto?>();

            string proc = "Usp_WO_Create";
            var param = new DynamicParameters();
            param.Add("@WOId", model.WOId);
            param.Add("@ProductCode", model.ProductCode);
            param.Add("@BomVersion", model.BomVersion);
            param.Add("@Target", model.Target);
            param.Add("@Description", model.Description);
            param.Add("@ManufacturingDate", model.ManufacturingDate);
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
                    returnData = await GetById(model.WOId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<WODto?>> Modify(WODto model)
        {
            var returnData = new ResponseModel<WODto?>();

            string proc = "Usp_WO_Modify";
            var param = new DynamicParameters();
            param.Add("@WOId", model.WOId);
            param.Add("@ProductCode", model.ProductCode);
            param.Add("@BomVersion", model.BomVersion);
            param.Add("@Target", model.Target);
            param.Add("@Description", model.Description);
            param.Add("@ManufacturingDate", model.ManufacturingDate);
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
                    returnData = await GetById(model.WOId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<WODto?>> GetById(long? WOId)
        {
            var returnData = new ResponseModel<WODto?>();
            string proc = "Usp_WO_GetById";
            var param = new DynamicParameters();
            param.Add("@WOId", WOId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WODto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }

        public async Task<ResponseModel<WODto?>> Delete(WODto model)
        {
            string proc = "Usp_WO_Delete";
            var param = new DynamicParameters();
            param.Add("@WOId", model.WOId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<WODto?>();
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

        public async Task<ResponseModel<WODto?>> Finish(WODto model)
        {
            string proc = "Usp_WO_Finish";
            var param = new DynamicParameters();
            param.Add("@WOId", model.WOId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<WODto?>();
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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetSelectBOM(long? ProductId)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_WO_GetSelectBOM";
                var param = new DynamicParameters();
                param.Add("@ProductId", ProductId);

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

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetSelectMold(long? WOProcessId)
        {
            var returnData = new ResponseModel<IEnumerable<dynamic>?>();
            var proc = $"Usp_WO_GetSelectMold";
            var param = new DynamicParameters();
            param.Add("@WOProcessId", WOProcessId);
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);

            if (!data.Any())
            {
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                returnData.HttpResponseCode = 204;
            }
            else
            {
                returnData.Data = data;
            }

            return returnData;
        }
        #endregion
        #region WO Process
        public async Task<ResponseModel<IEnumerable<WOProcessDto>?>> GetProcess(WOProcessDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WOProcessDto>?>();
                string proc = "Usp_WOProcess_GetAll"; var param = new DynamicParameters();
                param.Add("@WOId", model.WOId);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOProcessDto>(proc, param);
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
        public async Task<ResponseModel<WOProcessDto?>> CreateProcess(WOProcessDto model)
        {
            var returnData = new ResponseModel<WOProcessDto?>();

            string proc = "Usp_WOProcess_Create";
            var param = new DynamicParameters();
            param.Add("@WOProcessId", model.WOProcessId);
            param.Add("@WOId", model.WOId);
            param.Add("@ProcessCode", model.ProcessCode);
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
                    returnData = await GetByIdProcess(model.WOProcessId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<WOProcessDto?>> ModifyProcess(WOProcessDto model)
        {
            var returnData = new ResponseModel<WOProcessDto?>();

            string proc = "Usp_WOProcess_Modify";
            var param = new DynamicParameters();
            param.Add("@LineId", model.LineId);
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
                    returnData = await GetByIdProcess(model.WOProcessId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<WOProcessDto?>> GetByIdProcess(long? WOProcessId)
        {
            var returnData = new ResponseModel<WOProcessDto?>();
            string proc = "Usp_WOProcess_GetById";
            var param = new DynamicParameters();
            param.Add("@WOProcessId", WOProcessId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOProcessDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }

        public async Task<ResponseModel<WOProcessDto?>> DeleteProcess(WOProcessDto model)
        {
            string proc = "Usp_WOProcess_Delete";
            var param = new DynamicParameters();
            param.Add("@WOProcessId", model.WOProcessId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<WOProcessDto?>();
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

        #region Check PQC
        public async Task<ResponseModel<IEnumerable<PQCWOProcessDetailASDto>?>> GetValueCheck(long? QCPQCMasterId, long? WOProcessId)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<PQCWOProcessDetailASDto>?>();
                string proc = "Usp_WOProcess_GetCheckPQC";
                var param = new DynamicParameters();
                param.Add("@WOProcessId", WOProcessId);
                param.Add("@QCPQCMasterId", QCPQCMasterId);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<PQCWOProcessDetailASDto>(proc, param);
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

        public async Task<ResponseModel<PQCWOProcessCheckMasterASDto?>> CheckProcessPQC(PQCWOProcessCheckMasterASDto model)
        {
            var returnData = new ResponseModel<PQCWOProcessCheckMasterASDto?>();


            var jsonLotList = JsonConvert.SerializeObject(model.ValueCheck);
            string proc = "Usp_WOProcess_CheckPQC";
            var param = new DynamicParameters();
            param.Add("@WOProcessId", model.WOProcessId);
            param.Add("@QCPQCMasterId", model.QCPQCMasterId);
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
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }
        #endregion

        #region ProcessMold
        public async Task<ResponseModel<IEnumerable<WOProcessMoldDto>?>> GetProcessMold(WOProcessMoldDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WOProcessMoldDto>?>();
                string proc = "Usp_WOProcessMold_GetAll"; var param = new DynamicParameters();
                param.Add("@WOProcessId", model.WOProcessId);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOProcessMoldDto>(proc, param);
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

        public async Task<ResponseModel<WOProcessMoldDto?>> CreateProcessMold(WOProcessMoldDto model)
        {
            var returnData = new ResponseModel<WOProcessMoldDto?>();

            string proc = "Usp_WOProcessMold_Create";
            var param = new DynamicParameters();
            param.Add("@WOProcessMoldId", model.WOProcessMoldId);
            param.Add("@WOProcessId", model.WOProcessId);
            param.Add("@MoldId", model.MoldId);
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
                    returnData = await GetByIdProcessMold(model.WOProcessMoldId);
                    returnData.ResponseMessage = result;
                    break;
                case "Using_Mold_Other":
                    returnData.HttpResponseCode = 300;
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    returnData.ResponseMessage = result;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<WOProcessMoldDto?>> GetByIdProcessMold(long? WOProcessMoldId)
        {
            var returnData = new ResponseModel<WOProcessMoldDto?>();
            string proc = "Usp_WOProcessMold_GetById";
            var param = new DynamicParameters();
            param.Add("@WOProcessMoldId", WOProcessMoldId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOProcessMoldDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }

        public async Task<ResponseModel<WOProcessMoldDto?>> ModifyProcessMold(WOProcessMoldDto model)
        {
            var returnData = new ResponseModel<WOProcessMoldDto?>();

            string proc = "Usp_WOProcessMold_Modify";
            var param = new DynamicParameters();
            param.Add("@WOProcessMoldId", model.WOProcessMoldId);
            param.Add("@StartDate", model.StartDate);
            param.Add("@EndDate", model.EndDate);
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
                    returnData = await GetByIdProcessMold(model.WOProcessMoldId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    returnData.ResponseMessage = result;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<WOProcessMoldDto?>> DeleteProcessMold(WOProcessMoldDto model)
        {
            string proc = "Usp_WOProcessMold_Delete";
            var param = new DynamicParameters();
            param.Add("@WOProcessMoldId", model.WOProcessMoldId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<WOProcessMoldDto?>();
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
        public async Task<ResponseModel<IEnumerable<WOMoldPressingTimesDto>?>> GetWOProcessMold(WOMoldPressingTimesDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WOMoldPressingTimesDto>?>();
                string proc = "Usp_WOMoldPressingTimesDetail_Get"; var param = new DynamicParameters();
                param.Add("@MoldId", model.MoldId);
                param.Add("@WOProcessId", model.WOProcessId);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOMoldPressingTimesDto>(proc, param);
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

        #region ProcessStaff
        public async Task<ResponseModel<IEnumerable<WOProcessStaffDto>?>> GetProcessStaff(WOProcessStaffDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WOProcessStaffDto>?>();
                string proc = "Usp_WOProcessStaffMMS_GetAll"; var param = new DynamicParameters();
                param.Add("@WOProcessId", model.WOProcessId);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOProcessStaffDto>(proc, param);
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
        public async Task<ResponseModel<WOProcessStaffDto?>> CreateProcessStaff(WOProcessStaffDto model)
        {
            var returnData = new ResponseModel<WOProcessStaffDto?>();

            string proc = "Usp_WOProcessStaffMMS_Create";
            var param = new DynamicParameters();
            param.Add("@WOProcessStaffId", model.WOProcessStaffId);
            param.Add("@WOProcessId", model.WOProcessId);
            param.Add("@StaffId", model.StaffId);
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
                    returnData = await GetByIdProcessStaff(model.WOProcessStaffId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    returnData.ResponseMessage = result;
                    break;
            }
            return returnData;
        }
        public async Task<ResponseModel<WOProcessStaffDto?>> GetByIdProcessStaff(long? WOProcessStaffId)
        {
            var returnData = new ResponseModel<WOProcessStaffDto?>();
            string proc = "Usp_WOProcessStaffMMS_GetById";
            var param = new DynamicParameters();
            param.Add("@WOProcessStaffId", WOProcessStaffId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOProcessStaffDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }
        public async Task<ResponseModel<WOProcessStaffDto?>> ModifyProcessStaff(WOProcessStaffDto model)
        {
            var returnData = new ResponseModel<WOProcessStaffDto?>();

            string proc = "Usp_WOProcessStaffMMS_Modify";
            var param = new DynamicParameters();
            param.Add("@WOProcessStaffId", model.WOProcessStaffId);
            param.Add("@StartDate", model.StartDate);
            param.Add("@EndDate", model.EndDate);
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
                    returnData = await GetByIdProcessStaff(model.WOProcessStaffId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    returnData.ResponseMessage = result;
                    break;
            }
            return returnData;
        }
        public async Task<ResponseModel<WOProcessStaffDto?>> DeleteProcessStaff(WOProcessStaffDto model)
        {
            string proc = "Usp_WOProcessStaffMMS_Delete";
            var param = new DynamicParameters();
            param.Add("@WOProcessStaffId", model.WOProcessStaffId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<WOProcessStaffDto?>();
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

        #region ProcessLine
        public async Task<ResponseModel<IEnumerable<WOProcessLineDto>?>> GetProcessLine(WOProcessLineDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WOProcessLineDto>?>();
                string proc = "Usp_WOProcessLine_GetAll"; var param = new DynamicParameters();
                param.Add("@WOProcessId", model.WOProcessId);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOProcessLineDto>(proc, param);
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
        public async Task<ResponseModel<WOProcessLineDto?>> CreateProcessLine(WOProcessLineDto model)
        {
            var returnData = new ResponseModel<WOProcessLineDto?>();

            string proc = "Usp_WOProcessLine_Create";
            var param = new DynamicParameters();
            param.Add("@WOProcessLineId", model.WOProcessLineId);
            param.Add("@WOProcessId", model.WOProcessId);
            param.Add("@LineId", model.LineId);
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
                    returnData = await GetByIdProcessLine(model.WOProcessLineId);
                    returnData.ResponseMessage = result;
                    break;
                case "Using_Line_Other":
                    returnData.HttpResponseCode = 300;
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    returnData.ResponseMessage = result;
                    break;
            }
            return returnData;
        }
     
        public async Task<ResponseModel<WOProcessLineDto?>> GetByIdProcessLine(long? WOProcessLineId)
        {
            var returnData = new ResponseModel<WOProcessLineDto?>();
            string proc = "Usp_WOProcessLine_GetById";
            var param = new DynamicParameters();
            param.Add("@WOProcessLineId", WOProcessLineId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOProcessLineDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }
        public async Task<ResponseModel<WOProcessLineDto?>> ModifyProcessLine(WOProcessLineDto model)
        {
            var returnData = new ResponseModel<WOProcessLineDto?>();

            string proc = "Usp_WOProcessLine_Modify";
            var param = new DynamicParameters();
            param.Add("@WOProcessLineId", model.WOProcessLineId);
            param.Add("@StartDate", model.StartDate);
            param.Add("@EndDate", model.EndDate);
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
                    returnData = await GetByIdProcessLine(model.WOProcessLineId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    returnData.ResponseMessage = result;
                    break;
            }
            return returnData;
        }
        public async Task<ResponseModel<WOProcessLineDto?>> DeleteProcessLine(WOProcessLineDto model)
        {
            string proc = "Usp_WOProcessLine_Delete";
            var param = new DynamicParameters();
            param.Add("@WOProcessLineId", model.WOProcessLineId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<WOProcessLineDto?>();
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
        public async Task<ResponseModel<WOProcessLineDto?>> CreateProcessLineDup(WOProcessLineDto model)
        {
            var returnData = new ResponseModel<WOProcessLineDto?>();

            string proc = "Usp_WOProcessLineDup_Create";
            var param = new DynamicParameters();
            param.Add("@WOProcessLineId", model.WOProcessLineId);
            param.Add("@WOProcessId", model.WOProcessId);
            param.Add("@LineId", model.LineId);
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
                    returnData = await GetByIdProcessLine(model.WOProcessLineId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }
        #endregion

        #region ProcessMoldStaffLine
        public async Task<ResponseModel<IEnumerable<WOProcessMoldStaffLineDto>?>> GetProcessMoldStaffLine(WOProcessMoldStaffLineDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WOProcessMoldStaffLineDto>?>();
                string proc = "Usp_WOProcessMoldStaffLine_GetAll"; var param = new DynamicParameters();
                param.Add("@WOProcessId", model.WOProcessId);
                param.Add("@isActived", model.isActived);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOProcessMoldStaffLineDto>(proc, param);
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

        #region WO SemiLot MMS
        public async Task<ResponseModel<int>> CheckNewMoldWorkerMachine(long? WOProcessId)
        {
            try
            {
                var returnData = new ResponseModel<int>();
                var param = new DynamicParameters();
                param.Add("@Id", WOProcessId);

                var queryMold = " Select Count(WOProcessMoldId) from WOProcessMold where WOProcessId = @Id and isActived = 1 ";
                var checkMold = await _sqlDataAccess.LoadDataExecuteScalarAsync<int>(queryMold, param);


                var queryWorker = " Select Count(WOProcessStaffId) from WOProcessStaff where WOProcessId = @Id and isActived = 1";
                var checkWorker = await _sqlDataAccess.LoadDataExecuteScalarAsync<int>(queryWorker, param);

                var queryMachine = " Select Count(WOProcessLineId) from WOProcessLine where WOProcessId = @Id and isActived = 1";
                var checkMachine = await _sqlDataAccess.LoadDataExecuteScalarAsync<int>(queryMachine, param);

                returnData.HttpResponseCode = 204;
                if (checkMold == 0 && checkWorker == 0 && checkMachine == 0)
                {
                    returnData.Data = 1;
                    returnData.ResponseMessage = "WO.NotMappingMoldStaftMachine";
                    return (returnData);
                }
                else if (checkMold == 0)
                {
                    returnData.Data = 2;
                    returnData.ResponseMessage = "WO.NotMappingMold";
                    return (returnData);
                }
                else if (checkWorker == 0)
                {
                    returnData.Data = 3;
                    returnData.ResponseMessage = "WO.NotMappingdStaft";
                    return (returnData);
                }
                else if (checkMachine == 0)
                {
                    returnData.Data = 4;
                    returnData.ResponseMessage = "WO.NotMappingMachine";
                    return (returnData);
                }
                else
                {
                    returnData.Data = 0;
                    returnData.HttpResponseCode = 200;
                    returnData.ResponseMessage = StaticReturnValue.SUCCESS;
                    return (returnData);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<ResponseModel<IEnumerable<WOSemiLotMMSDto>?>> GetWoSemiLotMMS(WOSemiLotMMSDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WOSemiLotMMSDto>?>();
                string proc = "Usp_WOSemiLotMMS_GetAll"; var param = new DynamicParameters();
                param.Add("@WOProcessId", model.WOProcessId);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOSemiLotMMSDto>(proc, param);
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

        public async Task<bool> CheckMoldStaffMachineShift(long? WOProcessId)
        {
            try
            {
                string proc = @"EXEC Usp_WOStaffMoldMachineShift_GetById @WOProcessId";
                var param = new DynamicParameters();
                param.Add("@WOProcessId", WOProcessId);

                var data = await _sqlDataAccess.LoadDataExecuteScalarAsync<bool>(proc, param);
                return data;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<ResponseModel<WOSemiLotMMSDto?>> CreateSemiLot(WOSemiLotMMSDto model)
        {
            var returnData = new ResponseModel<WOSemiLotMMSDto?>();

            string proc = "Usp_WOSemiLotMMS_Create";
            var param = new DynamicParameters();
            param.Add("@WOSemiLotMMSId", model.WOSemiLotMMSId);
            param.Add("@WOProcessId", model.WOProcessId);
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
                    returnData = await GetByIdWOSemiLotMMS(model.WOSemiLotMMSId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    returnData.ResponseMessage = result;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<WOSemiLotMMSDto?>> GetByIdWOSemiLotMMS(long? WOSemiLotMMSId)
        {
            var returnData = new ResponseModel<WOSemiLotMMSDto?>();
            string proc = "Usp_WOSemiLotMMS_GetById";
            var param = new DynamicParameters();
            param.Add("@WOSemiLotMMSId", WOSemiLotMMSId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOSemiLotMMSDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }

        public async Task<bool> CheckSemiLotShiftOfStaff(long? WOProcessId, string SemiLotCode)
        {
            try
            {
                string proc = @"EXEC Usp_WOSemiCheckShift_GetById @WOProcessId, @SemiLotCode";
                var param = new DynamicParameters();
                param.Add("@WOProcessId", WOProcessId);
                param.Add("@SemiLotCode", SemiLotCode);

                var data = await _sqlDataAccess.LoadDataExecuteScalarAsync<bool>(proc, param);
                return data;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> CheckMaterialMapping(string MaterialLotCode)
        {
            try
            {
                string proc = @"SELECT CASE 
                                            WHEN wlm.WOSemiLotDetailId IS NULL THEN 'False' 
                                            ELSE 'TRUE' 
                                        END AS HasProfile 
                                FROM WOSemiLotMMSDetail wlm
                                WHERE wlm.MaterialLotCode = @MaterialLotCode ";
                var param = new DynamicParameters();
                param.Add("@MaterialLotCode", MaterialLotCode);

                var data = await _sqlDataAccess.LoadDataExecuteScalarAsync<bool>(proc, param);
                return data;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> IsMaterialFinished(string MaterialLotCode)
        {
            try
            {
                string proc = @"SELECT CASE 
			                    WHEN EXISTS(
					                    SELECT WOSemiLotDetailId FROM WOSemiLotMMSDetail WHERE SemiLotCode = @MaterialLotCode and IsFinish = 1
					                    ) THEN 'true'
				                    ELSE 'false'
			                    END ";
                var param = new DynamicParameters();
                param.Add("@MaterialLotCode", MaterialLotCode);

                var data = await _sqlDataAccess.LoadDataExecuteScalarAsync<bool>(proc, param);
                return data;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> CheckMaxSemilot(long? WOSemiLotMMSId, long? WOProcessId)
        {
            try
            {
                string proc = @"SELECT CASE   
                                    WHEN WOSemiLotMMSId IS NULL THEN 'False'   ELSE 'TRUE'  END AS MaxSemi 
                                    FROM  WOSemiLotMMS WHERE WOSemiLotMMSId = @WOSemiLotMMSId  
                                    AND  WOSemiLotMMSId = (SELECT MAX(WOSemiLotMMSId) 
                                    FROM WOSemiLotMMS WHERE WOProcessId = @WOProcessId)";
                var param = new DynamicParameters();
                param.Add("@WOProcessId", WOProcessId);
                param.Add("@WOSemiLotMMSId", WOSemiLotMMSId);

                var data = await _sqlDataAccess.LoadDataExecuteScalarAsync<bool>(proc, param);
                return data;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<WOSemiLotMMSDetailDto>> GetMaterialMappingByMaterialCode(string SemiLotCode)
        {
            try
            {
                var returnData = new WOSemiLotMMSDetailDto();
                string proc = "SELECT * FROM WOSemiLotMMSDetail  where SemiLotCode = @SemiLotCode";
                var param = new DynamicParameters();
                param.Add("@SemiLotCode", SemiLotCode);

                var data = await _sqlDataAccess.LoadDataUsingRawQuery<WOSemiLotMMSDetailDto>(proc, param);
                return data;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<MaterialLotDto> GetMaterialLotCode(string MaterialLotCode)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@MaterialLotCode", MaterialLotCode);

                var sql = "SELECT * FROM MaterialLot ml where ml.MaterialLotCode = @MaterialLotCode ";

                var data = await _sqlDataAccess.LoadDataFirstOrDefaultAsync<MaterialLotDto>(sql, param);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> GetCountMaterialMapping(string SemiLotCode, string MaterialLotCode)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@SemiLotCode", SemiLotCode);
                param.Add("@MaterialLotCode", MaterialLotCode);

                var query = @"Select Count(*) from  WOSemiLotMMSDetail Where MaterialLotCode = @MaterialLotCode and SemiLotCode <>  @SemiLotCode";
                var result = await _sqlDataAccess.LoadDataExecuteScalarAsync<int>(query, param);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> UpdateMaterialLot(MaterialLotDto model)
        {
            try
            {
                string proc = "update MaterialLot set WOProcessId = @WOProcessId , LotStatus = @LotStatus, modifiedDate = GETDATE(), modifiedBy = @createdBy  WHERE MaterialLotCode = @MaterialLotCode";
                var param = new DynamicParameters();
                param.Add("@MaterialLotCode", model.MaterialLotCode);
                param.Add("@createdBy", model.createdBy);

                var data = await _sqlDataAccess.LoadDataExecuteScalarAsync<int>(proc, param);
                return data;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<ResponseModel<WOSemiLotMMSDto?>> DeleteSemiLot(WOSemiLotMMSDto model)
        {
            string proc = "Usp_WOSemiLotMMS_Detele";
            var param = new DynamicParameters();
            param.Add("@WOSemiLotMMSId", model.WOSemiLotMMSId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<WOSemiLotMMSDto?>();
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
        public async Task<ResponseModel<WOSemiLotMMSDto?>> ModifyWOSemiLotQuantity(WOSemiLotMMSDto model)
        {
            var returnData = new ResponseModel<WOSemiLotMMSDto?>();

            string proc = "Usp_WOSemiLotMMS_ModifyQuantity";
            var param = new DynamicParameters();
            param.Add("@WOSemiLotMMSId", model.WOSemiLotMMSId);
            param.Add("@ActualQty", model.ActualQty);
            param.Add("@modifiedBy", model.modifiedBy);
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
                    returnData = await GetByIdWOSemiLotMMS(model.WOSemiLotMMSId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    returnData.ResponseMessage = result;

                    break;
            }
            return returnData;
        }
        public async Task<ResponseModel<WOMoldPressingTimesDto?>> CreateWOMoldPressingTimes(WOMoldPressingTimesDto model)
        {
            var returnData = new ResponseModel<WOMoldPressingTimesDto?>();

            string proc = "Usp_WOMoldPressingTimes_Create";
            var param = new DynamicParameters();
            param.Add("@WOMoldPressingId", model.WOMoldPressingId);
            param.Add("@MoldId", model.MoldId);
            param.Add("@WOProcessId", model.WOProcessId);
            param.Add("@PressingTimes", model.PressingTimes);
            param.Add("@createdBy", model.createdBy);
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
                    //returnData = await GetByIdWOSemiLotMMS(model.WOSemiLotMMSId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    returnData.ResponseMessage = result;

                    break;
            }
            return returnData;
        }
        public async Task<ResponseModel<IEnumerable<QCPQCDetailSLDto>?>> GetListPQCSL(long? QCPQCMasterId, long? WOSemiLotMMSId)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<QCPQCDetailSLDto>?>();
                string proc = "Usp_QCPQCDetailSL_GetAll";
                var param = new DynamicParameters();
                param.Add("@QCPQCMasterId", QCPQCMasterId);
                param.Add("@isActived", 1);
                param.Add("@page", 0);
                param.Add("@pageSize", 0);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCPQCDetailSLDto>(proc, param);
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

        public async Task<ResponseModel<IEnumerable<WOSemiLotMMSDetailSLDto>?>> GetValuePQCSL(long? WOSemiLotMMSId)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WOSemiLotMMSDetailSLDto>?>();
                string proc = "Usp_WOSemiLotMMSDetailSL_GetAll";
                var param = new DynamicParameters();
                param.Add("@WOSemiLotMMSId", WOSemiLotMMSId);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOSemiLotMMSDetailSLDto>(proc, param);
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

        public async Task<ResponseModel<WOSemiLotMMSCheckMasterSLDto?>> CheckPQCSL(WOSemiLotMMSCheckMasterSLDto model)
        {
            var returnData = new ResponseModel<WOSemiLotMMSCheckMasterSLDto?>();
            var jsonLotList = JsonConvert.SerializeObject(model.ValueCheck);

            string proc = "Usp_WOSemiLotMMSCheckMasterSL_Create";
            var param = new DynamicParameters();
            param.Add("@WOSemiLotMMSId", model.WOSemiLotMMSId);
            param.Add("@QCPQCMasterId", model.QCPQCMasterId);
            param.Add("@StaffId", model.StaffId);
            param.Add("@CheckDate", model.CheckDate);
            param.Add("@CheckResult", model.CheckResult);
            param.Add("@JsonlistItemDetail", jsonLotList);
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
        public async Task<WOProcessDto> GetWOProcessName(long? WOId, int? ProcessLevel)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@WOId", WOId);
                param.Add("@ProcessLevel", ProcessLevel);
                var sql = @"SELECT top 1 (p.ProductCode)ProductCode, (tcd.commonDetailName)ProcessCode, w.ProcessLevel
                    FROM WOProcess w 
                    LEFT JOIN WO w1 ON w.WOId = w1.WOId
                    LEFT JOIN Product p ON w1.ProductCode = p.ProductCode
                    LEFT JOIN sysTbl_CommonDetail tcd ON w.ProcessCode = tcd.commonDetailName and tcd.commonMasterCode = 'BOMPROCESS'
                    WHERE w.WOId = @WOId
                    AND w.ProcessLevel < @ProcessLevel 	and w.AreaCode = 'WORKSHOP' 
                    AND w.isActived  = 1 order by w.ProcessLevel desc ";
                var data = await _sqlDataAccess.LoadDataFirstOrDefaultAsync<WOProcessDto>(sql, param);
                return data;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public async Task<ResponseModel<WOSemiLotMMSDetailDto?>> CreateSemiLotDetailSemiLot(WOSemiLotMMSDetailDto model)
        {
            var returnData = new ResponseModel<WOSemiLotMMSDetailDto?>();

            string proc = "Usp_WOSemiLotMMSDetailSemiLot_Create";
            var param = new DynamicParameters();
            param.Add("@WOSemiLotDetailId", model.WOSemiLotDetailId);
            param.Add("@WOSemiLotMMSId", model.WOSemiLotMMSId);
            param.Add("@MaterialLotCode", model.MaterialLotCode);
            param.Add("@WOProcessId", model.WOProcessId);
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
                    returnData = await GetByIdWOSemiLotMMSDetail(model.WOSemiLotDetailId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    returnData.ResponseMessage = result;

                    break;
            }
            return returnData;
        }
        public async Task<ResponseModel<dynamic?>> GetByIdWOSemiLotMMSPrint(long? WOSemiLotMMSId)
        {
            var returnData = new ResponseModel<dynamic?>();
            string proc = "Usp_WOSemiLotMMS_Print";
            var param = new DynamicParameters();
            param.Add("@WOSemiLotMMSId", WOSemiLotMMSId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }

        #endregion

        #region WO SemiLot MMS Detail
        public async Task<ResponseModel<IEnumerable<WOSemiLotMMSDetailDto>?>> GetWoSemiLotMMSDetail(WOSemiLotMMSDetailDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WOSemiLotMMSDetailDto>?>();
                string proc = "Usp_WOSemiLotMMSDetail_GetAll";
                var param = new DynamicParameters();
                param.Add("@WOSemiLotMMSId", model.WOSemiLotMMSId);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOSemiLotMMSDetailDto>(proc, param);
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

        public async Task<ResponseModel<IEnumerable<MaterialLotDto>?>> GetWoMaterialLot(MaterialLotDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialLotDto>?>();
                string proc = "Usp_WOMaterialLot_GetForSelect"; var param = new DynamicParameters();
                param.Add("@MaterialLotCode", model.MaterialLotCode);
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

        public async Task<ResponseModel<IEnumerable<WOSemiLotMMSDto>?>> GetWoSemilot(WOSemiLotMMSDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WOSemiLotMMSDto>?>();
                string proc = "Usp_WOSemiLot_GetForSelect"; var param = new DynamicParameters();
                param.Add("@WOProcessId", model.WOProcessId);
                param.Add("@SemiLotCode", model.SemiLotCode);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOSemiLotMMSDto>(proc, param);
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

        public async Task<WOSemiLotMMSDto> GetSemiLotCode(long? WOSemiLotMMSId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@WOSemiLotMMSId", WOSemiLotMMSId);
                var sql = "SELECT * FROM WOSemiLotMMS  where WOSemiLotMMSId = @WOSemiLotMMSId ";
                var data = await _sqlDataAccess.LoadDataFirstOrDefaultAsync<WOSemiLotMMSDto>(sql, param);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> IsMaterialInfoExistByProcess(string MaterialLotCode, long? WOProcessId)
        {
            try
            {
                string proc = @"EXEC Usp_WOMaterialLotExist_GetByProcessId @WOProcessId,@MaterialLotCode";
                var param = new DynamicParameters();
                param.Add("@WOProcessId", WOProcessId);
                param.Add("@MaterialLotCode", MaterialLotCode);

                var data = await _sqlDataAccess.LoadDataExecuteScalarAsync<bool>(proc, param);
                return data;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<ResponseModel<WOSemiLotMMSDetailDto?>> CreateSemiLotDetail(WOSemiLotMMSDetailDto model)
        {
            try
            {
                var returnData = new ResponseModel<WOSemiLotMMSDetailDto?>();

                string proc = "Usp_WOSemiLotMMSDetail_Create";
                var param = new DynamicParameters();
                param.Add("@WOSemiLotDetailId", model.WOSemiLotDetailId);
                param.Add("@WOSemiLotMMSId", model.WOSemiLotMMSId);
                param.Add("@MaterialLotCode", model.MaterialLotCode);
                param.Add("@WOProcessId", model.WOProcessId);
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
                        returnData = await GetByIdWOSemiLotMMSDetail(model.WOSemiLotDetailId);
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

        public async Task<ResponseModel<WOSemiLotMMSDetailDto?>> GetByIdWOSemiLotMMSDetail(long? WOSemiLotDetailId)
        {
            var returnData = new ResponseModel<WOSemiLotMMSDetailDto?>();
            string proc = "Usp_WOSemiLotMMSDetail_GetById";
            var param = new DynamicParameters();
            param.Add("@WOSemiLotDetailId", WOSemiLotDetailId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOSemiLotMMSDetailDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }
        public async Task<WOSemiLotMMSDetailDto> GetFistWOSemiLotMMSDetail(long? WOSemiLotDetailId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@WOSemiLotDetailId", WOSemiLotDetailId);

                var sql = "SELECT * FROM  WOSemiLotMMSDetail where WOSemiLotDetailId = @WOSemiLotDetailId ";

                var data = await _sqlDataAccess.LoadDataFirstOrDefaultAsync<WOSemiLotMMSDetailDto>(sql, param);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<int> checkMaterialOrSemiLot(string MaterialLotCode)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@MaterialLotCode", MaterialLotCode);


                var QueryCheckInMaterialLot = @"Select Count(*) from MaterialLot Where MaterialLotCode = @MaterialLotCode";
                var ReusltCheckInMaterialLot = await _sqlDataAccess.LoadDataExecuteScalarAsync<int>(QueryCheckInMaterialLot, param);

                var QueryCheckInSemiLotMMS = @"Select Count(*) from WOSemiLotMMS Where SemiLotCode = @MaterialLotCode";
                var ReusltCheckInSemiLotMMS = await _sqlDataAccess.LoadDataExecuteScalarAsync<int>(QueryCheckInSemiLotMMS, param);

                var result = 0;
                if (ReusltCheckInMaterialLot > 0)
                {
                    result = 1;
                }
                if (ReusltCheckInSemiLotMMS > 0)
                {
                    result = 2;
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<ResponseModel<WOSemiLotMMSDetailDto?>> DeleteSemiLotDetail(WOSemiLotMMSDetailDto model)
        {
            string proc = "Usp_WOSemiLotMMSDetail_Detele";
            var param = new DynamicParameters();
            param.Add("@WOSemiLotDetailId", model.WOSemiLotDetailId);
            param.Add("@WOProcessId", model.WOProcessId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<WOSemiLotMMSDetailDto?>();
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
        public async Task<ResponseModel<WOSemiLotMMSDetailDto?>> FinishSemiLotDetail(WOSemiLotMMSDetailDto model)
        {
            var returnData = new ResponseModel<WOSemiLotMMSDetailDto?>();

            string proc = "Usp_WOSemiLotMMSDetail_Finish";
            var param = new DynamicParameters();
            param.Add("@WOSemiLotDetailId", model.WOSemiLotDetailId);
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
                    returnData = await GetByIdWOSemiLotMMSDetail(model.WOSemiLotDetailId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    returnData.ResponseMessage = result;

                    break;
            }
            return returnData;
        }
        public async Task<ResponseModel<WOSemiLotMMSDetailDto?>> FinishSemiLotDetailSemi(WOSemiLotMMSDetailDto model)
        {
            var returnData = new ResponseModel<WOSemiLotMMSDetailDto?>();

            string proc = "Usp_WOSemiLotMMSDetailSemi_Finish";
            var param = new DynamicParameters();
            param.Add("@WOSemiLotDetailId", model.WOSemiLotDetailId);
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
                    returnData = await GetByIdWOSemiLotMMSDetail(model.WOSemiLotDetailId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    returnData.ResponseMessage = result;

                    break;
            }
            return returnData;
        }
        public async Task<ResponseModel<WOSemiLotMMSDetailDto?>> ReturnSemiLotDetail(WOSemiLotMMSDetailDto model)
        {
            var returnData = new ResponseModel<WOSemiLotMMSDetailDto?>();

            string proc = "Usp_WOSemiLotMMSDetail_Return";
            var param = new DynamicParameters();
            param.Add("@WOSemiLotDetailId", model.WOSemiLotDetailId);
            param.Add("@MaterialLotCode", model.MaterialLotCode);
            param.Add("@ActualQty", model.ActualQty);
            param.Add("@RemainQty", model.RemainQty);
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
                    returnData = await GetByIdWOSemiLotMMSDetail(model.WOSemiLotDetailId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    returnData.ResponseMessage = result;

                    break;
            }
            return returnData;
        }
        public async Task<WOSemiLotMMSDto> GetSemiLotCodeByCode(string SemiLotCode)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@SemiLotCode", SemiLotCode);
                var sql = "SELECT * FROM WOSemiLotMMS  where SemiLotCode = @SemiLotCode ";
                var data = await _sqlDataAccess.LoadDataFirstOrDefaultAsync<WOSemiLotMMSDto>(sql, param);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region List material lot code From BOM
        public async Task<ResponseModel<IEnumerable<BomProcessMaterialDto>?>> GetListMaterialLotFromBom(BomProcessMaterialDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<BomProcessMaterialDto>?>();
                string proc = "Usp_WOMaterialLotFromBom_GetAll"; var param = new DynamicParameters();
                param.Add("@WOId", model.WOId);
                param.Add("@WOProcessId", model.WOProcessId);
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
        #endregion
        #region general
        public async Task<WOProcessDto> GetWOProcessById(long? WOProcessId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@WOProcessId", WOProcessId);
                var sql = "SELECT top 1 * FROM WOProcess  where WOProcessId = @WOProcessId  and isActived = 1";
                var data = await _sqlDataAccess.LoadDataFirstOrDefaultAsync<WOProcessDto>(sql, param);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> IsMaterialReturn(string MaterialLotCode)
        {
            try
            {
                string proc = @"SELECT CASE 
			                    WHEN EXISTS(
					                    SELECT MaterialLotId FROM MaterialLot WHERE OriginMaterialLotCode =@MaterialLotCode
					                    ) THEN 'true'
				                    ELSE 'false'
			                    END ";

                var param = new DynamicParameters();
                param.Add("@MaterialLotCode", MaterialLotCode);

                var data = await _sqlDataAccess.LoadDataExecuteScalarAsync<bool>(proc, param);
                return data;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
        #region PressLot
        public async Task<ResponseModel<IEnumerable<WOPressLotMMSDto>?>> GetSemiPressLotMMS(WOPressLotMMSDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WOPressLotMMSDto>?>();
                string proc = "Usp_WOSemiPressLotMMS_GetAll"; var param = new DynamicParameters();
                param.Add("@WOProcessId", model.WOProcessId);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOPressLotMMSDto>(proc, param);
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
        public async Task<ResponseModel<WOPressLotMMSDto?>> CreatePressLot(WOPressLotMMSDto model)
        {
            var returnData = new ResponseModel<WOPressLotMMSDto?>();

            string proc = "Usp_WOSemiLotMMSPressLot_Create";
            var param = new DynamicParameters();
            param.Add("@listQR", ParameterTvp.GetTableValuedParameter_BigInt(model.ListId));
            param.Add("@WOProcessId", model.WOProcessId);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var result = await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
            returnData.ResponseMessage = result;
            long WOSemiLotMMSId = model.ListId.FirstOrDefault();

            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.SUCCESS:
                    returnData = await GetByCodeWOPressLotMMS(WOSemiLotMMSId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    returnData.ResponseMessage = result;
                    break;
            }
            return returnData;
        }
        public async Task<ResponseModel<IEnumerable<WOPressLotMMSDto>?>> GetPressLotMMS(WOPressLotMMSDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WOPressLotMMSDto>?>();
                string proc = "Usp_WOPressLotMMS_GetAll"; var param = new DynamicParameters();
                param.Add("@WOProcessId", model.WOProcessId);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOPressLotMMSDto>(proc, param);
                returnData.Data = data;
                returnData.TotalRow = param.Get<int>("totalRow");
                if (!data.Any())
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                }
                return returnData;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public async Task<ResponseModel<WOPressLotMMSDto?>> UnMaping(WOPressLotMMSDto model)
        {
            var returnData = new ResponseModel<WOPressLotMMSDto?>();

            string proc = "Usp_PressLot_UnMapping";
            var param = new DynamicParameters();
            param.Add("@PressLotCode", model.PressLotCode);
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
        public async Task<ResponseModel<WOPressLotMMSDto?>> GetByCodeWOPressLotMMS(long WOSemiLotMMSId)
        {
            try
            {
                var returnData = new ResponseModel<WOPressLotMMSDto?>();
                string proc = "Usp_WOPressLotMMS_GetByCode";
                var param = new DynamicParameters();
                param.Add("@WOSemiLotMMSId", WOSemiLotMMSId);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOPressLotMMSDto>(proc, param);
                returnData.Data = data.FirstOrDefault();
                if (!data.Any())
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = "NO DATA";
                }
                return returnData;
            }
            catch (Exception e)
            {

                throw;
            }
           
        }
   
        #endregion
    }
}
