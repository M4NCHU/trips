using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace backend.Services
{
    public class ImageService
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly string _imageFolder;

        public ImageService(IWebHostEnvironment hostEnvironment, string imageFolder)
        {
            _hostEnvironment = hostEnvironment;
            _imageFolder = imageFolder;

            // Ensure the image folder exists
            EnsureImageFolderExists();
        }

        private void EnsureImageFolderExists()
        {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, _imageFolder);
            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }
        }

        public async Task<string> SaveImage(IFormFile imageFile, string folder)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, _imageFolder, folder, imageName);
            // Use the 'folder' parameter in the Path.Combine method
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return imageName;
        }


        public void DeleteImage(string imageName)
        {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, _imageFolder, imageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
        }
    }
}
