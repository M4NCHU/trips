namespace backend.Models
{
    public class TripDestination
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public int DestinationId { get; set; }
    }
}
