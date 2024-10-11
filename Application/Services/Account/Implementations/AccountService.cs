using Application.Common;
using Application.Services.Account.Dtos.Requests;
using Application.Services.Account.Dtos.Responses;
using Application.Services.Account.Interfaces;
using Application.Services.Auth.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services.Account.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAppUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        public AccountService(IAccountRepository repository, IAppUserRepository userRepository, ITokenService tokenService, IPasswordHasher passwordHasher)
        {
            _accountRepository = repository;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        public async Task<RequestResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await _accountRepository.EmailExists(registerDto.Email))
                return RequestResult<UserDto>.FailureResult("E-mail já cadastrado", null);

            using var hmac = new HMACSHA512();
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password!));
            var passwordSalt = hmac.Key;

            var user = new AppUser(registerDto.Username, registerDto.Email.ToLower(), passwordHash, passwordSalt);
            await _accountRepository.Register(user);

            return RequestResult<UserDto>.SuccessResult(
            [
                new UserDto()
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            ]);
        }

        public async Task<RequestResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userRepository.GetByEmail(loginDto.Email.ToLower());

            if (user == null)
                return RequestResult<UserDto>.FailureResult("E-mail inválido ou inexistente.", null);

            var isPasswordValid = _passwordHasher.VerifyPassword(loginDto.Password, user.PasswordHash, user.PasswordSalt);

            if (!isPasswordValid)
            {
                return RequestResult<UserDto>.FailureResult("Senha inválida.", null);
            }

            return RequestResult<UserDto>.SuccessResult(
            [
                 new UserDto()
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            ]);
        }

        public async Task<RequestResult<UpdatedUserDto>> UpdateUsername(UpdateUsernameDto userDto)
        {
            var existingUser = await _userRepository.GetById(userDto.Id);
            if (existingUser == null)
            {
                return RequestResult<UpdatedUserDto>.FailureResult("Usuário não encontrado.", null);
            }

            existingUser.UpdateUsername(userDto.Username);
            await _accountRepository.Update(existingUser);

            return RequestResult<UpdatedUserDto>.SuccessResult([new UpdatedUserDto
            {
                Id = existingUser.Id,
                Username = existingUser.UserName,
                Email = existingUser.Email
            }]);
        }

        public async Task<RequestResult<bool>> Delete(int id)
        {
            var existingUser = await _userRepository.GetById(id);
            if (existingUser == null)
            {
                return RequestResult<bool>.FailureResult("Usuário não encontrado.", null);
            }

            await _accountRepository.Delete(existingUser);
            return RequestResult<bool>.SuccessResult(null);
        }
    }
}
