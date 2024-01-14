using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class DestinationDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        public string Description { get; set; }
        public string? Location { get; set; }
        public string? PhotoUrl { get; set; }

        
        public int? CategoryId { get; set; }
        
        // Property for handling file uploads
        public IFormFile ImageFile { get; set; }

        public CategoryDTO? Category { get; set; }

        public List<VisitPlaceDTO> VisitPlaces { get; set; }
    }


}
