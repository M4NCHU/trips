using backend.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using backend.Infrastructure.Authentication;

namespace backend.Application.Services
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
            _logger.LogInformation("Fetching all trip participants.");
            if (_context.TripParticipant == null)
            {
                _logger.LogWarning("TripParticipant entity set is null.");
                return new NotFoundResult();
            }

            var tripParticipants = await _context.TripParticipant
                .OrderBy(x => x.Id)
                .Select(x => new TripParticipantDTO
                {
                    Id = x.Id,
                    TripId = x.TripId,
                    ParticipantId = x.ParticipantId
                })
                .ToListAsync();

            _logger.LogInformation("Fetched {Count} trip participants.", tripParticipants.Count);
            return tripParticipants;
        }

        public async Task<ActionResult<TripParticipantDTO>> GetTripParticipant(Guid id)
        {
            _logger.LogInformation("Fetching trip participant with ID {TripParticipantId}.", id);
            if (_context.TripParticipant == null)
            {
                _logger.LogWarning("TripParticipant entity set is null.");
                return new NotFoundResult();
            }

            var tripParticipant = await _context.TripParticipant.FindAsync(id);
            if (tripParticipant == null)
            {
                _logger.LogWarning("Trip participant with ID {TripParticipantId} not found.", id);
                return new NotFoundResult();
            }

            var tripParticipantDTO = new TripParticipantDTO
            {
                Id = tripParticipant.Id,
                TripId = tripParticipant.TripId,
                ParticipantId = tripParticipant.ParticipantId
            };

            _logger.LogInformation("Fetched trip participant with ID {TripParticipantId}.", id);
            return tripParticipantDTO;
        }

        public async Task<ActionResult<IEnumerable<TripParticipantDTO>>> GetTripParticipants(Guid tripId)
        {
            _logger.LogInformation("Fetching trip participants for trip ID {TripId}.", tripId);
            if (_context.TripParticipant == null)
            {
                _logger.LogWarning("TripParticipant entity set is null.");
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
                            PhotoUrl = $"{_baseUrl}/Images/Participant/{p.PhotoUrl}"
                        })
                        .FirstOrDefault()
                })
                .ToListAsync();

            if (!tripParticipants.Any())
            {
                _logger.LogWarning("No participants found for trip ID {TripId}.", tripId);
                return new NotFoundResult();
            }

            _logger.LogInformation("Fetched {Count} trip participants for trip ID {TripId}.", tripParticipants.Count, tripId);
            return tripParticipants;
        }

        public async Task<IActionResult> PutTripParticipant(Guid id, TripParticipantDTO tripParticipantDTO)
        {
            _logger.LogInformation("Updating trip participant with ID {TripParticipantId}.", id);
            if (id != tripParticipantDTO.Id)
            {
                _logger.LogWarning("Mismatched IDs for updating trip participant. Expected {TripParticipantId}, got {InputId}.", id, tripParticipantDTO.Id);
                return new BadRequestResult();
            }

            var tripParticipant = new TripParticipantModel
            {
                Id = id,
                TripId = tripParticipantDTO.TripId,
                ParticipantId = tripParticipantDTO.ParticipantId
            };

            _context.Entry(tripParticipant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully updated trip participant with ID {TripParticipantId}.", id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripParticipantExists(id))
                {
                    _logger.LogWarning("Trip participant with ID {TripParticipantId} not found during update.", id);
                    return new NotFoundResult();
                }
                throw;
            }

            return new NoContentResult();
        }

        public async Task<ActionResult<TripParticipantDTO>> CreateTripParticipant(Guid tripId, Guid participantId)
        {
            _logger.LogInformation("Creating trip participant for trip ID {TripId} and participant ID {ParticipantId}.", tripId, participantId);

            var trip = await _context.Trip.FindAsync(tripId);
            if (trip == null)
            {
                _logger.LogWarning("Trip with ID {TripId} not found.", tripId);
                return new NotFoundObjectResult($"Trip with Id {tripId} not found.");
            }

            var participant = await _context.Participant.FindAsync(participantId);
            if (participant == null)
            {
                _logger.LogWarning("Participant with ID {ParticipantId} not found.", participantId);
                return new NotFoundObjectResult($"Participant with Id {participantId} not found.");
            }

            var tripParticipant = new TripParticipantModel
            {
                TripId = tripId,
                ParticipantId = participantId
            };

            _context.TripParticipant.Add(tripParticipant);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully created trip participant for trip ID {TripId} and participant ID {ParticipantId}.", tripId, participantId);

            var tripParticipantDTO = new TripParticipantDTO
            {
                Id = tripParticipant.Id,
                TripId = tripId,
                ParticipantId = participantId
            };

            return new CreatedAtActionResult("GetTripParticipant", "TripParticipant", new { id = tripParticipant.Id }, tripParticipantDTO);
        }

        public async Task<IActionResult> DeleteTripParticipant(Guid id)
        {
            _logger.LogInformation("Deleting trip participant with ID {TripParticipantId}.", id);
            if (_context.TripParticipant == null)
            {
                _logger.LogWarning("TripParticipant entity set is null.");
                return new NotFoundResult();
            }

            var tripParticipant = await _context.TripParticipant.FindAsync(id);
            if (tripParticipant == null)
            {
                _logger.LogWarning("Trip participant with ID {TripParticipantId} not found.", id);
                return new NotFoundResult();
            }

            _context.TripParticipant.Remove(tripParticipant);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully deleted trip participant with ID {TripParticipantId}.", id);
            return new NoContentResult();
        }

        private bool TripParticipantExists(Guid id)
        {
            _logger.LogInformation("Checking if trip participant with ID {TripParticipantId} exists.", id);
            return _context.TripParticipant?.Any(e => e.Id == id) ?? false;
        }
    }
}
