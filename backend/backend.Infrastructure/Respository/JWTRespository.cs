using backend.Application.Services;
using backend.Domain.Authentication;
using backend.Infrastructure.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;



namespace backend.Infrastructure.Services
{
    public class JWTService : IJWTService
    {
        
        private readonly IConfiguration _configuration;
        private readonly ImageService _imageService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly BaseUrlService _baseUrlService;
        private readonly string _baseUrl;
        private readonly SymmetricSecurityKey _jwtKey;
        private readonly UserManager<UserModel> _userManager;

        public JWTService(ImageService imageService, IWebHostEnvironment hostEnvironment, BaseUrlService baseUrlService, IConfiguration configuration, UserManager<UserModel> userManager)
        {

            _imageService = imageService;
            _hostEnvironment = hostEnvironment;
            _baseUrlService = baseUrlService;
            _baseUrl = _baseUrlService.GetBaseUrl();
            _configuration = configuration;
            _jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            _userManager = userManager;
        }



        public async Task<string> CreateJWT(UserModel User)
        {
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, User.Id),
                new Claim(ClaimTypes.Email, User.Email),
                new Claim(ClaimTypes.GivenName, User.FirstName),
                new Claim(ClaimTypes.Surname, User.LastName)
            };

            var userRoles = await _userManager.GetRolesAsync(User);
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
                /*Audience = _configuration["JWT:ValidAudience"],*/
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(jwt);
        }

        public string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }


    }
}
