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

        public async Task<bool> CategoryExistsAsync(Guid id)
        {
            _logger.LogInformation("Checking if category with ID {Id} exists", id);
            return await _context.Category.AnyAsync(e => e.Id == id);
        }
    }
}
