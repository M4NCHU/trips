using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.Infrastructure.Authentication;
using Microsoft.Extensions.Hosting;
using backend.Application.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using backend.Domain.DTOs;
using backend.Infrastructure.Respository;
using static backend.Infrastructure.Respository.ApplicationException;


namespace backend.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly TripsDbContext _context;
        private readonly ImageService _imageService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly BaseUrlService _baseUrlService;
        private readonly ILogger<CategoryService> _logger;
        private readonly string _baseUrl;


        public CategoryService(TripsDbContext context, ImageService imageService, IWebHostEnvironment hostEnvironment, BaseUrlService baseUrlService, ILogger<CategoryService> logger)
        {
            _context = context;
            _imageService = imageService;
            _hostEnvironment = hostEnvironment;
            _baseUrlService = baseUrlService;
            _baseUrl = _baseUrlService.GetBaseUrl();
            _logger = logger;
        }

        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            if (_context.Category == null)
            {
                return new NotFoundResult();
            }

            var categories = await _context.Category
                .OrderBy(x => x.Id)
                .ToListAsync();

            var categoryDTOs = categories.Select(x => MapToCategoryDTO(x)).ToList();

            return categoryDTOs;
        }


        public async Task<ActionResult<CategoryDTO>> GetCategory(Guid id)
        {
            if (_context.Category == null)
            {
                _logger.LogError("Entity set 'TripsDbContext.Category' is null.");
                throw new InternalErrorException("Entity set 'TripsDbContext.Category' is null.");
            }

            var category = await _context.Category.FindAsync(id);

            if (category == null)
            {
                return new NotFoundResult();
            }

            var CategoryDTO = MapToCategoryDTO(category);

            return CategoryDTO;
        }

        public async Task<IActionResult> PutCategory(Guid id, CategoryDTO CategoryDTO)
        {
            if (id != CategoryDTO.Id)
            {
                return new BadRequestResult();
            }

            var category = new CategoryModel
            {
                Id = id,
                Name = CategoryDTO.Name,
                Description = CategoryDTO.Description,
                PhotoUrl = CategoryDTO.PhotoUrl
            };

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return new NotFoundResult();
                }
                else
                {
                    throw;
                }
            }

            return new NoContentResult();
        }

        public async Task<CreateCategoryRequestDTO> PostCategory([FromForm] CreateCategoryRequestDTO CategoryDTO)
        {
            try
            {
                if (_context.Category == null)
                {
                    _logger.LogError("Entity set 'TripsDbContext.Category' is null.");
                    throw new InternalErrorException("Entity set 'TripsDbContext.Category' is null.");
                }

                if (CategoryDTO.ImageFile == null)
                {
                    _logger.LogError("The 'ImageFileDTO' field is required.");
                    throw new ValidationException("The 'ImageFileDTO' field is required.");
                }

                var photoUrl = await _imageService.SaveImage(CategoryDTO.ImageFile, "Category", "category");

                var currentDate = DateTime.Now.ToUniversalTime();

                var category = new CategoryModel
                {
                    Id = Guid.NewGuid(),
                    Name = CategoryDTO.Name,
                    Description = CategoryDTO.Description,
                    PhotoUrl = photoUrl,
                    CreatedAt = currentDate,
                    ModifiedAt = currentDate,
                    ImageFile = CategoryDTO.ImageFile
                };

                _context.Category.Add(category);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Successfully posted category '{CategoryDTO.Name}' at {currentDate}.");

                return CategoryDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while posting a new category.");
                throw; 
            }
        }

        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            if (_context.Category == null)
            {
                _logger.LogError("Entity set 'TripsDbContext.Category' is null.");
                throw new InternalErrorException("Entity set 'TripsDbContext.Category' is null.");
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return new NotFoundResult();
            }

            _context.Category.Remove(category);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }

        private bool CategoryExists(Guid id)
        {
            return (_context.Category?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public CategoryDTO MapToCategoryDTO(CategoryModel category)
        {
            return new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                PhotoUrl = $"{_baseUrl}/Images/Category/{category.PhotoUrl}"
            };
        }


        private CategoryModel MapToCategoryModel(CategoryDTO categoryDTO, CategoryModel existingCategory = null)
        {
            var category = existingCategory ?? new CategoryModel();

            category.Name = categoryDTO.Name;
            category.Description = categoryDTO.Description;
            category.PhotoUrl = categoryDTO.PhotoUrl;

            return category;
        }
    }
}
