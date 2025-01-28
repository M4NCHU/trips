using backend.Domain.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Domain.DTO.Reservation
{
    public class ReservationItemDTO
    {
        public Guid ItemId { get; set; }
        public CartItemType ItemType { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}
