using backend.Domain.DTOs;
using backend.Domain.Filters;
using backend.Infrastructure.Authentication;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.Infrastructure.Respository
{
    public class DestinationRepository : Repository<DestinationModel>, IDestinationRepository
    {
        private readonly TripsDbContext _context;
        private readonly ILogger<DestinationRepository> _logger;

        public DestinationRepository(TripsDbContext context, ILogger<DestinationRepository> logger) : base(context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PagedResult<DestinationModel>> GetDestinationsAsync(DestinationFilter filter, int page, int pageSize)
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

        public async Task<IEnumerable<DestinationModel>> SearchDestinationsAsync(string searchTerm)
        {
            _logger.LogInformation("Searching destinations with term: {SearchTerm}", searchTerm);

            return await _context.Destination
                .Where(d => d.Name.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<List<DestinationModel>> GetDestinationsForTripAsync(Guid tripId)
        {
            _logger.LogInformation("Fetching destinations for trip ID {TripId}", tripId);

            return await _context.TripDestination
                .Where(td => td.TripId == tripId)
                .Select(td => td.Destination)
                .ToListAsync();
        }

        public async Task<List<DestinationModel>> GetDestinationsByIdsAsync(List<Guid> ids)
        {
            if (ids == null || !ids.Any())
                return new List<DestinationModel>();

            try
            {
                _logger.LogInformation("Executing query for Destination IDs: {@ids}", ids);

                var destinations = await _context.Destination
                    .Where(d => ids.Contains(d.Id))
                    .ToListAsync();

                _logger.LogInformation("Fetched {count} destinations.", destinations.Count);

                return destinations;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching destinations by IDs.");
                throw;
            }
        }




        public async Task<bool> DestinationExistsAsync(Guid id)
        {
            _logger.LogInformation("Checking if destination with ID {Id} exists", id);
            return await _context.Destination.AnyAsync(d => d.Id == id);
        }

        public async Task<List<DestinationModel>> GetByIdsAsync(List<Guid> ids)
        {
            _logger.LogInformation("Fetching destinations for IDs: {Ids}", ids);

            return await _context.Destination
                .Where(d => ids.Contains(d.Id))
                .Include(d => d.GeoLocation) 
                .Include(d => d.Category)    
                .ToListAsync();
        }

        public async Task<DestinationModel?> GetWithDetailsAsync(Guid id)
        {
            _logger.LogInformation("Fetching destination with ID: {Id}", id);

            return await _context.Destination
                .Include(d => d.GeoLocation) 
                .Include(d => d.Category)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

    }
}