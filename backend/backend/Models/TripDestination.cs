namespace backend.Models
{
    public class TripDestination
    {
        
        public int Id { get; set; }
        public int TripId { get; set; }
        public TripModel Trip { get; set; }

        public int DestinationId { get; set; }
        public Destination Destination { get; set; }

        public List<SelectedPlaceModel> SelectedPlaces { get; set; }
    }
}
