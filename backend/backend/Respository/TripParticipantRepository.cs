using backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.Authentication;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace backend.Services
{
    public class TripParticipantService : ITripParticipantService
    {
        private readonly TripsDbContext _context;
        private readonly ImageService _imageService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly BaseUrlService _baseUrlService;
        private readonly string _baseUrl;

        public TripParticipantService(TripsDbContext context, ImageService imageService, IWebHostEnvironment hostEnvironment, BaseUrlService baseUrlService)
        {
            _context = context;
            _imageService = imageService;
            _hostEnvironment = hostEnvironment;
            _baseUrlService = baseUrlService;
            _baseUrl = _baseUrlService.GetBaseUrl();
        }

        

        public async Task<ActionResult<IEnumerable<TripParticipantDTO>>> GetTripParticipants()
        {
            if (_context.TripParticipant == null)
            {
                return new NotFoundResult();
            }


            var tripParticipants = await _context.TripParticipant
                .OrderBy(x => x.Id)
                .Select(x => new TripParticipantDTO
                {
                    Id = x.Id,
                    
                })
                .ToListAsync();

            return tripParticipants;
        }


        public async Task<ActionResult<TripParticipantDTO>> GetTripParticipant(int id)
        {
            if (_context.TripParticipant == null)
            {
                return new NotFoundResult();
            }

            var tripParticipant = await _context.TripParticipant.FindAsync(id);

            if (tripParticipant == null)
            {
                return new NotFoundResult();
            }

            var TripParticipantResult = new TripParticipantDTO
            {
                Id = tripParticipant.Id,
                
            };

            return TripParticipantResult;

           
        }

        public async Task<ActionResult<IEnumerable<TripParticipantDTO>>> GetTripParticipants(int tripId)
        {
            if (_context.TripParticipant == null)
            {
                return new NotFoundResult();
            }

            var tripParticipants = await _context.TripParticipant
        .Where(tp => tp.TripId == tripId)
        .Select(tp => new TripParticipantDTO
        {
            Id = tp.Id,
            TripId = tp.TripId,
            ParticipantId = tp.ParticipantId,
            Participants = _context.Participant
                .Where(p => p.Id == tp.ParticipantId)
                .Select(p => new ParticipantDTO
                {
                    
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    PhotoUrl = $"{_baseUrl}/Images/Participant/{p.PhotoUrl}",

                })
                .FirstOrDefault()
        })
        .ToListAsync();

            if (tripParticipants == null)
            {
                return new NotFoundResult();
            }

            

            return tripParticipants;


        }




        public async Task<IActionResult> PutTripParticipant(int id, TripParticipantDTO tripParticipantDTO)
        {
            if (id != tripParticipantDTO.Id)
            {
                return new BadRequestResult();
            }

            var tripParticipant = new TripParticipantModel
            {
                Id = id,
                
            };

            _context.Entry(tripParticipant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripParticipantExists(id))
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

        public async Task<ActionResult<TripParticipantDTO>> PostTripParticipant([FromForm] TripParticipantDTO tripParticipantDTO)
        {
            if (_context.TripParticipant == null)
            {
                return new ObjectResult("Entity set 'TripsDbContext.TripParticipant' is null.")
                {
                    StatusCode = 500
                };
            }

            
           

            var tripParticipant = new TripParticipantModel
            {
                Id = tripParticipantDTO.Id,
                
            };

            

            _context.TripParticipant.Add(tripParticipant);
            await _context.SaveChangesAsync();

            return new CreatedAtActionResult("GetTripParticipant", "TripParticipant", new { id = tripParticipant.Id }, tripParticipantDTO);
        }

        public async Task<IActionResult> DeleteTripParticipant(int id)
        {
            if (_context.TripParticipant == null)
            {
                return new NotFoundResult();
            }

            var tripParticipant = await _context.TripParticipant.FindAsync(id);
            if (tripParticipant == null)
            {
                return new NotFoundResult();
            }

            _context.TripParticipant.Remove(tripParticipant);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }

        private bool TripParticipantExists(int id)
        {
            return (_context.TripParticipant?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
