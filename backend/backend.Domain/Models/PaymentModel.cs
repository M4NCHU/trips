using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using backend.Domain.enums;

namespace backend.Domain.Models
{
    public class PaymentModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ReservationId { get; set; }

        [ForeignKey(nameof(ReservationId))]
        public ReservationModel Reservation { get; set; }
        [Required]
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        [Required]
        public double Amount { get; set; }
        [Required]
        public PaymentMethod Method { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? CompletedAt { get; set; }
    }
}
