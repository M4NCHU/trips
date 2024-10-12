using backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public interface ITripParticipantRepository : IRepository<TripParticipantModel>
    {
        Task<IEnumerable<TripParticipantModel>> GetAllTripParticipantsAsync();
        Task<TripParticipantModel> GetTripParticipantByIdAsync(Guid id);
        Task<IEnumerable<TripParticipantModel>> GetTripParticipantsByTripIdAsync(Guid tripId);
        Task<bool> TripParticipantExistsAsync(Guid id);
        Task<bool> UpdateTripParticipantAsync(TripParticipantModel tripParticipant);
        Task<bool> DeleteTripParticipantAsync(Guid id);
    }
}
