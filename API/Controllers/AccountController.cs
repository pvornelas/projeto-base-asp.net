using Application.Services.Account.Dtos.Requests;
using Application.Services.Account.Dtos.Responses;
using Application.Services.Account.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class AccountController : BaseApiController
    {
        private readonly IAccountService _service;
        public AccountController(IAccountService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            var result = await _service.Register(registerDto);

            if (result.Success)
                return CreatedAtAction(nameof(Register), result);

            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var result = await _service.Login(loginDto);

            if (result.Success)
                return Ok(result);

            return Unauthorized(result);
        }

        [HttpPut("update-username")]
        public async Task<ActionResult> UpdateUsername(UpdateUsernameDto userDto)
        {
            var result = await _service.UpdateUsername(userDto);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _service.Delete(id);

            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }
    }
}
