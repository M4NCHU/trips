using backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public interface ITripParticipantRepository
    {
        Task<IEnumerable<TripParticipantModel>> GetAllTripParticipantsAsync();
        Task<TripParticipantModel> GetTripParticipantByIdAsync(Guid id);
        Task<IEnumerable<TripParticipantModel>> GetTripParticipantsByTripIdAsync(Guid tripId);
        Task AddTripParticipantAsync(TripParticipantModel tripParticipant);
        Task UpdateTripParticipantAsync(TripParticipantModel tripParticipant);
        Task DeleteTripParticipantAsync(Guid id);
        Task<bool> TripParticipantExistsAsync(Guid id);
    }
}
