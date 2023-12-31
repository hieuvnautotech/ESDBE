﻿using ESD.DbAccess;
using ESD.Models.Dtos.Common;
using static ESD.Extensions.ServiceExtensions;

namespace ESD.Services.Common
{
    public interface IUserAuthorizationService
    {
        Task<IEnumerable<UserAuthorizationDto>> GetUserAuthorization();
    }

    [SingletonRegistration]
    public class UserAuthorizationService : IUserAuthorizationService
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public UserAuthorizationService(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<IEnumerable<UserAuthorizationDto>> GetUserAuthorization()
        {
            string proc = "sysUsp_GetAuthorization";
            return await _sqlDataAccess.LoadDataUsingStoredProcedure<UserAuthorizationDto>(proc);
        }
    }
}
