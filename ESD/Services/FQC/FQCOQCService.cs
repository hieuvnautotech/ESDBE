using Dapper;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Models.Dtos.APP;
using ESD.Models.Dtos.Common;
using ESD.Models.Dtos.FQC;
using ESD.Models.Dtos.Slitting;
using Newtonsoft.Json;
using System.Data;
using static ESD.Extensions.ServiceExtensions;

namespace ESD.Services.APP
{
    public interface IFQCOQCService
    {
        Task<ResponseModel<IEnumerable<OQCDto>?>> GetAll(WOSemiLotFQCDto model);
        Task<ResponseModel<IEnumerable<WOSemiLotFQCDto>?>> GetDetail(WOSemiLotFQCDto model);
        Task<ResponseModel<WOSemiLotFQCDto?>> ScanLot(WOSemiLotFQCDto model);
        Task<ResponseModel<WOSemiLotFQCDto?>> Delete(WOSemiLotFQCDto model);

        Task<ResponseModel<OQCCheckDto?>> GetCheckQC(OQCDto model);
        Task<ResponseModel<OQCCheckDto?>> CheckQC(OQCCheckDto model);
    }
    [ScopedRegistration]
    public class FQCOQCService : IFQCOQCService
    {
        private readonly ISqlDataAccess _sqlDataAccess;
        public FQCOQCService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<ResponseModel<IEnumerable<OQCDto>?>> GetAll(WOSemiLotFQCDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<OQCDto>?>();
                string proc = "Usp_FQCOQC_Get"; 
                var param = new DynamicParameters();
                param.Add("@WOCode", model.WOCode);
                param.Add("@ProductCode", model.ProductCode);
                param.Add("@PressLotCode", model.PressLotCode);
                param.Add("@SemiLotCode", model.SemiLotCode);
                param.Add("@status", model.CheckResult);
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<OQCDto>(proc, param);
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

        public async Task<ResponseModel<IEnumerable<WOSemiLotFQCDto>?>> GetDetail(WOSemiLotFQCDto model)
        {
            var returnData = new ResponseModel<IEnumerable<WOSemiLotFQCDto>?>();

            var proc = $"Usp_FQCOQC_GetDetail";
            var param = new DynamicParameters();
            param.Add("@PressLotCode", model.PressLotCode);
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

        public async Task<ResponseModel<WOSemiLotFQCDto?>> ScanLot(WOSemiLotFQCDto model)
        {
            var returnData = new ResponseModel<WOSemiLotFQCDto?>();

            string proc = "Usp_FQCOQC_Scan";
            var param = new DynamicParameters();
            param.Add("@PressLotCode", model.PressLotCode);
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
                    break;
                default:
                    returnData.ResponseMessage = result;
                    returnData.HttpResponseCode = 400;
                    break;
            }
            return returnData;
        }

        public async Task<ResponseModel<WOSemiLotFQCDto?>> Delete(WOSemiLotFQCDto model)
        {
            string proc = "Usp_FQCOQC_Delete";
            var param = new DynamicParameters();
            param.Add("@PressLotCode", model.PressLotCode);
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

        #region Check QC
        public async Task<ResponseModel<OQCCheckDto?>> GetCheckQC(OQCDto model)
        {
            try
            {
                var returnData = new ResponseModel<OQCCheckDto?>();
                string proc = "Usp_FQCOQC_GetCheckValue";
                var param = new DynamicParameters();
                param.Add("@PressLotCode", model.PressLotCode);
                param.Add("@CheckType", model.CheckType);

                var data = await _sqlDataAccess.LoadMultiDataSetUsingStoredProcedure<OQCDto, QCOQCDetailDto>(proc, param);
                if (!data.Item1.Any() && !data.Item2.Any())
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                }
                else
                {
                    returnData.Data = new OQCCheckDto
                    {
                        Master = data.Item1.First(),
                        Detail = data.Item2
                    };
                }

                return returnData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseModel<OQCCheckDto?>> CheckQC(OQCCheckDto model)
        {
            var returnData = new ResponseModel<OQCCheckDto?>();
            var jsonLotList = JsonConvert.SerializeObject(model.Detail);
            string proc = "Usp_FQCOQC_CheckValue";
            var param = new DynamicParameters();
            param.Add("@PressLotCode", model.Master.PressLotCode);
            param.Add("@QCOQCMasterId", model.Master.QCOQCMasterId);
            param.Add("@StaffId", model.Master.StaffId);
            param.Add("@CheckDate", model.Master.CheckDate);
            param.Add("@CheckType", model.Master.CheckType);
            param.Add("@CheckResult", model.Master.CheckResult);
            param.Add("@Jsonlist", jsonLotList);
            param.Add("@createdBy", model.Master.createdBy);
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
