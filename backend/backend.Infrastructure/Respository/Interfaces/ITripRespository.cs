using backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public interface ITripRepository
    {
        Task<IEnumerable<TripModel>> GetAllTripsAsync();
        Task<TripModel> GetTripByIdAsync(Guid id);
        Task<IEnumerable<TripModel>> GetTripsByUserIdAsync(string userId);
        Task AddTripAsync(TripModel trip);
        Task UpdateTripAsync(TripModel trip);
        Task DeleteTripAsync(Guid id);
        Task<bool> TripExistsAsync(Guid id);
    }
}
