using backend.Application.Services;
using backend.Domain.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace backend.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly ILogger<UserService> _logger;

        public UserService(UserManager<UserModel> userManager, ILogger<UserService> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IdentityResult> AssignRoleToUser(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError($"User not found with ID: {userId}");
                return IdentityResult.Failed(new IdentityError { Description = $"User not found with ID: {userId}" });
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result;
        }
    }
}
