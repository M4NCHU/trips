using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class SelectedPlaceModel
    {
        [Key]
        public int Id { get; set; }

        public int TripDestinationId { get; set; }

        public TripDestinationModel TripDestination { get; set; }

        public int VisitPlaceId { get; set; }

        public VisitPlace VisitPlace { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; } = DateTime.Now;
    }
}
