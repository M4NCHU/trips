using backend.Enums;

namespace backend.DTOs
{
    public class TripDTO
    {
        public int Id { get; set; }
        public TripStatus Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<TripDestinationDTO>? TripDestinations { get; set; }
        public List<SelectedPlaceDTO>? SelectedPlaces { get; set; }
        public double TotalPrice { get; set; }

       /* public List<DestinationDTO>? Destinations { get; set; }
        public List<VisitPlaceDTO>? VisitPlaces { get; set; }*/
    }
}
