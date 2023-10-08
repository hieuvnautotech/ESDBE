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
    public interface IQCItemService
    {
        Task<ResponseModel<IEnumerable<QCItemDto>?>> GetAll(QCItemDto pageInfo);
        Task<ResponseModel<IEnumerable<QCItemDto>?>> GetActive(QCItemDto model);
        Task<ResponseModel<QCItemDto?>> GetById(long QCItemId);
        Task<string> Create(QCItemDto model);
        Task<string> Modify(QCItemDto model);
        Task<string> Delete(QCItemDto model);
     

    }
    [ScopedRegistration]
    public class QCItemService : IQCItemService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public QCItemService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public async Task<ResponseModel<IEnumerable<QCItemDto>?>> GetAll(QCItemDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<QCItemDto>?>();
                string proc = "Usp_QCItem_GetAll"; var param = new DynamicParameters();
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);
                param.Add("@QCName", model.QCName);
                param.Add("@QCApply", model.QCApply);
                param.Add("@showDelete", model.showDelete);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCItemDto>(proc, param);
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
        public async Task<ResponseModel<QCItemDto?>> GetById(long QCItemId)
        {
            var returnData = new ResponseModel<QCItemDto?>();
            var proc = $"Usp_QCItem_GetById";
            var param = new DynamicParameters();
            param.Add("@QCItemId", QCItemId);
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCItemDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
            }
            return returnData;
        }
        public async Task<string> Create(QCItemDto model)
        {
            try
            {
                string proc = "Usp_QCItem_Create";
                var param = new DynamicParameters();
                param.Add("@QCItemId", model.QCItemId);
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

        public async Task<string> Modify(QCItemDto model)
        {
            string proc = "Usp_QCItem_Modify";
            var param = new DynamicParameters();
            param.Add("@QCItemId", model.QCItemId);
            param.Add("@QCTypeId", model.QCTypeId);
            param.Add("@QCName", model.QCName);
            param.Add("@QCApply", model.QCApply);
            param.Add("@modifiedBy", model.modifiedBy);
            param.Add("@row_version", model.row_version);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            return await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
        }

        public async Task<string> Delete(QCItemDto model)
        {
            string proc = "Usp_QCItem_Delete";
            var param = new DynamicParameters();
            param.Add("@QCItemId", model.QCItemId);
            param.Add("@row_version", model.row_version);
            param.Add("@modifiedBy", model.modifiedBy);
            param.Add("@isActived", model.isActived);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            return await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
        }

        public async Task<ResponseModel<IEnumerable<QCItemDto>?>> GetActive(QCItemDto model)
        {
            var returnData = new ResponseModel<IEnumerable<QCItemDto>?>();
            var proc = $"Usp_QCItem_GetActive";
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCItemDto>(proc);

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
