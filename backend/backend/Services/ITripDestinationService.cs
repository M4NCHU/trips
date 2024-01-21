using backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Services
{
    public interface ITripDestinationService
    {
        Task<ActionResult<IEnumerable<TripDestinationDTO>>> GetTripDestinations();

        Task<ActionResult<TripDestinationDTO>> GetTripDestination(Guid id);

        /*Task<List<TripDestinationDTO>> GetTripDestinationsForTrip(int tripId);*/


        Task<IActionResult> PutTripDestination(Guid id, TripDestinationDTO tripDestination);

        Task<ActionResult<IEnumerable<TripDestinationDTO>>> GetTripDestinations(Guid TripId);

        Task<ActionResult<TripDestinationDTO>> PostTripDestination([FromForm] TripDestinationDTO tripDestination);

        Task<int> CountTripDestinations(Guid tripId);

        Task<IActionResult> DeleteTripDestination(Guid id);
    }
}
