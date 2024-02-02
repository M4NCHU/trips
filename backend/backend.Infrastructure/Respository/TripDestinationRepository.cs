using backend.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.Infrastructure.Authentication;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using backend.Application.Services;
using Microsoft.AspNetCore.Hosting;

namespace backend.Infrastructure.Services
{
    public class TripDestinationService : ITripDestinationService
    {
        private readonly TripsDbContext _context;
        private readonly ImageService _imageService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly BaseUrlService _baseUrlService;
        private readonly string _baseUrl;

        public TripDestinationService(TripsDbContext context, ImageService imageService, IWebHostEnvironment hostEnvironment, BaseUrlService baseUrlService)
        {
            _context = context;
            _imageService = imageService;
            _hostEnvironment = hostEnvironment;
            _baseUrlService = baseUrlService;
            _baseUrl = _baseUrlService.GetBaseUrl();
        }

        

        public async Task<ActionResult<IEnumerable<TripDestinationDTO>>> GetTripDestinations()
        {
            if (_context.TripDestination == null)
            {
                return new NotFoundResult();
            }


            var tripDestinations = await _context.TripDestination
                .OrderBy(x => x.Id)
                .Select(x => new TripDestinationDTO
                {
                    Id = x.Id,
                    
                })
                .ToListAsync();

            return tripDestinations;
        }


        public async Task<ActionResult<TripDestinationDTO>> GetTripDestination(Guid id)
        {
            if (_context.TripDestination == null)
            {
                return new NotFoundResult();
            }

            var tripDestination = await _context.TripDestination
        .Where(tp => tp.TripId == id)
        .Select(tp => new TripDestinationDTO
        {
            Id = tp.Id,
            TripId = tp.TripId,
            DestinationId = tp.DestinationId,
            Destination = _context.Destination
                .Where(p => p.Id == tp.DestinationId)
                .Select(p => new DestinationDTO
                {
                    Id = p.Id,
                    PhotoUrl = $"{_baseUrl}/Images/Destination/{p.PhotoUrl}",
                })
                .FirstOrDefault()
        })
        .FirstOrDefaultAsync();

            if (tripDestination == null)
            {
                return new NotFoundResult();
            }


            return tripDestination;

           
        }

        public async Task<ActionResult<IEnumerable<TripDestinationDTO>>> GetTripDestinations(Guid tripId)
        {
            if (_context.TripDestination == null)
            {
                return new NotFoundResult();
            }

            var tripDestinations = await _context.TripDestination
        .Where(tp => tp.TripId == tripId)
        .Select(tp => new TripDestinationDTO
        {
            Id = tp.Id,
            TripId = tp.TripId,
            DestinationId = tp.DestinationId,
            Destination = _context.Destination
                .Where(p => p.Id == tp.DestinationId)
                .Select(p => new DestinationDTO
                {
                    
                    Id = p.Id,
                    PhotoUrl = $"{_baseUrl}/Images/Destination/{p.PhotoUrl}",

                })
                .FirstOrDefault()
        })
        .ToListAsync();

            if (tripDestinations == null)
            {
                return new NotFoundResult();
            }

            

            return tripDestinations;


        }




        public async Task<IActionResult> PutTripDestination(Guid id, TripDestinationDTO tripDestinationDTO)
        {
            if (id != tripDestinationDTO.Id)
            {
                return new BadRequestResult();
            }

            var tripDestination = new TripDestinationModel
            {
                Id = id,
                
            };

            _context.Entry(tripDestination).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripDestinationExists(id))
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

        public async Task<ActionResult<TripDestinationDTO>> PostTripDestination([FromForm] TripDestinationDTO tripDestinationDTO)
        {
            if (_context.TripDestination == null)
            {
                return new ObjectResult("Entity set 'TripsDbContext.TripDestination' is null.")
                {
                    StatusCode = 500
                };
            }

            var currentDate = DateTime.Now.ToUniversalTime();


            var tripDestination = new TripDestinationModel
            {
                Id = tripDestinationDTO.Id,
                TripId = tripDestinationDTO.TripId,
                DestinationId = tripDestinationDTO.DestinationId,
                CreatedAt = currentDate,
                ModifiedAt = currentDate,
                SelectedPlace = new List<SelectedPlaceModel>()
            };

            

            _context.TripDestination.Add(tripDestination);
            await _context.SaveChangesAsync();

            if (tripDestinationDTO.SelectedPlaces != null && tripDestinationDTO.SelectedPlaces.Any())
            {
                foreach (var selectedPlaceDTO in tripDestinationDTO.SelectedPlaces)
                {
                    
                    var selectedPlace = new SelectedPlaceModel
                    {
                        VisitPlaceId = selectedPlaceDTO.VisitPlaceId,
                        TripDestinationId = tripDestination.Id,
                        CreatedAt = currentDate,
                        ModifiedAt = currentDate,

                    };
                    _context.SelectedPlace.Add(selectedPlace);
                }
                await _context.SaveChangesAsync();
            }

            return new CreatedAtActionResult("GetTripDestination", "TripDestination", new { id = tripDestination.Id }, tripDestinationDTO);
        }

        public async Task<IActionResult> DeleteTripDestination(Guid id)
        {
            if (_context.TripDestination == null)
            {
                return new NotFoundResult();
            }

            var tripDestination = await _context.TripDestination.FindAsync(id);
            if (tripDestination == null)
            {
                return new NotFoundResult();
            }

            _context.TripDestination.Remove(tripDestination);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }

        public async Task<int> CountTripDestinations(Guid tripId)
        {
            if (_context.TripDestination == null)
            {
                return 0; // or throw an exception, handle it as needed
            }

            int count = await _context.TripDestination
                .Where(tp => tp.TripId == tripId)
                .CountAsync();

            return count;
        }

        private bool TripDestinationExists(Guid id)
        {
            return (_context.TripDestination?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
