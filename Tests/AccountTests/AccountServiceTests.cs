using Application.Services.Account.Dtos.Requests;
using Application.Services.Account.Implementations;
using Application.Services.Account.Interfaces;
using Application.Services.Auth.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Tests.AccountServiceTests
{
    public class AccountServiceTests
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<IAppUserRepository> _userRepositoryMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IPasswordHasher> _passwordHasherServiceMock;
        private readonly IAccountService _accountService;

        public AccountServiceTests()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _userRepositoryMock = new Mock<IAppUserRepository>();
            _tokenServiceMock = new Mock<ITokenService>();
            _passwordHasherServiceMock = new Mock<IPasswordHasher>();

            _accountService = new AccountService(_accountRepositoryMock.Object, _userRepositoryMock.Object, _tokenServiceMock.Object, _passwordHasherServiceMock.Object);
        }

        [Fact]
        public async Task DeveRegistrarUsuarioComSucesso()
        {
            var registerDto = new RegisterDto { Username = "usuario", Email = "usuario@dominio.com", Password = "123456" };

            _accountRepositoryMock.Setup(x => x.EmailExists(It.IsAny<string>())).ReturnsAsync(false);

            _accountRepositoryMock.Setup(x => x.Register(It.IsAny<AppUser>()))
                .ReturnsAsync(new AppUser("usuario", "usuario@dominio.com", new byte[64], new byte[128]));

            var result = await _accountService.Register(registerDto);

            Assert.True(result.Success);
            Assert.True(result.Data.Any());
            Assert.Equal("usuario", result.Data.First().Username);
            Assert.Equal("usuario@dominio.com", result.Data.First().Email);
        }

        [Fact]
        public async Task DeveFalharRegistroQuandoEmailExiste()
        {
            var registerDto = new RegisterDto { Username = "usuario", Email = "usuario@dominio.com", Password = "123456" };
            _accountRepositoryMock.Setup(x => x.EmailExists(It.IsAny<string>())).ReturnsAsync(true);

            var result = await _accountService.Register(registerDto);

            Assert.False(result.Success);
            Assert.Equal("E-mail já cadastrado", result.Message);
        }

        [Fact]
        public async Task DeveLogarComSucesso()
        {
            var loginDto = new LoginDto { Email = "usuario@dominio.com", Password = "123456" };
            var user = new AppUser("usuario", "usuario@dominio.com", new byte[64], new byte[128]);

            _userRepositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>())).ReturnsAsync(user);
            _passwordHasherServiceMock.Setup(x => x.VerifyPassword(loginDto.Password, user.PasswordHash, user.PasswordSalt)).Returns(true);

            var result = await _accountService.Login(loginDto);

            Assert.True(result.Success);
            Assert.Equal("usuario", result.Data.First().Username);
            Assert.Equal("usuario@dominio.com", result.Data.First().Email);
        }

        [Fact]
        public async Task DeveFalharLoginQuandoUsuarioInexistente()
        {
            var loginDto = new LoginDto { Email = "usuario@dominio.com", Password = "123456" };
            _userRepositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>())).ReturnsAsync((AppUser)null);

            var result = await _accountService.Login(loginDto);

            Assert.False(result.Success);
            Assert.Equal("E-mail inválido ou inexistente.", result.Message);
        }

        [Fact]
        public async Task DeveFalharLoginQuandoSenhaInvalida()
        {
            var loginDto = new LoginDto { Email = "teste@dominio.com", Password = "senhaInvalida" };
            var user = new AppUser("usuario", "teste@dominio.com", new byte[64], new byte[128]);
            _userRepositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>())).ReturnsAsync(user);

            var result = await _accountService.Login(loginDto);

            Assert.False(result.Success);
            Assert.Equal("Senha inválida.", result.Message);
            _userRepositoryMock.Verify(x => x.GetByEmail(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task DeveAtualizarNomeUsuarioComSucesso()
        {
            var userDto = new UpdateUsernameDto { Id = 1, Username = "novoUsuario" };
            var user = new AppUser("usuario", "teste@dominio.com", new byte[64], new byte[128]);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(user);
            _accountRepositoryMock.Setup(x => x.Update(It.IsAny<AppUser>())).ReturnsAsync(user);

            var result = await _accountService.UpdateUsername(userDto);

            Assert.True(result.Success);
            Assert.Equal(userDto.Username, result.Data.First().Username);
        }

        [Fact]
        public async Task DeveFalharAlteracaoQuandoUsuarioInexistente()
        {
            var userDto = new UpdateUsernameDto { Id = 999, Username = "novoUsuario" };
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync((AppUser)null);

            var result = await _accountService.UpdateUsername(userDto);

            Assert.False(result.Success);
            Assert.Equal("Usuário não encontrado.", result.Message);
        }

        [Fact]
        public async Task DeveDeletarComSucesso()
        {
            var user = new AppUser("usuario", "teste@dominio.com", new byte[64], new byte[128]);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(user);
            _accountRepositoryMock.Setup(x => x.Delete(It.IsAny<AppUser>())).Returns(Task.CompletedTask);

            var result = await _accountService.Delete(1);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task DeveFalharDelecaoQuandoUsuarioInexistente()
        {
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync((AppUser)null);

            var result = await _accountService.Delete(1111);

            Assert.False(result.Success);
            Assert.Equal("Usuário não encontrado.", result.Message);
        }
    }
}