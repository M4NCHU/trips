using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Domain.DTOs
{
    public class VisitPlaceDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public double Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public float Duration { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        [NotMapped]

        public IFormFile ImageFile { get; set; }
        public Guid DestinationId { get; set; }
    }
}
