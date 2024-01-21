using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class CategoryModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string PhotoUrl { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; } = DateTime.Now;

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public ICollection<DestinationModel> Destinations { get; set; } = new List<DestinationModel>();



    }
}