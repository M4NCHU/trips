using backend.Application.Services;
using backend.Domain.DTOs;
using backend.Infrastructure.Authentication;
using backend.Infrastructure.Respository;
using backend.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ImageService _imageService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly BaseUrlService _baseUrlService;
        private readonly ILogger<CategoryService> _logger;
        private readonly string _baseUrl;

        public CategoryService(ICategoryRepository categoryRepository, ImageService imageService, IWebHostEnvironment hostEnvironment, BaseUrlService baseUrlService, ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _imageService = imageService;
            _hostEnvironment = hostEnvironment;
            _baseUrlService = baseUrlService;
            _baseUrl = _baseUrlService.GetBaseUrl();
            _logger = logger;
        }

        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            _logger.LogInformation("Fetching all categories.");

            var categories = await _categoryRepository.GetCategoriesAsync();
            if (!categories.Any())
            {
                _logger.LogWarning("No categories found.");
            }

            var categoryDTOs = categories.Select(x => MapToCategoryDTO(x)).ToList();
            _logger.LogInformation("Successfully fetched {count} categories.", categoryDTOs.Count);

            return categoryDTOs;
        }

        public async Task<ActionResult<CategoryDTO>> GetCategory(Guid id)
        {
            _logger.LogInformation("Fetching category with ID: {id}", id);

            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
            {
                _logger.LogWarning("Category with ID {id} not found.", id);
                return new NotFoundResult();
            }

            var categoryDTO = MapToCategoryDTO(category);
            _logger.LogInformation("Successfully fetched category with ID: {id}", id);

            return categoryDTO;
        }

        public async Task<IActionResult> PutCategory(Guid id, CategoryDTO categoryDTO)
        {
            _logger.LogInformation("Updating category with ID: {id}", id);

            if (id != categoryDTO.Id)
            {
                _logger.LogWarning("Category ID mismatch. Provided ID: {id}, DTO ID: {dtoId}", id, categoryDTO.Id);
                return new BadRequestResult();
            }

            var category = MapToCategoryModel(categoryDTO);
            await _categoryRepository.UpdateCategoryAsync(category);

            _logger.LogInformation("Successfully updated category with ID: {id}", id);
            return new NoContentResult();
        }

        public async Task<CreateCategoryRequestDTO> PostCategory([FromForm] CreateCategoryRequestDTO categoryDTO)
        {
            _logger.LogInformation("Creating new category.");

            if (categoryDTO.ImageFile == null)
            {
                _logger.LogError("The 'ImageFileDTO' field is required.");
                throw new ValidationException("The 'ImageFileDTO' field is required.");
            }

            var photoUrl = await _imageService.SaveImage(categoryDTO.ImageFile, "Category", "category");
            var currentDate = DateTime.UtcNow;

            var category = new CategoryModel
            {
                Id = Guid.NewGuid(),
                Name = categoryDTO.Name,
                Description = categoryDTO.Description,
                PhotoUrl = photoUrl,
                CreatedAt = currentDate,
                ModifiedAt = currentDate
            };

            await _categoryRepository.AddCategoryAsync(category);
            _logger.LogInformation("Successfully posted category '{name}' at {currentDate}.", categoryDTO.Name, currentDate);

            return categoryDTO;
        }

        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            _logger.LogInformation("Deleting category with ID: {id}", id);

            var categoryExists = await _categoryRepository.CategoryExistsAsync(id);
            if (!categoryExists)
            {
                _logger.LogWarning("Attempted to delete category with ID {id}, but it does not exist.", id);
                return new NotFoundResult();
            }

            await _categoryRepository.DeleteCategoryAsync(id);
            _logger.LogInformation("Successfully deleted category with ID: {id}", id);

            return new NoContentResult();
        }

        private CategoryDTO MapToCategoryDTO(CategoryModel category)
        {
            return new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                PhotoUrl = $"{_baseUrl}/Images/Category/{category.PhotoUrl}"
            };
        }

        private CategoryModel MapToCategoryModel(CategoryDTO categoryDTO)
        {
            return new CategoryModel
            {
                Id = categoryDTO.Id,
                Name = categoryDTO.Name,
                Description = categoryDTO.Description,
                PhotoUrl = categoryDTO.PhotoUrl
            };
        }
    }
}
