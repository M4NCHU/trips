using AutoMapper;
using backend.Application.Services;
using backend.Domain.DTOs;
using backend.Infrastructure.Respository;
using backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IImageService _imageService;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService, ILogger<CategoryService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
        _imageService = imageService;
    }
    public async Task<PagedResult<CategoryDTO>> GetCategories(int page, int pageSize)
    {
        var categoriesPaged = await _unitOfWork.Categories.GetPagedCategories(page, pageSize);

        var categoryDTOs = _mapper.Map<List<CategoryDTO>>(categoriesPaged.Items);

        return new PagedResult<CategoryDTO>
        {
            Items = categoryDTOs,
            TotalItems = categoriesPaged.TotalItems,
            PageSize = categoriesPaged.PageSize,
            CurrentPage = categoriesPaged.CurrentPage
        };
    }





    public async Task<ActionResult<CategoryDTO>> GetCategory(Guid id)
    {
        try
        {
            _logger.LogInformation("Fetching category with ID: {Id}", id);
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category == null)
            {
                _logger.LogWarning("Category with ID: {Id} not found", id);
                return new NotFoundResult();
            }

            var categoryDTO = _mapper.Map<CategoryDTO>(category);


            _logger.LogInformation("Successfully fetched category with ID: {Id}", id);
            return new OkObjectResult(categoryDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching category with ID: {Id}", id);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<IActionResult> PutCategory(Guid id, CreateCategoryRequestDTO categoryDTO)
    {
        try
        {
            _logger.LogInformation("Updating category with ID: {Id}", id);

            var categoryExists = await _unitOfWork.Categories.GetByIdAsync(id);

            if (categoryExists is null)
            {
                _logger.LogWarning("Provided Name: {Id}. Category doesn't exists", id);
                return new NotFoundResult();
            }

            _mapper.Map(categoryDTO, categoryExists);

            categoryExists.ModifiedAt = DateTime.UtcNow;
            categoryExists.PhotoUrl = string.Empty;

            if (categoryDTO.ImageFile != null)
            {
                var photoUrl = await _imageService.SaveImage(categoryDTO.ImageFile, "Category");
                categoryExists.PhotoUrl = photoUrl;
                _logger.LogInformation("Category image updated for ID: {id}. New photo URL: {photoUrl}", categoryExists.Id, photoUrl);
            }

            await _unitOfWork.Categories.UpdateAsync(categoryExists);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Successfully updated category with ID: {Id}", id);
            return new OkObjectResult(categoryExists);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating category with ID: {Id}", id);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<ActionResult<CategoryModel>> PostCategory([FromForm] CreateCategoryRequestDTO categoryDTO)
    {
        try
        {
            _logger.LogInformation("Creating new category with name: {Name}", categoryDTO.Name);

            var categoryExists = _unitOfWork.Categories.CategoryExistsAsync(categoryDTO.Name).Result;

            if(categoryExists)
            {
                _logger.LogWarning("Provided Name: {Name}. Category already exists", categoryDTO.Name);
                return new NotFoundResult();
            }

            var category = _mapper.Map<CategoryModel>(categoryDTO);
            category.CreatedAt = DateTime.UtcNow;
            category.PhotoUrl = string.Empty;

            if (categoryDTO.ImageFile != null)
            {
                var photoUrl = await _imageService.SaveImage(categoryDTO.ImageFile, "Category");
                category.PhotoUrl = photoUrl;
                _logger.LogInformation("Category image updated for ID: {id}. New photo URL: {photoUrl}", category.Id, photoUrl);
            }

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Successfully created category with ID: {Id}", category.Id);
            return new OkObjectResult(category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the category.");
            throw;  
        }
    }

    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        try
        {
            _logger.LogInformation("Deleting category with ID: {Id}", id);
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                _logger.LogWarning("Category with ID: {Id} not found", id);
                return new NotFoundResult();
            }

            await _unitOfWork.Categories.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Successfully deleted category with ID: {Id}", id);
            return new NoContentResult();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting category with ID: {Id}", id);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}
