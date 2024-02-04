﻿using backend.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
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


        Task<IActionResult> PutTrip(Guid id, TripDTO trip);

        Task<ActionResult<TripDTO>> PostTrip([FromForm] TripDTO trip);

        Task<IActionResult> DeleteTrip(Guid id);

        Task<IActionResult> ChangeTripTitle(Guid tripId, string newTitle);
    }
}
