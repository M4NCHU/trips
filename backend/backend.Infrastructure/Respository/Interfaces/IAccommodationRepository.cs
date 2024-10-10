using backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public interface IAccommodationRepository
    {
        Task<IEnumerable<AccommodationModel>> GetAccommodations(int page, int pageSize);
        Task<AccommodationModel> GetAccommodationById(Guid id);
        Task AddAccommodation(AccommodationModel accommodation);
        Task UpdateAccommodation(AccommodationModel accommodation);
        Task DeleteAccommodation(Guid id);
        Task<bool> AccommodationExists(Guid id);
    }
}
