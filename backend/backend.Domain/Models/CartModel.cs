using backend.Domain.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using backend.Domain.enums;
using backend.Domain.DTOs;

namespace backend.Models
{
    public class CartModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public UserModel? User { get; set; }
        [Required]
        public CartItemType ItemType { get; set; }
        [Required]
        public Guid ItemId { get; set; }
        public int Quantity { get; set; } = 1;
        public string? AdditionalData { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        [NotMapped]
        public virtual DestinationDTO? Destination { get; set; }
  /*      public virtual AccommodationDTO? Accommodation { get; set; }*/
/*        public virtual VisitPlaceDTO? VisitPlace { get; set; }
        public virtual TripDestinationDTO? Trip { get; set; }*/

    }
}
