using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Domain.DTOs
{
    public class CreateTripDestinationDTO
    {
        public Guid TripId { get; set; }
        public Guid DestinationId { get; set; }

        public string CreatedBy { get; set; }
        public string? TripTitle { get; set; }
    }
}
