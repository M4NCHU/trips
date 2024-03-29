﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace backend.Domain.DTOs
{
    public class ParticipantDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Emergency contact name is required.")]
        public string EmergencyContact { get; set; }

        [Required(ErrorMessage = "Emergency contact phone is required.")]
        public string EmergencyContactPhone { get; set; }

        public string MedicalConditions { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [Required(ErrorMessage = "Trip ID is required.")]
        public Guid TripId { get; set; }

        public string PhotoUrl { get; set; }
        
        public IFormFile ImageFile { get; set; }

       
        public List<TripParticipantDTO>? TripParticipants { get; set; }


    }
}