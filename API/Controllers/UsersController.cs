using Application.Services.Account.Dtos.Responses;
using Application.Services.Users.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IAppUserService _service;
        public UsersController(IAppUserService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            var result = await _service.GetAll();
            if (result.Success)
                return Ok(result);

            return NoContent();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var result = await _service.GetById(id);
            if (result.Success)
                return Ok(result);

            return NotFound();
        }
    }
}
