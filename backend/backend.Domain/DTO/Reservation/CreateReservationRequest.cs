using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Domain.DTO.Reservation
{
    public class CreateReservationRequest
    {
        public List<Guid> ItemIds { get; set; }
        public string PaymentDetails { get; set; }
        public string AdditionalNotes { get; set; }
    }
}
