using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Infrastructure.Authentication;

namespace backend.Infrastructure.Respository
{
    public class TripParticipantRepository : Repository<TripParticipantModel>, ITripParticipantRepository
    {
        private readonly TripsDbContext _context;
        private readonly ILogger<TripParticipantRepository> _logger;

        public TripParticipantRepository(TripsDbContext context, ILogger<TripParticipantRepository> logger) : base(context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<TripParticipantModel>> GetAllTripParticipantsAsync()
        {
            _logger.LogInformation("Fetching all trip participants.");
            return await _context.TripParticipant
                .Include(tp => tp.Participant)
                .ToListAsync();
        }

        public async Task<TripParticipantModel> GetTripParticipantByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching trip participant with ID {TripParticipantId}.", id);
            return await _context.TripParticipant
                .Include(tp => tp.Participant)
                .FirstOrDefaultAsync(tp => tp.Id == id);
        }

        public async Task<IEnumerable<TripParticipantModel>> GetTripParticipantsByTripIdAsync(Guid tripId)
        {
            _logger.LogInformation("Fetching trip participants for trip ID {TripId}.", tripId);
            return await _context.TripParticipant
                .Include(tp => tp.Participant)
                .Where(tp => tp.TripId == tripId)
                .ToListAsync();
        }

        public async Task<bool> TripParticipantExistsAsync(Guid id)
        {
            _logger.LogInformation("Checking if trip participant with ID {TripParticipantId} exists.", id);
            return await _context.TripParticipant.AnyAsync(tp => tp.Id == id);
        }

        public async Task<bool> UpdateTripParticipantAsync(TripParticipantModel tripParticipant)
        {
            _logger.LogInformation("Updating trip participant with ID {TripParticipantId}.", tripParticipant.Id);
            _context.Entry(tripParticipant).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TripParticipantExistsAsync(tripParticipant.Id))
                {
                    _logger.LogWarning("Trip participant with ID {TripParticipantId} does not exist.", tripParticipant.Id);
                    return false;
                }
                throw;
            }
        }

        public async Task<bool> DeleteTripParticipantAsync(Guid id)
        {
            _logger.LogInformation("Deleting trip participant with ID {TripParticipantId}.", id);
            var tripParticipant = await GetTripParticipantByIdAsync(id);
            if (tripParticipant == null)
            {
                _logger.LogWarning("Trip participant with ID {TripParticipantId} not found.", id);
                return false;
            }

            _context.TripParticipant.Remove(tripParticipant);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully deleted trip participant with ID {TripParticipantId}.", id);
            return true;
        }
    }
}
