using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Domain.DTOs
{
    public class CreateVisitPlaceDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public double Price { get; set; }
        public float Duration { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public IFormFile ImageFile { get; set; }
        public Guid DestinationId { get; set; }
    }
}
