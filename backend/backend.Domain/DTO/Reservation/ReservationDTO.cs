using backend.Domain.Authentication;
using backend.Domain.enums;
using backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Domain.DTO
{
    public class ReservationDTO
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public List<ReservationItemModel> ReservationItems { get; set; }
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
        public double TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; set; }
    }
}
