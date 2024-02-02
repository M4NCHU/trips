namespace backend.Domain.DTOs
{
    public class SelectedPlaceDTO
    {
        public Guid Id { get; set; }
        public Guid TripDestinationId { get; set; }
        public Guid VisitPlaceId { get; set; }
        public VisitPlaceDTO? VisitPlace { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

    }
}
