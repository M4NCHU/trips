using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using Microsoft.Extensions.Hosting;
using backend.Application.Services;
using backend.Domain.DTOs;
using backend.Domain.Enums;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly TripService _tripService;

        public TripController(TripService tripService)
        {
            _tripService = tripService;
        }

        // GET: api/Trip
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<TripDTO>>> GetTrip()
        {
            var trip = await _tripService.GetTrips();
            return trip;
        }


        // GET: api/Trip/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TripDTO>> GetTripById(Guid id)
        {
            var trip = await _tripService.GetTripById(id);

            

            return trip;
        }

        // GET: api/Trip/UserId/5
        [HttpGet("UserId/{userId}")]
        public async Task<ActionResult<IEnumerable<TripDTO>>> GetUserTripsList(string userId)
        {
            var trip = await _tripService.GetUserTripsList(userId);

            return trip;
        }

        // GET: api/count/userId/5
        [HttpGet("count/userId/{userId}/status/{status}")]
        public async Task<ActionResult<int>> CountUserTrips(string userId, TripStatus status)
        {
            var trip = await _tripService.CountUserTrips(userId, status);

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


        // PATCH: api/Trip/{tripId}/Title/{newTitle}

        [HttpPatch("{tripId}/Title/{newTitle}")]
        public async Task<IActionResult> ChangeTripTitle(Guid tripId, string newTitle)
        {
            return await _tripService.ChangeTripTitle(tripId, newTitle);
        }

        [HttpGet("ensure/userId/{userId}")]
        public async Task<(Guid, bool)> EnsureActiveTripExists(string userId)
        {
            return await _tripService.EnsureActiveTripExists(userId);
        }

        // POST: api/Trip

        [HttpPost()]
        public async Task<CreateTripDTO> PostTrip([FromForm] CreateTripDTO tripDTO)
        {
            return await _tripService.PostTrip(tripDTO);
        }

        // DELETE: api/Trip/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrip(Guid id)
        {
            return await _tripService.DeleteTrip(id);
        }
    }
}
