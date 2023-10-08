using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Models;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Models.Dtos.StandardQC;
using ESD.Services.Base;
using System.Data;
using static ESD.Extensions.ServiceExtensions;

namespace ESD.Services.Standard.Information.StandardQC
{
    public interface IQCToolService
    {
        Task<ResponseModel<IEnumerable<QCToolDto>?>> GetAll(QCToolDto pageInfo);
        Task<ResponseModel<IEnumerable<QCToolDto>?>> GetActive(QCToolDto model);
        Task<ResponseModel<QCToolDto?>> GetById(long QCToolId);
        Task<string> Create(QCToolDto model);
        Task<string> Modify(QCToolDto model);
        Task<string> Delete(QCToolDto model);
     

    }
    [ScopedRegistration]
    public class QCToolService : IQCToolService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public QCToolService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public async Task<ResponseModel<IEnumerable<QCToolDto>?>> GetAll(QCToolDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<QCToolDto>?>();
                string proc = "Usp_QCTool_GetAll"; var param = new DynamicParameters();
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);
                param.Add("@QCName", model.QCName);
                param.Add("@QCApply", model.QCApply);
                param.Add("@showDelete", model.showDelete);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCToolDto>(proc, param);
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
        public async Task<ResponseModel<QCToolDto?>> GetById(long QCToolId)
        {
            var returnData = new ResponseModel<QCToolDto?>();
            var proc = $"Usp_QCTool_GetById";
            var param = new DynamicParameters();
            param.Add("@QCToolId", QCToolId);
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCToolDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
            }
            return returnData;
        }
        public async Task<string> Create(QCToolDto model)
        {
            try
            {
                string proc = "Usp_QCTool_Create";
                var param = new DynamicParameters();
                param.Add("@QCToolId", model.QCToolId);
                param.Add("@QCTypeId", model.QCTypeId);
                param.Add("@QCItemId", model.QCItemId);
                param.Add("@QCStandardId", model.QCStandardId);
                param.Add("@QCName", model.QCName);
                param.Add("@QCApply", model.QCApply);
                param.Add("@createdBy", model.createdBy);
                param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

                return await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task<string> Modify(QCToolDto model)
        {
            string proc = "Usp_QCTool_Modify";
            var param = new DynamicParameters();
            param.Add("@QCToolId", model.QCToolId);
            param.Add("@QCTypeId", model.QCTypeId);
            param.Add("@QCItemId", model.QCItemId);
            param.Add("@QCStandardId", model.QCStandardId);
            param.Add("@QCName", model.QCName);
            param.Add("@QCApply", model.QCApply);
            param.Add("@modifiedBy", model.modifiedBy);
            param.Add("@row_version", model.row_version);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            return await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
        }

        public async Task<string> Delete(QCToolDto model)
        {
            string proc = "Usp_QCTool_Delete";
            var param = new DynamicParameters();
            param.Add("@QCToolId", model.QCToolId);
            param.Add("@row_version", model.row_version);
            param.Add("@modifiedBy", model.modifiedBy);
            param.Add("@isActived", model.isActived);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            return await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
        }

        public async Task<ResponseModel<IEnumerable<QCToolDto>?>> GetActive(QCToolDto model)
        {
            var returnData = new ResponseModel<IEnumerable<QCToolDto>?>();
            var proc = $"Usp_QCTool_GetActive";
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCToolDto>(proc);

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
       

    }
}
