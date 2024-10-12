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

        public async Task<ActionResult<VisitPlaceDTO>> GetVisitPlace(Guid id)
        {
            _logger.LogInformation("Fetching visit place with ID {VisitPlaceId}.", id.ToString());
            var visitPlace = await _unitOfWork.VisitPlaces.GetByIdAsync(id);
            if (visitPlace == null)
            {
                _logger.LogWarning("Visit place with ID {VisitPlaceId} not found.", id.ToString());
                return new NotFoundResult();
            }

            var visitPlaceDTO = _mapper.Map<VisitPlaceDTO>(visitPlace);
            visitPlaceDTO.PhotoUrl = $"{_baseUrl}/Images/VisitPlace/{visitPlaceDTO.PhotoUrl}";
            _logger.LogInformation("Fetched visit place with ID {VisitPlaceId}.", id.ToString());
            return new OkObjectResult(visitPlaceDTO);
        }

        public async Task<ActionResult<IEnumerable<VisitPlaceDTO>>> GetVisitPlacesByDestination(Guid destinationId)
        {
            _logger.LogInformation("Fetching visit places for destination ID {DestinationId}.", destinationId.ToString());
            var visitPlaces = await _unitOfWork.VisitPlaces.GetVisitPlacesByDestinationAsync(destinationId);
            var visitPlaceDTOs = _mapper.Map<IEnumerable<VisitPlaceDTO>>(visitPlaces);

            foreach (var place in visitPlaceDTOs)
            {
                place.PhotoUrl = $"{_baseUrl}/Images/VisitPlace/{place.PhotoUrl}";
            }
            _logger.LogInformation("Fetched {Count} visit places for destination ID {DestinationId}.", visitPlaceDTOs.Count(), destinationId.ToString());
            return new OkObjectResult(visitPlaceDTOs);
        }

        public async Task<IActionResult> PutVisitPlace(Guid id, VisitPlaceDTO visitPlaceDTO)
        {
            _logger.LogInformation("Updating visit place with ID {VisitPlaceId}.", id.ToString());
            if (id != visitPlaceDTO.Id)
            {
                _logger.LogWarning("Visit place ID mismatch: {VisitPlaceId} vs {DTOId}.", id.ToString(), visitPlaceDTO.Id.ToString());
                return new BadRequestResult();
            }

            var visitPlaceModel = _mapper.Map<VisitPlaceModel>(visitPlaceDTO);
            await _unitOfWork.VisitPlaces.UpdateAsync(visitPlaceModel);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Successfully updated visit place with ID {VisitPlaceId}.", id.ToString());
            return new NoContentResult();
        }

        public async Task<IActionResult> PostVisitPlace(CreateVisitPlaceDTO visitPlaceDTO)
        {
            _logger.LogInformation("Creating new visit place with name {Name}.", visitPlaceDTO.Name);

            if (visitPlaceDTO.ImageFile == null)
            {
                _logger.LogError("The 'ImageFileDTO' field is required.");
                throw new ArgumentException("The 'ImageFileDTO' field is required.");
            }

            visitPlaceDTO.PhotoUrl = await _imageService.SaveImage(visitPlaceDTO.ImageFile, "VisitPlace");
            var visitPlaceModel = _mapper.Map<VisitPlaceModel>(visitPlaceDTO);
            await _unitOfWork.VisitPlaces.AddAsync(visitPlaceModel);
            await _unitOfWork.SaveChangesAsync();

            var visitPlaceDTOResult = _mapper.Map<VisitPlaceDTO>(visitPlaceModel);
            _logger.LogInformation("Successfully created visit place with name {Name}.", visitPlaceDTO.Name);

            return new CreatedAtActionResult(nameof(GetVisitPlace), "VisitPlaces", new { id = visitPlaceDTOResult.Id }, visitPlaceDTOResult);
        }


        public async Task<IActionResult> DeleteVisitPlace(Guid id)
        {
            _logger.LogInformation("Attempting to delete visit place with ID {VisitPlaceId}.", id.ToString());

            var visitPlace = await _unitOfWork.VisitPlaces.GetByIdAsync(id);
            if (visitPlace == null)
            {
                _logger.LogWarning("Visit place with ID {VisitPlaceId} not found.", id.ToString());
                return new NotFoundResult();  
            }

            await _unitOfWork.VisitPlaces.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Successfully deleted visit place with ID {VisitPlaceId}.", id.ToString());
            return new NoContentResult();
        }

    }
}
