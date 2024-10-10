using backend.Application.Services;
using backend.Domain.DTOs;
using backend.Infrastructure.Respository;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Application.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly ImageService _imageService;
        private readonly BaseUrlService _baseUrlService;
        private readonly ILogger<ParticipantService> _logger;
        private readonly string _baseUrl;

        public ParticipantService(
            IParticipantRepository participantRepository,
            ImageService imageService,
            BaseUrlService baseUrlService,
            ILogger<ParticipantService> logger)
        {
            _participantRepository = participantRepository;
            _imageService = imageService;
            _baseUrlService = baseUrlService;
            _logger = logger;
            _baseUrl = _baseUrlService.GetBaseUrl();
        }

        public async Task<ActionResult<IEnumerable<ParticipantDTO>>> GetParticipants(int page = 1, int pageSize = 2)
        {
            _logger.LogInformation("Fetching participants: Page {Page}, PageSize {PageSize}", page, pageSize);
            var participants = await _participantRepository.GetParticipantsAsync(page, pageSize);
            var participantDTOs = participants.Select(p => new ParticipantDTO
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Address = p.Address,
                CreatedAt = p.CreatedAt,
                DateOfBirth = p.DateOfBirth,
                Email = p.Email,
                EmergencyContact = p.EmergencyContact,
                EmergencyContactPhone = p.EmergencyContactPhone,
                MedicalConditions = p.MedicalConditions,
                ModifiedAt = p.ModifiedAt,
                PhoneNumber = p.PhoneNumber,
            }).ToList();

            _logger.LogInformation("Fetched {Count} participants", participantDTOs.Count);
            return participantDTOs;
        }

        public async Task<ActionResult<ParticipantDTO>> GetParticipant(Guid id)
        {
            _logger.LogInformation("Fetching participant with ID {ParticipantId}", id);
            var participant = await _participantRepository.GetParticipantByIdAsync(id);
            if (participant == null)
            {
                _logger.LogWarning("Participant with ID {ParticipantId} not found", id);
                return new NotFoundResult();
            }

            var participantDTO = new ParticipantDTO
            {
                Id = participant.Id,
                FirstName = participant.FirstName,
                LastName = participant.LastName,
                Address = participant.Address,
                CreatedAt = participant.CreatedAt,
                DateOfBirth = participant.DateOfBirth,
                Email = participant.Email,
                EmergencyContact = participant.EmergencyContact,
                EmergencyContactPhone = participant.EmergencyContactPhone,
                MedicalConditions = participant.MedicalConditions,
                ModifiedAt = participant.ModifiedAt,
                PhoneNumber = participant.PhoneNumber,
            };

            _logger.LogInformation("Fetched participant with ID {ParticipantId}", id);
            return participantDTO;
        }

        public async Task<IActionResult> PutParticipant(Guid id, ParticipantDTO participantDTO)
        {
            if (id != participantDTO.Id)
            {
                _logger.LogWarning("Attempted to update participant with mismatched ID {ParticipantId}", id);
                return new BadRequestResult();
            }

            var participant = new ParticipantModel
            {
                Id = id,
                FirstName = participantDTO.FirstName,
                LastName = participantDTO.LastName,
            };

            _logger.LogInformation("Updating participant with ID {ParticipantId}", id);
            await _participantRepository.UpdateParticipantAsync(participant);

            _logger.LogInformation("Successfully updated participant with ID {ParticipantId}", id);
            return new NoContentResult();
        }

        public async Task<CreateParticipantDTO> PostParticipant(CreateParticipantDTO participantDTO)
        {
            _logger.LogInformation("Adding new participant: {ParticipantName}", participantDTO.FirstName);

            participantDTO.PhotoUrl = await _imageService.SaveImage(participantDTO.ImageFile, "Participant");

            var participant = new ParticipantModel
            {
                Id = participantDTO.Id,
                FirstName = participantDTO.FirstName,
                LastName = participantDTO.LastName,
                Address = participantDTO.Address,
                CreatedAt = DateTime.UtcNow,
                DateOfBirth = participantDTO.DateOfBirth,
                Email = participantDTO.Email,
                EmergencyContact = participantDTO.EmergencyContact,
                EmergencyContactPhone = participantDTO.EmergencyContactPhone,
                MedicalConditions = participantDTO.MedicalConditions,
                PhoneNumber = participantDTO.PhoneNumber,
                PhotoUrl = participantDTO.PhotoUrl,
            };

            await _participantRepository.AddParticipantAsync(participant);
            _logger.LogInformation("Successfully added participant {ParticipantName}", participantDTO.FirstName);

            return participantDTO;
        }

        public async Task<IActionResult> DeleteParticipant(Guid id)
        {
            _logger.LogInformation("Deleting participant with ID {ParticipantId}", id);
            var participant = await _participantRepository.GetParticipantByIdAsync(id);
            if (participant == null)
            {
                _logger.LogWarning("Participant with ID {ParticipantId} not found", id);
                return new NotFoundResult();
            }

            await _participantRepository.DeleteParticipantAsync(id);
            _logger.LogInformation("Successfully deleted participant with ID {ParticipantId}", id);
            return new NoContentResult();
        }
    }
}
