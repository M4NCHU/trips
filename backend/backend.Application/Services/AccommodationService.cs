using AutoMapper;
using backend.Infrastructure;
using backend.Domain.DTOs;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Infrastructure.Respository;

namespace backend.Application.Services
{
    public class AccommodationService : IAccommodationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService; 
        private readonly IMapper _mapper;

        public AccommodationService(IUnitOfWork unitOfWork, IImageService imageService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
            _mapper = mapper;
        }

        public async Task<ActionResult<IEnumerable<AccommodationDTO>>> GetAccommodations(int page = 1, int pageSize = 2)
        {
            var accommodations = await _unitOfWork.Accommodations.GetAccommodations(page, pageSize);
            var accommodationsDTO = _mapper.Map<IEnumerable<AccommodationDTO>>(accommodations);
            return new ActionResult<IEnumerable<AccommodationDTO>>(accommodationsDTO);
        }

        public async Task<ActionResult<AccommodationDTO>> GetAccommodation(Guid id)
        {
            var accommodation = await _unitOfWork.Accommodations.GetByIdAsync(id);
            if (accommodation == null)
            {
                return new NotFoundResult();
            }

            var accommodationDTO = _mapper.Map<AccommodationDTO>(accommodation);
            return new ActionResult<AccommodationDTO>(accommodationDTO);
        }

        public async Task<IActionResult> PutAccommodation(Guid id, AccommodationDTO accommodationDTO)
        {
            if (id != accommodationDTO.Id)
            {
                return new BadRequestResult();
            }

            var accommodation = _mapper.Map<AccommodationModel>(accommodationDTO);
            await _unitOfWork.Accommodations.UpdateAsync(accommodation);
            await _unitOfWork.SaveChangesAsync();

            return new NoContentResult();
        }

        public async Task<ActionResult<AccommodationDTO>> PostAccommodation([FromForm] AccommodationDTO accommodationDTO)
        {
            if (accommodationDTO.ImageFile == null)
            {
                return new BadRequestObjectResult("The 'ImageFileDTO' field is required.");
            }

            accommodationDTO.PhotoUrl = await _imageService.SaveImage(accommodationDTO.ImageFile, "Accommodations");
            var accommodation = _mapper.Map<AccommodationModel>(accommodationDTO);

            await _unitOfWork.Accommodations.AddAsync(accommodation);
            await _unitOfWork.SaveChangesAsync();

            return new CreatedAtActionResult("GetAccommodation", "Accommodations", new { id = accommodation.Id }, accommodationDTO);
        }

        public async Task<IActionResult> DeleteAccommodation(Guid id)
        {
            var accommodation = await _unitOfWork.Accommodations.GetByIdAsync(id);
            if (accommodation == null)
            {
                return new NotFoundResult();
            }

            await _unitOfWork.Accommodations.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return new NoContentResult();
        }
    }
}
