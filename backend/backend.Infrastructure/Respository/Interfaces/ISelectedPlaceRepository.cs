using backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public interface ISelectedPlaceRepository : IRepository<SelectedPlaceModel>
    {
        Task<IEnumerable<SelectedPlaceModel>> GetSelectedPlacesAsync();
        Task<IEnumerable<SelectedPlaceModel>> GetSelectedPlacesByDestinationIdAsync(Guid destinationId);
        Task<bool> SelectedPlaceExistsAsync(Guid id);
    }
}
