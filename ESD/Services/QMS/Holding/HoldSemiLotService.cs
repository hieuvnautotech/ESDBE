using Dapper;
using ESD.Extensions;
using ESD.Models.Dtos.Common;
using ESD.Models.Dtos;
using System.Data;
using static ESD.Extensions.ServiceExtensions;
using ESD.DbAccess;
using ESD.Helpers;
using Newtonsoft.Json;
using ESD.Models.Dtos.FQC;
using ESD.Models.Dtos.MMS;

namespace ESD.Services.QMS.Holding
{
    public interface IHoldSemiLotService
    {
       Task<ResponseModel<IEnumerable<dynamic>?>> GetAllSemiFQC(WOSemiLotFQCDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetAllSemiMMS(SemiMMSDto model);
        Task<ResponseModel<HoldDto?>> HoldFQC(HoldDto model);
        Task<ResponseModel<HoldDto?>> UnHoldFQC(HoldDto model);
        Task<ResponseModel<HoldDto?>> ScrapFQC(HoldDto model);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetPrintFQC(List<long>? listQR);

        Task<ResponseModel<HoldDto?>> HoldMMS(HoldDto model);
        Task<ResponseModel<HoldDto?>> UnHoldMMS(HoldDto model);
        Task<ResponseModel<HoldDto?>> ScrapMMS(HoldDto model);
        Task<ResponseModel<IEnumerable<QCPQCDetailSLDto>?>> GetListPQCSL(long? QCPQCMasterId, long? WOSemiLotMMSId);
        Task<ResponseModel<IEnumerable<WOSemiLotMMSDetailSLDto>?>> GetValuePQCSL(long? WOSemiLotMMSId);
        Task<ResponseModel<WOSemiLotMMSCheckMasterSLDto?>> CheckPQCSL(WOSemiLotMMSCheckMasterSLDto model);
    }
    [ScopedRegistration]
    public class HoldSemiLotService : IHoldSemiLotService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public HoldSemiLotService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetAllSemiFQC(WOSemiLotFQCDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_HoldSemiLotFQC_GetAll";
                var param = new DynamicParameters();
                param.Add("@WorkOrder", model.WorkOrder);
                param.Add("@SemiLotCode", model.SemiLotCode);
                param.Add("@FQCSOName", model.LotStatus);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@isActived", model.isActived);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);
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
        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetAllSemiMMS(SemiMMSDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<dynamic>?>();
                string proc = "Usp_HoldSemiLotMMS_GetAll";
                var param = new DynamicParameters();
                param.Add("@WorkOrder", model.WorkOrder);
                param.Add("@SemiLotCode", model.SemiLotCode);
                param.Add("@ReceivedDate", model.ReceivedDate?.ToString("yyyy-MM-dd"));
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@isActived", model.isActived);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);
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
        public async Task<ResponseModel<HoldDto?>> HoldFQC(HoldDto model)
        {
            var returnData = new ResponseModel<HoldDto?>();

            string proc = "Usp_HoldSemiLotFQC_Hold";
            var param = new DynamicParameters();
            //param.Add("@HoldLogId", model.HoldLogId);
            //param.Add("@WOSemiLotFQCId", model.WOSemiLotFQCId);
            param.Add("@ListId", ParameterTvp.GetTableValuedParameter_BigInt(model.ListId));
            param.Add("@Reason", model.Reason);
            param.Add("@FileName", model.FileName);
            param.Add("@IsPicture", model.IsPicture);
            //param.Add("@row_version", model.row_version);
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
                    //returnData = await GetById(model.QCPQCMasterId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }
        public async Task<ResponseModel<HoldDto?>> UnHoldFQC(HoldDto model)
        {
            var returnData = new ResponseModel<HoldDto?>();

            string proc = "Usp_HoldSemiLotFQC_UnHold";
            var param = new DynamicParameters();
            //param.Add("@HoldLogId", model.HoldLogId);
            //param.Add("@WOSemiLotFQCId", model.WOSemiLotFQCId);
            param.Add("@ListId", ParameterTvp.GetTableValuedParameter_BigInt(model.ListId));
            param.Add("@Reason", model.Reason);
            param.Add("@FileName", model.FileName);
            param.Add("@IsPicture", model.IsPicture);
            //param.Add("@row_version", model.row_version);
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
                    //returnData = await GetById(model.QCPQCMasterId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }
        public async Task<ResponseModel<HoldDto?>> ScrapFQC(HoldDto model)
        {

            try
            {
                var returnData = new ResponseModel<HoldDto?>();

                string proc = "Usp_HoldSemiLotFQC_Scrap";
                var param = new DynamicParameters();
                //param.Add("@HoldLogId", model.HoldLogId);
                //param.Add("@WOSemiLotFQCId", model.WOSemiLotFQCId);
                param.Add("@ListId", ParameterTvp.GetTableValuedParameter_BigInt(model.ListId));
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
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetPrintFQC(List<long>? listQR)
        {
            var returnData = new ResponseModel<IEnumerable<dynamic>?>();
            var proc = $"Usp_HoldSemiLot_GetPrint";
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

        #region MMS
        public async Task<ResponseModel<HoldDto?>> HoldMMS(HoldDto model)
        {
            var returnData = new ResponseModel<HoldDto?>();

            string proc = "Usp_HoldSemiLotMMS_Hold";
            var param = new DynamicParameters();
            //param.Add("@HoldLogId", model.HoldLogId);
            //param.Add("@WOSemiLotMMSId", model.WOSemiLotMMSId);
            param.Add("@ListId", ParameterTvp.GetTableValuedParameter_BigInt(model.ListId));
            param.Add("@Reason", model.Reason);
            param.Add("@FileName", model.FileName);
            param.Add("@IsPicture", model.IsPicture);
            //param.Add("@row_version", model.row_version);
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
                    //returnData = await GetById(model.QCPQCMasterId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }
        public async Task<ResponseModel<HoldDto?>> UnHoldMMS(HoldDto model)
        {
            var returnData = new ResponseModel<HoldDto?>();

            string proc = "Usp_HoldSemiLotMMS_UnHold";
            var param = new DynamicParameters();
            //param.Add("@HoldLogId", model.HoldLogId);
            //param.Add("@WOSemiLotMMSId", model.WOSemiLotMMSId);
            param.Add("@ListId", ParameterTvp.GetTableValuedParameter_BigInt(model.ListId));
            param.Add("@Reason", model.Reason);
            param.Add("@FileName", model.FileName);
            param.Add("@IsPicture", model.IsPicture);
            //param.Add("@row_version", model.row_version);
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
                    //returnData = await GetById(model.QCPQCMasterId);
                    returnData.ResponseMessage = result;
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }
        public async Task<ResponseModel<HoldDto?>> ScrapMMS(HoldDto model)
        {

            try
            {
                var returnData = new ResponseModel<HoldDto?>();

                string proc = "Usp_HoldSemiLotMMS_Scrap";
                var param = new DynamicParameters();
                //param.Add("@HoldLogId", model.HoldLogId);
                //param.Add("@WOSemiLotMMSId", model.WOSemiLotMMSId);
                param.Add("@ListId", ParameterTvp.GetTableValuedParameter_BigInt(model.ListId));
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
            catch (Exception)
            {
                throw;
            }
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
                string proc = "Usp_HoldSemiLotMMS_GetReCheck";
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

            string proc = "Usp_HoldSemiLotMMS_ReCheck";
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
                    break;
            }
            return returnData;
        }
        #endregion
    }
}
