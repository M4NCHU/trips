using backend.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.Infrastructure.Authentication;
using backend.Application.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;


namespace backend.Infrastructure.Services
{
    public class VisitPlaceService : IVisitPlaceService
    {
        private readonly TripsDbContext _context;
        private readonly ImageService _imageService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly BaseUrlService _baseUrlService;
        private readonly string _baseUrl;
        private readonly ILogger<VisitPlaceService> _logger;


        public VisitPlaceService(TripsDbContext context, ImageService imageService, IWebHostEnvironment hostEnvironment, BaseUrlService baseUrlService, ILogger<VisitPlaceService> logger)
        {
            _context = context;
            _imageService = imageService;
            _hostEnvironment = hostEnvironment;
            _baseUrlService = baseUrlService;
            _baseUrl = _baseUrlService.GetBaseUrl();
            _logger = logger;
        }

        public async Task<ActionResult<IEnumerable<VisitPlaceDTO>>> GetVisitPlaces()
        {
            if (_context.VisitPlace == null)
            {
                return new NotFoundResult();
            }

            var visitPlaces = await _context.VisitPlace
                .OrderBy(x => x.Id)
                .Select(x => new VisitPlaceDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    PhotoUrl = $"{_baseUrl}/Images/VisitPlace/{x.PhotoUrl}",
                    DestinationId = x.DestinationId
                })
                .ToListAsync();

            return visitPlaces;
        }


        public async Task<ActionResult<VisitPlaceDTO>> GetVisitPlace(Guid id)
        {
            if (_context.VisitPlace == null)
            {
                return new NotFoundResult();
            }

            var visitPlace = await _context.VisitPlace.FindAsync(id);

            if (visitPlace == null)
            {
                return new NotFoundResult();
            }

            var VisitPlaceDTO = new VisitPlaceDTO
            {
                Name = visitPlace.Name,
                Description = visitPlace.Description,
                PhotoUrl = $"{_baseUrl}/Images/VisitPlace/{visitPlace.PhotoUrl}",
                DestinationId = visitPlace.DestinationId,
                Price = visitPlace.Price
            };

            return VisitPlaceDTO;
        }


        public async Task<ActionResult<IEnumerable<VisitPlaceDTO>>> GetVisitPlacesByDestination(Guid destinationId)
        {
            
            var visitPlaces = await _context.VisitPlace
                .Where(vp => vp.DestinationId == destinationId)
                .ToListAsync();

            var visitPlaceDTOs = visitPlaces.Select(vp => new VisitPlaceDTO
            {
                Id = vp.Id,
                Name = vp.Name,
                Description = vp.Description,
                PhotoUrl = $"{_baseUrl}/Images/VisitPlace/{vp.PhotoUrl}",
                DestinationId = vp.DestinationId,
                Price = vp.Price,
                
            }).ToList();

            return visitPlaceDTOs;
        }



        private ActionResult<IEnumerable<VisitPlaceDTO>> StatusCode(int v1, string v2)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> PutVisitPlace(Guid id, VisitPlaceDTO VisitPlaceDTO)
        {
            if (id != VisitPlaceDTO.Id)
            {
                return new BadRequestResult();
            }

            var visitPlace = new VisitPlace
            {
                Id = id,
                Name = VisitPlaceDTO.Name,
                Description = VisitPlaceDTO.Description,
                PhotoUrl = VisitPlaceDTO.PhotoUrl,
                DestinationId = VisitPlaceDTO.DestinationId
            };

            _context.Entry(visitPlace).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VisitPlaceExists(id))
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

        public async Task<CreateVisitPlaceDTO> PostVisitPlace(CreateVisitPlaceDTO visitPlaceDTO)
        {
            if (_context.VisitPlace == null)
            {
                _logger.LogError("Entity set 'TripsDbContext.VisitPlace' is null.");
                throw new InvalidOperationException("Entity set 'TripsDbContext.VisitPlace' is null.");
            }

            if (visitPlaceDTO.ImageFile == null)
            {
                _logger.LogError("The 'ImageFileDTO' field is required.");
                throw new ArgumentException("The 'ImageFileDTO' field is required.");
            }

            visitPlaceDTO.PhotoUrl = await _imageService.SaveImage(visitPlaceDTO.ImageFile, "VisitPlace");
            var currentDate = DateTime.UtcNow;

            var visitPlace = new VisitPlace
            {
                Id = Guid.NewGuid(),
                Name = visitPlaceDTO.Name,
                Description = visitPlaceDTO.Description,
                PhotoUrl = visitPlaceDTO.PhotoUrl,
                DestinationId = visitPlaceDTO.DestinationId,
                CreatedAt = currentDate,
                ModifiedAt = currentDate,
            };

            _context.VisitPlace.Add(visitPlace);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"New visit place '{visitPlace.Name}' added successfully.");

            
            return visitPlaceDTO;
        }

        public async Task<IActionResult> DeleteVisitPlace(Guid id)
        {
            if (_context.VisitPlace == null)
            {
                return new NotFoundResult();
            }

            var visitPlace = await _context.VisitPlace.FindAsync(id);
            if (visitPlace == null)
            {
                return new NotFoundResult();
            }

            _context.VisitPlace.Remove(visitPlace);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }

        private bool VisitPlaceExists(Guid id)
        {
            return (_context.VisitPlace?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        


    }
}
