using Microsoft.AspNetCore.Mvc;
using backend.Application.Services.Interfaces;
using backend.Domain.DTOs;
using System;
using System.Threading.Tasks;
using backend.Domain.DTO.Cart;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateReservation()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            var cartCookie = Request.Cookies["cart"];
            if (string.IsNullOrEmpty(cartCookie))
                return BadRequest(new { message = "Cart cookie is missing or empty." });

            var result = _reservationService.CreateReservationAsync(cartCookie, userId).Result;

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = "Reservation created successfully.", reservation = result.Reservation });
        }



        [HttpGet]
        [Route("{reservationId}/summary")]
        public async Task<IActionResult> GetReservationSummary(Guid reservationId)
        {
            var userId = User.Identity?.Name;
            var summary = await _reservationService.GetReservationSummaryAsync(reservationId, userId);

            if (summary == null)
                return NotFound(new { message = "Reservation not found." });

            return Ok(summary);
        }

        [HttpGet]
        [Route("user-reservations")]
        public async Task<IActionResult> GetUserReservations()
        {
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User is not authorized." });
            }

            var reservations = await _reservationService.GetUserReservationsAsync(userId);

            if (reservations == null || !reservations.Any())
            {
                return NotFound(new { message = "No reservations found for the user." });
            }

            return Ok(reservations);
        }

        [HttpGet]
        [Route("{reservationId}")]
        public async Task<IActionResult> GetReservationById(Guid reservationId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User is not authorized." });
            }

            var reservation = await _reservationService.GetReservationByIdAsync(reservationId, userId);

            if (reservation == null)
            {
                return NotFound(new { message = "Reservation not found or access denied." });
            }

            return Ok(reservation);
        }



    }
}
