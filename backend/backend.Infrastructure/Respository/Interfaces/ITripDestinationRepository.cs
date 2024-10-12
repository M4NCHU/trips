using backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public interface ITripDestinationRepository : IRepository<TripDestinationModel>
    {
        Task<IEnumerable<TripDestinationModel>> GetAllTripDestinationsAsync();
        Task<TripDestinationModel> GetTripDestinationByIdAsync(Guid id);
        Task<IEnumerable<TripDestinationModel>> GetTripDestinationsByTripIdAsync(Guid tripId);
        Task<int> CountTripDestinationsByTripIdAsync(Guid tripId);
        Task<bool> UpdateTripDestinationAsync(TripDestinationModel tripDestination);
        Task<bool> DeleteTripDestinationAsync(Guid id);
    }
}
