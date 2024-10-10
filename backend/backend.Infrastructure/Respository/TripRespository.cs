using backend.Infrastructure.Authentication;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public class TripRepository : ITripRepository
    {
        private readonly TripsDbContext _context;

        public TripRepository(TripsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TripModel>> GetAllTripsAsync()
        {
            return await _context.Trip
                .Include(t => t.User)
                .Include(t => t.TripDestinations)
                .ThenInclude(td => td.SelectedPlace)
                .ThenInclude(sp => sp.VisitPlace)
                .ToListAsync();
        }

        public async Task<TripModel> GetTripByIdAsync(Guid id)
        {
            return await _context.Trip
                .Include(t => t.User)
                .Include(t => t.TripDestinations)
                .ThenInclude(td => td.SelectedPlace)
                .ThenInclude(sp => sp.VisitPlace)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TripModel>> GetTripsByUserIdAsync(string userId)
        {
            return await _context.Trip
                .Where(t => t.User.Id == userId)
                .Include(t => t.TripDestinations)
                .ThenInclude(td => td.SelectedPlace)
                .ThenInclude(sp => sp.VisitPlace)
                .ToListAsync();
        }

        public async Task AddTripAsync(TripModel trip)
        {
            await _context.Trip.AddAsync(trip);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTripAsync(TripModel trip)
        {
            _context.Trip.Update(trip);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTripAsync(Guid id)
        {
            var trip = await _context.Trip.FindAsync(id);
            if (trip != null)
            {
                _context.Trip.Remove(trip);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> TripExistsAsync(Guid id)
        {
            return await _context.Trip.AnyAsync(t => t.Id == id);
        }
    }
}
