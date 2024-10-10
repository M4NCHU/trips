using backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Infrastructure.Authentication;

namespace backend.Infrastructure.Respository
{
    public class TripParticipantRepository : ITripParticipantRepository
    {
        private readonly TripsDbContext _context;

        public TripParticipantRepository(TripsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TripParticipantModel>> GetAllTripParticipantsAsync()
        {
            return await _context.TripParticipant
                .Include(tp => tp.Participant)
                .ToListAsync();
        }

        public async Task<TripParticipantModel> GetTripParticipantByIdAsync(Guid id)
        {
            return await _context.TripParticipant
                .Include(tp => tp.Participant)
                .FirstOrDefaultAsync(tp => tp.Id == id);
        }

        public async Task<IEnumerable<TripParticipantModel>> GetTripParticipantsByTripIdAsync(Guid tripId)
        {
            return await _context.TripParticipant
                .Include(tp => tp.Participant)
                .Where(tp => tp.TripId == tripId)
                .ToListAsync();
        }

        public async Task AddTripParticipantAsync(TripParticipantModel tripParticipant)
        {
            await _context.TripParticipant.AddAsync(tripParticipant);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTripParticipantAsync(TripParticipantModel tripParticipant)
        {
            _context.TripParticipant.Update(tripParticipant);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTripParticipantAsync(Guid id)
        {
            var tripParticipant = await _context.TripParticipant.FindAsync(id);
            if (tripParticipant != null)
            {
                _context.TripParticipant.Remove(tripParticipant);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> TripParticipantExistsAsync(Guid id)
        {
            return await _context.TripParticipant.AnyAsync(tp => tp.Id == id);
        }
    }
}
