using backend.Domain.Filters;
using backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public interface IDestinationRepository
    {
        Task<IEnumerable<DestinationModel>> GetDestinationsAsync(DestinationFilter filter, int page, int pageSize);
        Task<DestinationModel> GetDestinationByIdAsync(Guid id);
        Task<IEnumerable<DestinationModel>> SearchDestinationsAsync(string searchTerm);
        Task<List<DestinationModel>> GetDestinationsForTripAsync(Guid tripId);
        Task AddDestinationAsync(DestinationModel destination);
        Task UpdateDestinationAsync(DestinationModel destination);
        Task DeleteDestinationAsync(Guid id);
        Task<bool> DestinationExistsAsync(Guid id);
    }
}
