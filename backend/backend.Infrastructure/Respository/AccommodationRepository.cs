using backend.Infrastructure.Authentication;
using backend.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Domain.DTOs;
using backend.Domain.Filters;

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

        public async Task<PagedResult<DestinationModel>> GetAccomodationAsync(DestinationFilter filter, int page, int pageSize)
        {
            _logger.LogInformation("Fetching destinations with filter and pagination. Page: {Page}, PageSize: {PageSize}", page, pageSize);

            var query = _context.Destination.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.CategoryId) && Guid.TryParse(filter.CategoryId, out Guid parsedCategoryId))
            {
                query = query.Where(d => d.CategoryId == parsedCategoryId);
            }

            if (filter != null &&
                filter.NorthEastLat.HasValue && filter.NorthEastLng.HasValue &&
                filter.SouthWestLat.HasValue && filter.SouthWestLng.HasValue)
            {
                query = query.Where(d =>
                    d.GeoLocation != null &&
                    d.GeoLocation.Latitude <= filter.NorthEastLat &&
                    d.GeoLocation.Latitude >= filter.SouthWestLat &&
                    d.GeoLocation.Longitude <= filter.NorthEastLng &&
                    d.GeoLocation.Longitude >= filter.SouthWestLng);
            }

            var totalItems = await query.CountAsync();

            var destinations = await query
                .OrderBy(d => d.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(d => d.GeoLocation)
                .ToListAsync();

            return new PagedResult<DestinationModel>
            {
                Items = destinations,
                TotalItems = totalItems,
                PageSize = pageSize,
                CurrentPage = page
            };
        }
        public async Task<bool> AccommodationExists(Guid id)
        {
            _logger.LogInformation("Checking if accommodation with ID {Id} exists", id);
            return await _context.Accommodation.AnyAsync(e => e.Id == id);
        }
    }
}