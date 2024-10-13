using backend.Domain.DTOs;
using backend.Infrastructure.Authentication;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.Infrastructure.Respository
{
    public class CategoryRepository : Repository<CategoryModel>, ICategoryRepository
    {
        private readonly TripsDbContext _context;
        private readonly ILogger<CategoryRepository> _logger;

        public CategoryRepository(TripsDbContext context, ILogger<CategoryRepository> logger) : base(context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PagedResult<CategoryModel>> GetPagedCategories(int page, int pageSize)
        {
            _logger.LogInformation("Fetching paginated categories for page {Page} with page size {PageSize}", page, pageSize);


            var totalItems = await _context.Category.CountAsync();


            var categories = await _context.Category
                .OrderBy(c => c.Name) 
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<CategoryModel>
            {
                Items = categories,
                TotalItems = totalItems,
                PageSize = pageSize,
                CurrentPage = page
            };
        }


        public IQueryable<CategoryModel> GetAllCategoriesAsQueryable()
        {
            _logger.LogInformation("Fetching all categories as IQueryable");
            return _context.Category.AsQueryable();
        }

        public async Task<bool> CategoryExistsAsync(Guid id)
        {
            _logger.LogInformation("Checking if category with ID {Id} exists", id);
            return await _context.Category.AnyAsync(e => e.Id == id);
        }
        public async Task<bool> CategoryExistsAsync(string name)
        {
            _logger.LogInformation("Checking if category with name {Name} exists", name);
            return await _context.Category.AnyAsync(e => e.Name == name);
        }

    }
}
