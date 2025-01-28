using backend.Domain.DTOs;
using backend.Domain.Filters;
using backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public interface IDestinationRepository : IRepository<DestinationModel>
    {
        Task<PagedResult<DestinationModel>> GetDestinationsAsync(DestinationFilter filter, int page, int pageSize);
        Task<IEnumerable<DestinationModel>> SearchDestinationsAsync(string searchTerm);
        Task<List<DestinationModel>> GetDestinationsForTripAsync(Guid tripId);
        Task<List<DestinationModel>> GetDestinationsByIdsAsync(List<Guid> ids);

        Task<bool> DestinationExistsAsync(Guid id);
        Task<List<DestinationModel>> GetByIdsAsync(List<Guid> ids);
        Task<DestinationModel?> GetWithDetailsAsync(Guid id);
    }
}
