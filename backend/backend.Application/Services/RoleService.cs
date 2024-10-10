using backend.Application.Services;
using backend.Domain.DTO.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace backend.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RoleService> _logger;

        public RoleService(RoleManager<IdentityRole> roleManager, ILogger<RoleService> logger)
        {
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<bool> CreateRoleAsync(RoleDTO roleDto)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleDto.Name);
            if (!roleExist)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(roleDto.Name));
                return result.Succeeded;
            }
            _logger.LogInformation($"Role {roleDto.Name} already exists.");
            return false;
        }
    }
}
