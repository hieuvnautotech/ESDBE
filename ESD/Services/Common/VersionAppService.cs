using Dapper;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Helpers;
using ESD.Models;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using System.Data;
using System.Numerics;
using static ESD.Extensions.ServiceExtensions;

namespace ESD.Services.Common
{
    public interface IVersionAppService
    {
        Task<ResponseModel<IEnumerable<VersionAppDto>?>> CheckVersionApp(int versionCode);
        Task<ResponseModel<VersionAppDto?>> GetAll();
        Task<string> Modify(VersionAppDto model);
    }

    [SingletonRegistration]
    public class VersionAppService : IVersionAppService
    {
        private readonly ISqlDataAccess _sqlDataAccess;
        
        public VersionAppService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<ResponseModel<IEnumerable<VersionAppDto>?>> CheckVersionApp(int versionCode)
        {
            var returnData = new ResponseModel<IEnumerable<VersionAppDto>?>();
            string proc = "sysUsp_VersionApp_GetAll";
            var param = new DynamicParameters();
            param.Add("@versionCode", versionCode);
       
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<VersionAppDto>(proc, param);
            returnData.Data = data;
            returnData.ResponseMessage = param.Get<string>("output");
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
            }
            return returnData;
        }
        public async Task<ResponseModel<VersionAppDto?>> GetAll()
        {
            var returnData = new ResponseModel<VersionAppDto?>();
            var proc = $"sysUsp_VersionApp_GetActive";
            //var param = new DynamicParameters();
            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<VersionAppDto>(proc);
            
            if (!data.Any())
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
            }
            else
            {
                returnData.Data = data.FirstOrDefault();
            }
            return returnData;
        }

        public async Task<string> Modify(VersionAppDto model)
        {
            string proc = "sysUsp_VersionApp_Modify";
            var param = new DynamicParameters();
            param.Add("@id_app", model.id_app);
            param.Add("@app_version", model.app_version);
            param.Add("@url", model.url);
            param.Add("@name_file", model.name_file);
            param.Add("@createdBy", model.createdBy);
            param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);//luôn để DataOutput trong stored procedure

            return await _sqlDataAccess.SaveDataUsingStoredProcedure<string>(proc, param);
        }
    }
}
