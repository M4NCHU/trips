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
    public class TripParticipantController : ControllerBase
    {
        private readonly ITripParticipantService _tripParticipantService;

        public TripParticipantController(ITripParticipantService tripParticipantService)
        {
            _tripParticipantService = tripParticipantService;
        }

        

        // GET: api/TripParticipant
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TripParticipantDTO>>> GetTripParticipant(int page = 1, int pageSize = 2)
        {
            var tripParticipants = await _tripParticipantService.GetTripParticipants();
            return tripParticipants;
        }

        // GET: api/TripParticipant/GetTripParticipants/1
        [HttpGet("trip/{tripId}")]
        public async Task<ActionResult<IEnumerable<TripParticipantDTO>>> GetTripParticipants(Guid tripId)
        {
            try
            {
                var tripParticipants = await _tripParticipantService.GetTripParticipants(tripId);
                return tripParticipants;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // GET: api/TripParticipant/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TripParticipantDTO>> GetTripParticipant(Guid id)
        {
            return await _tripParticipantService.GetTripParticipant(id);
        }

        // PUT: api/TripParticipant/5
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTripParticipant(Guid id, TripParticipantDTO tripParticipant)
        {
            return await _tripParticipantService.PutTripParticipant(id, tripParticipant);
        }

        // POST: api/TripParticipant
        
        [HttpPost]
        public async Task<ActionResult<TripParticipantDTO>> PostTripParticipant(Guid tripId, Guid participantId)
        {
            return await _tripParticipantService.CreateTripParticipant(tripId, participantId);
        }

        // DELETE: api/TripParticipant/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTripParticipant(Guid id)
        {
            return await _tripParticipantService.DeleteTripParticipant(id);
        }
    }
}
