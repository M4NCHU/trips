﻿using backend.Domain.Authentication;
using backend.Domain.DTO.Authentication;
using backend.Infrastructure.Respository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Application.Services
{
    public class AuthenticationService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly JWTService _jwtService;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(IUnitOfWork unitOfWork, IConfiguration configuration, JWTService jwtService, ILogger<AuthenticationService> logger)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<LoginResultDTO> Login(LoginDTO model)
        {
            _logger.LogInformation("User {Username} attempting to log in.", model.Username);

            var user = await _unitOfWork.Auth.FindByNameAsync(model.Username);

            var passwordVerificationResult = user != null
                ? await _unitOfWork.Auth.CheckPasswordAsync(user, model.Password)
                : false;

            if (!passwordVerificationResult)
            {
                if (user != null)
                {
                    await _unitOfWork.Auth.AccessFailedAsync(user);
                    if (await _unitOfWork.Auth.IsLockedOutAsync(user))
                    {
                        _logger.LogWarning("Account {Username} is locked due to multiple failed login attempts.", model.Username);
                        return new LoginResultDTO { Success = false, Error = "Account locked due to multiple failed login attempts. Please try again later or reset your password." };
                    }
                }

                _logger.LogWarning("Invalid login attempt for username: {Username}.", model.Username);
                return new LoginResultDTO { Success = false, Error = "Invalid username or password" };
            }

            await _unitOfWork.Auth.ResetAccessFailedCountAsync(user);
            _logger.LogInformation("User {Username} logged in successfully.", model.Username);

            var token = await _jwtService.CreateJWT(user);
            await _unitOfWork.Auth.SetAuthenticationTokenAsync(user, "JWT", "RefreshToken", token);

            _logger.LogInformation("JWT generated for user {Username}.", model.Username);

            return new LoginResultDTO { Success = true, Token = token, User = await CreateUserDto(user, token) };
        }

        public async Task<RegistrationResultDTO> Register(RegisterDTO model)
        {
            _logger.LogInformation("Attempting to register new user: {Username}.", model.Username);

            var userExists = await _unitOfWork.Auth.FindByNameAsync(model.Username);
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

            var result = await _unitOfWork.Auth.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogError("Error during user registration: {Error}", error.Description);
                }
                return new RegistrationResultDTO { Success = false, Error = "User registration failed! Please check user details and try again." };
            }

            _logger.LogInformation("User {Username} registered successfully.", model.Username);

            return new RegistrationResultDTO { Success = true, Message = "User registered successfully!" };
        }

        public async Task<bool> IsRefreshTokenValid(string refreshToken, string userId)
        {
            _logger.LogInformation("Validating refresh token for user ID: {UserId}.", userId);

            var user = await _unitOfWork.Auth.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError("User not found with ID: {UserId}.", userId);
                return false;
            }

            var storedRefreshToken = await _unitOfWork.Auth.GetAuthenticationTokenAsync(user, "JWT", "RefreshToken");

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

            var user = await _unitOfWork.Auth.FindByIdAsync(logoutData.userId);
            if (user == null)
            {
                _logger.LogError("User not found with ID: {UserId}.", logoutData.userId);
                return false;
            }

            var storedRefreshToken = await _unitOfWork.Auth.GetAuthenticationTokenAsync(user, "JWT", "RefreshToken");

            if (storedRefreshToken == logoutData.RefreshToken)
            {
                await _unitOfWork.Auth.RemoveAuthenticationTokenAsync(user, "JWT", "RefreshToken");
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

            var user = await _unitOfWork.Auth.FindByIdAsync(userId);
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
            var roles = await _unitOfWork.Auth.GetRolesAsync(user);
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