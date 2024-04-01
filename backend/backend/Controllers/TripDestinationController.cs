using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using Microsoft.Extensions.Hosting;
using backend.Application.Services;
using backend.Domain.DTOs;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripDestinationController : ControllerBase
    {
        private readonly ITripDestinationService _tripDestinationService;

        public TripDestinationController(ITripDestinationService tripDestinationService)
        {
            _tripDestinationService = tripDestinationService;
        }

        

        // GET: api/TripDestination
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TripDestinationDTO>>> GetTripDestination(int page = 1, int pageSize = 2)
        {
            var tripDestinations = await _tripDestinationService.GetTripDestinations();
            return tripDestinations;
        }

        // GET: api/TripDestination/trip/1
        [HttpGet("trip/{tripId}")]
        public async Task<ActionResult<IEnumerable<TripDestinationDTO>>> GetTripDestinations(Guid tripId)
        {
            try
            {
                var tripDestinations = await _tripDestinationService.GetTripDestinations(tripId);
                return tripDestinations;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // GET: api/TripDestination/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TripDestinationDTO>> GetTripDestination(Guid id)
        {
            return await _tripDestinationService.GetTripDestination(id);
        }

        // PUT: api/TripDestination/5
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTripDestination(Guid id, TripDestinationDTO tripDestination)
        {
            return await _tripDestinationService.PutTripDestination(id, tripDestination);
        }

        // POST: api/TripDestination

       /* [HttpPost]
        public async Task<ActionResult<TripDestinationDTO>> PostTripDestination([FromForm] TripDestinationDTO tripDestination)
        {
            return await _tripDestinationService.PostTripDestination(tripDestination);
        }*/

        [HttpPost]
        public async Task<TripDestinationDTO> PostTripDestinationAsync([FromForm] TripDestinationDTO tripDestinationDTO)
        {

             return await _tripDestinationService.PostTripDestinationAsync(tripDestinationDTO);

               
            
           
        }

        // GET: api/TripDestination/trip/5
        [HttpGet("trip-destination-count/{tripId}")]
        public async Task<int> CountTripDestinations(Guid tripId)
        {
            return await _tripDestinationService.CountTripDestinations(tripId);
        }

        // DELETE: api/TripDestination/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTripDestination(Guid id)
        {
            return await _tripDestinationService.DeleteTripDestination(id);
        }
    }
}
