using backend.Infrastructure.Authentication;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public class SelectedPlaceRepository : ISelectedPlaceRepository
    {
        private readonly TripsDbContext _context;

        public SelectedPlaceRepository(TripsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SelectedPlaceModel>> GetSelectedPlacesAsync()
        {
            return await _context.SelectedPlace.OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<SelectedPlaceModel> GetSelectedPlaceByIdAsync(Guid id)
        {
            return await _context.SelectedPlace.FindAsync(id);
        }

        public async Task<IEnumerable<SelectedPlaceModel>> GetSelectedPlacesByDestinationIdAsync(Guid destinationId)
        {
            return await _context.SelectedPlace
                .Where(sp => sp.TripDestinationId == destinationId)
                .ToListAsync();
        }

        public async Task AddSelectedPlaceAsync(SelectedPlaceModel selectedPlace)
        {
            await _context.SelectedPlace.AddAsync(selectedPlace);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSelectedPlaceAsync(SelectedPlaceModel selectedPlace)
        {
            _context.Entry(selectedPlace).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSelectedPlaceAsync(Guid id)
        {
            var selectedPlace = await _context.SelectedPlace.FindAsync(id);
            if (selectedPlace != null)
            {
                _context.SelectedPlace.Remove(selectedPlace);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> SelectedPlaceExistsAsync(Guid id)
        {
            return await _context.SelectedPlace.AnyAsync(e => e.Id == id);
        }
    }
}
