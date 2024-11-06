using backend.Domain.DTO.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Application.Services;
using Microsoft.AspNetCore.Identity;
using backend.Domain.Authentication;
using System.Security.Claims;
using backend.Domain.DTOs;

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

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, 
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            var authCookieOptions = new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(15)
            };

            Response.Cookies.Append("jwt", result.User.JWT, cookieOptions);
            Response.Cookies.Append("isAuthenticated", "true", authCookieOptions);

            result.User.JWT = null;

            return Ok(new { user = result.User });
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

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            // Sprawdź, czy użytkownik jest uwierzytelniony
            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized(new { message = "User ID not found in claims." });
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                // Wygeneruj nowy token JWT
                var newToken = await _jwtService.CreateJWT(user);

                // Ustaw nowe ciasteczko z tokenem
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                  /*  Secure = true, // Ustaw na true w środowisku produkcyjnym*/
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(1)
                };

                Response.Cookies.Append("jwt", newToken, cookieOptions);

                return Ok(new { message = "Token refreshed successfully." });
            }
            else
            {
                return Unauthorized(new { message = "User is not authenticated." });
            }
        }


        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout([FromForm] LogoutDTO logoutData)
        {
            var result = await _authService.Logout(logoutData);
            Response.Cookies.Delete("jwt");
            Response.Cookies.Delete("isAuthenticated");
            if (!result)
            {
                return BadRequest(new { message = "Failed to logout." });
            }

            return Ok(new { message = "Logout successful." });
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Ok(new { user = (AccountDTO)null });
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return Ok(new { user = (AccountDTO)null });
                }

                var roles = await _userManager.GetRolesAsync(user);

                var userDto = new AccountDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Roles = roles.ToList()
                };

                return Ok(new { user = userDto });
            }
            else
            {
                return Ok(new { user = (AccountDTO)null });
            }
        }

    }

}
