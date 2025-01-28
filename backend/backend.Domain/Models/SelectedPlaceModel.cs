using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class SelectedPlaceModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid TripDestinationId { get; set; }
        public TripDestinationModel TripDestination { get; set; }
        public Guid VisitPlaceId { get; set; }
        public VisitPlaceModel VisitPlace { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
    }
}
