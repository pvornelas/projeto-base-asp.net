using Application.Common;
using Application.Services.Users.Dtos.Responses;

namespace Application.Services.Users.Interfaces
{
    public interface IAppUserService
    {
        Task<RequestResult<AppUserDto>> GetAll();
        Task<RequestResult<AppUserDto>> GetById(int id);
    }
}
