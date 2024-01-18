﻿using backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.Authentication;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace backend.Services
{
    public class DestinationService : IDestinationService
    {
        private readonly TripsDbContext _context;
        private readonly ImageService _imageService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly BaseUrlService _baseUrlService;
        private readonly string _baseUrl;

        public DestinationService(TripsDbContext context, ImageService imageService, IWebHostEnvironment hostEnvironment, BaseUrlService baseUrlService)
        {
            _context = context;
            _imageService = imageService;
            _hostEnvironment = hostEnvironment;
            _baseUrlService = baseUrlService;
            _baseUrl = _baseUrlService.GetBaseUrl();
        }

        

        public async Task<ActionResult<IEnumerable<DestinationDTO>>> GetDestinations(int page = 1, int pageSize = 2)
        {
            if (_context.Destination == null)
            {
                return new NotFoundResult();
            }


            var destinations = await _context.Destination
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new DestinationDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Location = x.Location,
                    PhotoUrl = $"{_baseUrl}/Images/Destinations/{x.PhotoUrl}",
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


        public async Task<ActionResult<DestinationDTO>> GetDestination(int id)
        {
            try
            {
                var destination = await _context.Destination
                    .Include(d => d.Category) // Include the Category information
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (destination == null)
                {
                    return new NotFoundResult();
                }

                var currentDate = DateTime.Now.ToUniversalTime();

                var destinationDTO = new DestinationDTO
                {
                    Id = destination.Id,
                    Name = destination.Name,
                    Description = destination.Description,
                    Location = destination.Location,
                    PhotoUrl = $"{_baseUrl}/Images/Destinations/{destination.PhotoUrl}",
                    Price = destination.Price,
                    CategoryId = destination.CategoryId, 
                    CreatedAt = currentDate,
                    ModifiedAt = currentDate,
                    Category = destination.Category != null ? new CategoryDTO
                    {
                        Id = destination.Category.Id,
                        Name = destination.Category.Name,
                        PhotoUrl = destination.Category.PhotoUrl
                    } : null
                };

                return destinationDTO;
            }
            catch (Exception ex)
            {
                
                return null;
            }
        }

        public async Task<List<DestinationDTO>> GetDestinationsForTrip(int tripId)
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


        public async Task<IActionResult> PutDestination(int id, DestinationDTO destinationDTO)
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

        public async Task<ActionResult<DestinationDTO>> PostDestination([FromForm] DestinationDTO destinationDTO)
        {
            if (_context.Destination == null)
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

            var currentDate = DateTime.Now.ToUniversalTime();

            var destination = new DestinationModel
            {
                Name = destinationDTO.Name,
                Description = destinationDTO.Description,
                Location = destinationDTO.Location,
                PhotoUrl = destinationDTO.PhotoUrl,
                CategoryId = destinationDTO.CategoryId,
                CreatedAt = currentDate,
                ModifiedAt = currentDate,
                Price = destinationDTO.Price,
            };

            _context.Destination.Add(destination);
            await _context.SaveChangesAsync();

            return new CreatedAtActionResult("GetDestination", "Destinations", new { id = destination.Id }, destinationDTO);
        }

        public async Task<IActionResult> DeleteDestination(int id)
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

        private bool DestinationExists(int id)
        {
            return (_context.Destination?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
