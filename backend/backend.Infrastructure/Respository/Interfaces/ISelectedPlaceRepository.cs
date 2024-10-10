using backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public interface ISelectedPlaceRepository
    {
        Task<IEnumerable<SelectedPlaceModel>> GetSelectedPlacesAsync();
        Task<SelectedPlaceModel> GetSelectedPlaceByIdAsync(Guid id);
        Task<IEnumerable<SelectedPlaceModel>> GetSelectedPlacesByDestinationIdAsync(Guid destinationId);
        Task AddSelectedPlaceAsync(SelectedPlaceModel selectedPlace);
        Task UpdateSelectedPlaceAsync(SelectedPlaceModel selectedPlace);
        Task DeleteSelectedPlaceAsync(Guid id);
        Task<bool> SelectedPlaceExistsAsync(Guid id);
    }
}
