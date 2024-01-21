using backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.Authentication;
using Microsoft.Extensions.Hosting;

namespace backend.Services
{
    public class TripService : ITripService
    {
        private readonly TripsDbContext _context;
        private readonly ImageService _imageService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IDestinationService _destinationService;

        public TripService(TripsDbContext context, ImageService imageService, IWebHostEnvironment hostEnvironment, IDestinationService destinationService)
        {
            _context = context;
            _imageService = imageService;
            _hostEnvironment = hostEnvironment;
            _destinationService = destinationService;
        }   

        public async Task<ActionResult<IEnumerable<TripDTO>>> GetTrips()
        {
            var trips = await _context.Trip
                .OrderBy(x => x.Id)
                .Select(x => new TripDTO
                {
                    Id = x.Id,
                    Status = x.Status,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    TotalPrice = x.TotalPrice,
                    TripDestinations = x.TripDestinations.Select(td => new TripDestinationDTO
                    {
                        DestinationId = td.DestinationId,
                        TripId = td.TripId,
                        
                        SelectedPlaces = td.SelectedPlace.Select(sp => new SelectedPlaceDTO
                        {
                            Id = sp.Id,
                            TripDestinationId = sp.TripDestinationId,
                            VisitPlaceId = sp.VisitPlaceId,
                            
                        }).ToList()
                    }).ToList(),
                    SelectedPlaces = x.SelectedPlaces.Select(sp => new SelectedPlaceDTO
                    {
                        VisitPlaceId = sp.VisitPlaceId,
                        
                    }).ToList()
                })
                .ToListAsync();

            return trips;
        }

        public async Task<ActionResult<TripDTO>> GetTripById(Guid id)
        {
            var tripModel = await _context.Trip.FindAsync(id);

            if (tripModel == null)
            {
                return new NotFoundResult(); 
            }

            var tripDTO = new TripDTO
            {
                Id = tripModel.Id,
                Status = tripModel.Status,
                StartDate = tripModel.StartDate,
                EndDate = tripModel.EndDate,
                TotalPrice = tripModel.TotalPrice,
                TripDestinations = _context.TripDestination
        .Where(td => td.TripId == tripModel.Id)
        .Select(td => new TripDestinationDTO
        {
            TripId = td.TripId,
            DestinationId = td.DestinationId,
            
            SelectedPlaces = _context.SelectedPlace
                .Where(sp => sp.TripDestinationId == td.Id)
                .Select(sp => new SelectedPlaceDTO
                {
                    Id = sp.Id,
                    TripDestinationId = sp.TripDestinationId,
                    VisitPlaceId = sp.VisitPlace.Id,
                   
                }).ToList() ?? new List<SelectedPlaceDTO>()
        }).ToList(),
            };

            return tripDTO;
           
        }

        

        public async Task<List<VisitPlaceDTO>> GetVisitPlacesForTrip(Guid tripId)
        {
            var visitPlaces = await _context.TripDestination
                .Where(td => td.TripId == tripId)
                .SelectMany(td => td.SelectedPlace)
                .Select(sp => new VisitPlaceDTO
                {
                    Id = sp.VisitPlace.Id,
                    Name = sp.VisitPlace.Name,
                    Description = sp.VisitPlace.Description,
                    PhotoUrl = sp.VisitPlace.PhotoUrl,
                    Price = sp.VisitPlace.Price,
                    ImageFile = null, // You might need to adjust this based on your requirements
                    DestinationId = sp.VisitPlace.DestinationId
                })
                .ToListAsync();

            return visitPlaces;
        }


        public async Task<IActionResult> PutTrip(Guid id, TripDTO tripDTO)
        {
            if (id != tripDTO.Id)
            {
                return new BadRequestResult();
            }

            var trip = new TripModel
            {
                Id = id,
                Status = tripDTO.Status,
                StartDate = tripDTO.StartDate,
                EndDate = tripDTO.EndDate,
                TotalPrice = tripDTO.TotalPrice,
                TripDestinations = tripDTO.TripDestinations.Select(td => new TripDestinationModel
                {
                    DestinationId = td.DestinationId,
                    SelectedPlace = td.SelectedPlaces.Select(sp => new SelectedPlaceModel
                    {
                        VisitPlaceId = sp.VisitPlaceId
                    }).ToList()
                }).ToList(),
                SelectedPlaces = tripDTO.SelectedPlaces.Select(sp => new SelectedPlaceModel
                {
                    VisitPlaceId = sp.VisitPlaceId
                }).ToList()
            };

            _context.Entry(trip).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripExists(id))
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

        public async Task<ActionResult<TripDTO>> PostTrip([FromForm] TripDTO tripDTO)
        {
            var currentDate = DateTime.Now.ToUniversalTime();


            var trip = new TripModel
            {
                Status = tripDTO.Status,
                StartDate = tripDTO.StartDate,
                EndDate = tripDTO.EndDate,
                TotalPrice = tripDTO.TotalPrice,
                CreatedAt = currentDate,
                ModifiedAt = currentDate,

                TripDestinations = tripDTO.TripDestinations.Select(td => new TripDestinationModel
                {
                    DestinationId = td.DestinationId,
                    SelectedPlace = td.SelectedPlaces.Select(sp => new SelectedPlaceModel
                    {
                        VisitPlaceId = sp.VisitPlaceId
                    }).ToList()
                }).ToList(),
                SelectedPlaces = tripDTO.SelectedPlaces.Select(sp => new SelectedPlaceModel
                {
                    VisitPlaceId = sp.VisitPlaceId
                }).ToList()
            };

            _context.Trip.Add(trip);
            await _context.SaveChangesAsync();

            return new CreatedAtActionResult("GetTrip", "Trip", new { id = trip.Id }, tripDTO);
        }

        public async Task<IActionResult> DeleteTrip(Guid id)
        {
            var trip = await _context.Trip.FindAsync(id);
            if (trip == null)
            {
                return new NotFoundResult();
            }

            _context.Trip.Remove(trip);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }

        private bool TripExists(Guid id)
        {
            return (_context.Trip?.Any(e => e.Id == id)).GetValueOrDefault();
        }



    }
}
