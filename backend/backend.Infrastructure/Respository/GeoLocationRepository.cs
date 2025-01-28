using backend.Domain.DTOs;
using backend.Domain.Filters;
using backend.Domain.Models;
using backend.Infrastructure.Authentication;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.Infrastructure.Respository
{
    public class GeoLocationRepository : Repository<GeoLocationModel>, IGeoLocationRepository
    {
        private readonly TripsDbContext _context;
        private readonly ILogger<GeoLocationRepository> _logger;

        public GeoLocationRepository(TripsDbContext context, ILogger<GeoLocationRepository> logger) : base(context)
        {
            _context = context;
            _logger = logger;
        }

       
    }
}