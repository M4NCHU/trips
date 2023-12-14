using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Destination
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Location { get; set; }
        public string? PhotoUrl { get; set; }

        [NotMapped]
        public IFormFile ImageFile {  get; set; }


    }
}
