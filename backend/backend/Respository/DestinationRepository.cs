using backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.Authentication;
using Microsoft.Extensions.Hosting;

namespace backend.Services
{
    public class DestinationService : IDestinationService
    {
        private readonly TripsDbContext _context;
        private readonly ImageService _imageService;
        private readonly IWebHostEnvironment _hostEnvironment;


        public DestinationService(TripsDbContext context, ImageService imageService, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _imageService = imageService;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<ActionResult<IEnumerable<DestinationDTO>>> GetDestinations(int page = 1, int pageSize = 2, string scheme = "https", string host = "example.com", string pathBase = "/basepath")
        {
            if (_context.Destinations == null)
            {
                return new NotFoundResult();
            }


            var destinations = await _context.Destinations
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new DestinationDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Location = x.Location,
                    PhotoUrl = string.Format("{0}://{1}{2}/Images/Destinations/{3}", scheme, host, pathBase, x.PhotoUrl),
                    Price = x.Price,
                    CategoryId = x.CategoryId, // Assign CategoryId
                    Category = x.Category != null ? new CategoryDTO // Check if Category is not null
                    {
                        Id = x.Category.Id,
                        Name = x.Category.Name,
                        PhotoUrl = x.Category.PhotoUrl
                    } : null
                })
                .ToListAsync();

            return destinations;
        }


        public async Task<ActionResult<DestinationDTO>> GetDestination(int id, string scheme = "https", string host = "example.com", string pathBase = "/basepath")
        {
            if (_context.Destinations == null)
            {
                return new NotFoundResult();
            }

            var destination = await _context.Destinations
        .Include(d => d.Category) // Include the Category information
        .FirstOrDefaultAsync(d => d.Id == id);

            if (destination == null)
            {
                return new NotFoundResult();
            }

            if (destination == null)
            {
                return new NotFoundResult();
            }

            var destinationDTO = new DestinationDTO
            {
                Id = destination.Id,
                Name = destination.Name,
                Description = destination.Description,
                Location = destination.Location,
                PhotoUrl = string.Format("{0}://{1}{2}/Images/Destinations/{3}", scheme, host, pathBase, destination.PhotoUrl),
                Price = destination.Price,
                CategoryId = destination.CategoryId, // Assign CategoryId
                Category = destination.Category != null ? new CategoryDTO // Check if Category is not null
                {
                    Id = destination.Category.Id,
                    Name = destination.Category.Name,
                    PhotoUrl = destination.Category.PhotoUrl
                } : null
            };

            return destinationDTO;
        }

        public async Task<List<DestinationDTO>> GetDestinationsForTrip(int tripId)
        {
            var destinations = await _context.TripDestinations
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


        public async Task<IActionResult> PutDestination(int id, DestinationDTO destinationDTO)
        {
            if (id != destinationDTO.Id)
            {
                return new BadRequestResult();
            }

            var destination = new Destination
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

        public async Task<ActionResult<DestinationDTO>> PostDestination([FromForm] DestinationDTO destinationDTO)
        {
            if (_context.Destinations == null)
            {
                return new ObjectResult("Entity set 'TripsDbContext.Destinations' is null.")
                {
                    StatusCode = 500
                };
            }

            if (destinationDTO.ImageFile == null)
            {
                return new BadRequestObjectResult("The 'ImageFileDTO' field is required.");
            }

            destinationDTO.PhotoUrl = await _imageService.SaveImage(destinationDTO.ImageFile, "Destinations");

            var destination = new Destination
            {
                Name = destinationDTO.Name,
                Description = destinationDTO.Description,
                Location = destinationDTO.Location,
                PhotoUrl = destinationDTO.PhotoUrl,
                CategoryId = destinationDTO.CategoryId
            };

            _context.Destinations.Add(destination);
            await _context.SaveChangesAsync();

            return new CreatedAtActionResult("GetDestination", "Destinations", new { id = destination.Id }, destinationDTO);
        }

        public async Task<IActionResult> DeleteDestination(int id)
        {
            if (_context.Destinations == null)
            {
                return new NotFoundResult();
            }

            var destination = await _context.Destinations.FindAsync(id);
            if (destination == null)
            {
                return new NotFoundResult();
            }

            _context.Destinations.Remove(destination);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }

        private bool DestinationExists(int id)
        {
            return (_context.Destinations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
