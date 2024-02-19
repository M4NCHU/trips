using backend.Domain.Authentication;
using backend.Domain.Enums;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class TripModel
    {
        [Key]
        public Guid Id { get; set; }

        public TripStatus Status { get; set; }

        public string Title { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; } = DateTime.Now;

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public UserModel User { get; set; }

        public List<TripDestinationModel>? TripDestinations { get; set; }

        public List<SelectedPlaceModel>? SelectedPlaces { get; set; }

        public List<TripParticipantModel> TripParticipants { get; set; }

        public double TotalPrice { get; set; }
        

    }
}
