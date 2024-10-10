using backend.Domain.Filters;
using backend.Infrastructure.Authentication;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public class DestinationRepository : IDestinationRepository
    {
        private readonly TripsDbContext _context;

        public DestinationRepository(TripsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DestinationModel>> GetDestinationsAsync(DestinationFilter filter, int page, int pageSize)
        {
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

        public async Task<DestinationModel> GetDestinationByIdAsync(Guid id)
        {
            return await _context.Destination
                .Include(d => d.Category)
                .Include(d => d.VisitPlaces)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<DestinationModel>> SearchDestinationsAsync(string searchTerm)
        {
            return await _context.Destination
                .Where(d => d.Name.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<List<DestinationModel>> GetDestinationsForTripAsync(Guid tripId)
        {
            return await _context.TripDestination
                .Where(td => td.TripId == tripId)
                .Select(td => td.Destination)
                .ToListAsync();
        }

        public async Task AddDestinationAsync(DestinationModel destination)
        {
            await _context.Destination.AddAsync(destination);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDestinationAsync(DestinationModel destination)
        {
            _context.Entry(destination).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDestinationAsync(Guid id)
        {
            var destination = await _context.Destination.FindAsync(id);
            if (destination != null)
            {
                _context.Destination.Remove(destination);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> DestinationExistsAsync(Guid id)
        {
            return await _context.Destination.AnyAsync(d => d.Id == id);
        }
    }
}
