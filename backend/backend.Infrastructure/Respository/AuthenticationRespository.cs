using backend.Domain.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly UserManager<UserModel> _userManager;

        public AuthenticationRepository(UserManager<UserModel> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserModel> FindByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<UserModel> FindByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<bool> CheckPasswordAsync(UserModel user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> CreateAsync(UserModel user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(UserModel user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<string> GetAuthenticationTokenAsync(UserModel user, string loginProvider, string tokenName)
        {
            return await _userManager.GetAuthenticationTokenAsync(user, loginProvider, tokenName);
        }

        public async Task SetAuthenticationTokenAsync(UserModel user, string loginProvider, string tokenName, string token)
        {
            await _userManager.SetAuthenticationTokenAsync(user, loginProvider, tokenName, token);
        }

        public async Task RemoveAuthenticationTokenAsync(UserModel user, string loginProvider, string tokenName)
        {
            await _userManager.RemoveAuthenticationTokenAsync(user, loginProvider, tokenName);
        }

        public async Task<IdentityResult> AccessFailedAsync(UserModel user)
        {
            return await _userManager.AccessFailedAsync(user);
        }

        public async Task<bool> IsLockedOutAsync(UserModel user)
        {
            return await _userManager.IsLockedOutAsync(user);
        }

        public async Task ResetAccessFailedCountAsync(UserModel user)
        {
            await _userManager.ResetAccessFailedCountAsync(user);
        }

        public async Task<IList<string>> GetRolesAsync(UserModel user)
        {
            return await _userManager.GetRolesAsync(user);
        }
    }
}
