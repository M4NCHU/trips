using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Domain.DTOs
{
    public class CreateAccommodationDTO
    {
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? Location { get; set; }
        public double Price { get; set; }
        public int BedAmount { get; set; } = 0;

        public GeoLocationDTO? GeoLocation { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}
