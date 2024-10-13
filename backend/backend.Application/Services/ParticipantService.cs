using AutoMapper;
using backend.Application.Services;
using backend.Domain.DTOs;
using backend.Infrastructure.Respository;
using backend.Models;
using Microsoft.AspNetCore.Http;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        private readonly IBaseUrlService _baseUrlService;
        private readonly IMapper _mapper;
        private readonly ILogger<ParticipantService> _logger;
        private readonly string _baseUrl;

        public ParticipantService(
            IUnitOfWork unitOfWork,
            IImageService imageService,
            IBaseUrlService baseUrlService,
            IMapper mapper,
            ILogger<ParticipantService> logger)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
            _baseUrlService = baseUrlService;
            _mapper = mapper;
            _logger = logger;
            _baseUrl = _baseUrlService.GetBaseUrl();
        }

        public async Task<ActionResult<IEnumerable<ParticipantDTO>>> GetParticipants(int page = 1, int pageSize = 2)
        {
            try
            {
                _logger.LogInformation("Fetching participants: Page {Page}, PageSize {PageSize}", page, pageSize);
                var participants = await _unitOfWork.Participants.GetAllAsync();

                if (!participants.Any())
                {
                    _logger.LogWarning("No participants found");
                    return new NotFoundResult();
                }

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
                return new OkObjectResult(participantDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching participants.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ActionResult<ParticipantDTO>> GetParticipant(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching participant with ID {ParticipantId}", id);
                var participant = await _unitOfWork.Participants.GetByIdAsync(id);
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
                return new OkObjectResult(participantDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the participant with ID {ParticipantId}", id);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<IActionResult> PutParticipant(Guid id, ParticipantDTO participantDTO)
        {
            try
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
                    Address = participantDTO.Address,
                    DateOfBirth = participantDTO.DateOfBirth,
                    Email = participantDTO.Email,
                    EmergencyContact = participantDTO.EmergencyContact,
                    EmergencyContactPhone = participantDTO.EmergencyContactPhone,
                    MedicalConditions = participantDTO.MedicalConditions,
                    PhoneNumber = participantDTO.PhoneNumber,
                    ModifiedAt = DateTime.UtcNow
                };

                _logger.LogInformation("Updating participant with ID {ParticipantId}", id);
                await _unitOfWork.Participants.UpdateAsync(participant);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Successfully updated participant with ID {ParticipantId}", id);
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the participant with ID {ParticipantId}", id);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<CreateParticipantDTO> PostParticipant(CreateParticipantDTO participantDTO)
        {
            try
            {
                _logger.LogInformation("Adding new participant: {ParticipantName}", participantDTO.FirstName);

                participantDTO.PhotoUrl = await _imageService.SaveImage(participantDTO.ImageFile, "Participant");

                var participant = new ParticipantModel
                {
                    Id = Guid.NewGuid(),
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

                await _unitOfWork.Participants.AddAsync(participant);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Successfully added participant {ParticipantName}", participantDTO.FirstName);
                return participantDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new participant.");
                throw;
            }
        }

        public async Task<IActionResult> DeleteParticipant(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting participant with ID {ParticipantId}", id);
                var participant = await _unitOfWork.Participants.GetByIdAsync(id);
                if (participant == null)
                {
                    _logger.LogWarning("Participant with ID {ParticipantId} not found", id);
                    return new NotFoundResult();
                }

                await _unitOfWork.Participants.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Successfully deleted participant with ID {ParticipantId}", id);
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the participant with ID {ParticipantId}", id);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
