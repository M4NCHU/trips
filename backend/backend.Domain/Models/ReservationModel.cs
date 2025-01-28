using backend.Domain.Authentication;
using backend.Domain.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Domain.Models
{
    public class ReservationModel
    {
        [Key]
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public UserModel? User { get; set; }
        public List<ReservationItemModel>? ReservationItems { get; set; }
        public ReservationDetailsModel? ReservationDetails { get; set; }

        [Required]
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
        [Required]
        public double TotalPrice { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; set; }
    }
}
