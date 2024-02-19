using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class VisitPlace
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public string PhotoUrl { get; set; }

        [Required]
        public double Price { get; set; }

        public float Duration { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; } = DateTime.Now;

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public Guid DestinationId { get; set; }

        public DestinationModel Destination { get; set; }
    }
}
