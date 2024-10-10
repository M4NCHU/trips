using backend.Application.Services;
using backend.Domain.DTO.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] UserRoleAssignmentDto dto)
        {
            var result = await _userService.AssignRoleToUser(dto.UserId, dto.RoleName);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }
    }

   
}
