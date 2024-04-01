using backend.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.Infrastructure.Authentication;
using Microsoft.Extensions.Hosting;
using System.Net;
using backend.Application.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using backend.Domain.Filters;


namespace backend.Infrastructure.Services
{
    public class DestinationService : IDestinationService
    {
        private readonly TripsDbContext _context;
        private readonly ImageService _imageService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly BaseUrlService _baseUrlService;
        private readonly string _baseUrl;
        private readonly ILogger<DestinationService> _logger;

        public DestinationService(TripsDbContext context, ImageService imageService, IWebHostEnvironment hostEnvironment, BaseUrlService baseUrlService, ILogger<DestinationService> logger)
        {
            _context = context;
            _imageService = imageService;
            _hostEnvironment = hostEnvironment;
            _baseUrlService = baseUrlService;
            _baseUrl = _baseUrlService.GetBaseUrl();
            _logger = logger;
        }

        

        public async Task<ActionResult<IEnumerable<ResponseDestinationDTO>>> GetDestinations([FromQuery] DestinationFilter filter, int page = 1, int pageSize = 2)
        {
            if (_context.Destination == null)
            {
                _logger.LogWarning("Attempted to query Destinations, but the set was null.");
                return new NotFoundResult();
            }

            var query = _context.Destination.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.CategoryId) && Guid.TryParse(filter.CategoryId, out Guid parsedCategoryId))
            {
                _logger.LogInformation($"Applying category filter: {parsedCategoryId}");
                query = query.Where(d => d.CategoryId == parsedCategoryId);
            }



            var destinations = await query
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (!destinations.Any())
            {
                _logger.LogInformation("No destinations found with the applied filters.");
            }
            else
            {
                _logger.LogInformation($"Retrieved {destinations.Count} destinations for page {page} with page size {pageSize}.");
            }

