using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string? PhotoUrl { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public ICollection<Destination> Destinations { get; set; } = new List<Destination>();



    }
}