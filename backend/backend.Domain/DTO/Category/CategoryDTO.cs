using backend.Domain.DTOs;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace backend.Domain.DTOs
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;

        public string? PhotoUrl { get; set; } = string.Empty;

        public DateTime? CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public IFormFile? ImageFile { get; set; }

        public ICollection<DestinationDTO>? Destinations { get; set; } = new List<DestinationDTO>();



    }
}
