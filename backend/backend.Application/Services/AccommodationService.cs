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
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly ImageService _imageService;
        private readonly IMapper _mapper;

        public AccommodationService(
            IAccommodationRepository accommodationRepository,
            ImageService imageService,
            IMapper mapper)
        {
            _accommodationRepository = accommodationRepository;
            _imageService = imageService;
            _mapper = mapper;
        }

        public async Task<ActionResult<IEnumerable<AccommodationDTO>>> GetAccommodations(int page = 1, int pageSize = 2)
        {
            var accommodations = await _accommodationRepository.GetAccommodations(page, pageSize);
            var accommodationsDTO = _mapper.Map<IEnumerable<AccommodationDTO>>(accommodations);
            return new ActionResult<IEnumerable<AccommodationDTO>>(accommodationsDTO);
        }

        public async Task<ActionResult<AccommodationDTO>> GetAccommodation(Guid id)
        {
            var accommodation = await _accommodationRepository.GetAccommodationById(id);
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
            await _accommodationRepository.UpdateAccommodation(accommodation);

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

            await _accommodationRepository.AddAccommodation(accommodation);

            return new CreatedAtActionResult("GetAccommodation", "Accommodations", new { id = accommodation.Id }, accommodationDTO);
        }

        public async Task<IActionResult> DeleteAccommodation(Guid id)
        {
            var accommodation = await _accommodationRepository.GetAccommodationById(id);
            if (accommodation == null)
            {
                return new NotFoundResult();
            }

            await _accommodationRepository.DeleteAccommodation(id);
            return new NoContentResult();
        }
    }
}
