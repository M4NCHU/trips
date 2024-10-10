using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace backend.Application.Services
{
    public interface IImageService
    {
        Task<string> SaveImage(IFormFile imageFile, string folder, string prefix = "");
        void DeleteImage(string imageName);
    }
}
