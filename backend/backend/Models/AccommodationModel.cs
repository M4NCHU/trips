using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class AccommodationModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string? PhotoUrl { get; set; }
        public double Price { get; set; }
        public int BedAmmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}

