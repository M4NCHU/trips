using System;
using System.Threading.Tasks;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PhotosController : ControllerBase
	{
		private readonly ImageService _imageService;

		public PhotosController(ImageService imageService)
		{
			_imageService = imageService;
		}

		[HttpPost]
		public async Task<IActionResult> UploadPhoto([FromForm] IFormFile imageFile)
		{
			try
			{
				var imageName = await _imageService.SaveImage(imageFile);
				return Ok(new { ImageName = imageName });
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpDelete("{imageName}")]
		public IActionResult DeletePhoto(string imageName)
		{
			try
			{
				_imageService.DeleteImage(imageName);
				return NoContent();
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}
	}
}
