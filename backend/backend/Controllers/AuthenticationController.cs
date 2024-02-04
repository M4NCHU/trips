using backend.Domain.DTO.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Application.Services;
using Microsoft.AspNetCore.Identity;
using backend.Domain.Authentication;
using backend.Infrastructure.Services;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<UserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly JWTService _jwtService;
        private readonly SignInManager<UserModel> _signInManager;

        public AuthenticationController(IAuthService authenticationService, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<UserModel> signInManager, JWTService jWTService)
        {
            _authService = authenticationService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _jwtService = jWTService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromForm] LoginDTO model)
        {
            var result = await _authService.Login(model);
            if (!result.Success)
            {
                return Unauthorized(new { message = result.Error });
            }

            return Ok(new { token = result.Token, user = result.User });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromForm] RegisterDTO model)
        {
            var result = await _authService.Register(model);
            if (!result.Success)
            {
                return Unauthorized(new { message = result.Error });
            }

            return Ok();
        }



        
        [HttpGet("new-token")]
        public async Task<ActionResult> RefreshUserToken(string refreshToken, string userId)
        {
            var result = await _authService.RefreshUserToken(refreshToken, userId);
            if (!result.Success)
            {
                return Unauthorized(new { message = result.Error });
            }

            return Ok(new { token = result.Token, user = result.User });
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout([FromForm] LogoutDTO logoutData)
        {
            
            var result = await _authService.Logout(logoutData);
            if (!result)
            {
                return BadRequest(new { message = "Failed to logout." });
            }

            return Ok(new { message = "Logout successful." });
        }

        /* [HttpPost]
         [Route("register-admin")]
         public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDTO model)
         {
             var result = await _authService.RegisterAdmin(model);
             return result;
         }*/
    }
}
