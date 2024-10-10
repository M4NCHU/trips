using backend.Domain.DTOs;
using backend.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Application.Services
{
    public interface ITripService
    {
        Task<ActionResult<IEnumerable<TripDTO>>> GetTrips();
        Task<ActionResult<TripDTO>> GetTripById(Guid id);
        Task<ActionResult<IEnumerable<TripDTO>>> GetUserTripsList(string userId);
        Task<List<VisitPlaceDTO>> GetVisitPlacesForTrip(Guid tripId);
        Task<IActionResult> ChangeTripTitle(Guid tripId, string newTitle);
        Task<CreateTripDTO> PostTrip(CreateTripDTO tripDTO);
        Task<IActionResult> PutTrip(Guid id, TripDTO tripDTO);
        Task<IActionResult> DeleteTrip(Guid id);
        Task<ActionResult<int>> CountUserTrips(string userId, TripStatus status);
        Task<(Guid, bool)> EnsureActiveTripExists(string userId);
    }
}
