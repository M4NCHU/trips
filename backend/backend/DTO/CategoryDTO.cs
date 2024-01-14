using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        public string Description { get; set; }
        public string? PhotoUrl { get; set; }

        // Dodano właściwość ImageFileDTO, aby obsługiwać przesyłanie plików
        public IFormFile ImageFile { get; set; }
    }
}
