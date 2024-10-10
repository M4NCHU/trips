using backend.Domain.Authentication;
using System.Threading.Tasks;

namespace backend.Application.Services
{
    public interface IJWTService
    {
        Task<string> CreateJWT(UserModel user);
        string CreateRefreshToken();
    }
}
