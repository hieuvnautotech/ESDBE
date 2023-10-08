using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Models;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Services.Base;
using System.Data;
using static ESD.Extensions.ServiceExtensions;

namespace ESD.Services.Common.Standard.Information
{
    public interface IQCDetailService
    {
        Task<ResponseModel<IEnumerable<QCDetailDto>?>> GetAll(QCDetailDto pageInfo);
        Task<ResponseModel<QCDetailDto?>> GetById(long QCDetailId);
        Task<string> Create(QCDetailDto model);
        Task<string> Modify(QCDetailDto model);
        Task<string> Delete(QCDetailDto model);
        Task<ResponseModel<QCDetailDto?>> CreateByExcel(List<QCDetailExcelDto> model, long userCreate);

    }
    [ScopedRegistration]
    public class QCDetailService : IQCDetailService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public QCDetailService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public async Task<ResponseModel<IEnumerable<QCDetailDto>?>> GetAll(QCDetailDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<QCDetailDto>?>();
                string proc = "Usp_QCDetail_GetAll"; var param = new DynamicParameters();
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);
                param.Add("@QCMasterId", model.QCMasterId);
                param.Add("@QCId", model.QCId);
                param.Add("@showDelete", model.showDelete);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCDetailDto>(proc, param);
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
        public async Task<ResponseModel<QCDetailDto?>> GetById(long QCDetailId)
        {
            var returnData = new ResponseModel<QCDetailDto?>();
            var proc = $"Usp_QCDetail_GetById";
            var param = new DynamicParameters();
            param.Add("@QCDetailId", QCDetailId);
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCDetailDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
            }
            return returnData;
        }
        public async Task<string> Create(QCDetailDto model)
        {
            try
            {
                string proc = "Usp_QCDetail_Create";
                var param = new DynamicParameters();
                param.Add("@QCDetailId", model.QCDetailId);
                param.Add("@QCMasterId", model.QCMasterId);
                param.Add("@QCId", model.QCId);
                param.Add("@createdBy", model.createdBy);
                param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

                return await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
            }
            catch (Exception e)
            {

                throw;
            }
       
        }

        public async Task<string> Modify(QCDetailDto model)
        {
            string proc = "Usp_QCDetail_Modify";
            var param = new DynamicParameters();
            param.Add("@QCDetailId", model.QCDetailId);
            param.Add("@QCMasterId", model.QCMasterId);
            param.Add("@QCId", model.QCId);
            param.Add("@modifiedBy", model.modifiedBy);
            param.Add("@row_version", model.row_version);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            return await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
        }

        public async Task<string> Delete(QCDetailDto model)
        {
            string proc = "Usp_QCDetail_Delete";
            var param = new DynamicParameters();
            param.Add("@QCDetailId", model.QCDetailId);
            param.Add("@row_version", model.row_version);
            param.Add("@modifiedBy", model.modifiedBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            return await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
        }

        public async Task<ResponseModel<QCDetailDto?>> CreateByExcel(List<QCDetailExcelDto> model, long userCreate)
        {
            var returnData = new ResponseModel<QCDetailDto?>();

            var jsonLotList = JsonConvert.SerializeObject(model);

            string proc = "Usp_QCDetail_CreateByExcel";
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
    }
}
