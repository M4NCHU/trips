﻿using backend.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Infrastructure.Respository;
using backend.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace backend.Application.Services
{
    public class TripDestinationService : ITripDestinationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<TripDestinationService> _logger;

        public TripDestinationService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<TripDestinationService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ActionResult<IEnumerable<TripDestinationDTO>>> GetTripDestinations()
        {
            try
            {
                _logger.LogInformation("Fetching all trip destinations.");
                var tripDestinations = await _unitOfWork.TripDestinations.GetAllAsync();
                if (!tripDestinations.Any())
                {
                    _logger.LogWarning("No trip destinations found.");
                    return new NotFoundResult();
                }

                var tripDestinationDTOs = _mapper.Map<IEnumerable<TripDestinationDTO>>(tripDestinations);
                _logger.LogInformation("Fetched {Count} trip destinations.", tripDestinationDTOs.Count());
                return new OkObjectResult(tripDestinationDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching trip destinations.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ActionResult<TripDestinationDTO>> GetTripDestination(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching trip destination with ID {TripDestinationId}", id);
                var tripDestination = await _unitOfWork.TripDestinations.GetByIdAsync(id);
                if (tripDestination == null)
                {
                    _logger.LogWarning("Trip destination with ID {TripDestinationId} not found.", id);
                    return new NotFoundResult();
                }

                var tripDestinationDTO = _mapper.Map<TripDestinationDTO>(tripDestination);
                _logger.LogInformation("Fetched trip destination with ID {TripDestinationId}.", id);
                return new OkObjectResult(tripDestinationDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the trip destination with ID {TripDestinationId}.", id);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ActionResult<IEnumerable<TripDestinationDTO>>> GetTripDestinations(Guid tripId)
        {
            try
            {
                _logger.LogInformation("Fetching trip destinations for trip ID {TripId}", tripId);
                var tripDestinations = await _unitOfWork.TripDestinations.GetTripDestinationsByTripIdAsync(tripId);
                if (!tripDestinations.Any())
                {
                    _logger.LogWarning("No trip destinations found for trip ID {TripId}", tripId);
                    return new NotFoundResult();
                }

                var tripDestinationDTOs = _mapper.Map<IEnumerable<TripDestinationDTO>>(tripDestinations);
                _logger.LogInformation("Fetched {Count} trip destinations for trip ID {TripId}.", tripDestinationDTOs.Count(), tripId);
                return new OkObjectResult(tripDestinationDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching trip destinations for trip ID {TripId}.", tripId);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<IActionResult> PutTripDestination(Guid id, TripDestinationDTO tripDestinationDTO)
        {
            try
            {
                if (id != tripDestinationDTO.Id)
                {
                    _logger.LogWarning("Mismatched ID {TripDestinationId} while updating trip destination.", id);
                    return new BadRequestResult();
                }

                var tripDestination = await _unitOfWork.TripDestinations.GetByIdAsync(id);
                if (tripDestination == null)
                {
                    _logger.LogWarning("Trip destination with ID {TripDestinationId} not found.", id);
                    return new NotFoundResult();
                }

                _mapper.Map(tripDestinationDTO, tripDestination);
                _logger.LogInformation("Updating trip destination with ID {TripDestinationId}", id);
                await _unitOfWork.TripDestinations.UpdateAsync(tripDestination);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Successfully updated trip destination with ID {TripDestinationId}.", id);
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the trip destination with ID {TripDestinationId}.", id);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<TripDestinationDTO> PostTripDestinationAsync(TripDestinationDTO tripDestinationDTO)
        {
            try
            {
                var tripDestination = _mapper.Map<TripDestinationModel>(tripDestinationDTO);
                _logger.LogInformation("Adding new trip destination for trip ID {TripId}", tripDestination.TripId);
                await _unitOfWork.TripDestinations.AddAsync(tripDestination);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Successfully added new trip destination with ID {TripDestinationId}", tripDestination.Id);
                return tripDestinationDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new trip destination for trip ID {TripId}", tripDestinationDTO.TripId);
                throw;
            }
        }

        public async Task<IActionResult> DeleteTripDestination(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting trip destination with ID {TripDestinationId}", id);
                var tripDestination = await _unitOfWork.TripDestinations.GetByIdAsync(id);
                if (tripDestination == null)
                {
                    _logger.LogWarning("Trip destination with ID {TripDestinationId} not found.", id);
                    return new NotFoundResult();
                }

                await _unitOfWork.TripDestinations.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Successfully deleted trip destination with ID {TripDestinationId}", id);
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the trip destination with ID {TripDestinationId}.", id);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<int> CountTripDestinations(Guid tripId)
        {
            try
            {
                _logger.LogInformation("Counting trip destinations for trip ID {TripId}", tripId);
                var count = await _unitOfWork.TripDestinations.CountTripDestinationsByTripIdAsync(tripId);
                _logger.LogInformation("Found {Count} trip destinations for trip ID {TripId}", count, tripId);
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while counting trip destinations for trip ID {TripId}.", tripId);
                return 0;
            }
        }
    }
}
