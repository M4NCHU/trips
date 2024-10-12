using backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public interface IParticipantRepository : IRepository<ParticipantModel>
    {
        Task<IEnumerable<ParticipantModel>> GetParticipantsAsync(int page, int pageSize);
        Task<bool> ParticipantExistsAsync(Guid id);
    }
}
