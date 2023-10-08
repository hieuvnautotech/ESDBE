using Dapper;
using ESD.DbAccess;
using ESD.Extensions;
using ESD.Models.Dtos.Common;
using System.Data;
using static ESD.Extensions.ServiceExtensions;

namespace ESD.Services.Common
{
    public interface ILoginService
    {
        Task<string> CheckLogin(LoginModelDto model);
        Task<ResponseModel<IEnumerable<UserDto>?>> GetOnlineUsers();
    }

    [SingletonRegistration]
    public class LoginService : ILoginService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public LoginService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<string> CheckLogin(LoginModelDto model)
        {

            model.userPassword = MD5Encryptor.MD5Hash(model.userPassword);

            string proc = "sysUsp_User_CheckLogin";
            var param = new DynamicParameters();
            param.Add("@userName", model.userName);
            param.Add("@userPassword", model.userPassword);
            //param.Add("@output", dbType: DbType.String, direction: ParameterDirection.Output);//luôn để DataOutput trong stored procedure

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<string>(proc, param);
            return data.FirstOrDefault() ?? string.Empty;
        }

        public async Task<ResponseModel<IEnumerable<UserDto>?>> GetOnlineUsers()
        {
            var returnData = new ResponseModel<IEnumerable<UserDto>?>();
            string proc = "sysUsp_User_GetOnlineUsers";

            var data = await _sqlDataAccess.LoadDataUsingStoredProcedure<UserDto>(proc);
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
