using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace backend.Application.Services
{
    public interface IUserService
    {
        Task<IdentityResult> AssignRoleToUser(string userId, string roleName);
    }
}
