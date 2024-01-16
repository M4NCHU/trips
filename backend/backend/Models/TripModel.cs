using backend.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class TripModel
    {
        
        public int Id { get; set; }
        public TripStatus Status { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }


        public List<TripDestination>? TripDestinations { get; set; }
        public List<SelectedPlaceModel>? SelectedPlaces { get; set; }

        public double TotalPrice { get; set; }
    }
}
