


namespace backend.Domain.DTOs
{
    public class TripParticipantDTO
    {
        public Guid Id { get; set; }
        public Guid TripId { get; set; }
        public Guid ParticipantId { get; set; }

        public List<ParticipantDTO>? Participant { get; set; }

        
        
        public TripDTO Trip { get; set; }

        
        public ParticipantDTO Participants { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }
    }
}