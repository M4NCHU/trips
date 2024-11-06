using backend.Infrastructure.Authentication;
using backend.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Respository
{
    public class AccommodationRepository : Repository<AccommodationModel>, IAccommodationRepository
    {
        private readonly TripsDbContext _context;
        private readonly ILogger<AccommodationRepository> _logger;

        public AccommodationRepository(TripsDbContext context, ILogger<AccommodationRepository> logger) : base(context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<AccommodationModel>> GetAccommodations(int page, int pageSize)
        {
            _logger.LogInformation("Fetching accommodations with pagination. Page: {Page}, PageSize: {PageSize}", page, pageSize);
            return await _context.Accommodation
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<bool> AccommodationExists(Guid id)
        {
            _logger.LogInformation("Checking if accommodation with ID {Id} exists", id);
            return await _context.Accommodation.AnyAsync(e => e.Id == id);
        }
    }
}