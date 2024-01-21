using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class TripDestinationModel
    {
        [Key]
        public Guid Id { get; set; }

        public Guid TripId { get; set; }

        public TripModel Trip { get; set; }

        public Guid DestinationId { get; set; }

        public DestinationModel Destination { get; set; }

        public List<SelectedPlaceModel> SelectedPlace { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }
    }
}
