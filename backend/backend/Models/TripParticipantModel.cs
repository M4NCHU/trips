using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class TripParticipantModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid TripId { get; set; }
        public Guid ParticipantId { get; set; }

        public List<ParticipantModel>? Participants { get; set; }

        [ForeignKey("TripId")]
        public TripModel Trip { get; set; }

        [ForeignKey("ParticipantId")]
        public ParticipantModel Participant { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

    }
}
