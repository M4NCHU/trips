namespace backend.Models
{
    public class SelectedPlaceModel
    {
        public int Id { get; set; }

        public int TripDestinationId { get; set; }
        public TripDestination TripDestination { get; set; }

        public int VisitPlaceId { get; set; }
        public VisitPlace VisitPlace { get; set; }
    }
}
