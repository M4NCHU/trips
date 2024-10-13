using backend.Application.Services;
using backend.Domain.DTOs;
using backend.Domain.Enums;
using backend.Infrastructure.Respository;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace backend.Application.Services
{
    public class TripService : ITripService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDestinationService _destinationService;
        private readonly IBaseUrlService _baseUrlService;
        private readonly ILogger<TripService> _logger;
        private readonly IMapper _mapper;
        private readonly string _baseUrl;

        public TripService(
            IUnitOfWork unitOfWork,
            IDestinationService destinationService,
            IBaseUrlService baseUrlService,
            ILogger<TripService> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _destinationService = destinationService;
            _baseUrlService = baseUrlService;
            _logger = logger;
            _mapper = mapper;
            _baseUrl = _baseUrlService.GetBaseUrl();
        }

        public async Task<ActionResult<IEnumerable<TripDTO>>> GetTrips()
        {
            try
            {
                _logger.LogInformation("Fetching all trips.");
                var trips = await _unitOfWork.Trips.GetAllAsync();
                var tripDTOs = _mapper.Map<List<TripDTO>>(trips);

                _logger.LogInformation("Fetched {Count} trips.", tripDTOs.Count);
                return new OkObjectResult(tripDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all trips.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ActionResult<TripDTO>> GetTripById(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching trip with ID {TripId}.", id);
                var trip = await _unitOfWork.Trips.GetByIdAsync(id);
                if (trip == null)
                {
                    _logger.LogWarning("Trip with ID {TripId} not found.", id);
                    return new NotFoundResult();
                }

                var tripDTO = _mapper.Map<TripDTO>(trip);
                _logger.LogInformation("Fetched trip with ID {TripId}.", id);
                return new OkObjectResult(tripDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the trip with ID {TripId}.", id);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ActionResult<IEnumerable<TripDTO>>> GetUserTripsList(string userId)
        {
            try
            {
                _logger.LogInformation("Fetching trips for user with ID {UserId}.", userId);
                var trips = await _unitOfWork.Trips.GetTripsByUserIdAsync(userId);
                if (!trips.Any())
                {
                    _logger.LogWarning("No trips found for user with ID {UserId}.", userId);
                    return new NotFoundResult();
                }

                var tripDTOs = _mapper.Map<List<TripDTO>>(trips);
                _logger.LogInformation("Fetched {Count} trips for user with ID {UserId}.", tripDTOs.Count, userId);
                return new OkObjectResult(tripDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching trips for user with ID {UserId}.", userId);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<IActionResult> ChangeTripTitle(Guid tripId, string newTitle)
        {
            try
            {
                _logger.LogInformation("Changing title for trip ID {TripId} to {NewTitle}.", tripId, newTitle);
                var trip = await _unitOfWork.Trips.GetByIdAsync(tripId);
                if (trip == null)
                {
                    _logger.LogWarning("Trip with ID {TripId} not found.", tripId);
                    return new NotFoundResult();
                }

                trip.Title = newTitle;
                await _unitOfWork.Trips.UpdateAsync(trip);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Successfully changed title for trip ID {TripId} to {NewTitle}.", tripId, newTitle);
                return new OkObjectResult($"Title updated to {newTitle}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while changing the title for trip ID {TripId}.", tripId);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<CreateTripDTO> PostTrip(CreateTripDTO tripDTO)
        {
            try
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

                await _unitOfWork.Trips.AddAsync(trip);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Successfully created trip with ID {TripId}.", trip.Id);
                return tripDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new trip.");
                throw;
            }
        }

        public async Task<IActionResult> PutTrip(Guid id, TripDTO tripDTO)
        {
            try
            {
                _logger.LogInformation("Updating trip with ID {TripId}.", id);
                var trip = await _unitOfWork.Trips.GetByIdAsync(id);
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

                await _unitOfWork.Trips.UpdateAsync(trip);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Successfully updated trip with ID {TripId}.", id);
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the trip with ID {TripId}.", id);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<IActionResult> DeleteTrip(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting trip with ID {TripId}.", id);
                var trip = await _unitOfWork.Trips.GetByIdAsync(id);
                if (trip == null)
                {
                    _logger.LogWarning("Trip with ID {TripId} not found.", id);
                    return new NotFoundResult();
                }

                await _unitOfWork.Trips.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Successfully deleted trip with ID {TripId}.", id);
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the trip with ID {TripId}.", id);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ActionResult<int>> CountUserTrips(string userId, TripStatus status)
        {
            try
            {
                _logger.LogInformation("Counting trips for user {UserId} with status {Status}.", userId, status);
                var trips = await _unitOfWork.Trips.GetTripsByUserIdAsync(userId);
                var tripCount = trips.Count(t => t.Status == status);
                _logger.LogInformation("User {UserId} has {Count} trips with status {Status}.", userId, tripCount, status);
                return new OkObjectResult(tripCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while counting trips for user {UserId} with status {Status}.", userId, status);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<(Guid, bool)> EnsureActiveTripExists(string userId)
        {
            try
            {
                _logger.LogInformation("Ensuring active trip exists for user {UserId}.", userId);
                var activeTrip = (await _unitOfWork.Trips.GetTripsByUserIdAsync(userId))
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

                await _unitOfWork.Trips.AddAsync(newTrip);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Created new active trip for user {UserId}, trip ID {TripId}.", userId, newTrip.Id);
                return (newTrip.Id, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while ensuring an active trip exists for user {UserId}.", userId);
                throw;
            }
        }

        public async Task<List<VisitPlaceDTO>> GetVisitPlacesForTrip(Guid tripId)
        {
            try
            {
                _logger.LogInformation("Fetching visit places for trip ID {TripId}.", tripId);
                var trip = await _unitOfWork.Trips.GetTripByIdAsync(tripId);
                if (trip == null)
                {
                    _logger.LogWarning("Trip with ID {TripId} not found.", tripId);
                    return new List<VisitPlaceDTO>();
                }

                var visitPlaces = trip.TripDestinations
                    .SelectMany(td => td.SelectedPlace)
                    .Select(sp => new VisitPlaceDTO
                    {
                        Id = sp.VisitPlace.Id,
                        Name = sp.VisitPlace.Name,
                        Description = sp.VisitPlace.Description,
                        PhotoUrl = $"{_baseUrl}/Images/VisitPlace/{sp.VisitPlace.PhotoUrl}",
                        Price = sp.VisitPlace.Price
                    }).ToList();

                _logger.LogInformation("Fetched {Count} visit places for trip ID {TripId}.", visitPlaces.Count, tripId);
                return visitPlaces;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching visit places for trip ID {TripId}.", tripId);
                throw;
            }
        }
    }
}

