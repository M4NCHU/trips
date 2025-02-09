using backend.Domain.DTOs;
using backend.Domain.Filters;
using backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public interface IAccommodationRepository : IRepository<AccommodationModel>
    {
        Task<PagedResult<DestinationModel>> GetAccomodationAsync(DestinationFilter filter, int page, int pageSize);
        Task<bool> AccommodationExists(Guid id);
    }
}
