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
    public class TripDestinationRepository : Repository<TripDestinationModel>, ITripDestinationRepository
    {
        private readonly TripsDbContext _context;
        private readonly ILogger<TripDestinationRepository> _logger;

        public TripDestinationRepository(TripsDbContext context, ILogger<TripDestinationRepository> logger) : base(context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<TripDestinationModel>> GetAllTripDestinationsAsync()
        {
            _logger.LogInformation("Fetching all trip destinations.");
            return await _context.TripDestination
                .Include(td => td.Destination)
                .ToListAsync();
        }

        public async Task<TripDestinationModel> GetTripDestinationByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching trip destination with ID {TripDestinationId}.", id);
            return await _context.TripDestination
                .Include(td => td.Destination)
                .FirstOrDefaultAsync(td => td.Id == id);
        }

        public async Task<IEnumerable<TripDestinationModel>> GetTripDestinationsByTripIdAsync(Guid tripId)
        {
            _logger.LogInformation("Fetching trip destinations for trip ID {TripId}.", tripId);
            return await _context.TripDestination
                .Where(td => td.TripId == tripId)
                .Include(td => td.Destination)
                .ToListAsync();
        }

        public async Task<int> CountTripDestinationsByTripIdAsync(Guid tripId)
        {
            _logger.LogInformation("Counting trip destinations for trip ID {TripId}.", tripId);
            return await _context.TripDestination.CountAsync(td => td.TripId == tripId);
        }

        public async Task<bool> UpdateTripDestinationAsync(TripDestinationModel tripDestination)
        {
            _logger.LogInformation("Updating trip destination with ID {TripDestinationId}.", tripDestination.Id);
            _context.Entry(tripDestination).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TripDestinationExistsAsync(tripDestination.Id))
                {
                    _logger.LogWarning("Trip destination with ID {TripDestinationId} does not exist.", tripDestination.Id);
                    return false;
                }
                throw;
            }
        }

        public async Task<bool> DeleteTripDestinationAsync(Guid id)
        {
            _logger.LogInformation("Deleting trip destination with ID {TripDestinationId}.", id);
            var tripDestination = await GetTripDestinationByIdAsync(id);
            if (tripDestination == null)
            {
                _logger.LogWarning("Trip destination with ID {TripDestinationId} not found.", id);
                return false;
            }

            _context.TripDestination.Remove(tripDestination);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully deleted trip destination with ID {TripDestinationId}.", id);
            return true;
        }

        private async Task<bool> TripDestinationExistsAsync(Guid id)
        {
            _logger.LogInformation("Checking if trip destination with ID {TripDestinationId} exists.", id);
            return await _context.TripDestination.AnyAsync(e => e.Id == id);
        }
    }
}
