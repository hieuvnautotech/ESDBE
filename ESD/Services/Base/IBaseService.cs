using ESD.Models;
using ESD.Models.Dtos.Common;

namespace ESD.Services.Base
{
    public interface IBaseService<T>
    {
        Task<ResponseModel<IEnumerable<T>?>> GetAll(PageModel pageInfo, string roleName, string? keyWord, bool showDelete);
        Task<ResponseModel<T?>> GetById(long id);
        Task<string> Create(T model);
        Task<string> Modify(T model);
        Task<string> Delete(long commonMasterId, byte[] row_version);

    }
}
