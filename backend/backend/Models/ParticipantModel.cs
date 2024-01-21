using backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class ParticipantModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string EmergencyContact { get; set; }

        [Required]
        [Phone]
        public string EmergencyContactPhone { get; set; }

        public string MedicalConditions { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

        public string PhotoUrl { get; set; }

        public List<TripParticipantModel>? TripParticipants { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
        

    }
}
