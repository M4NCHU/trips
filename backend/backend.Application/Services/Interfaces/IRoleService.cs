using backend.Domain.DTO.Authentication;
using System.Threading.Tasks;

namespace backend.Application.Services
{
    public interface IRoleService
    {
        Task<bool> CreateRoleAsync(RoleDTO roleDto);
    }
}
