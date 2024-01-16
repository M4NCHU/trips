using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class AccommodationDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Location { get; set; }
        public string? PhotoUrl { get; set; }
        public double Price { get; set; }
        public int BedAmmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public IFormFile ImageFile { get; set; }

    }


}
