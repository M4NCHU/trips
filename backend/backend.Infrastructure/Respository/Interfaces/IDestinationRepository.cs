using backend.Domain.Filters;
using backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public interface IDestinationRepository : IRepository<DestinationModel>
    {
        Task<IEnumerable<DestinationModel>> GetDestinationsAsync(DestinationFilter filter, int page, int pageSize);
        Task<IEnumerable<DestinationModel>> SearchDestinationsAsync(string searchTerm);
        Task<List<DestinationModel>> GetDestinationsForTripAsync(Guid tripId);
        Task<bool> DestinationExistsAsync(Guid id);
    }
}
