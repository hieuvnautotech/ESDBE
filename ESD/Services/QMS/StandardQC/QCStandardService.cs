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
    public interface IQCStandardService
    {
        Task<ResponseModel<IEnumerable<QCStandardDto>?>> GetAll(QCStandardDto pageInfo);
        Task<ResponseModel<IEnumerable<QCStandardDto>?>> GetActive(QCStandardDto model);
        Task<ResponseModel<QCStandardDto?>> GetById(long QCStandardId);
        Task<string> Create(QCStandardDto model);
        Task<string> Modify(QCStandardDto model);
        Task<string> Delete(QCStandardDto model);
     

    }
    [ScopedRegistration]
    public class QCStandardService : IQCStandardService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public QCStandardService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public async Task<ResponseModel<IEnumerable<QCStandardDto>?>> GetAll(QCStandardDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<QCStandardDto>?>();
                string proc = "Usp_QCStandard_GetAll"; var param = new DynamicParameters();
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);
                param.Add("@QCName", model.QCName);
                param.Add("@QCApply", model.QCApply);
                param.Add("@showDelete", model.showDelete);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCStandardDto>(proc, param);
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
        public async Task<ResponseModel<QCStandardDto?>> GetById(long QCStandardId)
        {
            var returnData = new ResponseModel<QCStandardDto?>();
            var proc = $"Usp_QCStandard_GetById";
            var param = new DynamicParameters();
            param.Add("@QCStandardId", QCStandardId);
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCStandardDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
            }
            return returnData;
        }
        public async Task<string> Create(QCStandardDto model)
        {
            try
            {
                string proc = "Usp_QCStandard_Create";
                var param = new DynamicParameters();
                param.Add("@QCStandardId", model.QCStandardId);
                param.Add("@QCTypeId", model.QCTypeId);
                param.Add("@QCItemId", model.QCItemId);
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

        public async Task<string> Modify(QCStandardDto model)
        {
            string proc = "Usp_QCStandard_Modify";
            var param = new DynamicParameters();
            param.Add("@QCStandardId", model.QCStandardId);
            param.Add("@QCTypeId", model.QCTypeId);
            param.Add("@QCItemId", model.QCItemId);
            param.Add("@QCName", model.QCName);
            param.Add("@QCApply", model.QCApply);
            param.Add("@modifiedBy", model.modifiedBy);
            param.Add("@row_version", model.row_version);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            return await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
        }

        public async Task<string> Delete(QCStandardDto model)
        {
            string proc = "Usp_QCStandard_Delete";
            var param = new DynamicParameters();
            param.Add("@QCStandardId", model.QCStandardId);
            param.Add("@row_version", model.row_version);
            param.Add("@modifiedBy", model.modifiedBy);
            param.Add("@isActived", model.isActived);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            return await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
        }

        public async Task<ResponseModel<IEnumerable<QCStandardDto>?>> GetActive(QCStandardDto model)
        {
            var returnData = new ResponseModel<IEnumerable<QCStandardDto>?>();
            var proc = $"Usp_QCStandard_GetActive";
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCStandardDto>(proc);

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
