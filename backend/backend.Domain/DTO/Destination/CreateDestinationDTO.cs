﻿
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Domain.DTOs
{
    public class CreateDestinationDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Location { get; set; }
        public string? PhotoUrl { get; set; }
        public double Price { get; set; }
        public Guid CategoryId { get; set; }
        public IFormFile ImageFile { get; set; }
        public List<VisitPlaceDTO>? VisitPlaces { get; set; }
        public List<TripDestinationDTO>? TripDestinations { get; set; }

    }
}
