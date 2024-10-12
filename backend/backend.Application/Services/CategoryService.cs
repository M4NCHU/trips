using backend.Application.Services;
using backend.Domain.DTOs;
using backend.Infrastructure.Respository;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(IUnitOfWork unitOfWork, ILogger<CategoryService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
    {
        _logger.LogInformation("Fetching all categories");
        var categories = await _unitOfWork.Categories.GetAllAsync();

        if (!categories.Any())
        {
            _logger.LogWarning("No categories found");
        }

        var categoryDTOs = categories.Select(x => new CategoryDTO
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            PhotoUrl = x.PhotoUrl
        }).ToList();

        _logger.LogInformation("Successfully fetched {Count} categories", categoryDTOs.Count);
        return new OkObjectResult(categoryDTOs);
    }

    public async Task<ActionResult<CategoryDTO>> GetCategory(Guid id)
    {
        _logger.LogInformation("Fetching category with ID: {Id}", id);
        var category = await _unitOfWork.Categories.GetByIdAsync(id);

        if (category == null)
        {
            _logger.LogWarning("Category with ID: {Id} not found", id);
            return new NotFoundResult();
        }

        var categoryDTO = new CategoryDTO
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            PhotoUrl = category.PhotoUrl
        };

        _logger.LogInformation("Successfully fetched category with ID: {Id}", id);
        return new OkObjectResult(categoryDTO);
    }

    public async Task<IActionResult> PutCategory(Guid id, CategoryDTO categoryDTO)
    {
        _logger.LogInformation("Updating category with ID: {Id}", id);

        if (id != categoryDTO.Id)
        {
            _logger.LogWarning("Category ID mismatch. Provided ID: {Id}, DTO ID: {DTOId}", id, categoryDTO.Id);
            return new BadRequestResult();
        }

        var category = new CategoryModel
        {
            Id = categoryDTO.Id,
            Name = categoryDTO.Name,
            Description = categoryDTO.Description,
            PhotoUrl = categoryDTO.PhotoUrl
        };

        await _unitOfWork.Categories.UpdateAsync(category);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Successfully updated category with ID: {Id}", id);
        return new NoContentResult();
    }

    public async Task<CreateCategoryRequestDTO> PostCategory([FromForm] CreateCategoryRequestDTO categoryDTO)
    {
        _logger.LogInformation("Creating new category with name: {Name}", categoryDTO.Name);

        var category = new CategoryModel
        {
            Id = Guid.NewGuid(),
            Name = categoryDTO.Name,
            Description = categoryDTO.Description,
            PhotoUrl = categoryDTO.PhotoUrl
        };

        await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Successfully created category with ID: {Id}", category.Id);
        return categoryDTO;
    }

    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        _logger.LogInformation("Deleting category with ID: {Id}", id);
        await _unitOfWork.Categories.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Successfully deleted category with ID: {Id}", id);
        return new NoContentResult();
    }
}
