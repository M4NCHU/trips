using backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public interface IAccommodationRepository : IRepository<AccommodationModel>
    {
        Task<IEnumerable<AccommodationModel>> GetAccommodations(int page, int pageSize);
        Task<bool> AccommodationExists(Guid id);
    }
}
