using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using Microsoft.Extensions.Hosting;
using backend.Domain.DTOs;
using backend.Application.Services;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantController : ControllerBase
    {
        private readonly ParticipantService _participantService;

        public ParticipantController(ParticipantService participantService)
        {
            _participantService = participantService;
        }

        

        // GET: api/Participant
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParticipantDTO>>> GetParticipant(int page = 1, int pageSize = 2)
        {
            var participants = await _participantService.GetParticipants();
            return participants;
        }

        /*// GET: api/Participant/GetParticipantForTrip/1
        [HttpGet("GetParticipantForTrip/{tripId}")]
        public async Task<ActionResult<IEnumerable<ParticipantDTO>>> GetParticipantForTrip(int tripId)
        {
            try
            {
                var participantsForTrip = await _participantService.GetParticipantForTrip(tripId);
                return participantsForTrip;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }*/

        // GET: api/Participant/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ParticipantDTO>> GetParticipant(Guid id)
        {
            return await _participantService.GetParticipant(id);
        }

        // PUT: api/Participant/5
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParticipant(Guid id, ParticipantDTO participant)
        {
            return await _participantService.PutParticipant(id, participant);
        }

        // POST: api/Participant
        
        [HttpPost]
        public async Task<CreateParticipantDTO> PostParticipant(CreateParticipantDTO participantDTO)
        {
            return await _participantService.PostParticipant(participantDTO);
        }

        // DELETE: api/Participant/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParticipant(Guid id)
        {
            return await _participantService.DeleteParticipant(id);
        }
    }
}
