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

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        

        public CategoryModel? Category { get; set; }

        public List<VisitPlace>? VisitPlaces { get; set; }

        public List<TripDestinationModel>? TripDestinations { get; set; }

    }
}

