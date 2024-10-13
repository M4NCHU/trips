using AutoMapper;
using backend.Application.Services;
using backend.Domain.DTOs;
using backend.Infrastructure.Respository;
using backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Application.Services
{
    public class SelectedPlaceService : ISelectedPlaceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseUrlService _baseUrlService;
        private readonly ILogger<SelectedPlaceService> _logger;
        private readonly IMapper _mapper;
        private readonly string _baseUrl;

        public SelectedPlaceService(
            IUnitOfWork unitOfWork,
            IBaseUrlService baseUrlService,
            IMapper mapper,
            ILogger<SelectedPlaceService> logger)
        {
            _unitOfWork = unitOfWork;
            _baseUrlService = baseUrlService;
            _mapper = mapper;
            _logger = logger;
            _baseUrl = _baseUrlService.GetBaseUrl();
        }

        public async Task<ActionResult<IEnumerable<SelectedPlaceDTO>>> GetSelectedPlaces()
        {
            try
            {
                _logger.LogInformation("Fetching all selected places.");
                var selectedPlaces = await _unitOfWork.SelectedPlaces.GetAllAsync();

                if (!selectedPlaces.Any())
                {
                    _logger.LogWarning("No selected places found.");
                    return new NotFoundResult();
                }

                var selectedPlaceDTOs = selectedPlaces.Select(sp => new SelectedPlaceDTO
                {
                    Id = sp.Id,
                    TripDestinationId = sp.TripDestinationId,
                    VisitPlaceId = sp.VisitPlaceId
                }).ToList();

                _logger.LogInformation("Fetched {Count} selected places.", selectedPlaceDTOs.Count);
                return new OkObjectResult(selectedPlaceDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching selected places.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ActionResult<SelectedPlaceDTO>> GetSelectedPlace(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching selected place with ID {SelectedPlaceId}", id);
                var selectedPlace = await _unitOfWork.SelectedPlaces.GetByIdAsync(id);
                if (selectedPlace == null)
                {
                    _logger.LogWarning("Selected place with ID {SelectedPlaceId} not found.", id);
                    return new NotFoundResult();
                }

                var selectedPlaceDTO = new SelectedPlaceDTO
                {
                    Id = selectedPlace.Id,
                    TripDestinationId = selectedPlace.TripDestinationId,
                    VisitPlaceId = selectedPlace.VisitPlaceId
                };

                _logger.LogInformation("Fetched selected place with ID {SelectedPlaceId}", id);
                return new OkObjectResult(selectedPlaceDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the selected place with ID {SelectedPlaceId}.", id);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ActionResult<IEnumerable<SelectedPlaceDTO>>> GetSelectedPlaces(Guid destinationId)
        {
            try
            {
                _logger.LogInformation("Fetching selected places for destination with ID {DestinationId}", destinationId);
                var selectedPlaces = await _unitOfWork.SelectedPlaces.GetSelectedPlacesByDestinationIdAsync(destinationId);

                if (!selectedPlaces.Any())
                {
                    _logger.LogWarning("No selected places found for destination ID {DestinationId}.", destinationId);
                    return new NotFoundResult();
                }

                var selectedPlaceDTOs = selectedPlaces.Select(sp => new SelectedPlaceDTO
                {
                    Id = sp.Id,
                    TripDestinationId = sp.TripDestinationId,
                    VisitPlaceId = sp.VisitPlaceId
                }).ToList();

                _logger.LogInformation("Fetched {Count} selected places for destination ID {DestinationId}.", selectedPlaceDTOs.Count, destinationId);
                return new OkObjectResult(selectedPlaceDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching selected places for destination ID {DestinationId}.", destinationId);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<IActionResult> PutSelectedPlace(Guid id, SelectedPlaceDTO selectedPlaceDTO)
        {
            try
            {
                if (id != selectedPlaceDTO.Id)
                {
                    _logger.LogWarning("Mismatched ID {SelectedPlaceId} while updating selected place.", id);
                    return new BadRequestResult();
                }

                var selectedPlace = new SelectedPlaceModel
                {
                    Id = id,
                    TripDestinationId = selectedPlaceDTO.TripDestinationId,
                    VisitPlaceId = selectedPlaceDTO.VisitPlaceId
                };

                _logger.LogInformation("Updating selected place with ID {SelectedPlaceId}", id);
                await _unitOfWork.SelectedPlaces.UpdateAsync(selectedPlace);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Successfully updated selected place with ID {SelectedPlaceId}", id);
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the selected place with ID {SelectedPlaceId}.", id);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<CreateSelectedPlaceDTO> PostSelectedPlace(CreateSelectedPlaceDTO selectedPlaceDTO)
        {
            try
            {
                _logger.LogInformation("Adding new selected place for destination {TripDestinationId}", selectedPlaceDTO.TripDestinationId);

                var selectedPlace = new SelectedPlaceModel
                {
                    Id = Guid.NewGuid(),
                    TripDestinationId = selectedPlaceDTO.TripDestinationId,
                    VisitPlaceId = selectedPlaceDTO.VisitPlaceId,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow
                };

                await _unitOfWork.SelectedPlaces.AddAsync(selectedPlace);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Successfully added new selected place with ID {SelectedPlaceId}", selectedPlace.Id);
                return selectedPlaceDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new selected place for destination {TripDestinationId}.", selectedPlaceDTO.TripDestinationId);
                throw;
            }
        }

        public async Task<IActionResult> DeleteSelectedPlace(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting selected place with ID {SelectedPlaceId}", id);
                var selectedPlace = await _unitOfWork.SelectedPlaces.GetByIdAsync(id);
                if (selectedPlace == null)
                {
                    _logger.LogWarning("Selected place with ID {SelectedPlaceId} not found.", id);
                    return new NotFoundResult();
                }

                await _unitOfWork.SelectedPlaces.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Successfully deleted selected place with ID {SelectedPlaceId}", id);
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the selected place with ID {SelectedPlaceId}.", id);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
