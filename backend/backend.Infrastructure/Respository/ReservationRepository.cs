using backend.Domain.DTOs;
using backend.Domain.Filters;
using backend.Domain.Models;
using backend.Infrastructure.Authentication;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace backend.Infrastructure.Respository
{
    public class ReservationRepository : Repository<ReservationModel>, IReservationRepository
    {
        private readonly TripsDbContext _context;
        private readonly ILogger<ReservationRepository> _logger;

        public ReservationRepository(TripsDbContext context, ILogger<ReservationRepository> logger) : base(context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ReservationModel?> GetByConditionAsync(
     Expression<Func<ReservationModel, bool>> predicate,
     Func<IQueryable<ReservationModel>, IIncludableQueryable<ReservationModel, object>>? include = null)
        {
            IQueryable<ReservationModel> query = _context.Reservation;

            if (include != null)
                query = include(query);

            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task<List<ReservationModel>> GetReservationsByUserIdAsync(string userId)
        {
            try
            {
                _logger.LogInformation("Fetching reservations for User ID: {UserId}", userId);

                var reservations = await _context.Reservation
                    .Where(r => r.UserId == userId)
                    .Include(r => r.ReservationItems) 
                    .ToListAsync();

                if (!reservations.Any())
                {
                    _logger.LogWarning("No reservations found for User ID: {UserId}", userId);
                }

                return reservations;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching reservations for User ID: {UserId}", userId);
                throw;
            }
        }
        public async Task<ReservationModel?> GetReservationByIdAndUserIdAsync(Guid reservationId, string userId)
        {
            try
            {
                _logger.LogInformation("Fetching reservation with ID: {ReservationId} for User ID: {UserId}", reservationId, userId);

                var reservation = await _context.Reservation
                    .Include(r => r.ReservationItems)
                    .FirstOrDefaultAsync(r => r.Id == reservationId && r.UserId == userId);

                if (reservation == null)
                {
                    _logger.LogWarning("No reservation found with ID: {ReservationId} for User ID: {UserId}", reservationId, userId);
                }

                return reservation;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching reservation with ID: {ReservationId} for User ID: {UserId}", reservationId, userId);
                throw;
            }
        }


    }
}