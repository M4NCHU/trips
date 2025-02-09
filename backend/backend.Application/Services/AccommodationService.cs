using AutoMapper;
using backend.Infrastructure;
using backend.Domain.DTOs;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Infrastructure.Respository;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using backend.Domain.Filters;
using Microsoft.AspNetCore.Hosting;
using backend.Domain.enums;
using backend.Domain.Models;

namespace backend.Application.Services
{
    public class AccommodationService : IAccommodationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IBaseUrlService _baseUrlService;
        private readonly string _baseUrl;
        private readonly ILogger<AccommodationService> _logger;
        private readonly IMapper _mapper;

        public AccommodationService(
            IUnitOfWork unitOfWork,
            IImageService imageService,
            IWebHostEnvironment hostEnvironment,
            IBaseUrlService baseUrlService,
            ILogger<AccommodationService> logger,
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

        public async Task<ActionResult<PagedResult<AccommodationDTO>>> GetAccomodations(DestinationFilter filter, int page = 1, int pageSize = 2)
        {
            try
            {
                _logger.LogInformation("Fetching destinations with filter and pagination. Page: {page}, PageSize: {pageSize}", page, pageSize);

                var destinationsPaged = await _unitOfWork.Accommodations.GetAccomodationAsync(filter, page, pageSize);

                var AccomodationsDTOs = _mapper.Map<List<AccommodationDTO>>(destinationsPaged.Items, opts =>
                {
                    opts.Items["BaseUrl"] = _baseUrl;
                });

                if (AccomodationsDTOs == null || !AccomodationsDTOs.Any())
                {
                    _logger.LogWarning("No destinations found for the given filter.");
                    return new NoContentResult();
                }

                var pagedResult = new PagedResult<AccommodationDTO>
                {
                    Items = AccomodationsDTOs,
                    TotalItems = destinationsPaged.TotalItems,
                    PageSize = destinationsPaged.PageSize,
                    CurrentPage = destinationsPaged.CurrentPage
                };

                _logger.LogInformation("Successfully fetched {count} destinations.", AccomodationsDTOs.Count);
                return new OkObjectResult(pagedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching destinations.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ActionResult<AccommodationDTO>> GetAccommodation(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching accommodation with ID: {id}", id);
                var accommodation = await _unitOfWork.Accommodations.GetByIdAsync(id);
                if (accommodation == null)
                {
                    _logger.LogWarning("Accommodation with ID: {id} not found", id);
                    return new NotFoundResult();
                }

                var accommodationDTO = _mapper.Map<AccommodationDTO>(accommodation);
                return new ActionResult<AccommodationDTO>(accommodationDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the accommodation with ID: {id}", id);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<IActionResult> PutAccommodation(Guid id, AccommodationDTO accommodationDTO)
        {
            try
            {
                _logger.LogInformation("Updating accommodation with ID: {id}", id);

                if (id != accommodationDTO.Id)
                {
                    _logger.LogWarning("Mismatched IDs. Provided ID: {id} does not match AccommodationDTO ID", id);
                    return new BadRequestResult();
                }

                var accommodation = _mapper.Map<AccommodationModel>(accommodationDTO);
                await _unitOfWork.Accommodations.UpdateAsync(accommodation);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Successfully updated accommodation with ID: {id}", id);
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the accommodation with ID: {id}", id);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ActionResult<AccommodationDTO>> PostAccommodation(CreateAccommodationDTO accomodationDTO, [FromForm] GeoLocationDTO geoLocation)
        {
            try
            {
                _logger.LogInformation("Creating a new destination.");

                if (accomodationDTO.ImageFile == null)
                {
                    _logger.LogError("The 'ImageFileDTO' field is required.");
                    return new BadRequestResult();
                }

                if (accomodationDTO == null)
                {
                    _logger.LogWarning("Destination object is empty");
                    return new BadRequestResult();
                }

                var photoUrl = await _imageService.SaveImage(accomodationDTO.ImageFile, "Destinations");
                var destination = _mapper.Map<AccommodationModel>(accomodationDTO);
                destination.PhotoUrl = photoUrl;

                if (geoLocation != null)
                {
                    var geoLocationModel = _mapper.Map<GeoLocationModel>(geoLocation);

                    geoLocationModel.Id = Guid.NewGuid();
                    geoLocationModel.ItemId = destination.Id;
                    geoLocationModel.Type = CartItemType.Accommodation;
                    geoLocationModel.Description = destination.Description;
                    geoLocationModel.CreatedAt = DateTime.UtcNow;
                    geoLocationModel.ModifiedAt = DateTime.UtcNow;

                    await _unitOfWork.GeoLocations.AddAsync(geoLocationModel);
                    destination.GeoLocationId = geoLocationModel.Id;
                }


                await _unitOfWork.Accommodations.AddAsync(destination);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Successfully created destination with ID: {id}", destination.Id);

                return new OkObjectResult(accomodationDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the destination.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<IActionResult> DeleteAccommodation(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting accommodation with ID: {id}", id);

                var accommodation = await _unitOfWork.Accommodations.GetByIdAsync(id);
                if (accommodation == null)
                {
                    _logger.LogWarning("Accommodation with ID: {id} not found", id);
                    return new NotFoundResult();
                }

                await _unitOfWork.Accommodations.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Successfully deleted accommodation with ID: {id}", id);
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the accommodation with ID: {id}", id);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
