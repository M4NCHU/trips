using backend.Application.Services;
using backend.Domain.DTO.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleService _roleService;

        public RoleController(RoleService roleService)
        {
            _roleService = roleService;
        }


        /*[Authorize(Roles = "admin, driver")]*/
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateRole([FromForm] RoleDTO roleDto)
        {
            var result = await _roleService.CreateRoleAsync(roleDto);
            if (result)
            {
                return Ok(new { message = $"Role {roleDto.Name} created successfully." });
            }
            return BadRequest(new { message = $"Role {roleDto.Name} creation failed." });
        }
    }
}
