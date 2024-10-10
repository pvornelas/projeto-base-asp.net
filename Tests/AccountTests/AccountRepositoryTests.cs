using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Data.Account;
using Microsoft.EntityFrameworkCore;

namespace Tests.RepositoryTests
{
    public class AccountRepositoryTests
    {
        private readonly DataContext _context;
        private readonly AccountRepository _repository;

        public AccountRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseSqlite("DataSource=:memory:").Options;

            _context = new DataContext(options);
            _context.Database.OpenConnection();
            _context.Database.EnsureCreated();

            _repository = new AccountRepository(_context);
        }

        [Fact]
        public async Task DeveRegistarContaUsuario()
        {
            var user = new AppUser("usuario", "usuario@dominio.com", new byte[64], new byte[128]);
            var result = await _repository.Register(user);

            var dbUser = await _context.Users.FindAsync(result.Id);

            Assert.NotNull(dbUser);
            Assert.Equal(user.UserName, dbUser.UserName);
        }

        [Fact]
        public async Task DeveChecarSeEmailExiste()
        {
            var user = new AppUser("usuario", "usuario@dominio.com", new byte[64], new byte[128]);
            await _repository.Register(user);

            var emailExists = await _repository.EmailExists(user.Email);

            Assert.True(emailExists);
        }

        [Fact]
        public async Task DeveAtualizarNomeUsuario()
        {
            var user = new AppUser("usuario", "usuario@dominio.com", new byte[64], new byte[128]);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            user.UpdateUsername("novoUsuario");
            await _repository.Update(user);

            var updatedUser = await _context.Users.FindAsync(user.Id);
            Assert.Equal("novoUsuario", updatedUser!.UserName);
        }

        [Fact]
        public async Task DeveDeletarContaUsuario()
        {
            var user = new AppUser("usuario", "usuario@dominio.com", new byte[64], new byte[128]);
            await _repository.Register(user);
            await _repository.Delete(user);

            var dbUser = await _context.Users.FindAsync(user.Id);
            Assert.Null(dbUser);
        }
    }
}