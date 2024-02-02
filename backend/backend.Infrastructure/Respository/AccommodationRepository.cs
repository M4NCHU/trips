using backend.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.Infrastructure.Authentication;
using Microsoft.Extensions.Hosting;
using backend.Application.Services;
using Microsoft.AspNetCore.Hosting;

namespace backend.Infrastructure.Services
{
    public class AccommodationService : IAccommodationService
    {
        private readonly TripsDbContext _context;
        private readonly ImageService _imageService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly BaseUrlService _baseUrlService;
        private readonly string _baseUrl;


        public AccommodationService(TripsDbContext context, ImageService imageService, IWebHostEnvironment hostEnvironment, BaseUrlService baseUrlService)
        {
            _context = context;
            _imageService = imageService;
            _hostEnvironment = hostEnvironment;
            _baseUrlService = baseUrlService;
            _baseUrl = _baseUrlService.GetBaseUrl();
        }

        public async Task<ActionResult<IEnumerable<AccommodationDTO>>> GetAccommodations(int page = 1, int pageSize = 2)
        {
            if (_context.Accommodation == null)
            {
                return new NotFoundResult();
            }


            var accommodations = await _context.Accommodation
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new AccommodationDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Location = x.Location,
                    PhotoUrl = $"{_baseUrl}/Images/Accommodations/{x.PhotoUrl}",
                    Price = x.Price,
                    
                })
                .ToListAsync();

            return accommodations;
        }


        public async Task<ActionResult<AccommodationDTO>> GetAccommodation(Guid id)
        {
            if (_context.Accommodation == null)
            {
                return new NotFoundResult();
            }

            var accommodation = await _context.Accommodation.FindAsync(id);

            if (accommodation == null)
            {
                return new NotFoundResult();
            }

            if (accommodation == null)
            {
                return new NotFoundResult();
            }

            var accommodationDTO = new AccommodationDTO
            {
                Id = accommodation.Id,
                Name = accommodation.Name,
                Description = accommodation.Description,
                Location = accommodation.Location,
                PhotoUrl = $"{_baseUrl}/Images/Accommodations/{accommodation.PhotoUrl}",
                Price = accommodation.Price,
                
            };

            return accommodationDTO;
        }

        

        public async Task<IActionResult> PutAccommodation(Guid id, AccommodationDTO accommodationDTO)
        {
            if (id != accommodationDTO.Id)
            {
                return new BadRequestResult();
            }

            var accommodation = new AccommodationDTO
            {
                Id = id,
                Name = accommodationDTO.Name,
                Description = accommodationDTO.Description,
                Location = accommodationDTO.Location,
                PhotoUrl = accommodationDTO.PhotoUrl
            };

            _context.Entry(accommodation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccommodationExists(id))
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

        public async Task<ActionResult<AccommodationDTO>> PostAccommodation([FromForm] AccommodationDTO accommodationDTO)
        {
            if (_context.Accommodation == null)
            {
                return new ObjectResult("Entity set 'TripsDbContext.Accommodations' is null.")
                {
                    StatusCode = 500
                };
            }

            if (accommodationDTO.ImageFile == null)
            {
                return new BadRequestObjectResult("The 'ImageFileDTO' field is required.");
            }

            accommodationDTO.PhotoUrl = await _imageService.SaveImage(accommodationDTO.ImageFile, "Accommodations");

            var accommodation = new AccommodationModel
            {
                Name = accommodationDTO.Name,
                Description = accommodationDTO.Description,
                Location = accommodationDTO.Location,
                PhotoUrl = accommodationDTO.PhotoUrl,
            };

            _context.Accommodation.Add(accommodation);
            await _context.SaveChangesAsync();

            return new CreatedAtActionResult("GetAccommodation", "Accommodations", new { id = accommodation.Id }, accommodationDTO);
        }

        public async Task<IActionResult> DeleteAccommodation(Guid id)
        {
            if (_context.Accommodation == null)
            {
                return new NotFoundResult();
            }

            var accommodation = await _context.Accommodation.FindAsync(id);
            if (accommodation == null)
            {
                return new NotFoundResult();
            }

            _context.Accommodation.Remove(accommodation);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }

        private bool AccommodationExists(Guid id)
        {
            return (_context.Accommodation?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
