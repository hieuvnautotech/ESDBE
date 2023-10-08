using Dapper;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Models.Dtos.FQC;
using ESD.Models.Dtos.MMS;
using Newtonsoft.Json;
using System.Data;
using static ESD.Extensions.ServiceExtensions;

namespace ESD.Services.FQC
{
    public interface IActualService
    {
        Task<ResponseModel<IEnumerable<WOProcessDto>?>> GetProcessFQC(WOProcessDto model);
        Task<ResponseModel<WOProcessDto?>> CreateProcess(WOProcessDto model);
        Task<ResponseModel<WOProcessDto?>> DeleteProcessFQC(WOProcessDto model);
        Task<WOProcessDto> GetProcessById(long? WOProcessId);
        Task<ResponseModel<IEnumerable<WOProcessDto>?>> GetListProcess(long? WOId);

        Task<ResponseModel<IEnumerable<WOProcessStaffDto>?>> GetProcessStaff(WOProcessStaffDto model);
        Task<ResponseModel<WOProcessStaffDto?>> CreateProcessStaff(WOProcessStaffDto model);
        Task<ResponseModel<WOProcessStaffDto?>> ModifyProcessStaff(WOProcessStaffDto model);
        Task<ResponseModel<WOProcessStaffDto?>> DeleteProcessStaff(WOProcessStaffDto model);
        Task<ResponseModel<IEnumerable<WOSemiLotMMSDto>?>> GetWOSemoLot(WOSemiLotMMSDto model);

        Task<ResponseModel<IEnumerable<WOSemiLotFQCDto>?>> GetWoSemiLotFQC(WOSemiLotFQCDto model);
        Task<ResponseModel<WOSemiLotFQCDto?>> CreateSemiLot(WOSemiLotFQCDto model);
        Task<ResponseModel<WOSemiLotFQCDto?>> DeleteSemiLot(WOSemiLotFQCDto model);
        Task<ResponseModel<WOSemiLotFQCDto?>> GetByIdWOSemiLotFQC(long? WOSemiLotFQCId);

        Task<ResponseModel<IEnumerable<WOSemiLotFQCDetailDto>?>> GetWoSemiLotFQCDetail(WOSemiLotFQCDetailDto model);
        Task<ResponseModel<IEnumerable<MaterialLotDto>?>> GetWomaterialLot(MaterialLotDto model);
        Task<ResponseModel<WOSemiLotFQCDetailDto?>> CreateSemiLotDetail(WOSemiLotFQCDetailDto model);
        Task<WOSemiLotFQCDetailDto> CheckLot(string MaterialLotCode);
        Task<ResponseModel<WOSemiLotFQCDetailDto?>> DeleteSemiLotDetail(WOSemiLotFQCDetailDto model);
        Task<ResponseModel<WOSemiLotFQCDetailDto?>> DeleteSemiLotDetailHB(WOSemiLotFQCDetailDto model);
        Task<ResponseModel<WOSemiLotFQCDetailDto?>> StopInheritanceSemiLotDetail(WOSemiLotFQCDetailDto model);
        Task<WOProcessDto> GetProcess(long? WOProcessId);
        Task<WOProcessMaxLevelDto> NameBTPFQC(long? WOId, int ProcessLevel);
        Task<WOProcessMaxLevelDto> NameMinBTPMMS(long? WOId);
        Task<ProductDto> GetProduct(string ProductCode);
        Task<bool> CheckMaxSemilot(long? WOSemiLotFQCId, long? WOProcessId);
        Task<WOProcessMaxLevelDto> ProcessMax(long? WOId);

        Task<ResponseModel<IEnumerable<WOSemiLotFQCQCDto>?>> GetWoSemiLotFQCQC(WOSemiLotFQCQCDto model);
        Task<ResponseModel<WOSemiLotFQCQCDto?>> CreateSemiLotFQCQC(WOSemiLotFQCQCDto model);
        Task<ResponseModel<WOSemiLotFQCQCDto?>> DeleteSemiLotFQCQC(WOSemiLotFQCQCDto model);
        Task<ResponseModel<IEnumerable<QCAPPDetailDto>?>> GetWoSemiLotFQCListDetailQC(QCAPPDetailDto model);

