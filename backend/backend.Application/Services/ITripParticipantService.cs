using backend.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Application.Services
{
    public interface ITripParticipantService
    {
        Task<ActionResult<IEnumerable<TripParticipantDTO>>> GetTripParticipants();

        Task<ActionResult<TripParticipantDTO>> GetTripParticipant(Guid id);

        /*Task<List<TripParticipantDTO>> GetTripParticipantsForTrip(int tripId);*/


        Task<IActionResult> PutTripParticipant(Guid id, TripParticipantDTO tripParticipant);

        Task<ActionResult<IEnumerable<TripParticipantDTO>>> GetTripParticipants(Guid TripId);

        Task<ActionResult<TripParticipantDTO>> CreateTripParticipant(Guid tripId, Guid participantId);

        Task<IActionResult> DeleteTripParticipant(Guid id);
    }
}
