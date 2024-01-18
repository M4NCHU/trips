using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class TripParticipantDTO
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public int ParticipantId { get; set; }

        public List<ParticipantDTO>? Participant { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }
    }
}