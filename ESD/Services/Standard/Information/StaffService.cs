using Dapper;
using Newtonsoft.Json;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Models;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using System.Data;
using static ESD.Extensions.ServiceExtensions;
using ESD.Helpers;

namespace ESD.Services.Standard.Information
{
    public interface IStaffService
    {
        Task<ResponseModel<IEnumerable<StaffDto>?>> Get(StaffDto model);
        Task<ResponseModel<IEnumerable<StaffDto>?>> GetAll(StaffDto model);
        Task<string> Create(StaffDto model);
        Task<ResponseModel<StaffDto?>> GetById(long staffid);
        Task<string> Modify(StaffDto model);
        Task<string> Delete(StaffDto model);
        Task<ResponseModel<IEnumerable<StaffDto>?>> GetActive(StaffDto model);
        Task<ResponseModel<StaffDto?>> CreateByExcel(List<StaffDto> model, long userCreate);
        Task<ResponseModel<IEnumerable<StaffDto>?>> GetListPrintQR(List<long>? listQR);
    }

    [ScopedRegistration]
    public class StaffService : IStaffService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public StaffService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public async Task<ResponseModel<IEnumerable<StaffDto>?>> Get(StaffDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<StaffDto>?>();
                var proc = $"Usp_Staff_Get";
                var param = new DynamicParameters();
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@DeptId", model.DeptId);
                param.Add("@StaffCode", model.StaffCode);
                param.Add("@StaffName", model.StaffName);
                param.Add("@isActived", model.isActived);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);
                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<StaffDto>(proc, param);

                if (!data.Any())
                {
                    returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                    returnData.HttpResponseCode = 204;
                }
                else
                {
                    returnData.Data = data;
                    returnData.TotalRow = param.Get<int>("totalRow");
                }

                return returnData;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<ResponseModel<IEnumerable<StaffDto>?>> GetAll(StaffDto model)
        {
            var returnData = new ResponseModel<IEnumerable<StaffDto>?>();
            var proc = $"Usp_Staff_GetAll";
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<StaffDto>(proc);

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
        public async Task<string> Create(StaffDto model)
        {
            string proc = "Usp_Staff_Create";
            var param = new DynamicParameters();
            param.Add("@StaffId", model.StaffId);
            param.Add("@StaffCode", model.StaffCode);
            param.Add("@StaffName", model.StaffName);
            param.Add("@Contact", model.Contact);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            return await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
        }
        public async Task<ResponseModel<StaffDto?>> GetById(long staffid)
        {
            try
            {
                var returnData = new ResponseModel<StaffDto?>();
                var proc = $"Usp_Staff_GetById";
                var param = new DynamicParameters();
                param.Add("@StaffId", staffid);
                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<StaffDto>(proc, param);
                returnData.Data = data.FirstOrDefault();
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
        public async Task<string> Modify(StaffDto model)
        {
            string proc = "Usp_Staff_Modify";
            var param = new DynamicParameters();
            param.Add("@StaffId", model.StaffId);
            param.Add("@StaffCode", model.StaffCode);
            param.Add("@StaffName", model.StaffName);
            param.Add("@Contact", model.Contact);
            param.Add("@modifiedBy", model.modifiedBy);
            param.Add("@row_version", model.row_version);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            return await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
        }
        public async Task<string> Delete(StaffDto model)
        {
            string proc = "Usp_Staff_Delete";
            var param = new DynamicParameters();
            param.Add("@StaffId", model.StaffId);
            param.Add("@modifiedBy", model.modifiedBy);
            param.Add("@isActived", model.isActived);
            param.Add("@row_version", model.row_version);

            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            return await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
        }
        public async Task<ResponseModel<IEnumerable<StaffDto>?>> GetActive(StaffDto model)
        {
            var returnData = new ResponseModel<IEnumerable<StaffDto>?>();
            var proc = $"Usp_Staff_GetActive";
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<StaffDto>(proc);

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
        public async Task<ResponseModel<StaffDto?>> CreateByExcel(List<StaffDto> model, long userCreate)
        {
            var returnData = new ResponseModel<StaffDto?>();

            var jsonLotList = JsonConvert.SerializeObject(model);

            string proc = "Usp_Staff_CreateByExcel";
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

        public async Task<ResponseModel<IEnumerable<StaffDto>?>> GetListPrintQR(List<long>? listQR)
        {
            var returnData = new ResponseModel<IEnumerable<StaffDto>?>();
            var proc = $"Usp_Staff_GetPrint";
            var param = new DynamicParameters();
            param.Add("@listQR", ParameterTvp.GetTableValuedParameter_BigInt(listQR));
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<StaffDto>(proc, param);
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
