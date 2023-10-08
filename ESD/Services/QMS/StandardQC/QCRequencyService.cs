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
    public interface IQCFrequencyService
    {
        Task<ResponseModel<IEnumerable<QCFrequencyDto>?>> GetAll(QCFrequencyDto pageInfo);
        Task<ResponseModel<IEnumerable<QCFrequencyDto>?>> GetActive(QCFrequencyDto model);
        Task<ResponseModel<QCFrequencyDto?>> GetById(long QCFrequencyId);
        Task<string> Create(QCFrequencyDto model);
        Task<string> Modify(QCFrequencyDto model);
        Task<string> Delete(QCFrequencyDto model);
     

    }
    [ScopedRegistration]
    public class QCFrequencyService : IQCFrequencyService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public QCFrequencyService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }
        public async Task<ResponseModel<IEnumerable<QCFrequencyDto>?>> GetAll(QCFrequencyDto model)
        {
            try
            {
                var returnData = new ResponseModel<IEnumerable<QCFrequencyDto>?>();
                string proc = "Usp_QCFrequency_GetAll"; var param = new DynamicParameters();
                param.Add("@page", model.page);
                param.Add("@pageSize", model.pageSize);
                param.Add("@totalRow", 0, DbType.Int32, ParameterDirection.Output);
                param.Add("@QCName", model.QCName);
                param.Add("@QCApply", model.QCApply);
                param.Add("@showDelete", model.showDelete);

                var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCFrequencyDto>(proc, param);
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
        public async Task<ResponseModel<QCFrequencyDto?>> GetById(long QCFrequencyId)
        {
            var returnData = new ResponseModel<QCFrequencyDto?>();
            var proc = $"Usp_QCFrequency_GetById";
            var param = new DynamicParameters();
            param.Add("@QCFrequencyId", QCFrequencyId);
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCFrequencyDto>(proc, param);
            returnData.Data = data.FirstOrDefault();
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
            }
            return returnData;
        }
        public async Task<string> Create(QCFrequencyDto model)
        {
            try
            {
                string proc = "Usp_QCFrequency_Create";
                var param = new DynamicParameters();
                param.Add("@QCFrequencyId", model.QCFrequencyId);
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

        public async Task<string> Modify(QCFrequencyDto model)
        {
            string proc = "Usp_QCFrequency_Modify";
            var param = new DynamicParameters();
            param.Add("@QCFrequencyId", model.QCFrequencyId);
            param.Add("@QCName", model.QCName);
            param.Add("@QCApply", model.QCApply);
            param.Add("@modifiedBy", model.modifiedBy);
            param.Add("@row_version", model.row_version);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            return await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
        }

        public async Task<string> Delete(QCFrequencyDto model)
        {
            string proc = "Usp_QCFrequency_Delete";
            var param = new DynamicParameters();
            param.Add("@QCFrequencyId", model.QCFrequencyId);
            param.Add("@row_version", model.row_version);
            param.Add("@modifiedBy", model.modifiedBy);
            param.Add("@isActived", model.isActived);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            return await _sqlDataAccess.SaveDataUsingStoredProcedure<int>(proc, param);
        }

        public async Task<ResponseModel<IEnumerable<QCFrequencyDto>?>> GetActive(QCFrequencyDto model)
        {
            var returnData = new ResponseModel<IEnumerable<QCFrequencyDto>?>();
            var proc = $"Usp_QCFrequency_GetActive";
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<QCFrequencyDto>(proc);

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
