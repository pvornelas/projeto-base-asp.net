using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAppUserRepository
    {
        Task<IEnumerable<AppUser>> GetAll();
        Task<AppUser> GetById(int id);
        Task<AppUser> GetByEmail(string email);
    }
}
