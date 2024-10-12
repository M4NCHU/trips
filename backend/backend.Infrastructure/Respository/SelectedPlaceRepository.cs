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
    public class SelectedPlaceRepository : Repository<SelectedPlaceModel>, ISelectedPlaceRepository
    {
        private readonly TripsDbContext _context;
        private readonly ILogger<SelectedPlaceRepository> _logger;

        public SelectedPlaceRepository(TripsDbContext context, ILogger<SelectedPlaceRepository> logger) : base(context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<SelectedPlaceModel>> GetSelectedPlacesAsync()
        {
            _logger.LogInformation("Fetching all selected places.");
            return await _context.SelectedPlace
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<SelectedPlaceModel>> GetSelectedPlacesByDestinationIdAsync(Guid destinationId)
        {
            _logger.LogInformation("Fetching selected places for destination ID: {DestinationId}", destinationId);
            return await _context.SelectedPlace
                .Where(sp => sp.TripDestinationId == destinationId)
                .ToListAsync();
        }

        public async Task<bool> SelectedPlaceExistsAsync(Guid id)
        {
            _logger.LogInformation("Checking if selected place with ID {Id} exists.", id);
            return await _context.SelectedPlace.AnyAsync(e => e.Id == id);
        }
    }
}
