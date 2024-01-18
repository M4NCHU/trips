using System.Collections.Generic;
using System.Threading.Tasks;
using backend.DTOs;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using Microsoft.Extensions.Hosting;

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

        /*// GET: api/TripParticipant/GetTripParticipantForTrip/1
        [HttpGet("GetTripParticipantForTrip/{tripId}")]
        public async Task<ActionResult<IEnumerable<TripParticipantDTO>>> GetTripParticipantForTrip(int tripId)
        {
            try
            {
                var tripParticipantsForTrip = await _tripParticipantService.GetTripParticipantForTrip(tripId);
                return tripParticipantsForTrip;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }*/

        // GET: api/TripParticipant/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TripParticipantDTO>> GetTripParticipant(int id)
        {
            return await _tripParticipantService.GetTripParticipant(id);
        }

        // PUT: api/TripParticipant/5
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTripParticipant(int id, TripParticipantDTO tripParticipant)
        {
            return await _tripParticipantService.PutTripParticipant(id, tripParticipant);
        }

        // POST: api/TripParticipant
        
        [HttpPost]
        public async Task<ActionResult<TripParticipantDTO>> PostTripParticipant([FromForm] TripParticipantDTO tripParticipant)
        {
            return await _tripParticipantService.PostTripParticipant(tripParticipant);
        }

        // DELETE: api/TripParticipant/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTripParticipant(int id)
        {
            return await _tripParticipantService.DeleteTripParticipant(id);
        }
    }
}
