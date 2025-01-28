using backend.Domain.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class DestinationModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Location { get; set; }
        public string? PhotoUrl { get; set; }
        public double Price { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; } = DateTime.Now;
        public Guid CategoryId { get; set; }
        public int AvailablePlaces { get; set; } = 0;
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public CategoryModel? Category { get; set; }
        public List<VisitPlaceModel>? VisitPlaces { get; set; }
        public List<TripDestinationModel>? TripDestinations { get; set; }
        public Guid? GeoLocationId { get; set; }
        public virtual GeoLocationModel? GeoLocation { get; set; }

    }
}

