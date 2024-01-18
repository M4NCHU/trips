using backend.DTOs;
using backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class TripParticipantModel
    {
        [Key]
        public int Id { get; set; }
        public int TripId { get; set; }
        public int ParticipantId { get; set; }

        public List<ParticipantDTO>? Participants { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }
    }
}
