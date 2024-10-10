using Application.Services.Account.Implementations;
using Application.Services.Account.Interfaces;
using Application.Services.Auth.Implementations;
using Application.Services.Auth.Interfaces;
using Application.Services.Users.Implementations;
using Application.Services.Users.Interfaces;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Account;
using Infrastructure.Data.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Database Context
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(
                    configuration.GetConnectionString("DefaultConnection")
                 );
            });

            // Repositories
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAppUserRepository, AppUserRepository>();

            // Services
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAppUserService, AppUserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            return services;
        }
    }
}
