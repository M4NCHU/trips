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
    public class TripRepository : Repository<TripModel>, ITripRepository
    {
        private readonly TripsDbContext _context;
        private readonly ILogger<TripRepository> _logger;

        public TripRepository(TripsDbContext context, ILogger<TripRepository> logger) : base(context)
        {
            _context = context;
            _logger = logger;
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

        public async Task<bool> TripExistsAsync(Guid id)
        {
            return await _context.Trip.AnyAsync(t => t.Id == id);
        }
    }
}
