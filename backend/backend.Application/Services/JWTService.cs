using backend.Application.Services;
using backend.Domain.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace backend.Application.Services
{
    public class JWTService : IJWTService
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _jwtKey;
        private readonly UserManager<UserModel> _userManager;
        private readonly ILogger<JWTService> _logger;

        public JWTService(IConfiguration configuration, UserManager<UserModel> userManager, ILogger<JWTService> logger)
        {
            _configuration = configuration;
            _jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<string> CreateJWT(UserModel user)
        {
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName)
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var credentials = new SigningCredentials(_jwtKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(userClaims),
                Expires = DateTime.UtcNow.AddDays(int.Parse(_configuration["JWT:ExpiresInDays"])),
                SigningCredentials = credentials,
                Issuer = _configuration["JWT:ValidIssuer"],
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(jwt);

            _logger.LogInformation("JWT created for user {UserId} with expiration at {Expiration}", user.Id, tokenDescriptor.Expires);

            return tokenString;
        }

        public string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                var refreshToken = Convert.ToBase64String(randomNumber);
                _logger.LogInformation("Refresh token created.");
                return refreshToken;
            }
        }
    }
}
