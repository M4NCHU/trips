using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using Microsoft.Extensions.Hosting;
using backend.Application.Services;
using backend.Domain.DTOs;
using backend.Domain.Filters;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinationController : ControllerBase
    {
        private readonly IDestinationService _destinationService;

        public DestinationController(IDestinationService destinationService)
        {
            _destinationService = destinationService;
        }

        

        // GET: api/Destinations
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<ResponseDestinationDTO>>> GetDestinations([FromQuery] DestinationFilter filter, int page = 1, int pageSize = 2)
        {
            var destinations = await _destinationService.GetDestinations(filter, page, pageSize);
            return destinations;
        }

        // GET: api/Destinations/1
        [HttpGet("trip/{tripId}")]
        public async Task<ActionResult<IEnumerable<DestinationDTO>>> GetDestinationsForTrip(Guid tripId)
        {
            try
            {
                var destinationsForTrip = await _destinationService.GetDestinationsForTrip(tripId);
                return destinationsForTrip;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // GET: api/Destinations/5
        [HttpGet("{destinationId}")]
        public async Task<ActionResult<DestinationDTO>> GetDestination(Guid destinationId)
        {
            return await _destinationService.GetDestination(destinationId);
        }

        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<DestinationDTO>>> Search(string searchTerm)
        {
            var results = await _destinationService.SearchDestinations(searchTerm);

            if (results == null || !results.Any())
            {
                return NotFound("No destinations found matching the search term.");
            }

            return Ok(results);
        }


        // PUT: api/Destinations/5

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDestination(Guid id, CreateDestinationDTO destination)
        {
            return await _destinationService.PutDestination(id, destination);
        }

        // POST: api/Destinations
        
        [HttpPost()]
        public async Task<IActionResult> PostDestination(CreateDestinationDTO destinationDTO)
        {
            return await _destinationService.PostDestination(destinationDTO);
        }

        // DELETE: api/Destinations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDestination(Guid id)
        {
            return await _destinationService.DeleteDestination(id);
        }
    }
}
