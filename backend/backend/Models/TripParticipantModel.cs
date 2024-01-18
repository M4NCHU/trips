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

        public List<ParticipantModel>? Participants { get; set; }

        [ForeignKey("TripId")]
        public TripModel Trip { get; set; }

        [ForeignKey("ParticipantId")]
        public ParticipantModel Participant { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

    }
}
