using backend.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.Infrastructure.Authentication;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using backend.Application.Services;




namespace backend.Infrastructure.Services
{
    public class TripParticipantService : ITripParticipantService
    {
        private readonly TripsDbContext _context;
        private readonly ImageService _imageService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly BaseUrlService _baseUrlService;
        private readonly string _baseUrl;
        private readonly ILogger<TripParticipantService> _logger;


        public TripParticipantService(TripsDbContext context, ImageService imageService, IWebHostEnvironment hostEnvironment, BaseUrlService baseUrlService, ILogger<TripParticipantService> logger)
        {
            _context = context;
            _imageService = imageService;
            _hostEnvironment = hostEnvironment;
            _baseUrlService = baseUrlService;
            _baseUrl = _baseUrlService.GetBaseUrl();
            _logger = logger;
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


        public async Task<ActionResult<TripParticipantDTO>> GetTripParticipant(Guid id)
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

        public async Task<ActionResult<IEnumerable<TripParticipantDTO>>> GetTripParticipants(Guid tripId)
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




        public async Task<IActionResult> PutTripParticipant(Guid id, TripParticipantDTO tripParticipantDTO)
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

        public async Task<ActionResult<TripParticipantDTO>> CreateTripParticipant(Guid tripId, Guid participantId)
        {

            _logger.LogInformation($"Creating TripParticipant - TripId: {tripId}, ParticipantId: {participantId}");

            // Sprawdź, czy istnieje trip
            var trip = await _context.Trip.FindAsync(tripId);
            if (trip == null)
            {
                return new NotFoundObjectResult($"Trip with Id {tripId} not found.");
            }

            // Sprawdź, czy istnieje participant
            var participant = await _context.Participant.FindAsync(participantId);
            if (participant == null)
            {
                return new NotFoundObjectResult($"Participant with Id {participantId} not found.");
            }

            var tripParticipant = new TripParticipantModel
            {
                TripId = tripId,
                ParticipantId = participantId,
                // Uzupełnij pozostałe pola
            };

            _context.TripParticipant.Add(tripParticipant);
            await _context.SaveChangesAsync();

            var tripParticipantDTO = new TripParticipantDTO
            {
                Id = tripParticipant.Id, // Uzyskany id po zapisie
                                         // Uzupełnij pozostałe dane DTO
            };

            return new CreatedAtActionResult("GetTripParticipant", "TripParticipant", new { id = tripParticipant.Id }, tripParticipantDTO);
        }


        public async Task<IActionResult> DeleteTripParticipant(Guid id)
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

        private bool TripParticipantExists(Guid id)
        {
            return (_context.TripParticipant?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
