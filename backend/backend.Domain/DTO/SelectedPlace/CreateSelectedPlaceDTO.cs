using backend.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Domain.DTOs
{
    public class CreateSelectedPlaceDTO
    {
        public Guid TripDestinationId { get; set; }
        public Guid VisitPlaceId { get; set; }

    }
}
