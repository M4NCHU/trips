namespace backend.Domain.DTOs
{
    public class TripDestinationDTO
    {
        public Guid Id { get; set; }
        public Guid TripId { get; set; }
        public Guid DestinationId { get; set; }
        
        public string CreatedBy { get; set; }

        public string? Title { get; set; }
        public DestinationDTO? Destination { get; set; }

        public List<SelectedPlaceDTO>? SelectedPlaces { get; set; }

        public int DayNumber { get; set; }
    }
}
