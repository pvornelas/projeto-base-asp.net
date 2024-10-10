using Domain.Entities;

namespace Application.Services.Auth.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
