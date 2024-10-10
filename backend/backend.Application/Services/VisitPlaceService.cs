using backend.Application.Services;
using backend.Domain.DTOs;
using backend.Infrastructure.Respository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Application.Services
{
    public class VisitPlaceService : IVisitPlaceService
    {
        private readonly IVisitPlaceRepository _visitPlaceRepository;
        private readonly ImageService _imageService;
        private readonly BaseUrlService _baseUrlService;
        private readonly ILogger<VisitPlaceService> _logger;
        private readonly string _baseUrl;

        public VisitPlaceService(IVisitPlaceRepository visitPlaceRepository, ImageService imageService, BaseUrlService baseUrlService, ILogger<VisitPlaceService> logger)
        {
            _visitPlaceRepository = visitPlaceRepository;
            _imageService = imageService;
            _baseUrlService = baseUrlService;
            _logger = logger;
            _baseUrl = _baseUrlService.GetBaseUrl();
        }

        public async Task<ActionResult<IEnumerable<VisitPlaceDTO>>> GetVisitPlaces()
        {
            _logger.LogInformation("Fetching all visit places.");
            var visitPlaces = await _visitPlaceRepository.GetVisitPlacesAsync();
            foreach (var place in visitPlaces)
            {
                place.PhotoUrl = $"{_baseUrl}/Images/VisitPlace/{place.PhotoUrl}";
            }
            _logger.LogInformation("Fetched {Count} visit places.", visitPlaces.Count());
            return new OkObjectResult(visitPlaces);
        }

        public async Task<ActionResult<VisitPlaceDTO>> GetVisitPlace(Guid id)
        {
            _logger.LogInformation("Fetching visit place with ID {VisitPlaceId}.", id.ToString());
            var visitPlace = await _visitPlaceRepository.GetVisitPlaceByIdAsync(id);
            if (visitPlace == null)
            {
                _logger.LogWarning("Visit place with ID {VisitPlaceId} not found.", id.ToString());
                return new NotFoundResult();
            }

            visitPlace.PhotoUrl = $"{_baseUrl}/Images/VisitPlace/{visitPlace.PhotoUrl}";
            _logger.LogInformation("Fetched visit place with ID {VisitPlaceId}.", id.ToString());
            return new OkObjectResult(visitPlace);
        }

        public async Task<ActionResult<IEnumerable<VisitPlaceDTO>>> GetVisitPlacesByDestination(Guid destinationId)
        {
            _logger.LogInformation("Fetching visit places for destination ID {DestinationId}.", destinationId.ToString());
            var visitPlaces = await _visitPlaceRepository.GetVisitPlacesByDestinationAsync(destinationId);
            foreach (var place in visitPlaces)
            {
                place.PhotoUrl = $"{_baseUrl}/Images/VisitPlace/{place.PhotoUrl}";
            }
            _logger.LogInformation("Fetched {Count} visit places for destination ID {DestinationId}.", visitPlaces.Count(), destinationId.ToString());
            return new OkObjectResult(visitPlaces);
        }

        public async Task<IActionResult> PutVisitPlace(Guid id, VisitPlaceDTO visitPlaceDTO)
        {
            _logger.LogInformation("Updating visit place with ID {VisitPlaceId}.", id.ToString());
            if (id != visitPlaceDTO.Id)
            {
                _logger.LogWarning("Visit place ID mismatch: {VisitPlaceId} vs {DTOId}.", id.ToString(), visitPlaceDTO.Id.ToString());
                return new BadRequestResult();
            }

            var result = await _visitPlaceRepository.UpdateVisitPlaceAsync(visitPlaceDTO);
            if (result)
            {
                _logger.LogInformation("Successfully updated visit place with ID {VisitPlaceId}.", id.ToString());
                return new NoContentResult();
            }

            _logger.LogWarning("Visit place with ID {VisitPlaceId} not found.", id.ToString());
            return new NotFoundResult();
        }

        public async Task<CreateVisitPlaceDTO> PostVisitPlace(CreateVisitPlaceDTO visitPlaceDTO)
        {
            _logger.LogInformation("Creating new visit place with name {Name}.", visitPlaceDTO.Name);
            if (visitPlaceDTO.ImageFile == null)
            {
                _logger.LogError("The 'ImageFileDTO' field is required.");
                throw new ArgumentException("The 'ImageFileDTO' field is required.");
            }

            visitPlaceDTO.PhotoUrl = await _imageService.SaveImage(visitPlaceDTO.ImageFile, "VisitPlace");
            var createdVisitPlace = await _visitPlaceRepository.CreateVisitPlaceAsync(visitPlaceDTO);
            _logger.LogInformation("Successfully created visit place with name {Name}.", createdVisitPlace.Name);
            return createdVisitPlace;
        }

        public async Task<IActionResult> DeleteVisitPlace(Guid id)
        {
            _logger.LogInformation("Deleting visit place with ID {VisitPlaceId}.", id.ToString());
            var result = await _visitPlaceRepository.DeleteVisitPlaceAsync(id);
            if (result)
            {
                _logger.LogInformation("Successfully deleted visit place with ID {VisitPlaceId}.", id.ToString());
                return new NoContentResult();
            }

            _logger.LogWarning("Visit place with ID {VisitPlaceId} not found.", id.ToString());
            return new NotFoundResult();
        }
    }
}
