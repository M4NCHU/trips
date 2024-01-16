namespace backend.DTOs
{
    public class TripDestinationDTO
    {
        public int TripId { get; set; }
        public int DestinationId { get; set; }
        public DestinationDTO? Destination { get; set; }

        public List<SelectedPlaceDTO>? SelectedPlaces { get; set; }


    }
}
