using backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Services
{
    public interface ITripService
    {
        Task<ActionResult<IEnumerable<TripDTO>>> GetTrips();

        Task<ActionResult<TripDTO>> GetTrip(int id);
        Task<ActionResult<TripDTO>> GetTripById(int id);

        Task<List<VisitPlaceDTO>> GetVisitPlacesForTrip(int tripId);


        Task<IActionResult> PutTrip(int id, TripDTO trip);

        Task<ActionResult<TripDTO>> PostTrip([FromForm] TripDTO trip);

        Task<IActionResult> DeleteTrip(int id);
    }
}
