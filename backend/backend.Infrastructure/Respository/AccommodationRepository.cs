using backend.Infrastructure.Authentication;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Respository
{
  
    public class AccommodationRepository : IAccommodationRepository
    {
        private readonly TripsDbContext _context;

        public AccommodationRepository(TripsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AccommodationModel>> GetAccommodations(int page, int pageSize)
        {
            return await _context.Accommodation
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<AccommodationModel> GetAccommodationById(Guid id)
        {
            return await _context.Accommodation.FindAsync(id);
        }

        public async Task AddAccommodation(AccommodationModel accommodation)
        {
            _context.Accommodation.Add(accommodation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAccommodation(AccommodationModel accommodation)
        {
            _context.Entry(accommodation).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAccommodation(Guid id)
        {
            var accommodation = await GetAccommodationById(id);
            if (accommodation != null)
            {
                _context.Accommodation.Remove(accommodation);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> AccommodationExists(Guid id)
        {
            return await _context.Accommodation.AnyAsync(e => e.Id == id);
        }
    }
}
