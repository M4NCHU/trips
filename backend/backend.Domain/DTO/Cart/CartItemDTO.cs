using backend.Domain.DTOs;
using backend.Domain.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Domain.DTO.Cart
{
    public class CartItemDTO
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public CartItemType ItemType { get; set; }
        public Guid ItemId { get; set; }
        public int Quantity { get; set; } = 1;
        public string? AdditionalData { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

        public DestinationDTO? Destination { get; set; }
        /*       public virtual AccommodationDTO? Accommodation { get; set; }
               public virtual VisitPlaceDTO? VisitPlace { get; set; }
               public virtual TripDestinationDTO? Trip { get; set; }*/
    }

}
