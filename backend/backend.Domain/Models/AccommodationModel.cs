using backend.Domain.DTOs;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class AccommodationModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string? PhotoUrl { get; set; }
        public double Price { get; set; }
        public int BedAmmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public Guid? GeoLocationId { get; set; }
        public GeoLocationDTO? GeoLocation { get; set; }

    }
}

