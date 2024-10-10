using backend.Infrastructure.Authentication;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly TripsDbContext _context;

        public CategoryRepository(TripsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryModel>> GetCategoriesAsync()
        {
            return await _context.Category.OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<CategoryModel> GetCategoryByIdAsync(Guid id)
        {
            return await _context.Category.FindAsync(id);
        }

        public async Task AddCategoryAsync(CategoryModel category)
        {
            await _context.Category.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(CategoryModel category)
        {
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category != null)
            {
                _context.Category.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> CategoryExistsAsync(Guid id)
        {
            return await _context.Category.AnyAsync(e => e.Id == id);
        }
    }
}
