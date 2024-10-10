using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Data.Users;
using Microsoft.EntityFrameworkCore;

namespace Tests.RepositoryTests
{
    public class AppUserRepositoryTests
    {
        private readonly DataContext _context;
        private readonly AppUserRepository _repository;

        public AppUserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseSqlite("DataSource=:memory:").Options;

            _context = new DataContext(options);
            _context.Database.OpenConnection();
            _context.Database.EnsureCreated();

            _repository = new AppUserRepository(_context);
        }

        [Fact]
        public async Task DeveRetornarTodosUsuarios()
        {
            var users = new List<AppUser>
            {
                new AppUser("teiaCef", "teia@caixa.gov.br", new byte[64], new byte[128]),
                new AppUser("pvo", "pvo@caixa.gov.br", new byte[64], new byte[128])
            };

            _context.Users.AddRange(users);
            await _context.SaveChangesAsync();

            var result = await _repository.GetAll();

            Assert.NotNull(result);
            Assert.Equal(users.Count, result.Count());
        }

        [Fact]
        public async Task DeveRetornarUsuarioPorId()
        {
            var user = new AppUser("teiaCef", "teia@caixa.gov.br", new byte[64], new byte[128]);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = await _repository.GetById(user.Id);

            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
        }

        [Fact]
        public async Task DeveRetornarUsuarioPorEmail()
        {
            var user = new AppUser("teiaCef", "teia@caixa.gov.br", new byte[64], new byte[128]);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByEmail(user.Email);

            Assert.NotNull(result);
            Assert.Equal(user.Email, result.Email);
        }
    }
}