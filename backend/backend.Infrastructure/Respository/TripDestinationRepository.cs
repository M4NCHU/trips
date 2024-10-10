using backend.Infrastructure.Authentication;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public class TripDestinationRepository : ITripDestinationRepository
    {
        private readonly TripsDbContext _context;

        public TripDestinationRepository(TripsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TripDestinationModel>> GetAllTripDestinationsAsync()
        {
            return await _context.TripDestination
                .Include(td => td.Destination)
                .ToListAsync();
        }

        public async Task<TripDestinationModel> GetTripDestinationByIdAsync(Guid id)
        {
            return await _context.TripDestination
                .Include(td => td.Destination)
                .FirstOrDefaultAsync(td => td.Id == id);
        }

        public async Task<IEnumerable<TripDestinationModel>> GetTripDestinationsByTripIdAsync(Guid tripId)
        {
            return await _context.TripDestination
                .Where(td => td.TripId == tripId)
                .Include(td => td.Destination)
                .ToListAsync();
        }

        public async Task AddTripDestinationAsync(TripDestinationModel tripDestination)
        {
            await _context.TripDestination.AddAsync(tripDestination);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateTripDestinationAsync(TripDestinationModel tripDestination)
        {
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
                    return false;
                }
                throw;
            }
        }

        public async Task<bool> DeleteTripDestinationAsync(Guid id)
        {
            var tripDestination = await GetTripDestinationByIdAsync(id);
            if (tripDestination == null)
            {
                return false;
            }

            _context.TripDestination.Remove(tripDestination);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> CountTripDestinationsByTripIdAsync(Guid tripId)
        {
            return await _context.TripDestination.CountAsync(td => td.TripId == tripId);
        }

        private async Task<bool> TripDestinationExistsAsync(Guid id)
        {
            return await _context.TripDestination.AnyAsync(e => e.Id == id);
        }
    }
}
