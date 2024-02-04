using System.ComponentModel.DataAnnotations;

namespace backend.Domain.DTO.Authentication
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}