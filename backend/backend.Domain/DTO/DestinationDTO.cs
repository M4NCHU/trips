using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Domain.DTOs
{
    public class DestinationDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [StringLength(100, ErrorMessage = "Location cannot exceed 100 characters.")]
        public string? Location { get; set; }

        public string? PhotoUrl { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public double Price { get; set; }
        public int AvailablePlaces { get; set; } = 0;

        public Guid CategoryId { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }
        [NotMapped]
        [JsonIgnore]
        public IFormFile? ImageFile { get; set; }

       
        public CategoryDTO? Category { get; set; }

        public List<VisitPlaceDTO>? VisitPlaces { get; set; } = new List<VisitPlaceDTO>();
        public List<TripDestinationDTO>? TripDestinations { get; set; } = new List<TripDestinationDTO>();
        public GeoLocationDTO? GeoLocation { get; set; }

    }


}