        //Task<ResponseModel<IEnumerable<WOSemiLotFQCDto>?>> GetWaitSemiLotOQC(WOSemiLotFQCDto model);
        //Task<ResponseModel<WOSemiLotFQCDto?>> CreateSemiLotWaitOQC(WOSemiLotFQCDto model);
        //Task<ResponseModel<IEnumerable<WOSemiLotFQCDto>?>> GetSemiLotOQCList(WOSemiLotFQCDto model);
        //Task<ResponseModel<WOSemiLotFQCDto?>> DeleteSemiLotWaitOQC(WOSemiLotFQCDto model);

        //Task<ResponseModel<IEnumerable<QCOQCDetailDto>?>> GetCheckOQC(WOSemiLotFQCDto model);
        //Task<ResponseModel<WOSemiLotFQCOQCMasterDto?>> CheckOQC(WOSemiLotFQCOQCMasterDto model);

        Task<ResponseModel<IEnumerable<WOSemiLotFQCDto>?>> GetWOSemiLotReplacement(WOSemiLotFQCDto model);
        Task<ResponseModel<WOSemiLotFQCDetailDto?>> CreateWOSemiLotReplacement(WOSemiLotFQCDetailDto model);

        Task<WOSemiLotFQCDto> GetSemiLotCode(string SemiLotCode);
        Task<bool> CheckShiftOfStaff(long? WOProcessId);
        Task<WOSemiLotFQCDto> GetSemiLotCodeById(long? WOSemiLotFQCId);
        //Task<ResponseModel<WOProcessDto?>> GetListForProcess(long? WOId);
        Task<ResponseModel<IEnumerable<WOProcessWorkedDto>?>> GetWOProcessWorked(string SemiLotCode, long? WOId);
        Task<bool> CheckSemiLotMapping(string SemiLotCode);

    }
    [ScopedRegistration]
    public class ActualService : IActualService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public ActualService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        #region Process
        public async Task<ResponseModel<IEnumerable<WOProcessDto>?>> GetProcessFQC(WOProcessDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WOProcessDto>?>();
                string proc = "Usp_WOProcess_GetFQC";
                var param = new DynamicParameters();
                param.Add("@WOId", model.WOId);
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

            string proc = "Usp_WOProcess_CreateFQC";
            var param = new DynamicParameters();
            param.Add("@WOProcessId", model.WOProcessId);
            param.Add("@WOId", model.WOId);
            param.Add("@ProcessLevel", model.ProcessLevel);
            param.Add("@ProcessCode", model.ProcessCode);
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
                    returnData = await GetByIdProcess(model.WOProcessId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    returnData.ResponseMessage = result;
                    break;
            }
            return returnData;
        }
        public async Task<ResponseModel<WOProcessDto?>> GetByIdProcess(long? WOProcessId)
        {
            var returnData = new ResponseModel<WOProcessDto?>();
            string proc = "Usp_WOProcessFQC_GetById";
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
        public async Task<ResponseModel<WOProcessDto?>> DeleteProcessFQC(WOProcessDto model)
        {
            string proc = "Usp_WOProcessFQC_Delete";
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
        public async Task<WOProcessDto> GetProcessById(long? WOProcessId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@WOProcessId", WOProcessId);

                var sql = "SELECT * FROM WOProcess  where WOProcessId = @WOProcessId ";

                var data = await _sqlDataAccess.LoadDataFirstOrDefaultAsync<WOProcessDto>(sql, param);
                return data;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public async Task<ResponseModel<IEnumerable<WOProcessDto>?>> GetListProcess(long? WOId)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WOProcessDto>?>();
                var param = new DynamicParameters();
                param.Add("@WOId", WOId);

                var sql = "SELECT * FROM WOProcess  where WOId = @WOId and  AreaCode = 'FQC' and  isActived = 1 ";

                var data = await _sqlDataAccess.LoadDataUsingRawQuery<WOProcessDto>(sql, param);
                returnData.Data = data;
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
        #endregion

        #region ProcessStaff
        public async Task<ResponseModel<IEnumerable<WOProcessStaffDto>?>> GetProcessStaff(WOProcessStaffDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WOProcessStaffDto>?>();
                string proc = "Usp_WOProcessStaffFQC_GetAll";
                var param = new DynamicParameters();
                param.Add("@WOProcessId", model.WOProcessId);
                param.Add("@StaffId", model.StaffId);
                param.Add("@EndDate", model.EndDate);
                param.Add("@StartDate", model.StartDate);
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

            string proc = "Usp_WOProcessStaffFQC_Create";
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
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<WOProcessStaffDto?>> GetByIdProcessStaff(long? WOProcessStaffId)
        {
            var returnData = new ResponseModel<WOProcessStaffDto?>();
            string proc = "Usp_WOProcessStaffFQC_GetById";
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

            string proc = "Usp_WOProcessStaffFQC_Modify";
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
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<WOProcessStaffDto?>> DeleteProcessStaff(WOProcessStaffDto model)
        {
            string proc = "Usp_WOProcessStaffFQC_Delete";
            var param = new DynamicParameters();
            param.Add("@WOProcessStaffId", model.WOProcessStaffId);
            param.Add("@WOProcessId", model.WOProcessId);
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

        public async Task<ResponseModel<IEnumerable<WOSemiLotMMSDto>?>> GetWOSemoLot(WOSemiLotMMSDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WOSemiLotMMSDto>?>();
                string proc = "Usp_WOSemiLotMMS_GetForFQC";
                var param = new DynamicParameters();
                param.Add("@WOId", model.WOId);
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
        #endregion

        #region WO SemiLot FQC
        public async Task<ResponseModel<IEnumerable<WOSemiLotFQCDto>?>> GetWoSemiLotFQC(WOSemiLotFQCDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WOSemiLotFQCDto>?>();
                string proc = "Usp_WOSemiLotFQC_GetAll"; var param = new DynamicParameters();
                param.Add("@WOProcessId", model.WOProcessId);
                param.Add("@WOProcessStaffId", model.WOProcessStaffId);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOSemiLotFQCDto>(proc, param);
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

        public async Task<ResponseModel<WOSemiLotFQCDto?>> CreateSemiLot(WOSemiLotFQCDto model)
        {
            var returnData = new ResponseModel<WOSemiLotFQCDto?>();

            string proc = "Usp_WOSemiLotFQC_Create";
            var param = new DynamicParameters();
            param.Add("@WOSemiLotFQCId", model.WOSemiLotFQCId);
            param.Add("@WOProcessId", model.WOProcessId);
            param.Add("@WOProcessStaffId", model.WOProcessStaffId);
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
                    returnData = await GetByIdWOSemiLotFQC(model.WOSemiLotFQCId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<WOSemiLotFQCDto?>> GetByIdWOSemiLotFQC(long? WOSemiLotFQCId)
        {
            var returnData = new ResponseModel<WOSemiLotFQCDto?>();
            string proc = "Usp_WOSemiLotFQC_GetById";
            var param = new DynamicParameters();
            param.Add("@WOSemiLotFQCId", WOSemiLotFQCId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOSemiLotFQCDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }

        public async Task<ResponseModel<WOSemiLotFQCDto?>> DeleteSemiLot(WOSemiLotFQCDto model)
        {
            string proc = "Usp_WOSemiLotFQC_Detele";
            var param = new DynamicParameters();
            param.Add("@WOSemiLotFQCId", model.WOSemiLotFQCId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<WOSemiLotFQCDto?>();
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
        #endregion

        #region WO SemiLot FQC Detail
        public async Task<ResponseModel<IEnumerable<WOSemiLotFQCDetailDto>?>> GetWoSemiLotFQCDetail(WOSemiLotFQCDetailDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WOSemiLotFQCDetailDto>?>();
                string proc = "Usp_WOSemiLotFQCDetail_GetAll"; var param = new DynamicParameters();
                param.Add("@WOSemiLotFQCId", model.WOSemiLotFQCId);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOSemiLotFQCDetailDto>(proc, param);
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
        public async Task<ResponseModel<IEnumerable<MaterialLotDto>?>> GetWomaterialLot(MaterialLotDto model)
        {
          try
            {
                var returnData = new ResponseModel<IEnumerable<MaterialLotDto>?>();
                string proc = "Usp_WOMaterialLot_GetForSelect"; 
                var param = new DynamicParameters();
                param.Add("@MaterialLotCode", model.MaterialLotCode);
                param.Add("@WOProcessId", model.WOProcessId);
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
        public async Task<ResponseModel<WOSemiLotFQCDetailDto?>> CreateSemiLotDetail(WOSemiLotFQCDetailDto model)
        {
            var returnData = new ResponseModel<WOSemiLotFQCDetailDto?>();

            string proc = "Usp_WOSemiLotFQCDetail_Create";
            var param = new DynamicParameters();
            param.Add("@WOSemiLotDetailId", model.WOSemiLotDetailId);
            param.Add("@WOSemiLotFQCId", model.WOSemiLotFQCId);
            param.Add("@MaterialLotCode", model.MaterialLotCode);
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
                    returnData = await GetByIdWOSemiLotFQCDetail(model.WOSemiLotDetailId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }
        public async Task<ResponseModel<WOSemiLotFQCDetailDto?>> GetByIdWOSemiLotFQCDetail(long? WOSemiLotDetailId)
        {
            var returnData = new ResponseModel<WOSemiLotFQCDetailDto?>();
            string proc = "Usp_WOSemiLotFQCDetail_GetById";
            var param = new DynamicParameters();
            param.Add("@WOSemiLotDetailId", WOSemiLotDetailId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOSemiLotFQCDetailDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "NO DATA";
            }
            return returnData;
        }
        public async Task<WOSemiLotFQCDetailDto> CheckLot(string MaterialLotCode)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@MaterialLotCode", MaterialLotCode);

                var sql = "SELECT * FROM MaterialLot ml where ml.MaterialLotCode = @MaterialLotCode ";

                var data = await _sqlDataAccess.LoadDataFirstOrDefaultAsync<WOSemiLotFQCDetailDto>(sql, param);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ResponseModel<WOSemiLotFQCDetailDto?>> DeleteSemiLotDetail(WOSemiLotFQCDetailDto model)
        {
            string proc = "Usp_WOSemiLotFQCDetail_Detele";
            var param = new DynamicParameters();
            param.Add("@WOSemiLotDetailId", model.WOSemiLotDetailId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<WOSemiLotFQCDetailDto?>();
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
        public async Task<ResponseModel<WOSemiLotFQCDetailDto?>> DeleteSemiLotDetailHB(WOSemiLotFQCDetailDto model)
        {
            string proc = "Usp_WOSemiLotFQCDetailHB_Detele";
            var param = new DynamicParameters();
            param.Add("@WOSemiLotDetailId", model.WOSemiLotDetailId);
            param.Add("@SemiLotCode", model.SemiLotCode);
            param.Add("@MaterialLotCode", model.MaterialLotCode);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<WOSemiLotFQCDetailDto?>();
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
        public async Task<ResponseModel<WOSemiLotFQCDetailDto?>> StopInheritanceSemiLotDetail(WOSemiLotFQCDetailDto model)
        {
            var returnData = new ResponseModel<WOSemiLotFQCDetailDto?>();

            string proc = "Usp_WOSemiLotFQCDetail_StopInheritance";
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
                    returnData = await GetByIdWOSemiLotFQCDetail(model.WOSemiLotDetailId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }
        public async Task<WOProcessDto> GetProcess(long? WOProcessId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@WOProcessId", WOProcessId);

                var sql = "SELECT * FROM WOProcess  where WOProcessId = @WOProcessId ";

                var data = await _sqlDataAccess.LoadDataFirstOrDefaultAsync<WOProcessDto>(sql, param);
                return data;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public async Task<bool> CheckMaxSemilot(long? WOSemiLotFQCId, long? WOProcessId)
        {
            try
            {
                string proc = @"SELECT CASE   
                                    WHEN WOSemiLotFQCId IS NULL THEN 'False'   ELSE 'TRUE'  END AS MaxSemi 
                                    FROM  WOSemiLotFQC WHERE WOSemiLotFQCId = @WOSemiLotFQCId  
                                    AND  WOSemiLotFQCId = (SELECT MAX(WOSemiLotFQCId) 
                                    FROM WOSemiLotFQC WHERE WOProcessId = @WOProcessId)";
                var param = new DynamicParameters();
                param.Add("@WOProcessId", WOProcessId);
                param.Add("@WOSemiLotFQCId", WOSemiLotFQCId);

                var data = await _sqlDataAccess.LoadDataExecuteScalarAsync<bool>(proc, param);
                return data;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<WOProcessMaxLevelDto> NameBTPFQC(long? WOId, int ProcessLevel)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@WOId", WOId);
                param.Add("@ProcessLevel", ProcessLevel);

                var sql = @"SELECT top 1 (A.ProcessLevel) ProcessLevel,(a.ProcessCode) As NameProcess,(p.ProductCode) As Product
                                FROM WOProcess AS a
                                join WO as b on a.WOId = b.WOId
                             join Product as p on b.ProductCode = p.ProductCode
                                WHERE a.WOId = @WOId
                                AND a.ProcessLevel < @ProcessLevel and a.isActived = 1 and a.AreaCode = 'FQC'
                                order by ProcessLevel desc; ";

                var data = await _sqlDataAccess.LoadDataFirstOrDefaultAsync<WOProcessMaxLevelDto>(sql, param);
                return data;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<WOProcessMaxLevelDto> NameMinBTPMMS(long? WOId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@WOId", WOId);

                var sql = @"SELECT top 1 (A.ProcessLevel) ProcessLevel,(a.ProcessCode) As NameProcess,(p.ProductCode) As Product
                                FROM WOProcess AS a
                                join WO as b on a.WOId = b.WOId
                             join Product as p on  b.ProductCode = p.ProductCode
                                WHERE a.WOId = @WOId
                                AND a.isActived = 1 and a.AreaCode = 'FQC'
                                order by ProcessLevel ASC; ";

                var data = await _sqlDataAccess.LoadDataFirstOrDefaultAsync<WOProcessMaxLevelDto>(sql, param);
                return data;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public async Task<ProductDto> GetProduct(string ProductCode)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ProductCode", ProductCode);

                var sql = "SELECT top 1 * FROM Product where ProductCode = @ProductCode ";

                var data = await _sqlDataAccess.LoadDataFirstOrDefaultAsync<ProductDto>(sql, param);
                return data;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public async Task<WOProcessMaxLevelDto> ProcessMax(long? WOId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@WOId", WOId);

                var sql = @"SELECT top 1 (A.ProcessLevel) ProcessLevel,(a.ProcessCode) As NameProcess,(p.ProductCode) As Product
                                FROM WOProcess AS a
                                join WO as b on a.WOId = b.WOId
	                            join Product as p on b.ProductCode = p.ProductCode
                                WHERE a.WOId = @WOId
                                AND  a.isActived = 1 and a.AreaCode = 'FQC'
                                order by ProcessLevel desc; ";

                var data = await _sqlDataAccess.LoadDataFirstOrDefaultAsync<WOProcessMaxLevelDto>(sql, param);
                return data;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion

        #region WO SemiLot FQC QC
        public async Task<ResponseModel<IEnumerable<WOSemiLotFQCQCDto>?>> GetWoSemiLotFQCQC(WOSemiLotFQCQCDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WOSemiLotFQCQCDto>?>();
                string proc = "Usp_WOSemiLotFQCQC_GetAll";
                var param = new DynamicParameters();
                param.Add("@SemiLotCode", model.SemiLotCode);
                param.Add("@MaterialLotCode", model.MaterialLotCode);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOSemiLotFQCQCDto>(proc, param);
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

        public async Task<ResponseModel<WOSemiLotFQCQCDto?>> CreateSemiLotFQCQC(WOSemiLotFQCQCDto model)
        {
            var returnData = new ResponseModel<WOSemiLotFQCQCDto?>();
            var jsonLotList = JsonConvert.SerializeObject(model.CheckValue);
            string proc = "Usp_WOSemiLotFQCQC_Create";
            var param = new DynamicParameters();
            param.Add("@WOSemiLotFQCQCId", model.WOSemiLotFQCQCId);
            param.Add("@SemiLotCode", model.SemiLotCode);
            param.Add("@MaterialLotCode", model.MaterialLotCode);
            param.Add("@CheckQty", model.ActualQty);
            param.Add("@OKQty", model.OKQty);
            param.Add("@NGQty", model.NGQty);
            param.Add("@RemainQty", model.RemainQty);
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
                    // returnData = await GetByIdWOSemiLotFQC(model.WOSemiLotFQCId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<WOSemiLotFQCQCDto?>> DeleteSemiLotFQCQC(WOSemiLotFQCQCDto model)
        {
            string proc = "Usp_WOSemiLotFQCQC_Delete";
            var param = new DynamicParameters();
            param.Add("@WOSemiLotFQCQCId", model.WOSemiLotFQCQCId);
            param.Add("@row_version", model.row_version);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var returnData = new ResponseModel<WOSemiLotFQCQCDto?>();
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
                    returnData.ResponseMessage = result;
                    break;
            }

            return returnData;
        }
        public async Task<ResponseModel<IEnumerable<QCAPPDetailDto>?>> GetWoSemiLotFQCListDetailQC(QCAPPDetailDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<QCAPPDetailDto>?>();
                string proc = "Usp_WOSemiLotFQCCheckQCDetail_Get";
                var param = new DynamicParameters();
                param.Add("@SemiLotCode", model.SemiLotCode);
                param.Add("@MaterialLotCode", model.MaterialLotCode);
                param.Add("@QCFQCMasterId", model.QCFQCMasterId);


                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCAPPDetailDto>(proc, param);
                returnData.Data = data;
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
        #endregion

        //#region OQC
        //public async Task<ResponseModel<IEnumerable<WOSemiLotFQCDto>?>> GetWaitSemiLotOQC(WOSemiLotFQCDto model)
        //{
        //    try
        //    {
        //        var returnData = new ResponseModel<IEnumerable<WOSemiLotFQCDto>?>();
        //        string proc = "Usp_WOSemiLotFQCWaitOQC_GetAll"; var param = new DynamicParameters();
        //        param.Add("@WOProcessId", model.WOProcessId);
        //        param.Add("@SemiLotCode", model.SemiLotCode);
        //        param.Add("@page", model.page);
        //        param.Add("@pageSize", model.pageSize);
        //        param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

        //        var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOSemiLotFQCDto>(proc, param);
        //        returnData.Data = data;
        //        returnData.TotalRow = param.Get<int>("totalRow");
        //        if (!data.Any())
        //        {
        //            returnData.HttpResponseCode = 204;
        //            returnData.ResponseMessage = StaticReturnValue.NO_DATA;
        //        }
        //        return returnData;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //public async Task<ResponseModel<WOSemiLotFQCDto?>> CreateSemiLotWaitOQC(WOSemiLotFQCDto model)
        //{
        //    var returnData = new ResponseModel<WOSemiLotFQCDto?>();

        //    string proc = "Usp_WOSemiLotFQCWaitOQC_Create";
        //    var param = new DynamicParameters();
        //    param.Add("@SemiLotCode", model.SemiLotCode);
        //    param.Add("@WOProcessOQCId", model.WOProcessId);
        //    param.Add("@WOProcessStaffOQCId", model.WOProcessStaffOQCId);
        //    param.Add("@createdBy", model.createdBy);
        //    param.Add("@row_version", model.row_version);
        //    param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

        //    var result = await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);

        //    returnData.ResponseMessage = result;
        //    switch (result)
        //    {
        //        case StaticReturnValue.SYSTEM_ERROR:
        //            returnData.HttpResponseCode = 500;
        //            break;
        //        case StaticReturnValue.SUCCESS:
        //            returnData = await GetByIdWOSemiLotWaitOQC(model.WOSemiLotFQCId);
        //            returnData.ResponseMessage = result;
        //            break;
        //        default:
        //            returnData.HttpResponseCode = 400;
        //            break;
        //    }
        //    return returnData;
        //}
        //public async Task<ResponseModel<WOSemiLotFQCDto?>> GetByIdWOSemiLotWaitOQC(long? WOSemiLotFQCId)
        //{
        //    try
        //    {
        //        var returnData = new ResponseModel<WOSemiLotFQCDto?>();
        //        string proc = "Usp_WOSemiLotFQCWaitOQC_GetById";
        //        var param = new DynamicParameters();
        //        param.Add("@WOSemiLotFQCId", WOSemiLotFQCId);

        //        var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOSemiLotFQCDto>(proc, param);
        //        returnData.Data = data.FirstOrDefault();
        //        if (!data.Any())
        //        {
        //            returnData.HttpResponseCode = 204;
        //            returnData.ResponseMessage = "NO DATA";
        //        }
        //        return returnData;
        //    }
        //    catch (Exception e)
        //    {

        //        throw;
        //    }

        //}
        //public async Task<ResponseModel<IEnumerable<WOSemiLotFQCDto>?>> GetSemiLotOQCList(WOSemiLotFQCDto model)
        //{
        //    try
        //    {
        //        var returnData = new ResponseModel<IEnumerable<WOSemiLotFQCDto>?>();
        //        string proc = "Usp_WOSemiLotFQCOQC_GetAll"; var param = new DynamicParameters();
        //        param.Add("@WOProcessOQCId", model.WOProcessOQCId);
        //        param.Add("@WOProcessStaffOQCId", model.WOProcessStaffOQCId);
        //        param.Add("@page", model.page);
        //        param.Add("@pageSize", model.pageSize);
        //        param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

        //        var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOSemiLotFQCDto>(proc, param);
        //        returnData.Data = data;
        //        returnData.TotalRow = param.Get<int>("totalRow");
        //        if (!data.Any())
        //        {
        //            returnData.HttpResponseCode = 204;
        //            returnData.ResponseMessage = StaticReturnValue.NO_DATA;
        //        }
        //        return returnData;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //public async Task<ResponseModel<WOSemiLotFQCDto?>> DeleteSemiLotWaitOQC(WOSemiLotFQCDto model)
        //{
        //    string proc = "Usp_WOSemiLotWaitOQC_Delete";
        //    var param = new DynamicParameters();
        //    param.Add("@WOSemiLotFQCId", model.WOSemiLotFQCId);
        //    param.Add("@row_version", model.row_version);
        //    param.Add("@createdBy", model.createdBy);
        //    param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

        //    var returnData = new ResponseModel<WOSemiLotFQCDto?>();
        //    var result = await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
        //    returnData.ResponseMessage = result;
        //    switch (result)
        //    {
        //        case StaticReturnValue.SYSTEM_ERROR:
        //            returnData.HttpResponseCode = 500;
        //            break;
        //        case StaticReturnValue.REFRESH_REQUIRED:
        //            returnData.HttpResponseCode = 500;
        //            break;
        //        case StaticReturnValue.SUCCESS:
        //            break;
        //        default:
        //            returnData.HttpResponseCode = 400;
        //            returnData.ResponseMessage = result;
        //            break;
        //    }

        //    return returnData;
        //}
        //#endregion

        //#region Check OQC
        //public async Task<ResponseModel<IEnumerable<QCOQCDetailDto>?>> GetCheckOQC(WOSemiLotFQCDto model)
        //{
        //    try
        //    {
        //        var returnData = new ResponseModel<IEnumerable<QCOQCDetailDto>?>();
        //        string proc = "Usp_WOSemiLotFQCOQC_GetCheckValue";
        //        var param = new DynamicParameters();
        //        param.Add("@WOSemiLotFQCOQCId", model.WOSemiLotFQCId);
        //        param.Add("@QCOQCMasterId", model.QCOQCMasterId);

        //        var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCOQCDetailDto>(proc, param);
        //        returnData.Data = data;
        //        if (!data.Any())
        //        {
        //            returnData.HttpResponseCode = 204;
        //            returnData.ResponseMessage = StaticReturnValue.NO_DATA;
        //        }
        //        return returnData;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public async Task<ResponseModel<WOSemiLotFQCOQCMasterDto?>> CheckOQC(WOSemiLotFQCOQCMasterDto model)
        //{
        //    var returnData = new ResponseModel<WOSemiLotFQCOQCMasterDto?>();
        //    var jsonLotList = JsonConvert.SerializeObject(model.CheckValue);
        //    string proc = "Usp_WOSemiLotFQCOQC_CheckValue";
        //    var param = new DynamicParameters();
        //    param.Add("@WOSemiLotFQCId", model.WOSemiLotFQCId);
        //    param.Add("@QCOQCMasterId", model.QCOQCMasterId);
        //    param.Add("@StaffId", model.StaffId);
        //    param.Add("@CheckDate", model.CheckDate);
        //    param.Add("@CheckResult", model.CheckResult);
        //    param.Add("@Jsonlist", jsonLotList);
        //    param.Add("@createdBy", model.createdBy);
        //    param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

        //    var result = await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);

        //    returnData.ResponseMessage = result;
        //    switch (result)
        //    {
        //        case StaticReturnValue.SYSTEM_ERROR:
        //            returnData.HttpResponseCode = 500;
        //            break;
        //        case StaticReturnValue.SUCCESS:
        //            returnData.ResponseMessage = result;
        //            break;
        //        default:
        //            returnData.HttpResponseCode = 400;
        //            break;
        //    }
        //    return returnData;
        //}
        //#endregion

        #region ADD Replacement
        public async Task<ResponseModel<IEnumerable<WOSemiLotFQCDto>?>> GetWOSemiLotReplacement(WOSemiLotFQCDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WOSemiLotFQCDto>?>();
                string proc = "Usp_WOSemiLotFQCReplacement_GetAll";
                var param = new DynamicParameters();
                param.Add("@WOSemiLotFQCId", model.WOSemiLotFQCId);
                param.Add("@SemiLotCode", model.SemiLotCode);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOSemiLotFQCDto>(proc, param);
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
        public async Task<ResponseModel<WOSemiLotFQCDetailDto?>> CreateWOSemiLotReplacement(WOSemiLotFQCDetailDto model)
        {
            var returnData = new ResponseModel<WOSemiLotFQCDetailDto?>();

            string proc = "Usp_WOSemiLotFQCReplacement_Create";
            var param = new DynamicParameters();
            param.Add("@WOSemiLotDetailId", model.WOSemiLotDetailId);
            param.Add("@WOSemiLotFQCId", model.WOSemiLotFQCId);
            param.Add("@MaterialLotCode", model.MaterialLotCode);
            param.Add("@AddQty", model.AddQty);
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
                    returnData = await GetByIdWOSemiLotFQCDetail(model.WOSemiLotDetailId);

                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }
        #endregion

        #region General
        public async Task<WOSemiLotFQCDto> GetSemiLotCode(string SemiLotCode)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@SemiLotCode", SemiLotCode);

                var sql = "SELECT * FROM WOSemiLotFQC  where SemiLotCode = @SemiLotCode ";

                var data = await _sqlDataAccess.LoadDataFirstOrDefaultAsync<WOSemiLotFQCDto>(sql, param);
                return data;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public async Task<bool> CheckShiftOfStaff(long? WOProcessStaffId)
        {
            try
            {
                string proc = @"EXEC Usp_WOCheckShiftStaff_GetById @WOProcessStaffId";
                var param = new DynamicParameters();
                param.Add("@WOProcessStaffId", WOProcessStaffId);

                var data = await _sqlDataAccess.LoadDataExecuteScalarAsync<bool>(proc, param);
                return data;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<WOSemiLotFQCDto> GetSemiLotCodeById(long? WOSemiLotFQCId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@WOSemiLotFQCId", WOSemiLotFQCId);

                var sql = "SELECT * FROM WOSemiLotFQC  where WOSemiLotFQCId = @WOSemiLotFQCId ";

                var data = await _sqlDataAccess.LoadDataFirstOrDefaultAsync<WOSemiLotFQCDto>(sql, param);
                return data;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        //public async Task<ResponseModel<WOProcessDto?>> GetListForProcess(long? WOId)
        //{
        //    try
        //    {
        //        var returnData = new ResponseModel<WOProcessDto?>();
        //        var param = new DynamicParameters();
        //        param.Add("@WOId", WOId);

        //        var sql = "SELECT * FROM WOProcess   WHERE WOId=@WOId AND LocationName = 'FQC' AND ProcessCode != 'OQC' AND isActived=1; ";
        //        var data = await _sqlDataAccess.LoadDataUsingRawQuery<WOProcessDto>(sql, param);
        //        var data1 = data.FirstOrDefault();
        //        if (!data.Any())
        //        {
        //            returnData.HttpResponseCode = 204;
        //            returnData.ResponseMessage = "NO DATA";
        //        }
        //        return returnData;
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //}
        public async Task<ResponseModel<IEnumerable<WOProcessWorkedDto>?>> GetWOProcessWorked(string SemiLotCode, long? WOId)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<WOProcessWorkedDto>?>();

                string proc = "Usp_GetWOProcessWorked";
                var param = new DynamicParameters();
                param.Add("@SemiLotCode", SemiLotCode);
                param.Add("@WOId", WOId);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<WOProcessWorkedDto>(proc, param);
                returnData.Data = data;
                if (!data.Any())
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                }
                return returnData;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> CheckSemiLotMapping(string SemiLotCode)
        {
            try
            {
                string proc = @" SELECT CASE  WHEN count(wlm.WOSemiLotDetailId) > 0 THEN  1
                                            ELSE 0
                                        END AS HasProfile 
                                FROM WOSemiLotFQCDetail wlm
                                WHERE wlm.MaterialLotCode = @SemiLotCode ";
                var param = new DynamicParameters();
                param.Add("@SemiLotCode", SemiLotCode);

                var data = await _sqlDataAccess.LoadDataExecuteScalarAsync<bool>(proc, param);
                return data;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
    }
}
