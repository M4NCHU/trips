using backend.Domain.DTOs;
using backend.Domain.Filters;
using backend.Domain.Models;
using backend.Infrastructure.Authentication;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.Infrastructure.Respository
{
    public class PaymentRepository : Repository<PaymentModel>, IPaymentRepository
    {
        private readonly TripsDbContext _context;
        private readonly ILogger<PaymentRepository> _logger;

        public PaymentRepository(TripsDbContext context, ILogger<PaymentRepository> logger) : base(context)
        {
            _context = context;
            _logger = logger;
        }

       
    }
}