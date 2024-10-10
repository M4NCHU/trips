using backend.Infrastructure.Authentication;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly TripsDbContext _context;

        public ParticipantRepository(TripsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ParticipantModel>> GetParticipantsAsync(int page, int pageSize)
        {
            return await _context.Participant
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<ParticipantModel> GetParticipantByIdAsync(Guid id)
        {
            return await _context.Participant.FindAsync(id);
        }

        public async Task AddParticipantAsync(ParticipantModel participant)
        {
            await _context.Participant.AddAsync(participant);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateParticipantAsync(ParticipantModel participant)
        {
            _context.Entry(participant).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteParticipantAsync(Guid id)
        {
            var participant = await _context.Participant.FindAsync(id);
            if (participant != null)
            {
                _context.Participant.Remove(participant);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ParticipantExistsAsync(Guid id)
        {
            return await _context.Participant.AnyAsync(p => p.Id == id);
        }
    }
}
