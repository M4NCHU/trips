using backend.Domain.enums;
using backend.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Domain.Models
{
    public class GeoLocationModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid ItemId { get; set; }

        [Required]
        [MaxLength(255)]
        public CartItemType Type { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [Range(-90, 90)]
        public double Latitude { get; set; }

        [Required]
        [Range(-180, 180)]
        public double Longitude { get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public virtual DestinationModel? Destination { get; set; }
    }
}
