using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Domain.DTO.Reservation
{
    public class ReservationSummaryDTO
    {
        public Guid ReservationId { get; set; }
        public string? UserId { get; set; }
        public double TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ReservationItemDTO> Items { get; set; } = new();
    }
}
