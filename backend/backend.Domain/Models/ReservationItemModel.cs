using backend.Domain.enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.Domain.Models
{
    public class ReservationItemModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid ReservationId { get; set; }
        [ForeignKey(nameof(ReservationId))]
        public ReservationModel Reservation { get; set; }
        [Required]
        public CartItemType ItemType { get; set; }
        [Required]
        public Guid ItemId { get; set; }
        [Required]
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string? AdditionalData { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
