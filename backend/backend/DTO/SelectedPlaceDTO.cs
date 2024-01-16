namespace backend.DTOs
{
    public class SelectedPlaceDTO
    {
        public int Id { get; set; }
        public int TripDestinationId { get; set; }
        public int VisitPlaceId { get; set; }
        public VisitPlaceDTO VisitPlace { get; set; }

    }
}
