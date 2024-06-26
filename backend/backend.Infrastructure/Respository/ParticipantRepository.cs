﻿using backend.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.Infrastructure.Authentication;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using backend.Application.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace backend.Infrastructure.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly TripsDbContext _context;
        private readonly ImageService _imageService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly BaseUrlService _baseUrlService;
        private readonly string _baseUrl;
        private readonly ILogger<ParticipantService> _logger;

        public ParticipantService(TripsDbContext context, ImageService imageService, IWebHostEnvironment hostEnvironment, BaseUrlService baseUrlService, ILogger<ParticipantService> logger)
        {
            _context = context;
            _imageService = imageService;
            _hostEnvironment = hostEnvironment;
            _baseUrlService = baseUrlService;
            _baseUrl = _baseUrlService.GetBaseUrl();
            _logger = logger;
        }

        

        public async Task<ActionResult<IEnumerable<ParticipantDTO>>> GetParticipants(int page = 1, int pageSize = 2)
        {
            if (_context.Participant == null)
            {
                return new NotFoundResult();
            }


            var participants = await _context.Participant
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ParticipantDTO
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Address = x.Address,
                    CreatedAt = x.CreatedAt,
                    DateOfBirth = x.DateOfBirth,
                    Email = x.Email,
                    EmergencyContact = x.EmergencyContact,
                    EmergencyContactPhone = x.EmergencyContactPhone,
                    MedicalConditions = x.MedicalConditions,
                    ModifiedAt = x.ModifiedAt,
                    PhoneNumber = x.PhoneNumber,
                    
                    
                    
                })
                .ToListAsync();

            return participants;
        }


        public async Task<ActionResult<ParticipantDTO>> GetParticipant(Guid id)
        {
            if (_context.Participant == null)
            {
                return new NotFoundResult();
            }

            var participant = await _context.Participant.FindAsync(id);

            if (participant == null)
            {
                return new NotFoundResult();
            }

            var ParticipantResult = new ParticipantDTO
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

            return ParticipantResult;

           
        }

        /*public async Task<List<ParticipantDTO>> GetParticipantsForTrip(int tripId)
        {
            var participants = await _context.Participant
                .Where(x => x.TripId == tripId)
                .Select(x => new ParticipantDTO
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Address = x.Address,
                    CreatedAt = x.CreatedAt,
                    DateOfBirth = x.DateOfBirth,
                    Email = x.Email,
                    EmergencyContact = x.EmergencyContact,
                    EmergencyContactPhone = x.EmergencyContactPhone,
                    MedicalConditions = x.MedicalConditions,
                    ModifiedAt = x.ModifiedAt,
                    PhoneNumber = x.PhoneNumber,
                    TripId = x.TripId,

                })
                .ToListAsync();

            return participants;
        }*/


        public async Task<IActionResult> PutParticipant(Guid id, ParticipantDTO participantDTO)
        {
            if (id != participantDTO.Id)
            {
                return new BadRequestResult();
            }

            var participant = new ParticipantModel
            {
                Id = id,
                FirstName = participantDTO.FirstName,
                LastName = participantDTO.LastName,
            };

            _context.Entry(participant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParticipantExists(id))
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

        public async Task<CreateParticipantDTO> PostParticipant(CreateParticipantDTO participantDTO)
        {
            try
            {
                if (_context.Participant == null)
                {
                    _logger.LogError("Entity set 'TripsDbContext.Participant' is null.");
                    throw new InvalidOperationException("Entity set 'TripsDbContext.Participant' is null.");
                }

                if (participantDTO.ImageFile == null)
                {
                    _logger.LogError("The 'ImageFileDTO' field is required.");
                    throw new ArgumentException("The 'ImageFileDTO' field is required.");
                }

                participantDTO.PhotoUrl = await _imageService.SaveImage(participantDTO.ImageFile, "Participant");

                var currentDate = DateTime.Now.ToUniversalTime();

                var participant = new ParticipantModel
                {
                    Id = participantDTO.Id,
                    FirstName = participantDTO.FirstName,
                    LastName = participantDTO.LastName,
                    Address = participantDTO.Address,
                    CreatedAt = currentDate,
                    DateOfBirth = currentDate, 
                    Email = participantDTO.Email,
                    EmergencyContact = participantDTO.EmergencyContact,
                    EmergencyContactPhone = participantDTO.EmergencyContactPhone,
                    MedicalConditions = participantDTO.MedicalConditions,
                    ModifiedAt = currentDate,
                    PhoneNumber = participantDTO.PhoneNumber,
                    PhotoUrl = participantDTO.PhotoUrl,
                };

                _context.Participant.Add(participant);
                await _context.SaveChangesAsync();

                if (participantDTO.TripId != Guid.Empty)
                {
                    var tripParticipant = new TripParticipantModel
                    {
                        TripId = participantDTO.TripId,
                        ParticipantId = participant.Id
                    };

                    _context.TripParticipant.Add(tripParticipant);
                    await _context.SaveChangesAsync();
                }

                _logger.LogInformation($"Participant '{participant.FirstName} {participant.LastName}' successfully added.");

                return participantDTO; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while posting a new participant.");
                throw; 
            }
        }

        public async Task<IActionResult> DeleteParticipant(Guid id)
        {
            if (_context.Participant == null)
            {
                return new NotFoundResult();
            }

            var participant = await _context.Participant.FindAsync(id);
            if (participant == null)
            {
                return new NotFoundResult();
            }

            _context.Participant.Remove(participant);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }

        private bool ParticipantExists(Guid id)
        {
            return (_context.Participant?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
