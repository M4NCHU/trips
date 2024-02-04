using backend.Domain.Authentication;
using backend.Domain.DTO.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;


namespace backend.Application.Services
{
    public interface IUserService
    {
        Task<IdentityResult> AssignRoleToUser(string userId, string roleName);
    }
}
