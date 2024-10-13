using backend.Application.Services;
using backend.Domain.DTOs;
using backend.Infrastructure.Respository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using backend.Models;

namespace backend.Application.Services
{
    public class VisitPlaceService : IVisitPlaceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        private readonly IBaseUrlService _baseUrlService;
        private readonly ILogger<VisitPlaceService> _logger;
        private readonly IMapper _mapper;
        private readonly string _baseUrl;

        public VisitPlaceService(
            IUnitOfWork unitOfWork,
            IImageService imageService,
            IBaseUrlService baseUrlService,
            ILogger<VisitPlaceService> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
            _baseUrlService = baseUrlService;
            _logger = logger;
            _mapper = mapper;
            _baseUrl = _baseUrlService.GetBaseUrl();
        }

        public async Task<ActionResult<IEnumerable<VisitPlaceDTO>>> GetVisitPlaces()
        {
            try
            {
                _logger.LogInformation("Fetching all visit places.");
                var visitPlaces = await _unitOfWork.VisitPlaces.GetAllAsync();
                var visitPlaceDTOs = _mapper.Map<IEnumerable<VisitPlaceDTO>>(visitPlaces);

                foreach (var place in visitPlaceDTOs)
                {
                    place.PhotoUrl = $"{_baseUrl}/Images/VisitPlace/{place.PhotoUrl}";
                }

                _logger.LogInformation("Fetched {Count} visit places.", visitPlaceDTOs.Count());
                return new OkObjectResult(visitPlaceDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching visit places.");
                return new StatusCodeResult(500);
            }
        }

        public async Task<ActionResult<VisitPlaceDTO>> GetVisitPlace(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching visit place with ID {VisitPlaceId}.", id);
                var visitPlace = await _unitOfWork.VisitPlaces.GetByIdAsync(id);
                if (visitPlace == null)
                {
                    _logger.LogWarning("Visit place with ID {VisitPlaceId} not found.", id);
                    return new NotFoundResult();
                }

                var visitPlaceDTO = _mapper.Map<VisitPlaceDTO>(visitPlace);
                visitPlaceDTO.PhotoUrl = $"{_baseUrl}/Images/VisitPlace/{visitPlaceDTO.PhotoUrl}";

                _logger.LogInformation("Fetched visit place with ID {VisitPlaceId}.", id);
                return new OkObjectResult(visitPlaceDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching visit place with ID {VisitPlaceId}.", id);
                return new StatusCodeResult(500);
            }
        }

        public async Task<ActionResult<IEnumerable<VisitPlaceDTO>>> GetVisitPlacesByDestination(Guid destinationId)
        {
            try
            {
                _logger.LogInformation("Fetching visit places for destination ID {DestinationId}.", destinationId);
                var visitPlaces = await _unitOfWork.VisitPlaces.GetVisitPlacesByDestinationAsync(destinationId);
                var visitPlaceDTOs = _mapper.Map<IEnumerable<VisitPlaceDTO>>(visitPlaces);

                foreach (var place in visitPlaceDTOs)
                {
                    place.PhotoUrl = $"{_baseUrl}/Images/VisitPlace/{place.PhotoUrl}";
                }

                _logger.LogInformation("Fetched {Count} visit places for destination ID {DestinationId}.", visitPlaceDTOs.Count(), destinationId);
                return new OkObjectResult(visitPlaceDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching visit places for destination ID {DestinationId}.", destinationId);
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> PutVisitPlace(Guid id, VisitPlaceDTO visitPlaceDTO)
        {
            try
            {
                _logger.LogInformation("Updating visit place with ID {VisitPlaceId}.", id);
                if (id != visitPlaceDTO.Id)
                {
                    _logger.LogWarning("Visit place ID mismatch: {VisitPlaceId} vs {DTOId}.", id, visitPlaceDTO.Id);
                    return new BadRequestResult();
                }

                var visitPlaceModel = _mapper.Map<VisitPlaceModel>(visitPlaceDTO);
                await _unitOfWork.VisitPlaces.UpdateAsync(visitPlaceModel);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Successfully updated visit place with ID {VisitPlaceId}.", id);
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating visit place with ID {VisitPlaceId}.", id);
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> PostVisitPlace(CreateVisitPlaceDTO visitPlaceDTO)
        {
            try
            {
                _logger.LogInformation("Creating new visit place with name {Name}.", visitPlaceDTO.Name);

                if (visitPlaceDTO.ImageFile == null)
                {
                    _logger.LogError("The 'ImageFileDTO' field is required.");
                    return new BadRequestObjectResult("The 'ImageFileDTO' field is required.");
                }

                visitPlaceDTO.PhotoUrl = await _imageService.SaveImage(visitPlaceDTO.ImageFile, "VisitPlace");
                var visitPlaceModel = _mapper.Map<VisitPlaceModel>(visitPlaceDTO);
                await _unitOfWork.VisitPlaces.AddAsync(visitPlaceModel);
                await _unitOfWork.SaveChangesAsync();

                var visitPlaceDTOResult = _mapper.Map<VisitPlaceDTO>(visitPlaceModel);

                _logger.LogInformation("Successfully created visit place with name {Name}.", visitPlaceDTO.Name);
                return new CreatedAtActionResult(nameof(GetVisitPlace), "VisitPlaces", new { id = visitPlaceDTOResult.Id }, visitPlaceDTOResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new visit place with name {Name}.", visitPlaceDTO.Name);
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> DeleteVisitPlace(Guid id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete visit place with ID {VisitPlaceId}.", id);
                var visitPlace = await _unitOfWork.VisitPlaces.GetByIdAsync(id);
                if (visitPlace == null)
                {
                    _logger.LogWarning("Visit place with ID {VisitPlaceId} not found.", id);
                    return new NotFoundResult();
                }

                await _unitOfWork.VisitPlaces.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Successfully deleted visit place with ID {VisitPlaceId}.", id);
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting visit place with ID {VisitPlaceId}.", id);
                return new StatusCodeResult(500);
            }
        }
    }
}
