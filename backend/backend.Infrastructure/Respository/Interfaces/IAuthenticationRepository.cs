using backend.Domain.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public interface IAuthenticationRepository
    {
        Task<UserModel> FindByUsernameAsync(string username);
        Task<UserModel> FindByIdAsync(string userId);
        Task<bool> CheckPasswordAsync(UserModel user, string password);
        Task<IdentityResult> CreateAsync(UserModel user, string password);
        Task<IdentityResult> ConfirmEmailAsync(UserModel user, string token);
        Task<string> GetAuthenticationTokenAsync(UserModel user, string loginProvider, string tokenName);
        Task SetAuthenticationTokenAsync(UserModel user, string loginProvider, string tokenName, string token);
        Task RemoveAuthenticationTokenAsync(UserModel user, string loginProvider, string tokenName);
        Task<IdentityResult> AccessFailedAsync(UserModel user);
        Task<bool> IsLockedOutAsync(UserModel user);
        Task ResetAccessFailedCountAsync(UserModel user);
        Task<IList<string>> GetRolesAsync(UserModel user);
    }
}
