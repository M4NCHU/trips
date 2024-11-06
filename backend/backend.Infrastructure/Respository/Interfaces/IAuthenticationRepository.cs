using backend.Domain.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

public interface IAuthenticationRepository
{
    Task<UserModel> FindByNameAsync(string username);

    Task<UserModel> FindByIdAsync(string userId);

    Task<bool> CheckPasswordAsync(UserModel user, string password);

    Task<IdentityResult> CreateAsync(UserModel user, string password);

    Task<IdentityResult> ConfirmEmailAsync(UserModel user, string token);

    Task<string?> GetAuthenticationTokenAsync(UserModel user, string loginProvider, string tokenName);

    Task<IdentityResult> SetAuthenticationTokenAsync(UserModel user, string loginProvider, string tokenName, string? token);  // Zmieniono na Task<IdentityResult>

    Task<IdentityResult> RemoveAuthenticationTokenAsync(UserModel user, string loginProvider, string tokenName);  // Zmieniono na Task<IdentityResult>

    Task<IdentityResult> AccessFailedAsync(UserModel user);

    Task<bool> IsLockedOutAsync(UserModel user);

    Task<IdentityResult> ResetAccessFailedCountAsync(UserModel user);  // Zmieniono na Task<IdentityResult>

    Task<IList<string>> GetRolesAsync(UserModel user);

    Task LogCustomInformationAsync(string message);
}