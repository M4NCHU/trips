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
    public class TripController : ControllerBase
    {
        private readonly ITripService _tripService;

        public TripController(ITripService tripService)
        {
            _tripService = tripService;
        }

        // GET: api/Trip/GetAllTrips
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<TripDTO>>> GetTrip()
        {
            var trip = await _tripService.GetTrips();
            return trip;
        }


        // GET: api/Trip/GetTripById/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TripDTO>> GetTripById(Guid id)
        {
            var trip = await _tripService.GetTripById(id);

            

            return trip;
        }

       
        

        // GET: api/Trip/5/VisitPlaces
        [HttpGet("{id}/VisitPlaces")]
        public async Task<ActionResult<List<VisitPlaceDTO>>> GetSelectedPlacesForTrip(Guid id)
        {
            var visitPlaces = await _tripService.GetVisitPlacesForTrip(id);

            if (visitPlaces == null || visitPlaces.Count == 0)
            {
                return NotFound("No visit places found for the specified trip.");
            }

            return visitPlaces;
        }

        // PUT: api/Trip/5

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrip(Guid id, TripDTO trip)
        {
            return await _tripService.PutTrip(id, trip);
        }

        // POST: api/Trip
        
        [HttpPost()]
        public async Task<ActionResult<TripDTO>> PostTrip([FromForm] TripDTO trip)
        {
            return await _tripService.PostTrip(trip);
        }

        // DELETE: api/Trip/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrip(Guid id)
        {
            return await _tripService.DeleteTrip(id);
        }
    }
}
