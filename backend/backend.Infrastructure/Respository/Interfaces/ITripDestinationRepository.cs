using backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public interface ITripDestinationRepository
    {
        Task<IEnumerable<TripDestinationModel>> GetAllTripDestinationsAsync();
        Task<TripDestinationModel> GetTripDestinationByIdAsync(Guid id);
        Task<IEnumerable<TripDestinationModel>> GetTripDestinationsByTripIdAsync(Guid tripId);
        Task AddTripDestinationAsync(TripDestinationModel tripDestination);
        Task<bool> UpdateTripDestinationAsync(TripDestinationModel tripDestination);
        Task<bool> DeleteTripDestinationAsync(Guid id);
        Task<int> CountTripDestinationsByTripIdAsync(Guid tripId);
    }
}
