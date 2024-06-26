﻿using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using Microsoft.Extensions.Hosting;
using backend.Application.Services;
using Microsoft.AspNetCore.Authorization;
using backend.Infrastructure.Services;
using backend.Domain.DTOs;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryService> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryService> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        // GET: api/Category/GetAllDestinations
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            var categories = await _categoryService.GetCategories();
            return categories;
        }


        // GET: api/Category/GetCategoryById/5
        [HttpGet("{categoryId}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(Guid categoryId)
        {
            return await _categoryService.GetCategory(categoryId);
        }

        // PUT: api/Category/5

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(Guid id, CategoryDTO category)
        {
            return await _categoryService.PutCategory(id, category);
        }

        // POST: api/Category
        /*[Authorize(Roles = "admin")]*/
        [HttpPost()]
        public async Task<ActionResult<CreateCategoryRequestDTO>> PostCategory([FromForm] CreateCategoryRequestDTO category)
        {
            return await _categoryService.PostCategory(category);
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            return await _categoryService.DeleteCategory(id);
        }
    }
}