            var destinationDTOs = destinations.Select(d => new ResponseDestinationDTO
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                CategoryId = d.CategoryId,
                CreatedAt = d.CreatedAt,
                ModifiedAt = d.ModifiedAt,
                PhotoUrl = $"{_baseUrl}/Images/Destinations/{d.PhotoUrl}",
                Price = d.Price,
                Location = d.Location
            }).ToList();

            return destinationDTOs;
        }


        public async Task<ActionResult<DestinationDTO>> GetDestination(Guid id)
        {
            try
            {
                var destination = await _context.Destination
                    .Include(d => d.Category) // Include the Category information
                    .Include(d => d.VisitPlaces)
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (destination == null)
                {
                    return new NotFoundResult();
                }

                var currentDate = DateTime.Now.ToUniversalTime();

                var destinationDTO = MapToDestinationDTO(destination); 
                return destinationDTO;
            }
            catch (Exception ex)
            {
                
                return null;
            }
        }

        public async Task<IEnumerable<DestinationDTO>> SearchDestinations(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
               
                return Enumerable.Empty<DestinationDTO>();
            }

            var destinations = await _context.Destination
                .Where(d => d.Name.Contains(searchTerm))
                .Select(d => new DestinationDTO
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                   
                })
                .ToListAsync();

            return destinations;
        }



        public async Task<List<DestinationDTO>> GetDestinationsForTrip(Guid tripId)
        {
            var destinations = await _context.TripDestination
                .Where(td => td.TripId == tripId)
                .Select(td => new DestinationDTO
                {
                    Id = td.DestinationId,
                    Name = td.Destination.Name, // Assuming Destination has a Name property
                    Description = td.Destination.Description,
                    Location = td.Destination.Location,
                    PhotoUrl = td.Destination.PhotoUrl,
                    CategoryId = td.Destination.CategoryId
                   
                })
                .ToListAsync();

            return destinations;
        }


        public async Task<IActionResult> PutDestination(Guid id, DestinationDTO destinationDTO)
        {
            if (id != destinationDTO.Id)
            {
                return new BadRequestResult();
            }

            var destination = new DestinationModel
            {
                Id = id,
                Name = destinationDTO.Name,
                Description = destinationDTO.Description,
                Location = destinationDTO.Location,
                PhotoUrl = destinationDTO.PhotoUrl
            };

            _context.Entry(destination).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DestinationExists(id))
                {
                    return new NotFoundResult();
                }
                else
                {
                    throw;
                }
            }

            return new NoContentResult();
        }

        public async Task<CreateDestinationDTO> PostDestination(CreateDestinationDTO destinationDTO)
        {
            try
            {
                if (_context.Destination == null)
                {
                    _logger.LogError("Entity set 'TripsDbContext.Destinations' is null.");
                    throw new InvalidOperationException("Entity set 'TripsDbContext.Destinations' is null.");
                }

                if (destinationDTO.ImageFile == null)
                {
                    _logger.LogError("The 'ImageFileDTO' field is required.");
                    throw new ArgumentException("The 'ImageFileDTO' field is required.");
                }

                destinationDTO.PhotoUrl = await _imageService.SaveImage(destinationDTO.ImageFile, "Destinations");

                var currentDate = DateTime.Now.ToUniversalTime();

                var destination = new DestinationModel
                {
                    Id = new Guid(),
                    Name = destinationDTO.Name,
                    Description = destinationDTO.Description,
                    Location = destinationDTO.Location,
                    PhotoUrl = destinationDTO.PhotoUrl,
                    CategoryId = destinationDTO.CategoryId,
                    Price = destinationDTO.Price,
                    CreatedAt = currentDate,
                    ModifiedAt = currentDate,
                    ImageFile = destinationDTO.ImageFile,
                };
                _context.Destination.Add(destination);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Destination '{destination.Name}' was successfully created at {currentDate}.");

                return destinationDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while posting a new destination.");
                throw; 
            }
        }

        public async Task<IActionResult> DeleteDestination(Guid id)
        {
            if (_context.Destination == null)
            {
                return new NotFoundResult();
            }

            var destination = await _context.Destination.FindAsync(id);
            if (destination == null)
            {
                return new NotFoundResult();
            }

            _context.Destination.Remove(destination);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }

        private bool DestinationExists(Guid id)
        {
            return (_context.Destination?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private DestinationDTO MapToDestinationDTO(DestinationModel destination)
        {
            return new DestinationDTO
            {
                Id = destination.Id,
                Name = destination.Name,
                Description = destination.Description,
                Location = destination.Location,
                PhotoUrl = $"{_baseUrl}/Images/Destinations/{destination.PhotoUrl}",
                Price = destination.Price,
                CategoryId = destination.CategoryId,
                Category = destination.Category != null ? new CategoryDTO
                {
                    Id = destination.Category.Id,
                    Name = destination.Category.Name,
                    PhotoUrl = destination.Category.PhotoUrl
                } : null,
                VisitPlaces = destination.VisitPlaces != null ? destination.VisitPlaces.Select(vp => new VisitPlaceDTO
                {
                    Id = vp.Id,
                    Name = vp.Name,
                    Description = vp.Description,
                    PhotoUrl = $"{_baseUrl}/Images/VisitPlace/{vp.PhotoUrl}",
                    Price = vp.Price,
                    DestinationId = vp.DestinationId
                }).ToList() : new List<VisitPlaceDTO>()
            };
        }

        private DestinationModel MapToDestinationModel(DestinationDTO destinationDTO, DestinationModel existingDestination = null)
        {
            var destination = existingDestination ?? new DestinationModel();

            destination.Name = destinationDTO.Name;
            destination.Description = destinationDTO.Description;
            destination.Location = destinationDTO.Location;
            destination.PhotoUrl = destinationDTO.PhotoUrl; 
            destination.Price = destinationDTO.Price;
            destination.CategoryId = destinationDTO.CategoryId;

            return destination;
        }
    }
}
