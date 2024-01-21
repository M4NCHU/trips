using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class VisitPlaceDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        public string PhotoUrl { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public double Price { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

        public IFormFile ImageFile { get; set; }

        [Required(ErrorMessage = "Destination ID is required.")]
        public Guid DestinationId { get; set; }
    }
}
