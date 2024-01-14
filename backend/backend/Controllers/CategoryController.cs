using System.Collections.Generic;
using System.Threading.Tasks;
using backend.DTOs;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using Microsoft.Extensions.Hosting;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/Categorys
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            var categories = await _categoryService.GetCategories(Request.Scheme, Request.Host.ToString(), Request.PathBase.ToString());
            return categories;
        }


        // GET: api/Categorys/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            return await _categoryService.GetCategory(id);
        }

        // PUT: api/Categorys/5
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, CategoryDTO category)
        {
            return await _categoryService.PutCategory(id, category);
        }

        // POST: api/Categorys
        
        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> PostCategory([FromForm] CategoryDTO category)
        {
            return await _categoryService.PostCategory(category);
        }

        // DELETE: api/Categorys/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            return await _categoryService.DeleteCategory(id);
        }
    }
}
