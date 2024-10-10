using backend.Models;
using backend.Domain.DTOs;
using backend.Infrastructure.Authentication;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public class VisitPlaceRepository : IVisitPlaceRepository
    {
        private readonly TripsDbContext _context;

        public VisitPlaceRepository(TripsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<VisitPlaceDTO>> GetVisitPlacesAsync()
        {
            return await _context.VisitPlace
                .OrderBy(x => x.Id)
                .Select(x => new VisitPlaceDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    PhotoUrl = x.PhotoUrl,
                    DestinationId = x.DestinationId
                })
                .ToListAsync();
        }

        public async Task<VisitPlaceDTO> GetVisitPlaceByIdAsync(Guid id)
        {
            var visitPlace = await _context.VisitPlace.FindAsync(id);
            if (visitPlace == null)
            {
                return null;
            }

            return new VisitPlaceDTO
            {
                Id = visitPlace.Id,
                Name = visitPlace.Name,
                Description = visitPlace.Description,
                PhotoUrl = visitPlace.PhotoUrl,
                DestinationId = visitPlace.DestinationId
            };
        }

        public async Task<IEnumerable<VisitPlaceDTO>> GetVisitPlacesByDestinationAsync(Guid destinationId)
        {
            return await _context.VisitPlace
                .Where(vp => vp.DestinationId == destinationId)
                .Select(vp => new VisitPlaceDTO
                {
                    Id = vp.Id,
                    Name = vp.Name,
                    Description = vp.Description,
                    PhotoUrl = vp.PhotoUrl,
                    DestinationId = vp.DestinationId
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateVisitPlaceAsync(VisitPlaceDTO visitPlaceDTO)
        {
            var visitPlace = await _context.VisitPlace.FindAsync(visitPlaceDTO.Id);
            if (visitPlace == null)
            {
                return false;
            }

            visitPlace.Name = visitPlaceDTO.Name;
            visitPlace.Description = visitPlaceDTO.Description;
            visitPlace.PhotoUrl = visitPlaceDTO.PhotoUrl;
            visitPlace.DestinationId = visitPlaceDTO.DestinationId;

            _context.Entry(visitPlace).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<CreateVisitPlaceDTO> CreateVisitPlaceAsync(CreateVisitPlaceDTO visitPlaceDTO)
        {
            var currentDate = DateTime.UtcNow;
            var visitPlace = new VisitPlace
            {
                Id = Guid.NewGuid(),
                Name = visitPlaceDTO.Name,
                Description = visitPlaceDTO.Description,
                PhotoUrl = visitPlaceDTO.PhotoUrl,
                DestinationId = visitPlaceDTO.DestinationId,
                CreatedAt = currentDate,
                ModifiedAt = currentDate
            };

            _context.VisitPlace.Add(visitPlace);
            await _context.SaveChangesAsync();

            return visitPlaceDTO;
        }

        public async Task<bool> DeleteVisitPlaceAsync(Guid id)
        {
            var visitPlace = await _context.VisitPlace.FindAsync(id);
            if (visitPlace == null)
            {
                return false;
            }

            _context.VisitPlace.Remove(visitPlace);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
