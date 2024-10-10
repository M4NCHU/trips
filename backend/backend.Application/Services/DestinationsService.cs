﻿using AutoMapper;
using backend.Application.Services;
using backend.Domain.DTOs;
using backend.Domain.Filters;
using backend.Infrastructure.Authentication;
using backend.Infrastructure.Respository;
using backend.Models;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IDestinationRepository _destinationRepository;
        private readonly ImageService _imageService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly BaseUrlService _baseUrlService;
        private readonly string _baseUrl;
        private readonly ILogger<DestinationService> _logger;
        private readonly IMapper _mapper;

        public DestinationService(IDestinationRepository destinationRepository, ImageService imageService, IWebHostEnvironment hostEnvironment, BaseUrlService baseUrlService, ILogger<DestinationService> logger, IMapper mapper)
        {
            _destinationRepository = destinationRepository;
            _imageService = imageService;
            _hostEnvironment = hostEnvironment;
            _baseUrlService = baseUrlService;
            _baseUrl = _baseUrlService.GetBaseUrl();
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ActionResult<IEnumerable<ResponseDestinationDTO>>> GetDestinations(DestinationFilter filter, int page = 1, int pageSize = 2)
        {
            _logger.LogInformation("Fetching destinations with filter and pagination. Page: {page}, PageSize: {pageSize}", page, pageSize);

            var destinations = await _destinationRepository.GetDestinationsAsync(filter, page, pageSize);

            if (destinations == null || !destinations.Any())
            {
                _logger.LogWarning("No destinations found for the given filter.");
                return new NotFoundResult();
            }

            var destinationDTOs = _mapper.Map<List<ResponseDestinationDTO>>(destinations, opts => {
                opts.Items["BaseUrl"] = _baseUrl;
            });

            _logger.LogInformation("Successfully fetched {count} destinations.", destinationDTOs.Count);
            return destinationDTOs;
        }

        public async Task<ActionResult<DestinationDTO>> GetDestination(Guid id)
        {
            _logger.LogInformation("Fetching destination with ID: {id}", id);

            var destination = await _destinationRepository.GetDestinationByIdAsync(id);
            if (destination == null)
            {
                _logger.LogWarning("Destination with ID {id} not found.", id);
                return new NotFoundResult();
            }

            var destinationDto = _mapper.Map<DestinationDTO>(destination, opts => {
                opts.Items["BaseUrl"] = _baseUrl;
            });

            _logger.LogInformation("Successfully fetched destination with ID: {id}", id);
            return destinationDto;
        }

        public async Task<IEnumerable<DestinationDTO>> SearchDestinations(string searchTerm)
        {
            _logger.LogInformation("Searching destinations with term: {searchTerm}", searchTerm);

            var destinations = await _destinationRepository.SearchDestinationsAsync(searchTerm);
            if (!destinations.Any())
            {
                _logger.LogWarning("No destinations found for the search term: {searchTerm}", searchTerm);
            }

            return _mapper.Map<IEnumerable<DestinationDTO>>(destinations);
        }

        public async Task<List<DestinationDTO>> GetDestinationsForTrip(Guid tripId)
        {
            _logger.LogInformation("Fetching destinations for trip with ID: {tripId}", tripId);

            var destinations = await _destinationRepository.GetDestinationsForTripAsync(tripId);
            if (!destinations.Any())
            {
                _logger.LogWarning("No destinations found for trip ID: {tripId}", tripId);
            }

            return _mapper.Map<List<DestinationDTO>>(destinations);
        }

        public async Task<IActionResult> PutDestination(Guid id, DestinationDTO destinationDTO)
        {
            _logger.LogInformation("Updating destination with ID: {id}", id);

            if (id != destinationDTO.Id)
            {
                _logger.LogWarning("Mismatched ID during update. Provided ID: {id}, DTO ID: {dtoId}", id, destinationDTO.Id);
                return new BadRequestResult();
            }

            var destination = _mapper.Map<DestinationModel>(destinationDTO);
            await _destinationRepository.UpdateDestinationAsync(destination);

            _logger.LogInformation("Successfully updated destination with ID: {id}", id);
            return new NoContentResult();
        }

        public async Task<CreateDestinationDTO> PostDestination(CreateDestinationDTO destinationDTO)
        {
            _logger.LogInformation("Creating a new destination.");

            if (destinationDTO.ImageFile == null)
            {
                _logger.LogError("The 'ImageFileDTO' field is required.");
                throw new ArgumentException("The 'ImageFileDTO' field is required.");
            }

            destinationDTO.PhotoUrl = await _imageService.SaveImage(destinationDTO.ImageFile, "Destinations");

            var destination = _mapper.Map<DestinationModel>(destinationDTO);
            await _destinationRepository.AddDestinationAsync(destination);

            _logger.LogInformation("Successfully created destination with ID: {id}", destination.Id);
            return destinationDTO;
        }

        public async Task<IActionResult> DeleteDestination(Guid id)
        {
            _logger.LogInformation("Deleting destination with ID: {id}", id);

            var destinationExists = await _destinationRepository.DestinationExistsAsync(id);
            if (!destinationExists)
            {
                _logger.LogWarning("Attempted to delete destination with ID: {id}, but it does not exist.", id);
                return new NotFoundResult();
            }

            await _destinationRepository.DeleteDestinationAsync(id);

            _logger.LogInformation("Successfully deleted destination with ID: {id}", id);
            return new NoContentResult();
        }
    }
}
