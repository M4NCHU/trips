using backend.Application.Services;
using backend.Domain.DTOs;
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
    public class SelectedPlaceService : ISelectedPlaceService
    {
        private readonly ISelectedPlaceRepository _selectedPlaceRepository;
        private readonly BaseUrlService _baseUrlService;
        private readonly ILogger<SelectedPlaceService> _logger;
        private readonly string _baseUrl;

        public SelectedPlaceService(
            ISelectedPlaceRepository selectedPlaceRepository,
            BaseUrlService baseUrlService,
            ILogger<SelectedPlaceService> logger)
        {
            _selectedPlaceRepository = selectedPlaceRepository;
            _baseUrlService = baseUrlService;
            _logger = logger;
            _baseUrl = _baseUrlService.GetBaseUrl();
        }

        public async Task<ActionResult<IEnumerable<SelectedPlaceDTO>>> GetSelectedPlaces()
        {
            _logger.LogInformation("Fetching all selected places.");
            var selectedPlaces = await _selectedPlaceRepository.GetSelectedPlacesAsync();
            var selectedPlaceDTOs = selectedPlaces.Select(sp => new SelectedPlaceDTO
            {
                Id = sp.Id,
                TripDestinationId = sp.TripDestinationId,
                VisitPlaceId = sp.VisitPlaceId
            }).ToList();

            _logger.LogInformation("Fetched {Count} selected places.", selectedPlaceDTOs.Count);
            return selectedPlaceDTOs;
        }

        public async Task<ActionResult<SelectedPlaceDTO>> GetSelectedPlace(Guid id)
        {
            _logger.LogInformation("Fetching selected place with ID {SelectedPlaceId}", id);
            var selectedPlace = await _selectedPlaceRepository.GetSelectedPlaceByIdAsync(id);
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
            return selectedPlaceDTO;
        }

        public async Task<ActionResult<IEnumerable<SelectedPlaceDTO>>> GetSelectedPlaces(Guid destinationId)
        {
            _logger.LogInformation("Fetching selected places for destination with ID {DestinationId}", destinationId);
            var selectedPlaces = await _selectedPlaceRepository.GetSelectedPlacesByDestinationIdAsync(destinationId);
            var selectedPlaceDTOs = selectedPlaces.Select(sp => new SelectedPlaceDTO
            {
                Id = sp.Id,
                TripDestinationId = sp.TripDestinationId,
                VisitPlaceId = sp.VisitPlaceId
            }).ToList();

            _logger.LogInformation("Fetched {Count} selected places for destination ID {DestinationId}.", selectedPlaceDTOs.Count, destinationId);
            return selectedPlaceDTOs;
        }

        public async Task<IActionResult> PutSelectedPlace(Guid id, SelectedPlaceDTO selectedPlaceDTO)
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
            await _selectedPlaceRepository.UpdateSelectedPlaceAsync(selectedPlace);

            _logger.LogInformation("Successfully updated selected place with ID {SelectedPlaceId}", id);
            return new NoContentResult();
        }

        public async Task<CreateSelectedPlaceDTO> PostSelectedPlace(CreateSelectedPlaceDTO selectedPlaceDTO)
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

            await _selectedPlaceRepository.AddSelectedPlaceAsync(selectedPlace);

            _logger.LogInformation("Successfully added new selected place with ID {SelectedPlaceId}", selectedPlace.Id);
            return selectedPlaceDTO;
        }

        public async Task<IActionResult> DeleteSelectedPlace(Guid id)
        {
            _logger.LogInformation("Deleting selected place with ID {SelectedPlaceId}", id);
            var selectedPlace = await _selectedPlaceRepository.GetSelectedPlaceByIdAsync(id);
            if (selectedPlace == null)
            {
                _logger.LogWarning("Selected place with ID {SelectedPlaceId} not found.", id);
                return new NotFoundResult();
            }

            await _selectedPlaceRepository.DeleteSelectedPlaceAsync(id);
            _logger.LogInformation("Successfully deleted selected place with ID {SelectedPlaceId}", id);
            return new NoContentResult();
        }
    }
}
