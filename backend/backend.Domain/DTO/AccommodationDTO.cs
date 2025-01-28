using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace backend.Domain.DTOs
{
    public class AccommodationDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(32, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 32 characters.")]
        public string Name { get; set; }

        [StringLength(512, ErrorMessage = "Description cannot exceed 512 characters.")]
        public string Description { get; set; }

        [StringLength(128, ErrorMessage = "Location cannot exceed 128 characters.")]
        public string? Location { get; set; }
        public string? PhotoUrl { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public double Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Bed amount must be at least 1.")]
        public int BedAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public IFormFile ImageFile { get; set; }

    }


}
