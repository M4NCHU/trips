using backend.Domain.Filters;
using backend.Infrastructure.Authentication;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<DestinationModel>> GetDestinationsAsync(DestinationFilter filter, int page, int pageSize)
        {
            _logger.LogInformation("Fetching destinations with filter and pagination. Page: {Page}, PageSize: {PageSize}", page, pageSize);

            var query = _context.Destination.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.CategoryId) && Guid.TryParse(filter.CategoryId, out Guid parsedCategoryId))
            {
                query = query.Where(d => d.CategoryId == parsedCategoryId);
            }

            return await query
                .OrderBy(d => d.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
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

        public async Task<bool> DestinationExistsAsync(Guid id)
        {
            _logger.LogInformation("Checking if destination with ID {Id} exists", id);
            return await _context.Destination.AnyAsync(d => d.Id == id);
        }

    }
}
