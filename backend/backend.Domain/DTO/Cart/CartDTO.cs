using backend.Domain.Authentication;
using backend.Domain.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using backend.Domain.DTOs;

namespace backend.Domain.DTO
{
    public class CartDTO
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public CartItemType ItemType { get; set; }
        public Guid ItemId { get; set; }
        public int Quantity { get; set; } = 1;
        public string? AdditionalData { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

        public virtual DestinationDTO? Destination { get; set; }
/*        public virtual AccommodationDTO? Accommodation { get; set; }
        public virtual VisitPlaceDTO? VisitPlace { get; set; }
        public virtual TripDestinationDTO? Trip { get; set; }*/
    }
}
