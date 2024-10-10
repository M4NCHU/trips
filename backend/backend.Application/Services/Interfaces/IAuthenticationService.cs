using backend.Domain.Authentication;
using backend.Domain.DTO.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;


namespace backend.Application.Services
{
    public interface IAuthService
    {
        Task<LoginResultDTO> Login(LoginDTO model);

        Task<RegistrationResultDTO> Register(RegisterDTO model);

        Task<LoginResultDTO> RefreshUserToken(string refreshToken, string userId);

        Task<bool> IsRefreshTokenValid(string refreshToken, string userId);

        Task<bool> Logout(LogoutDTO logoutData);

        /*        Task<IActionResult> RegisterAdmin([FromBody] RegisterDTO model);
        */
    }
}
