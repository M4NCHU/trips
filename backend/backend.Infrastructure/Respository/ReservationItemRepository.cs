using backend.Domain.DTOs;
using backend.Domain.Filters;
using backend.Domain.Models;
using backend.Infrastructure.Authentication;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.Infrastructure.Respository
{
    public class ReservationItemRepository : Repository<ReservationItemModel>, IReservationItemRepository
    {
        private readonly TripsDbContext _context;
        private readonly ILogger<ReservationItemRepository> _logger;

        public ReservationItemRepository(TripsDbContext context, ILogger<ReservationItemRepository> logger) : base(context)
        {
            _context = context;
            _logger = logger;
        }

       
    }
}