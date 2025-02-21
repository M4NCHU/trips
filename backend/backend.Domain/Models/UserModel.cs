﻿using backend.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace backend.Domain.Authentication
{
    public class UserModel : IdentityUser
    {
        // Custom properties
        public string FirstName { get; set; }
        public string LastName { get; set; }  
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastUpdated { get; set; }
        public virtual ICollection<TripModel> Trips { get; set; }
    }
}
