using System.ComponentModel.DataAnnotations;

namespace backend.Domain.DTO.Authentication
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(24, MinimumLength = 1, ErrorMessage = "First name must be at least {2}, and maximum {1} characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(24, MinimumLength = 1, ErrorMessage = "Last name must be at least {2}, and maximum {1} characters")]
        public string LastName { get; set; }


        /*[Required(ErrorMessage = "User Name is required")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Username must be at least {2}, and maximum {1} characters")]*/
        public string Username { get; set; }

        [EmailAddress(ErrorMessage = "Email address is invalid")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

    }
}