using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task<AppUser> Register(AppUser user);
        Task<bool> EmailExists(string email);
        Task<AppUser> Update(AppUser user);
        Task Delete(AppUser user);
    }
}
