using backend.Domain.DTOs;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Application.Services
{
    public interface ICategoryService
    {
        Task<PagedResult<CategoryDTO>> GetCategories(int page, int pageSize);
        Task<ActionResult<CategoryDTO>> GetCategory(Guid id);
        Task<IActionResult> PutCategory(Guid id, CreateCategoryRequestDTO categoryDTO);
        Task<ActionResult<CategoryModel>> PostCategory([FromForm] CreateCategoryRequestDTO categoryDTO);
        Task<IActionResult> DeleteCategory(Guid id);
    }
}
