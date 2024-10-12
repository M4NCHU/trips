using backend.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.Extensions.Logging;
using backend.Infrastructure.Respository;
using AutoMapper;

namespace backend.Application.Services
{
    public class TripParticipantService : ITripParticipantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseUrlService _baseUrlService;
        private readonly IMapper _mapper;
        private readonly ILogger<TripParticipantService> _logger;
        private readonly string _baseUrl;

        public TripParticipantService(
            IUnitOfWork unitOfWork,
            IBaseUrlService baseUrlService,
            IMapper mapper,
            ILogger<TripParticipantService> logger)
        {
            _unitOfWork = unitOfWork;
            _baseUrlService = baseUrlService;
            _mapper = mapper;
            _logger = logger;
            _baseUrl = _baseUrlService.GetBaseUrl();
        }

        public async Task<ActionResult<IEnumerable<TripParticipantDTO>>> GetTripParticipants()
        {
            _logger.LogInformation("Fetching all trip participants.");
            var tripParticipants = await _unitOfWork.TripParticipants.GetAllAsync(); 
            var tripParticipantDTOs = tripParticipants.Select(tp => new TripParticipantDTO
            {
                Id = tp.Id,
                TripId = tp.TripId,
                ParticipantId = tp.ParticipantId
            }).ToList();

            _logger.LogInformation("Fetched {Count} trip participants.", tripParticipantDTOs.Count);
            return tripParticipantDTOs;
        }

        public async Task<ActionResult<IEnumerable<TripParticipantDTO>>> GetTripParticipants(Guid tripId)
        {
            _logger.LogInformation("Fetching trip participants for trip ID {TripId}.", tripId);

            var tripParticipants = await _unitOfWork.TripParticipants.GetTripParticipantsByTripIdAsync(tripId);
            if (!tripParticipants.Any())
            {
                _logger.LogWarning("No participants found for trip ID {TripId}.", tripId);
                return new NotFoundResult();
            }

            var tripParticipantDTOs = tripParticipants.Select(tp => new TripParticipantDTO
            {
                Id = tp.Id,
                TripId = tp.TripId,
                ParticipantId = tp.ParticipantId
            }).ToList();

            _logger.LogInformation("Fetched {Count} trip participants for trip ID {TripId}.", tripParticipantDTOs.Count, tripId);
            return new OkObjectResult(tripParticipantDTOs);
        }

        public async Task<ActionResult<TripParticipantDTO>> GetTripParticipant(Guid id)
        {
            _logger.LogInformation("Fetching trip participant with ID {TripParticipantId}.", id);
            var tripParticipant = await _unitOfWork.TripParticipants.GetByIdAsync(id); 
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

            await _unitOfWork.TripParticipants.UpdateAsync(tripParticipant); 
            await _unitOfWork.SaveChangesAsync(); 

            _logger.LogInformation("Successfully updated trip participant with ID {TripParticipantId}.", id);
            return new NoContentResult();
        }

        public async Task<ActionResult<TripParticipantDTO>> CreateTripParticipant(Guid tripId, Guid participantId)
        {
            _logger.LogInformation("Creating trip participant for trip ID {TripId} and participant ID {ParticipantId}.", tripId, participantId);

            var trip = await _unitOfWork.Trips.GetByIdAsync(tripId); 
            if (trip == null)
            {
                _logger.LogWarning("Trip with ID {TripId} not found.", tripId);
                return new NotFoundObjectResult($"Trip with Id {tripId} not found.");
            }

            var participant = await _unitOfWork.Participants.GetByIdAsync(participantId); 
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

            await _unitOfWork.TripParticipants.AddAsync(tripParticipant); 
            await _unitOfWork.SaveChangesAsync(); 

            var tripParticipantDTO = new TripParticipantDTO
            {
                Id = tripParticipant.Id,
                TripId = tripId,
                ParticipantId = participantId
            };

            _logger.LogInformation("Successfully created trip participant for trip ID {TripId} and participant ID {ParticipantId}.", tripId, participantId);
            return new CreatedAtActionResult("GetTripParticipant", "TripParticipant", new { id = tripParticipant.Id }, tripParticipantDTO);
        }

        public async Task<IActionResult> DeleteTripParticipant(Guid id)
        {
            _logger.LogInformation("Attempting to delete trip participant with ID {TripParticipantId}.", id);


            var tripParticipant = await _unitOfWork.TripParticipants.GetByIdAsync(id);
            if (tripParticipant == null)
            {
                _logger.LogWarning("Trip participant with ID {TripParticipantId} not found.", id);
                return new NotFoundResult(); 
            }

            await _unitOfWork.TripParticipants.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Successfully deleted trip participant with ID {TripParticipantId}.", id);
            return new NoContentResult();
        }



    }
}
