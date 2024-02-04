using backend.Domain.Authentication;
using backend.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Domain.DTOs
{
    public class TripDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public TripStatus Status { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid date format on start date.")]
        public DateTime? StartDate { get; set; }

        public string? Title { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid date format on end date.")]
        public DateTime? EndDate { get; set; }

        public List<TripDestinationDTO>? TripDestinations { get; set; }
        public List<SelectedPlaceDTO>? SelectedPlaces { get; set; }
        public List<TripParticipantDTO>? TripParticipants { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Total price must be greater than 0.")]
        public double TotalPrice { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public string CreatedBy { get; set; }

        public UserDTO User { get; set; }
    }
}
