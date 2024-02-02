using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;


namespace backend.Domain.DTOs
{
    public class ImageFileDTO
    {
        public IFormFile ImageFile { get; set; }
    }
}
