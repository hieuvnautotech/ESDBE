using Dapper;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Models;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Models.Validators;
using ESD.Services.Base;
using System.Data;
using static ESD.Extensions.ServiceExtensions;

namespace ESD.Services
{
    public interface ICustomService
    {
        Task<ResponseModel<IEnumerable<T>>> GetForSelect<T>(string Column, string Table, string Where, string Order);
        Task<ResponseModel<IEnumerable<dynamic>>> GetStandardQCForSelect(string QCType, string QCApply);
        Task<ResponseModel<IEnumerable<dynamic>>> GetQCTypeForSelect();
        Task<ResponseModel<IEnumerable<dynamic>?>> GetQCItemForSelect(long? QCTypeId);
        Task<ResponseModel<IEnumerable<dynamic>>> GetQCStandardForSelect(long? QCTypeId, long? QCItemId);
        Task<ResponseModel<IEnumerable<dynamic>>> GetQCToolForSelect(long? QCTypeId, long? QCItemId, long? QCStandardId);

        Task<ResponseModel<IEnumerable<dynamic>>> GetQCTypeForSelectByApply(string QCApply);
        Task<ResponseModel<IEnumerable<dynamic>?>> GetQCItemForSelectByApply(long? QCTypeId, string QCApply);
        Task<ResponseModel<IEnumerable<dynamic>>> GetQCStandardForSelectByApply(long? QCTypeId, long? QCItemId, string QCApply);
        Task<ResponseModel<IEnumerable<dynamic>>> GetQCToolForSelectByApply(long? QCTypeId, long? QCItemId, long? QCStandardId, string QCApply);
        Task<ResponseModel<IEnumerable<dynamic>>> GetQCFrequencyForSelectByApply(string QCApply);
    }
    [ScopedRegistration]
    public class CustomService : ICustomService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public CustomService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<ResponseModel<IEnumerable<T>>> GetForSelect<T>(string Column, string Table, string Where, string Order)
        {
            var returnData = new ResponseModel<IEnumerable<T>>();
            var proc = $"sysUsp_All_GetForSelect";
            var param = new DynamicParameters();
            param.Add("@Column", Column);
            param.Add("@Table", Table);
            param.Add("@Where", Where);
            param.Add("@Order", Order);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<T>(proc, param);
            
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

        public async Task<ResponseModel<IEnumerable<dynamic>>> GetStandardQCForSelect(string QCType, string QCApply)
        {
            var returnData = new ResponseModel<IEnumerable<dynamic>>();
            var proc = $"Usp_StandardQC_GetForSelect";
            var param = new DynamicParameters();
            param.Add("@QCType", QCType);
            param.Add("@QCApply", QCApply);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);

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
        public async Task<ResponseModel<IEnumerable<dynamic>>> GetQCTypeForSelect()
        {
            var returnData = new ResponseModel<IEnumerable<dynamic>>();
            var proc = $"Usp_QCType_GetForSelect";

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc);

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
        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetQCItemForSelect(long? QCTypeId)
        {
            var returnData = new ResponseModel<IEnumerable<dynamic>?>();
            var proc = $"Usp_QCItem_GetByQCTypeId";
            var param = new DynamicParameters();
            param.Add("@QCTypeId", QCTypeId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);

            returnData.Data = data;
            if (!data.Any())
            {
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                returnData.HttpResponseCode = 204;
            }

            return returnData;
        }
        public async Task<ResponseModel<IEnumerable<dynamic>>> GetQCStandardForSelect(long? QCTypeId, long? QCItemId)
        {
            var returnData = new ResponseModel<IEnumerable<dynamic>>();
            var proc = $"Usp_QCStandard_GetForSelect";
            var param = new DynamicParameters();
            param.Add("@QCTypeId", QCTypeId);
            param.Add("@QCItemId", QCItemId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);

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
        public async Task<ResponseModel<IEnumerable<dynamic>>> GetQCToolForSelect(long? QCTypeId, long? QCItemId, long? QCStandardId)
        {
            var returnData = new ResponseModel<IEnumerable<dynamic>>();
            var proc = $"Usp_QCTool_GetForSelect";
            var param = new DynamicParameters();
            param.Add("@QCTypeId", QCTypeId);
            param.Add("@QCItemId", QCItemId);
            param.Add("@QCStandardId", QCStandardId);

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);

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
        public async Task<ResponseModel<IEnumerable<dynamic>>> GetQCTypeForSelectByApply(string QCApply)
        {
            var returnData = new ResponseModel<IEnumerable<dynamic>>();
            var proc = $"Usp_QCType_GetForSelectByApply";
            var param = new DynamicParameters();
            param.Add("@QCApply", QCApply);
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);

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
        public async Task<ResponseModel<IEnumerable<dynamic>?>> GetQCItemForSelectByApply(long? QCTypeId, string QCApply)
        {
            var returnData = new ResponseModel<IEnumerable<dynamic>?>();
            var proc = $"Usp_QCItem_GetForSelectByApply";
            var param = new DynamicParameters();
            param.Add("@QCTypeId", QCTypeId);
            param.Add("@QCApply", QCApply);
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);

            returnData.Data = data;
            if (!data.Any())
            {
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                returnData.HttpResponseCode = 204;
            }

            return returnData;
        }
        public async Task<ResponseModel<IEnumerable<dynamic>>> GetQCStandardForSelectByApply(long? QCTypeId, long? QCItemId, string QCApply)
        {
            var returnData = new ResponseModel<IEnumerable<dynamic>>();
            var proc = $"Usp_QCStandard_GetForSelectByApply";
            var param = new DynamicParameters();
            param.Add("@QCTypeId", QCTypeId);
            param.Add("@QCItemId", QCItemId);
            param.Add("@QCApply", QCApply);
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);

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
        public async Task<ResponseModel<IEnumerable<dynamic>>> GetQCToolForSelectByApply(long? QCTypeId, long? QCItemId, long? QCStandardId, string QCApply)
        {
            var returnData = new ResponseModel<IEnumerable<dynamic>>();
            var proc = $"Usp_QCTool_GetForSelectByApply";
            var param = new DynamicParameters();
            param.Add("@QCTypeId", QCTypeId);
            param.Add("@QCItemId", QCItemId);
            param.Add("@QCStandardId", QCStandardId);
            param.Add("@QCApply", QCApply);
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);

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
        public async Task<ResponseModel<IEnumerable<dynamic>>> GetQCFrequencyForSelectByApply(string QCApply)
        {
            var returnData = new ResponseModel<IEnumerable<dynamic>>();
            var proc = $"Usp_QCFrequency_GetForSelectByApply";
            var param = new DynamicParameters();
            param.Add("@QCApply", QCApply);
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<dynamic>(proc, param);

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
