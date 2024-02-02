using backend.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.Infrastructure.Authentication;
using Microsoft.Extensions.Hosting;
using backend.Application.Services;
using Microsoft.AspNetCore.Hosting;

using System.ComponentModel.DataAnnotations;

namespace backend.Infrastructure.Services
{
    public class SelectedPlaceService : ISelectedPlaceService
    {
        private readonly TripsDbContext _context;
        private readonly ImageService _imageService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly BaseUrlService _baseUrlService;
        private readonly string _baseUrl;

        public SelectedPlaceService(TripsDbContext context, ImageService imageService, IWebHostEnvironment hostEnvironment, BaseUrlService baseUrlService)
        {
            _context = context;
            _imageService = imageService;
            _hostEnvironment = hostEnvironment;
            _baseUrlService = baseUrlService;
            _baseUrl = _baseUrlService.GetBaseUrl();
        }

        

        public async Task<ActionResult<IEnumerable<SelectedPlaceDTO>>> GetSelectedPlaces()
        {
            if (_context.SelectedPlace == null)
            {
                return new NotFoundResult();
            }


            var selectedPlaces = await _context.SelectedPlace
                .OrderBy(x => x.Id)
                .Select(x => new SelectedPlaceDTO
                {
                    Id = x.Id,
                    
                })
                .ToListAsync();

            return selectedPlaces;
        }


        public async Task<ActionResult<SelectedPlaceDTO>> GetSelectedPlace(Guid id)
        {
            if (_context.SelectedPlace == null)
            {
                return new NotFoundResult();
            }

            var selectedPlace = await _context.SelectedPlace
        .Where(tp => tp.Id == id)
        .Select(tp => new SelectedPlaceDTO
        {
            Id = tp.Id,
            TripDestinationId = tp.TripDestinationId,
            VisitPlaceId = tp.VisitPlaceId,
            VisitPlace = _context.VisitPlace
                .Where(p => p.Id == tp.VisitPlaceId)
                .Select(p => new VisitPlaceDTO
                {
                    Id = p.Id,
                    PhotoUrl = $"{_baseUrl}/Images/Destination/{p.PhotoUrl}",
                })
                .FirstOrDefault()
        })
        .FirstOrDefaultAsync();

            if (selectedPlace == null)
            {
                return new NotFoundResult();
            }


            return selectedPlace;

           
        }

        public async Task<ActionResult<IEnumerable<SelectedPlaceDTO>>> GetSelectedPlaces(Guid destinationId)
        {
            if (_context.SelectedPlace == null)
            {
                return new NotFoundResult();
            }

            var selectedPlaces = await _context.SelectedPlace
        .Where(tp => tp.TripDestinationId == destinationId)
        .Select(tp => new SelectedPlaceDTO
        {
            Id = tp.Id,
            TripDestinationId = tp.TripDestinationId,
            VisitPlaceId = tp.VisitPlaceId,
            CreatedAt = tp.CreatedAt,
            ModifiedAt = tp.ModifiedAt,
            
        })
        .ToListAsync();


            if (selectedPlaces == null)
            {
                return new NotFoundResult();
            }

            

            return selectedPlaces;


        }




        public async Task<IActionResult> PutSelectedPlace(Guid id, SelectedPlaceDTO selectedPlaceDTO)
        {
            if (id != selectedPlaceDTO.Id)
            {
                return new BadRequestResult();
            }

            var selectedPlace = new SelectedPlaceModel
            {
                Id = id,
                
            };

            _context.Entry(selectedPlace).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SelectedPlaceExists(id))
                {
                    return new NotFoundResult();
                }
                else
                {
                    throw;
                }
            }

            return new NoContentResult();
        }

        public async Task<ActionResult<SelectedPlaceDTO>> PostSelectedPlace([FromForm] SelectedPlaceDTO selectedPlaceDTO)
        {
            if (_context.SelectedPlace == null)
            {
                return new ObjectResult("Entity set 'TripsDbContext.SelectedPlace' is null.")
                {
                    StatusCode = 500
                };
            }

            
           

            var selectedPlace = new SelectedPlaceModel
            {
                Id = selectedPlaceDTO.Id,
                TripDestinationId = selectedPlaceDTO.TripDestinationId,
                VisitPlaceId = selectedPlaceDTO.VisitPlaceId,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
            };

            

            _context.SelectedPlace.Add(selectedPlace);
            await _context.SaveChangesAsync();

            return new CreatedAtActionResult("GetSelectedPlace", "SelectedPlace", new { id = selectedPlace.Id }, selectedPlaceDTO);
        }

        public async Task<IActionResult> DeleteSelectedPlace(Guid id)
        {
            if (_context.SelectedPlace == null)
            {
                return new NotFoundResult();
            }

            var selectedPlace = await _context.SelectedPlace.FindAsync(id);
            if (selectedPlace == null)
            {
                return new NotFoundResult();
            }

            _context.SelectedPlace.Remove(selectedPlace);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }

        private bool SelectedPlaceExists(Guid id)
        {
            return (_context.SelectedPlace?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
