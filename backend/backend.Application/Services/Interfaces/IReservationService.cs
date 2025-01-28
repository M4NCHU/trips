using backend.Domain.DTO.Cart;
using backend.Domain.DTO;
using backend.Domain.DTOs;
using backend.Domain.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using backend.Domain.DTO.Reservation;
using backend.Domain.Models;

namespace backend.Application.Services.Interfaces
{
    public interface IReservationService
    {
        Task<(bool Success, string Message, ReservationDTO? Reservation)> CreateReservationAsync(string cartCookie, string? userId);
        Task<ReservationSummaryDTO?> GetReservationSummaryAsync(Guid reservationId, string? userId);
        Task<List<ReservationDTO>> GetUserReservationsAsync(string userId);
        Task<ReservationDTO?> GetReservationByIdAsync(Guid reservationId, string userId);

    }
}
