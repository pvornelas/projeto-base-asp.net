using Application.Common;
using Application.Services.Account.Dtos.Requests;
using Application.Services.Account.Dtos.Responses;

namespace Application.Services.Account.Interfaces
{
    public interface IAccountService
    {
        Task<RequestResult<UserDto>> Register(RegisterDto registerDto);
        Task<RequestResult<UserDto>> Login(LoginDto loginDto);
        Task<RequestResult<UpdatedUserDto>> UpdateUsername(UpdateUsernameDto userDto);
        Task<RequestResult<bool>> Delete(int id);
    }
}
