using backend.Infrastructure.Authentication;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public class ParticipantRepository : Repository<ParticipantModel>, IParticipantRepository
    {
        private readonly TripsDbContext _context;
        private readonly ILogger<ParticipantRepository> _logger;

        public ParticipantRepository(TripsDbContext context, ILogger<ParticipantRepository> logger) : base(context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<ParticipantModel>> GetParticipantsAsync(int page, int pageSize)
        {
            _logger.LogInformation("Fetching participants with pagination. Page: {Page}, PageSize: {PageSize}", page, pageSize);

            return await _context.Participant
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<bool> ParticipantExistsAsync(Guid id)
        {
            _logger.LogInformation("Checking if participant with ID {Id} exists", id);
            return await _context.Participant.AnyAsync(p => p.Id == id);
        }
    }
}
