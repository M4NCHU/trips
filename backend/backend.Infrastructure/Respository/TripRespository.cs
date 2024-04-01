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
using backend.Domain.Mappings;
using Microsoft.Extensions.Logging;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using static backend.Infrastructure.Respository.ApplicationException;


namespace backend.Infrastructure.Services
{
    public class TripService : ITripService
    {
        private readonly TripsDbContext _context;
        private readonly ImageService _imageService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IDestinationService _destinationService;
        private readonly BaseUrlService _baseUrlService;
        private readonly string _baseUrl;
        private readonly ILogger<TripService> _logger;


        public TripService(TripsDbContext context, ImageService imageService, IWebHostEnvironment hostEnvironment, IDestinationService destinationService, BaseUrlService baseUrlService, ILogger<TripService> logger)
        {
            _context = context;
            _imageService = imageService;
            _hostEnvironment = hostEnvironment;
            _destinationService = destinationService;
            _baseUrlService = baseUrlService;
            _baseUrl = _baseUrlService.GetBaseUrl();
            _logger = logger;
        }   

        public async Task<ActionResult<IEnumerable<TripDTO>>> GetTrips()
        {
            var trips = await _context.Trip
            .Include(t => t.User)
            .Include(t => t.TripDestinations)
            .ThenInclude(td => td.SelectedPlace)
            .ThenInclude(td=> td.VisitPlace)
            .OrderBy(x => x.Id)
            .Select(x =>DestinationMapping.MapToTripDTO(x, _baseUrl))
            .ToListAsync();

            return trips;
        }

        public async Task<ActionResult<TripDTO>> GetTripById(Guid id)
        {
            var tripModel = await _context.Trip
            .Include(t => t.User)
            .Include(t => t.TripDestinations)
                .ThenInclude(td => td.Destination) 
            .ThenInclude(d => d.VisitPlaces) 
            .Include(t => t.TripDestinations)
                .ThenInclude(td => td.SelectedPlace)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(t => t.Id == id);

            if (tripModel == null)
            {
                return new NotFoundResult();
            }

            var tripDTO = DestinationMapping.MapToTripDTO(tripModel, _baseUrl);

            return new OkObjectResult(tripDTO);

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
            var tripId = await EnsureActiveTripExists(userId);


            var tripModels = await _context.Trip
                .Include(t => t.User)
                .Include(t => t.TripDestinations)
                .ThenInclude(td => td.SelectedPlace)
                .ThenInclude(sp => sp.VisitPlace)
                .Where(t => t.User.Id == userId) 
                .ToListAsync();

            if (!tripModels.Any()) 
            {
                return new NotFoundResult();
            }
            var tripDTOs = tripModels.Select(tdto => DestinationMapping.MapToTripDTO(tdto, _baseUrl)).ToList();
            
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

        public async Task<CreateTripDTO> PostTrip([FromForm]CreateTripDTO tripDTO)
        {
            try
            {
                if (_context.Trip == null)
                {
                    _logger.LogError("Entity set 'TripsDbContext.Trip' is null.");
                    throw new InternalErrorException("Entity set 'TripsDbContext.Category' is null.");
                }

                var userExists = await _context.Users.AnyAsync(u => u.Id == tripDTO.CreatedBy);
                if (!userExists)
                {
                    _logger.LogError($"User with ID {tripDTO.CreatedBy} does not exist.");
                    throw new ArgumentException($"User with ID {tripDTO.CreatedBy} does not exist.");
                }

                var currentDate = DateTime.Now.ToUniversalTime();

                var trip = new TripModel
                {
                    Id = Guid.NewGuid(),
                    Title = tripDTO.Title,
                    Status = TripStatus.Planning,
                    StartDate = currentDate,
                    EndDate = currentDate,
                    CreatedAt = currentDate,
                    ModifiedAt = currentDate,
                    CreatedBy = tripDTO.CreatedBy,
                    TotalPrice = 0
                };

                _context.Trip.Add(trip);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Trip titled '{trip.Title}' created by {trip.CreatedBy} on {currentDate}.");

                return tripDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new trip.");
                throw; 
            }
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

        public async Task<(Guid, bool)> EnsureActiveTripExists(string userId)
        {
            // Sprawdź, czy istnieje aktywny trip (o statusie Planning)
            var activeTrip = await _context.Trip
                .FirstOrDefaultAsync(t => t.User.Id == userId && t.Status == TripStatus.Planning);

            // Jeśli aktywny trip istnieje, zwróć jego ID i false (nie był nowo utworzony)
            if (activeTrip != null)
            {
                return (activeTrip.Id, false);
            }

            // Jeśli nie, utwórz nowy trip z domyślnymi wartościami
            var newTrip = new TripModel
            {
                Status = TripStatus.Planning,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                CreatedBy = userId,
                TotalPrice = 0
            };

            _context.Trip.Add(newTrip);
            await _context.SaveChangesAsync();

            // Zwróć ID nowo utworzonego trip i true (trip został utworzony)
            return (newTrip.Id, true);
        }


        private bool TripExists(Guid id)
        {
            return (_context.Trip?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        

        public TripModel MapToTripModel(TripDTO tripDTO, TripModel existingTrip = null)
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
