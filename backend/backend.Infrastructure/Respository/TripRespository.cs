using backend.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.Infrastructure.Authentication;
using backend.Application.Services;
using Microsoft.AspNetCore.Hosting;
using backend.Domain.Enums;


namespace backend.Infrastructure.Services
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
            .Include(t => t.User)
            .Include(t => t.TripDestinations)
            .ThenInclude(td => td.SelectedPlace)
            .OrderBy(x => x.Id)
            .Select(x => MapToTripDTO(x))
            .ToListAsync();

            return trips;
        }

        public async Task<ActionResult<TripDTO>> GetTripById(Guid id)
        {
            var tripModel = await _context.Trip
            .Include(t => t.User)
            .Include(t => t.TripDestinations)
            .ThenInclude(td => td.SelectedPlace)
            .FirstOrDefaultAsync(t => t.Id == id);

            if (tripModel == null)
            {
                return new NotFoundResult();
            }

            var tripDTO = MapToTripDTO(tripModel); 

            return tripDTO;

        }

        public async Task<IActionResult> ChangeTripTitle(Guid tripId, string newTitle)
        {
            // Check if the new title is not null or empty
            if (string.IsNullOrWhiteSpace(newTitle))
            {
                return new BadRequestResult();
            }

            // Find the trip by its ID
            var trip = await _context.Trip.FirstOrDefaultAsync(t => t.Id == tripId);

            // If no trip found, return NotFound
            if (trip == null)
            {
                return new NotFoundResult();
            }

            // Update the title of the trip
            trip.Title = newTitle;

            // Mark the entity as modified
            _context.Entry(trip).State = EntityState.Modified;

            try
            {
                // Save changes to the database
                await _context.SaveChangesAsync();
                return new OkObjectResult($"Title of trip ID {tripId} updated successfully to {newTitle}");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripExists(tripId))
                {
                    return new NotFoundResult();
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<ActionResult<IEnumerable<TripDTO>>> GetUserTripsList(string userId)
        {
            var tripModels = await _context.Trip
                .Include(t => t.User)
                .Where(t => t.User.Id == userId) 
                .ToListAsync();

            if (!tripModels.Any()) 
            {
                return new NotFoundResult();
            }

            var tripDTOs = tripModels.Select(tripModel => MapToTripDTO(tripModel)).ToList(); 

            return tripDTOs;
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

            var tripModel = await _context.Trip.FindAsync(id);
            if (tripModel == null)
            {
                return new NotFoundResult();
            }

            var trip = MapToTripModel(tripDTO, tripModel); // Use mapping method
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
                CreatedBy = tripDTO.CreatedBy,

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

        public async Task<ActionResult<int>> CountUserTrips(string userId, TripStatus status)
        {
            var user = await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();

            if (user == null) return new NotFoundResult();
 
            var result = await _context.Trip.Where(t => t.User.Id == userId && t.Status == status).CountAsync();

            if (result == null) return new NotFoundResult();

            return result;
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

        private TripDTO MapToTripDTO(TripModel tripModel)
        {
            return new TripDTO
            {
                Id = tripModel.Id,
                Status = tripModel.Status,
                StartDate = tripModel.StartDate,
                EndDate = tripModel.EndDate,
                TotalPrice = tripModel.TotalPrice,
                CreatedBy = tripModel.CreatedBy,
                Title = tripModel.Title,
                User = tripModel.User != null ? new UserDTO
                {
                    
                    FirstName = tripModel.User.FirstName, 
                    Id = tripModel.User.Id
                } : null,
                TripDestinations = tripModel.TripDestinations?.Select(td => new TripDestinationDTO
                {
                    DestinationId = td.DestinationId,
                    TripId = td.TripId,
                    SelectedPlaces = td.SelectedPlace?.Select(sp => new SelectedPlaceDTO
                    {
                        Id = sp.Id,
                        TripDestinationId = sp.TripDestinationId,
                        VisitPlaceId = sp.VisitPlaceId,
                    }).ToList()
                }).ToList(),
                SelectedPlaces = tripModel.SelectedPlaces?.Select(sp => new SelectedPlaceDTO
                {
                    Id = sp.Id,
                    VisitPlaceId = sp.VisitPlaceId,
                }).ToList(),
                
            };
        }

        private TripDTO MapToListTripDTO(TripModel tripModel)
        {
            return new TripDTO
            {
                Id = tripModel.Id,
                Status = tripModel.Status,
                TotalPrice = tripModel.TotalPrice,
                User = tripModel.User != null ? new UserDTO
                {

                    FirstName = tripModel.User.FirstName,
                    Id = tripModel.User.Id
                } : null,

            };
        }

        private TripModel MapToTripModel(TripDTO tripDTO, TripModel existingTrip = null)
        {
            var trip = existingTrip ?? new TripModel();

            trip.Status = tripDTO.Status;
            trip.StartDate = tripDTO.StartDate;
            trip.EndDate = tripDTO.EndDate;
            trip.TotalPrice = tripDTO.TotalPrice;
            trip.CreatedBy = tripDTO.CreatedBy; 
            trip.TripDestinations = tripDTO.TripDestinations?.Select(td => new TripDestinationModel
            {
                DestinationId = td.DestinationId,
                TripId = td.TripId,
                SelectedPlace = td.SelectedPlaces?.Select(sp => new SelectedPlaceModel
                {
                    Id = sp.Id,
                    TripDestinationId = sp.TripDestinationId,
                    VisitPlaceId = sp.VisitPlaceId,
                }).ToList()
            }).ToList();
            trip.SelectedPlaces = tripDTO.SelectedPlaces?.Select(sp => new SelectedPlaceModel
            {
                Id = sp.Id,
                VisitPlaceId = sp.VisitPlaceId,
            }).ToList();
            

            return trip;
        }

    }
}
