using Application.Common;
using Application.Services.Users.Dtos.Responses;
using Application.Services.Users.Interfaces;
using Domain.Interfaces;

namespace Application.Services.Users.Implementations
{
    public class AppUserService : IAppUserService
    {
        private readonly IAppUserRepository _repository;

        public AppUserService(IAppUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<RequestResult<AppUserDto>> GetAll()
        {
            var users = await _repository.GetAll();
            return RequestResult<AppUserDto>.SuccessResult(users.Select(u => new AppUserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email
            }));
        }

        public async Task<RequestResult<AppUserDto>> GetById(int id)
        {
            var user = await _repository.GetById(id);
            return RequestResult<AppUserDto>.SuccessResult(
            [
                new AppUserDto()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email
                }
            ]);
        }
    }
}
