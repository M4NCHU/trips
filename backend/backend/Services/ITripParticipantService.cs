using backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Services
{
    public interface ITripParticipantService
    {
        Task<ActionResult<IEnumerable<TripParticipantDTO>>> GetTripParticipants();

        Task<ActionResult<TripParticipantDTO>> GetTripParticipant(int id);

        /*Task<List<TripParticipantDTO>> GetTripParticipantsForTrip(int tripId);*/


        Task<IActionResult> PutTripParticipant(int id, TripParticipantDTO tripParticipant);

        Task<ActionResult<IEnumerable<TripParticipantDTO>>> GetTripParticipants(int TripId);

        Task<ActionResult<TripParticipantDTO>> PostTripParticipant([FromForm] TripParticipantDTO tripParticipant);

        Task<IActionResult> DeleteTripParticipant(int id);
    }
}
