using backend.Application.Authentication;
using backend.Application.Services;
using backend.Domain.Authentication;
using backend.Domain.DTO.Authentication;
using backend.Infrastructure.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;


namespace backend.Infrastructure.Services
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



        public async Task<LoginResultDTO> Login([FromForm] LoginDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            // Mitigate timing attacks by always performing password check (use dummy if user not found)
            var passwordVerificationResult = user != null
                ? await _userManager.CheckPasswordAsync(user, model.Password)
                : false;

            // Account Lockout Mechanism
            if (!passwordVerificationResult)
            {
                if (user != null)
                {
                    // Record the failed attempt
                    await _userManager.AccessFailedAsync(user);

                    // Check if account is locked out
                    if (await _userManager.IsLockedOutAsync(user))
                    {
                        return new LoginResultDTO { Success = false, Error = "Account locked due to multiple failed login attempts. Please try again later or reset your password." };
                    }
                }

                // Log the failed login attempt
                _logger.LogWarning(model.Username); 

                return new LoginResultDTO { Success = false, Error = "Invalid username or password" };
            }

            // Reset the lockout count on successful login
            await _userManager.ResetAccessFailedCountAsync(user);

            if (!user.EmailConfirmed)
            {
                return new LoginResultDTO { Success = false, Error = "Please confirm your email" };
            }



            var token = _jwtService.CreateJWT(user);

            await _userManager.SetAuthenticationTokenAsync(user, "JWT", "RefreshToken", token);

            return new LoginResultDTO { Success = true, Token = token, User = CreateUserDto(user, token) };
        }


        public async Task<RegistrationResultDTO> Register([FromForm] RegisterDTO model)
        {
            // Check if the user already exists
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                return new RegistrationResultDTO { Success = false, Error = "User already exists!" };
            }

            UserModel user = new UserModel()
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            // Attempt to create the user with the password
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                // Log errors and return
                foreach (var error in result.Errors)
                {
                    _logger.LogError(error.Description);
                }
                return new RegistrationResultDTO { Success = false, Error = "User registration failed! Please check user details and try again." };
            }
            

            // auto-confirming the email. 
            await _userManager.ConfirmEmailAsync(user, await _userManager.GenerateEmailConfirmationTokenAsync(user));

            // Return success result
            return new RegistrationResultDTO { Success = true, Message = "User registered successfully!" };
        }

        public async Task<bool> IsRefreshTokenValid(string refreshToken, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError($"User not found with ID: {userId}");
                return false;
            }

            var storedRefreshToken = await _userManager.GetAuthenticationTokenAsync(user, "JWT", "RefreshToken");

            // Check if the token from the client matches the one in the database and is not expired
            if (storedRefreshToken != refreshToken)
            {
                _logger.LogWarning($"Invalid refresh token for user: {user.UserName}");
                return false;
            }

            

            return true;
        }

        public async Task<bool> Logout(LogoutDTO logoutData)
        {
            var user = await _userManager.FindByIdAsync(logoutData.userId);
            if (user == null)
            {
                _logger.LogError($"User not found with ID: {logoutData.userId}");
                return false;
            }

            var storedRefreshToken = await _userManager.GetAuthenticationTokenAsync(user, "JWT", "RefreshToken");

            if (storedRefreshToken == logoutData.RefreshToken)
            {
                await _userManager.RemoveAuthenticationTokenAsync(user, "JWT", "RefreshToken");
                return true;
            }

            _logger.LogWarning($"Invalid refresh token for user: {user.UserName}");
            return false;
        }



        public async Task<LoginResultDTO> RefreshUserToken(string refreshToken, string userId)
        {

            if (!await IsRefreshTokenValid(refreshToken, userId))
            {
                return new LoginResultDTO { Success = false, Error = "Invalid or expired refresh token" };
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new LoginResultDTO { Success = false, Error = "User not found" };
            }

            // Generate a new JWT token for the user
            var newToken = _jwtService.CreateJWT(user);


            // Create the AccountDTO with the new token
            var accountDto = CreateUserDto(user, newToken);

            return new LoginResultDTO { Success = true, Token = newToken, User = CreateUserDto(user, newToken) };
        }


        private AccountDTO CreateUserDto(UserModel user, string token)
        {
            return new AccountDTO()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                JWT = token,
                UserName = user.UserName,
                Email = user.Email,
                Id = user.Id,
            };
        }
    }
}
