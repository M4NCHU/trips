using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace backend.Application.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly string _imageFolder;
        private readonly ILogger<ImageService> _logger;

        public ImageService(IWebHostEnvironment hostEnvironment, string imageFolder, ILogger<ImageService> logger)
        {
            _hostEnvironment = hostEnvironment;
            _imageFolder = imageFolder;
            _logger = logger;

            EnsureImageFolderExists();
        }

        private void EnsureImageFolderExists()
        {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, _imageFolder);
            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
                _logger.LogInformation("Image folder created at path: {ImagePath}", imagePath);
            }
        }

        public async Task<string> SaveImage(IFormFile imageFile, string folder, string prefix = "")
        {
            if (string.IsNullOrEmpty(prefix))
            {
                prefix = folder;
            }

            string imageName = GenerateRandomImageName(prefix) + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, _imageFolder, folder, imageName);

            EnsureFolderExists(folder);

            _logger.LogInformation("Saving image {ImageName} at path: {ImagePath}", imageName, imagePath);

            try
            {
                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                _logger.LogInformation("Image {ImageName} saved successfully.", imageName);
                return imageName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save image {ImageName} at path: {ImagePath}", imageName, imagePath);
                throw;
            }
        }

        public void DeleteImage(string imageName)
        {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, _imageFolder, imageName);
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
                _logger.LogInformation("Image {ImageName} deleted successfully.", imageName);
            }
            else
            {
                _logger.LogWarning("Image {ImageName} not found at path: {ImagePath}.", imageName, imagePath);
            }
        }

        private void EnsureFolderExists(string folder)
        {
            var folderPath = Path.Combine(_hostEnvironment.ContentRootPath, _imageFolder, folder);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                _logger.LogInformation("Folder {FolderPath} created.", folderPath);
            }
        }

        private string GenerateRandomImageName(string prefix)
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var guid = Guid.NewGuid().ToString();
            var randomImageName = $"{prefix}_{timestamp}_{guid}";
            return randomImageName;
        }
    }
}
