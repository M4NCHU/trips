using backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.Authentication;
using Microsoft.Extensions.Hosting;

namespace backend.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly TripsDbContext _context;
        private readonly ImageService _imageService;
        private readonly IWebHostEnvironment _hostEnvironment;


        public CategoryService(TripsDbContext context, ImageService imageService, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _imageService = imageService;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories(string scheme = "https", string host = "example.com", string pathBase = "/basepath")
        {
            if (_context.Category == null)
            {
                return new NotFoundResult();
            }

            var categories = await _context.Category
                .OrderBy(x => x.Id)
                .Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    PhotoUrl = string.Format("{0}://{1}{2}/Images/Category/{3}", scheme, host, pathBase, x.PhotoUrl)
                })
                .ToListAsync();

            return categories;
        }


        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            if (_context.Category == null)
            {
                return new NotFoundResult();
            }

            var category = await _context.Category.FindAsync(id);

            if (category == null)
            {
                return new NotFoundResult();
            }

            var CategoryDTO = new CategoryDTO
            {
                Name = category.Name,
                Description = category.Description,
                PhotoUrl = category.PhotoUrl
            };

            return CategoryDTO;
        }

        public async Task<IActionResult> PutCategory(int id, CategoryDTO CategoryDTO)
        {
            if (id != CategoryDTO.Id)
            {
                return new BadRequestResult();
            }

            var category = new Category
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

        public async Task<ActionResult<CategoryDTO>> PostCategory([FromForm] CategoryDTO CategoryDTO)
        {
            if (_context.Category == null)
            {
                return new ObjectResult("Entity set 'TripsDbContext.Category' is null.")
                {
                    StatusCode = 500
                };
            }

            if (CategoryDTO.ImageFile == null)
            {
                return new BadRequestObjectResult("The 'ImageFileDTO' field is required.");
            }

            CategoryDTO.PhotoUrl = await _imageService.SaveImage(CategoryDTO.ImageFile, "Category");

            var category = new Category
            {
                Name = CategoryDTO.Name,
                Description = CategoryDTO.Description,
                PhotoUrl = CategoryDTO.PhotoUrl
            };

            _context.Category.Add(category);
            await _context.SaveChangesAsync();

            return new CreatedAtActionResult("GetCategory", "Category", new { id = category.Id }, CategoryDTO);
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (_context.Category == null)
            {
                return new NotFoundResult();
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

        private bool CategoryExists(int id)
        {
            return (_context.Category?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
