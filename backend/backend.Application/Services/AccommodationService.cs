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

namespace backend.Application.Services
{
    public class AccommodationService : IAccommodationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        private readonly ILogger<AccommodationService> _logger;

        public AccommodationService(IUnitOfWork unitOfWork, IImageService imageService, IMapper mapper, ILogger<AccommodationService> logger)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ActionResult<IEnumerable<AccommodationDTO>>> GetAccommodations(int page = 1, int pageSize = 2)
        {
            try
            {
                _logger.LogInformation("Fetching accommodations, Page: {page}, PageSize: {pageSize}", page, pageSize);
                var accommodations = await _unitOfWork.Accommodations.GetAccommodations(page, pageSize);
                var accommodationsDTO = _mapper.Map<IEnumerable<AccommodationDTO>>(accommodations);
                return new ActionResult<IEnumerable<AccommodationDTO>>(accommodationsDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching accommodations.");
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

        public async Task<ActionResult<AccommodationDTO>> PostAccommodation([FromForm] AccommodationDTO accommodationDTO)
        {
            try
            {
                _logger.LogInformation("Creating a new accommodation.");

                if (accommodationDTO.ImageFile == null)
                {
                    _logger.LogWarning("The 'ImageFileDTO' field is required.");
                    return new BadRequestObjectResult("The 'ImageFileDTO' field is required.");
                }

                accommodationDTO.PhotoUrl = await _imageService.SaveImage(accommodationDTO.ImageFile, "Accommodations");
                var accommodation = _mapper.Map<AccommodationModel>(accommodationDTO);

                await _unitOfWork.Accommodations.AddAsync(accommodation);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Successfully created accommodation with ID: {id}", accommodation.Id);
                return new CreatedAtActionResult("GetAccommodation", "Accommodations", new { id = accommodation.Id }, accommodationDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the accommodation.");
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
