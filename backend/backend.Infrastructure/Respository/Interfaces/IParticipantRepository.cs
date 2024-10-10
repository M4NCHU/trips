using backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public interface IParticipantRepository
    {
        Task<IEnumerable<ParticipantModel>> GetParticipantsAsync(int page, int pageSize);
        Task<ParticipantModel> GetParticipantByIdAsync(Guid id);
        Task AddParticipantAsync(ParticipantModel participant);
        Task UpdateParticipantAsync(ParticipantModel participant);
        Task DeleteParticipantAsync(Guid id);
        Task<bool> ParticipantExistsAsync(Guid id);
    }
}
