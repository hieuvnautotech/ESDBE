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
    public interface IQCTypeService
    {
        Task<ResponseModel<IEnumerable<QCTypeDto>?>> GetAll(QCTypeDto pageInfo);
        Task<ResponseModel<IEnumerable<QCTypeDto>?>> GetActive(QCTypeDto model);
        Task<ResponseModel<QCTypeDto?>> GetById(long QCTypeId);
        Task<string> Create(QCTypeDto model);
        Task<string> Modify(QCTypeDto model);
        Task<string> Delete(QCTypeDto model);

    }
    [ScopedRegistration]
    public class QCTypeService : IQCTypeService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public QCTypeService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public async Task<ResponseModel<IEnumerable<QCTypeDto>?>> GetAll(QCTypeDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<QCTypeDto>?>();
                string proc = "Usp_QCType_GetAll"; var param = new DynamicParameters();
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);
                param.Add("@QCName", model.QCName);
                param.Add("@QCApply", model.QCApply);
                param.Add("@showDelete", model.showDelete);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCTypeDto>(proc, param);
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
        public async Task<ResponseModel<QCTypeDto?>> GetById(long QCTypeId)
        {
            var returnData = new ResponseModel<QCTypeDto?>();
            var proc = $"Usp_QCType_GetById";
            var param = new DynamicParameters();
            param.Add("@QCTypeId", QCTypeId);
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCTypeDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
            }
            return returnData;
        }
        public async Task<string> Create(QCTypeDto model)
        {
            try
            {
                string proc = "Usp_QCType_Create";
                var param = new DynamicParameters();
                param.Add("@QCTypeId", model.QCTypeId);
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

        public async Task<string> Modify(QCTypeDto model)
        {
            string proc = "Usp_QCType_Modify";
            var param = new DynamicParameters();
            param.Add("@QCTypeId", model.QCTypeId);
            param.Add("@QCName", model.QCName);
            param.Add("@QCApply", model.QCApply);
            param.Add("@modifiedBy", model.modifiedBy);
            param.Add("@row_version", model.row_version);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            return await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
        }

        public async Task<string> Delete(QCTypeDto model)
        {
            string proc = "Usp_QCType_Delete";
            var param = new DynamicParameters();
            param.Add("@QCTypeId", model.QCTypeId);
            param.Add("@row_version", model.row_version);
            param.Add("@modifiedBy", model.modifiedBy);
            param.Add("@isActived", model.isActived);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            return await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
        }

        public async Task<ResponseModel<IEnumerable<QCTypeDto>?>> GetActive(QCTypeDto model)
        {
            var returnData = new ResponseModel<IEnumerable<QCTypeDto>?>();
            var proc = $"Usp_QCType_GetActive";
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCTypeDto>(proc);

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
