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
    public class VisitPlaceService : IVisitPlaceService
    {
        private readonly TripsDbContext _context;
        private readonly ImageService _imageService;
        private readonly IWebHostEnvironment _hostEnvironment;


        public VisitPlaceService(TripsDbContext context, ImageService imageService, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _imageService = imageService;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<ActionResult<IEnumerable<VisitPlaceDTO>>> GetVisitPlaces(string scheme = "https", string host = "example.com", string pathBase = "/basepath")
        {
            if (_context.VisitPlaces == null)
            {
                return new NotFoundResult();
            }

            var visitPlaces = await _context.VisitPlaces
                .OrderBy(x => x.Id)
                .Select(x => new VisitPlaceDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    PhotoUrl = string.Format("{0}://{1}{2}/Images/VisitPlace/{3}", scheme, host, pathBase, x.PhotoUrl),
                    DestinationId = x.DestinationId
                })
                .ToListAsync();

            return visitPlaces;
        }


        public async Task<ActionResult<VisitPlaceDTO>> GetVisitPlace(int id, string scheme = "https", string host = "example.com", string pathBase = "/basepath")
        {
            if (_context.VisitPlaces == null)
            {
                return new NotFoundResult();
            }

            var visitPlace = await _context.VisitPlaces.FindAsync(id);

            if (visitPlace == null)
            {
                return new NotFoundResult();
            }

            var VisitPlaceDTO = new VisitPlaceDTO
            {
                Name = visitPlace.Name,
                Description = visitPlace.Description,
                PhotoUrl = string.Format("{0}://{1}{2}/Images/VisitPlace/{3}", scheme, host, pathBase, visitPlace.PhotoUrl),
                DestinationId = visitPlace.DestinationId,
                Price = visitPlace.Price
            };

            return VisitPlaceDTO;
        }


        public async Task<ActionResult<IEnumerable<VisitPlaceDTO>>> GetVisitPlacesByDestination(int destinationId, string scheme = "https", string host = "example.com", string pathBase = "/basepath")
        {
            
            var visitPlaces = await _context.VisitPlaces
                .Where(vp => vp.DestinationId == destinationId)
                .ToListAsync();

            var visitPlaceDTOs = visitPlaces.Select(vp => new VisitPlaceDTO
            {
                Id = vp.Id,
                Name = vp.Name,
                Description = vp.Description,
                PhotoUrl = string.Format("{0}://{1}{2}/Images/VisitPlace/{3}", scheme, host, pathBase, vp.PhotoUrl),
                DestinationId = vp.DestinationId,
                Price = vp.Price,
                
            }).ToList();

            return visitPlaceDTOs;
        }



        private ActionResult<IEnumerable<VisitPlaceDTO>> StatusCode(int v1, string v2)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> PutVisitPlace(int id, VisitPlaceDTO VisitPlaceDTO)
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

        public async Task<ActionResult<VisitPlaceDTO>> PostVisitPlace([FromForm] VisitPlaceDTO VisitPlaceDTO)
        {
            if (_context.VisitPlaces == null)
            {
                return new ObjectResult("Entity set 'TripsDbContext.VisitPlace' is null.")
                {
                    StatusCode = 500
                };
            }

            if (VisitPlaceDTO.ImageFile == null)
            {
                return new BadRequestObjectResult("The 'ImageFileDTO' field is required.");
            }

            VisitPlaceDTO.PhotoUrl = await _imageService.SaveImage(VisitPlaceDTO.ImageFile, "VisitPlace");

            var visitPlace = new VisitPlace
            {
                Name = VisitPlaceDTO.Name,
                Description = VisitPlaceDTO.Description,
                PhotoUrl = VisitPlaceDTO.PhotoUrl,
                DestinationId = VisitPlaceDTO.DestinationId
            };

            _context.VisitPlaces.Add(visitPlace);
            await _context.SaveChangesAsync();

            return new CreatedAtActionResult("GetVisitPlace", "VisitPlace", new { id = visitPlace.Id }, VisitPlaceDTO);
        }

        public async Task<IActionResult> DeleteVisitPlace(int id)
        {
            if (_context.VisitPlaces == null)
            {
                return new NotFoundResult();
            }

            var visitPlace = await _context.VisitPlaces.FindAsync(id);
            if (visitPlace == null)
            {
                return new NotFoundResult();
            }

            _context.VisitPlaces.Remove(visitPlace);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }

        private bool VisitPlaceExists(int id)
        {
            return (_context.VisitPlaces?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        


    }
}
