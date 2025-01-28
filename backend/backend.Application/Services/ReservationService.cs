using AutoMapper;
using backend.Application.Services.Interfaces;
using backend.Domain.DTO.Cart;
using backend.Domain.DTO;
using backend.Domain.DTOs;
using backend.Domain.Filters;
using backend.Domain.Models;
using backend.Infrastructure.Respository;
using backend.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using backend.Domain.enums;
using backend.Domain.DTO.Reservation;
using Microsoft.EntityFrameworkCore;

namespace backend.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        private readonly ICartService _cartService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IBaseUrlService _baseUrlService;
        private readonly string _baseUrl;
        private readonly ILogger<ReservationService> _logger;
        private readonly IMapper _mapper;

        public ReservationService(
            IUnitOfWork unitOfWork,
            IImageService imageService,
            ICartService cartService,
            IWebHostEnvironment hostEnvironment,
            IBaseUrlService baseUrlService,
            ILogger<ReservationService> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
            _cartService = cartService;
            _hostEnvironment = hostEnvironment;
            _baseUrlService = baseUrlService;
            _baseUrl = _baseUrlService.GetBaseUrl();
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<(bool Success, string Message, ReservationDTO? Reservation)> CreateReservationAsync(string cartCookie, string? userId)
        {
            try
            {
                var cartItems = await _cartService.GetCartAsync(cartCookie, userId);

                if (cartItems == null || !cartItems.Any())
                {
                    return (false, "Cart is empty or invalid.", null);
                }

                var reservationItems = cartItems.Select(item =>
                {
                    var price = item.Destination?.Price ?? 0;

                    return new ReservationItemModel
                    {
                        Id = Guid.NewGuid(),
                        ItemId = item.ItemId,
                        ItemType = item.ItemType,
                        Price = price,
                        Quantity = item.Quantity,
                        CreatedAt = DateTime.UtcNow,
                        ReservationId = Guid.Empty 
                    };
                }).ToList();

                if (!reservationItems.Any())
                {
                    return (false, "No valid items found for reservation.", null);
                }

                var reservationId = Guid.NewGuid();

                reservationItems.ForEach(item => item.ReservationId = reservationId);

                var reservation = new ReservationModel
                {
                    Id = reservationId,
                    UserId = userId,
                    ReservationItems = reservationItems,
                    TotalPrice = reservationItems.Sum(item => item.Price * item.Quantity),
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    Status = ReservationStatus.Pending
                };

                await _unitOfWork.Reservations.AddAsync(reservation);
                await _unitOfWork.SaveChangesAsync();

                return (true, "Reservation created successfully.", _mapper.Map<ReservationDTO>(reservation));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating reservation.");
                return (false, "An error occurred while creating reservation.", null);
            }
        }




        public async Task<ReservationSummaryDTO?> GetReservationSummaryAsync(Guid reservationId, string? userId)
        {
            try
            {
                var reservation = await _unitOfWork.Reservations
                    .GetByConditionAsync(
                        r => r.Id == reservationId && r.UserId == userId,
                        include: r => r.Include(x => x.ReservationItems)
                    );

                if (reservation == null)
                {
                    _logger.LogWarning("Reservation not found for User ID: {UserId} and Reservation ID: {ReservationId}", userId, reservationId);
                    return null;
                }
                var reservationSummary = new ReservationSummaryDTO
                {
                    ReservationId = reservation.Id,
                    UserId = reservation.UserId,
                    TotalPrice = reservation.TotalPrice,
                    CreatedAt = reservation.CreatedAt,
                    Items = reservation.ReservationItems.Select(item => new ReservationItemDTO
                    {
                        ItemId = item.ItemId,
                        ItemType = item.ItemType,
                        Price = item.Price,
                        Quantity = item.Quantity
                    }).ToList()
                };

                return reservationSummary;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching reservation summary for ID: {ReservationId}", reservationId);
                throw;
            }
        }

        public async Task<List<ReservationDTO>> GetUserReservationsAsync(string userId)
        {
            try
            {
                var reservations = await _unitOfWork.Reservations.GetReservationsByUserIdAsync(userId);

                if (reservations == null || !reservations.Any())
                {
                    _logger.LogWarning("No reservations found for User ID: {UserId}", userId);
                    return new List<ReservationDTO>();
                }

                return _mapper.Map<List<ReservationDTO>>(reservations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching reservations for User ID: {UserId}", userId);
                throw;
            }
        }

        public async Task<ReservationDTO?> GetReservationByIdAsync(Guid reservationId, string userId)
        {
            try
            {
                var reservation = await _unitOfWork.Reservations.GetReservationByIdAndUserIdAsync(reservationId, userId);

                if (reservation == null)
                {
                    _logger.LogWarning("Reservation not found or access denied for Reservation ID: {ReservationId} and User ID: {UserId}", reservationId, userId);
                    return null;
                }

                return _mapper.Map<ReservationDTO>(reservation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching reservation for ID: {ReservationId} and User ID: {UserId}", reservationId, userId);
                throw;
            }
        }

    }
}
