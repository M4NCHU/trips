﻿using backend.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;


namespace backend.Application.Services
{
    public interface ICategoryService
    {
        Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories();

        Task<ActionResult<CategoryDTO>> GetCategory(Guid id);

        Task<IActionResult> PutCategory(Guid id, CategoryDTO category);

        Task<CreateCategoryRequestDTO> PostCategory([FromForm]CreateCategoryRequestDTO categoryDTO);

        Task<IActionResult> DeleteCategory(Guid id);
    }
}
