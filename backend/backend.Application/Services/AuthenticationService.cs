using backend.Application.Services;
using backend.Domain.Authentication;
using backend.Domain.DTO.Authentication;
using backend.Infrastructure.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Application.Services
{
    public class AuthenticationService : IAuthService
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IConfiguration _configuration;
        private readonly JWTService _jwtService;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly TripsDbContext _dbContext;

        public AuthenticationService(UserManager<UserModel> userManager, IConfiguration configuration, JWTService jWTService, ILogger<AuthenticationService> logger, TripsDbContext context)
        {
            _userManager = userManager;
            _configuration = configuration;
            _jwtService = jWTService;
            _logger = logger;
            _dbContext = context;
        }

        public async Task<LoginResultDTO> Login(LoginDTO model)
        {
            _logger.LogInformation("User {Username} attempting to log in.", model.Username);

            var user = await _userManager.FindByNameAsync(model.Username);

            var passwordVerificationResult = user != null
                ? await _userManager.CheckPasswordAsync(user, model.Password)
                : false;

            if (!passwordVerificationResult)
            {
                if (user != null)
                {
                    await _userManager.AccessFailedAsync(user);
                    if (await _userManager.IsLockedOutAsync(user))
                    {
                        _logger.LogWarning("Account {Username} is locked due to multiple failed login attempts.", model.Username);
                        return new LoginResultDTO { Success = false, Error = "Account locked due to multiple failed login attempts. Please try again later or reset your password." };
                    }
                }

                _logger.LogWarning("Invalid login attempt for username: {Username}.", model.Username);
                return new LoginResultDTO { Success = false, Error = "Invalid username or password" };
            }

            await _userManager.ResetAccessFailedCountAsync(user);
            _logger.LogInformation("User {Username} logged in successfully.", model.Username);

            if (!user.EmailConfirmed)
            {
                _logger.LogWarning("Email not confirmed for user {Username}.", model.Username);
                return new LoginResultDTO { Success = false, Error = "Please confirm your email" };
            }

            var token = await _jwtService.CreateJWT(user);
            await _userManager.SetAuthenticationTokenAsync(user, "JWT", "RefreshToken", token);

            _logger.LogInformation("JWT generated for user {Username}.", model.Username);

            return new LoginResultDTO { Success = true, Token = token, User = await CreateUserDto(user, token) };
        }

        public async Task<RegistrationResultDTO> Register(RegisterDTO model)
        {
            _logger.LogInformation("Attempting to register new user: {Username}.", model.Username);

            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                _logger.LogWarning("User {Username} already exists.", model.Username);
                return new RegistrationResultDTO { Success = false, Error = "User already exists!" };
            }

            UserModel user = new UserModel()
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogError("Error during user registration: {Error}", error.Description);
                }
                return new RegistrationResultDTO { Success = false, Error = "User registration failed! Please check user details and try again." };
            }

            await _userManager.ConfirmEmailAsync(user, await _userManager.GenerateEmailConfirmationTokenAsync(user));
            _logger.LogInformation("User {Username} registered successfully.", model.Username);

            return new RegistrationResultDTO { Success = true, Message = "User registered successfully!" };
        }

        public async Task<bool> IsRefreshTokenValid(string refreshToken, string userId)
        {
            _logger.LogInformation("Validating refresh token for user ID: {UserId}.", userId);

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError("User not found with ID: {UserId}.", userId);
                return false;
            }

            var storedRefreshToken = await _userManager.GetAuthenticationTokenAsync(user, "JWT", "RefreshToken");

            if (storedRefreshToken != refreshToken)
            {
                _logger.LogWarning("Invalid refresh token for user: {UserName}.", user.UserName);
                return false;
            }

            _logger.LogInformation("Refresh token validated for user: {UserName}.", user.UserName);
            return true;
        }

        public async Task<bool> Logout(LogoutDTO logoutData)
        {
            _logger.LogInformation("User {UserId} attempting to log out.", logoutData.userId);

            var user = await _userManager.FindByIdAsync(logoutData.userId);
            if (user == null)
            {
                _logger.LogError("User not found with ID: {UserId}.", logoutData.userId);
                return false;
            }

            var storedRefreshToken = await _userManager.GetAuthenticationTokenAsync(user, "JWT", "RefreshToken");

            if (storedRefreshToken == logoutData.RefreshToken)
            {
                await _userManager.RemoveAuthenticationTokenAsync(user, "JWT", "RefreshToken");
                _logger.LogInformation("User {UserId} logged out successfully.", logoutData.userId);
                return true;
            }

            _logger.LogWarning("Invalid refresh token during logout for user: {UserName}.", user.UserName);
            return false;
        }

        public async Task<LoginResultDTO> RefreshUserToken(string refreshToken, string userId)
        {
            _logger.LogInformation("Refreshing JWT for user ID: {UserId}.", userId);

            if (!await IsRefreshTokenValid(refreshToken, userId))
            {
                _logger.LogWarning("Invalid or expired refresh token for user ID: {UserId}.", userId);
                return new LoginResultDTO { Success = false, Error = "Invalid or expired refresh token" };
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError("User not found with ID: {UserId}.", userId);
                return new LoginResultDTO { Success = false, Error = "User not found" };
            }

            var newToken = await _jwtService.CreateJWT(user);
            _logger.LogInformation("New JWT generated for user {UserName}.", user.UserName);

            return new LoginResultDTO { Success = true, Token = newToken, User = await CreateUserDto(user, newToken) };
        }

        private async Task<AccountDTO> CreateUserDto(UserModel user, string token)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return new AccountDTO()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                JWT = token,
                UserName = user.UserName,
                Email = user.Email,
                Id = user.Id,
                Roles = roles.ToList()
            };
        }
    }
}
