using AutoMapper;
using backend.Domain.DTOs;
using backend.Domain.Filters;
using backend.Infrastructure.Respository;
using backend.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Application.Services
{
    public class DestinationService : IDestinationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IBaseUrlService _baseUrlService;
        private readonly string _baseUrl;
        private readonly ILogger<DestinationService> _logger;
        private readonly IMapper _mapper;

        public DestinationService(
            IUnitOfWork unitOfWork,
            IImageService imageService,
            IWebHostEnvironment hostEnvironment,
            IBaseUrlService baseUrlService,
            ILogger<DestinationService> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
            _hostEnvironment = hostEnvironment;
            _baseUrlService = baseUrlService;
            _baseUrl = _baseUrlService.GetBaseUrl();
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ActionResult<DestinationDTO>> GetDestination(Guid id)
        {
            try
            {
                var destination = await _unitOfWork.Destinations.GetByIdAsync(id);
                if (destination == null)
                {
                    return new NotFoundResult();
                }

                var destinationDto = _mapper.Map<DestinationDTO>(destination, opts =>
                {
                    opts.Items["BaseUrl"] = _baseUrl;
                });

                return new OkObjectResult(destinationDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the destination with ID: {id}", id);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<IActionResult> PostDestination(CreateDestinationDTO destinationDTO)
        {
            try
            {
                _logger.LogInformation("Creating a new destination.");

                if (destinationDTO.ImageFile == null)
                {
                    _logger.LogError("The 'ImageFileDTO' field is required.");
                    return new BadRequestResult();
                }

                if (destinationDTO == null)
                {
                    _logger.LogWarning("Destination object is empty");
                    return new BadRequestResult();
                }

                var photoUrl = await _imageService.SaveImage(destinationDTO.ImageFile, "Destinations");
                var destination = _mapper.Map<DestinationModel>(destinationDTO);
                destination.PhotoUrl = photoUrl;

                await _unitOfWork.Destinations.AddAsync(destination);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Successfully created destination with ID: {id}", destination.Id);

                return new OkObjectResult(destinationDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the destination.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<IActionResult> PutDestination(Guid id, CreateDestinationDTO destinationDTO)
        {
            _logger.LogInformation("Updating destination with ID: {id}", id);

            var destinationExists = await _unitOfWork.Destinations.DestinationExistsAsync(id);
            if (!destinationExists)
            {
                _logger.LogWarning("Provided ID: {id}. Destination doesn't exist", id);
                return new NotFoundResult();
            }

            if (destinationDTO == null)
            {
                _logger.LogWarning("Destination object is empty");
                return new BadRequestResult();
            }

            var existingDestination = await _unitOfWork.Destinations.GetByIdAsync(id);
            if (existingDestination == null)
            {
                return new NotFoundResult();
            }

            _mapper.Map(destinationDTO, existingDestination);


            if (destinationDTO.ImageFile != null)
            {
                var photoUrl = await _imageService.SaveImage(destinationDTO.ImageFile, "Destinations");
                existingDestination.PhotoUrl = photoUrl;
                _logger.LogInformation("Destination image updated for ID: {id}. New photo URL: {photoUrl}", id, photoUrl);
            }

            existingDestination.ModifiedAt = DateTime.UtcNow;

            await _unitOfWork.Destinations.UpdateAsync(existingDestination);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Successfully updated destination with ID: {id}", id);
            return new OkObjectResult(existingDestination);
        }

        public async Task<IActionResult> DeleteDestination(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting destination with ID: {id}", id);

                var destinationExists = await _unitOfWork.Destinations.GetByIdAsync(id);
                if (destinationExists == null)
                {
                    _logger.LogWarning("Attempted to delete destination with ID: {id}, but it does not exist.", id);
                    return new NotFoundResult();
                }

                await _unitOfWork.Destinations.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Successfully deleted destination with ID: {id}", id);
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the destination with ID: {id}", id);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ActionResult<IEnumerable<ResponseDestinationDTO>>> GetDestinations(DestinationFilter filter, int page = 1, int pageSize = 2)
        {
            try
            {
                _logger.LogInformation("Fetching destinations with filter and pagination. Page: {page}, PageSize: {pageSize}", page, pageSize);

                var destinations = await _unitOfWork.Destinations.GetDestinationsAsync(filter, page, pageSize);

                if (destinations == null || !destinations.Any())
                {
                    _logger.LogWarning("No destinations found for the given filter.");
                    return new NotFoundResult();
                }

                var destinationDTOs = _mapper.Map<List<ResponseDestinationDTO>>(destinations, opts =>
                {
                    opts.Items["BaseUrl"] = _baseUrl;
                });

                _logger.LogInformation("Successfully fetched {count} destinations.", destinationDTOs.Count);
                return new OkObjectResult(destinationDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching destinations.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<IEnumerable<DestinationDTO>> SearchDestinations(string searchTerm)
        {
            try
            {
                _logger.LogInformation("Searching destinations with term: {searchTerm}", searchTerm);

                var destinations = await _unitOfWork.Destinations.SearchDestinationsAsync(searchTerm);
                if (!destinations.Any())
                {
                    _logger.LogWarning("No destinations found for the search term: {searchTerm}", searchTerm);
                }

                return _mapper.Map<IEnumerable<DestinationDTO>>(destinations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching destinations.");
                return new List<DestinationDTO>();
            }
        }

        public async Task<List<DestinationDTO>> GetDestinationsForTrip(Guid tripId)
        {
            try
            {
                _logger.LogInformation("Fetching destinations for trip with ID: {tripId}", tripId);

                var destinations = await _unitOfWork.Destinations.GetDestinationsForTripAsync(tripId);
                if (!destinations.Any())
                {
                    _logger.LogWarning("No destinations found for trip ID: {tripId}", tripId);
                }

                return _mapper.Map<List<DestinationDTO>>(destinations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching destinations for trip ID: {tripId}", tripId);
                return new List<DestinationDTO>(); 
            }
        }

    }
}
