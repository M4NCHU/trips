using backend.Application.Services;
using backend.Domain.DTOs;
using backend.Domain.Enums;
using backend.Domain.Mappings;
using backend.Infrastructure.Respository;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Application.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _tripRepository;
        private readonly IDestinationService _destinationService;
        private readonly BaseUrlService _baseUrlService;
        private readonly ILogger<TripService> _logger;
        private readonly string _baseUrl;

        public TripService(
            ITripRepository tripRepository,
            IDestinationService destinationService,
            BaseUrlService baseUrlService,
            ILogger<TripService> logger)
        {
            _tripRepository = tripRepository;
            _destinationService = destinationService;
            _baseUrlService = baseUrlService;
            _logger = logger;
            _baseUrl = _baseUrlService.GetBaseUrl();
        }

        public async Task<ActionResult<IEnumerable<TripDTO>>> GetTrips()
        {
            _logger.LogInformation("Fetching all trips.");
            var trips = await _tripRepository.GetAllTripsAsync();
            var tripDTOs = trips.Select(trip => DestinationMapping.MapToTripDTO(trip, _baseUrl)).ToList();
            _logger.LogInformation("Fetched {Count} trips.", tripDTOs.Count);
            return new OkObjectResult(tripDTOs);
        }

        public async Task<ActionResult<TripDTO>> GetTripById(Guid id)
        {
            _logger.LogInformation("Fetching trip with ID {TripId}.", id);
            var trip = await _tripRepository.GetTripByIdAsync(id);
            if (trip == null)
            {
                _logger.LogWarning("Trip with ID {TripId} not found.", id);
                return new NotFoundResult();
            }

            var tripDTO = DestinationMapping.MapToTripDTO(trip, _baseUrl);
            _logger.LogInformation("Fetched trip with ID {TripId}.", id);
            return new OkObjectResult(tripDTO);
        }

        public async Task<ActionResult<IEnumerable<TripDTO>>> GetUserTripsList(string userId)
        {
            _logger.LogInformation("Fetching trips for user with ID {UserId}.", userId);
            var trips = await _tripRepository.GetTripsByUserIdAsync(userId);
            if (!trips.Any())
            {
                _logger.LogWarning("No trips found for user with ID {UserId}.", userId);
                return new NotFoundResult();
            }

            var tripDTOs = trips.Select(trip => DestinationMapping.MapToTripDTO(trip, _baseUrl)).ToList();
            _logger.LogInformation("Fetched {Count} trips for user with ID {UserId}.", tripDTOs.Count, userId);
            return new OkObjectResult(tripDTOs);
        }

        public async Task<List<VisitPlaceDTO>> GetVisitPlacesForTrip(Guid tripId)
        {
            _logger.LogInformation("Fetching visit places for trip ID {TripId}.", tripId);
            var trip = await _tripRepository.GetTripByIdAsync(tripId);
            var visitPlaces = trip?.TripDestinations
                .SelectMany(td => td.SelectedPlace)
                .Select(sp => new VisitPlaceDTO
                {
                    Id = sp.VisitPlace.Id,
                    Name = sp.VisitPlace.Name,
                    Description = sp.VisitPlace.Description,
                    PhotoUrl = $"{_baseUrl}/Images/VisitPlace/{sp.VisitPlace.PhotoUrl}",
                    Price = sp.VisitPlace.Price
                }).ToList();

            _logger.LogInformation("Fetched {Count} visit places for trip ID {TripId}.", visitPlaces?.Count ?? 0, tripId);
            return visitPlaces ?? new List<VisitPlaceDTO>();
        }

        public async Task<IActionResult> ChangeTripTitle(Guid tripId, string newTitle)
        {
            _logger.LogInformation("Changing title for trip ID {TripId} to {NewTitle}.", tripId, newTitle);
            var trip = await _tripRepository.GetTripByIdAsync(tripId);
            if (trip == null)
            {
                _logger.LogWarning("Trip with ID {TripId} not found.", tripId);
                return new NotFoundResult();
            }

            trip.Title = newTitle;
            await _tripRepository.UpdateTripAsync(trip);
            _logger.LogInformation("Successfully changed title for trip ID {TripId} to {NewTitle}.", tripId, newTitle);
            return new OkObjectResult($"Title updated to {newTitle}");
        }

        public async Task<CreateTripDTO> PostTrip(CreateTripDTO tripDTO)
        {
            _logger.LogInformation("Creating new trip with title {Title} by user {CreatedBy}.", tripDTO.Title, tripDTO.CreatedBy);
            var trip = new TripModel
            {
                Id = Guid.NewGuid(),
                Title = tripDTO.Title,
                Status = TripStatus.Planning,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
                CreatedBy = tripDTO.CreatedBy
            };

            await _tripRepository.AddTripAsync(trip);
            _logger.LogInformation("Successfully created trip with ID {TripId}.", trip.Id);
            return tripDTO;
        }

        public async Task<IActionResult> PutTrip(Guid id, TripDTO tripDTO)
        {
            _logger.LogInformation("Updating trip with ID {TripId}.", id);
            var trip = await _tripRepository.GetTripByIdAsync(id);
            if (trip == null)
            {
                _logger.LogWarning("Trip with ID {TripId} not found.", id);
                return new NotFoundResult();
            }

            trip.Status = tripDTO.Status;
            trip.StartDate = tripDTO.StartDate;
            trip.EndDate = tripDTO.EndDate;
            trip.TotalPrice = tripDTO.TotalPrice;
            trip.ModifiedAt = DateTime.UtcNow;

            await _tripRepository.UpdateTripAsync(trip);
            _logger.LogInformation("Successfully updated trip with ID {TripId}.", id);
            return new NoContentResult();
        }

        public async Task<IActionResult> DeleteTrip(Guid id)
        {
            _logger.LogInformation("Deleting trip with ID {TripId}.", id);
            var trip = await _tripRepository.GetTripByIdAsync(id);
            if (trip == null)
            {
                _logger.LogWarning("Trip with ID {TripId} not found.", id);
                return new NotFoundResult();
            }

            await _tripRepository.DeleteTripAsync(id);
            _logger.LogInformation("Successfully deleted trip with ID {TripId}.", id);
            return new NoContentResult();
        }

        public async Task<ActionResult<int>> CountUserTrips(string userId, TripStatus status)
        {
            _logger.LogInformation("Counting trips for user {UserId} with status {Status}.", userId, status);
            var trips = await _tripRepository.GetTripsByUserIdAsync(userId);
            var tripCount = trips.Count(t => t.Status == status);
            _logger.LogInformation("User {UserId} has {Count} trips with status {Status}.", userId, tripCount, status);
            return new OkObjectResult(tripCount);
        }

        public async Task<(Guid, bool)> EnsureActiveTripExists(string userId)
        {
            _logger.LogInformation("Ensuring active trip exists for user {UserId}.", userId);
            var activeTrip = (await _tripRepository.GetTripsByUserIdAsync(userId))
                .FirstOrDefault(t => t.Status == TripStatus.Planning);

            if (activeTrip != null)
            {
                _logger.LogInformation("Active trip found for user {UserId}, trip ID {TripId}.", userId, activeTrip.Id);
                return (activeTrip.Id, false);
            }

            var newTrip = new TripModel
            {
                Id = Guid.NewGuid(),
                Title = "New Trip",
                Status = TripStatus.Planning,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(7),
                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };

            await _tripRepository.AddTripAsync(newTrip);
            _logger.LogInformation("Created new active trip for user {UserId}, trip ID {TripId}.", userId, newTrip.Id);
            return (newTrip.Id, true);
        }
    }
}
