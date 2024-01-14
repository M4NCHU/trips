using backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Services
{
    public interface ICategoryService
    {
        Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories(string scheme = "https", string host = "example.com", string pathBase = "/basepath");

        Task<ActionResult<CategoryDTO>> GetCategory(int id);

        Task<IActionResult> PutCategory(int id, CategoryDTO category);

        Task<ActionResult<CategoryDTO>> PostCategory([FromForm] CategoryDTO category);

        Task<IActionResult> DeleteCategory(int id);
    }
}
