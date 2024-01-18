using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class ImageFileDTO
    {
        [Required(ErrorMessage = "Image file is required.")]
        [DataType(DataType.Upload)]
        public IFormFile ImageFile { get; set; }
    }
}
