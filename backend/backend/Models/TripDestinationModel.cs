using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class TripDestinationModel
    {
        [Key]
        public int Id { get; set; }

        public int TripId { get; set; }

        public TripModel Trip { get; set; }

        public int DestinationId { get; set; }

        public DestinationModel Destination { get; set; }

        public List<SelectedPlaceModel> SelectedPlace { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }
    }
}
