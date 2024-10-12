using backend.Domain.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Domain.DTOs
{
    public class CreateCategoryRequestDTO
    {
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public IFormFile ImageFile { get; set; }
        public ICollection<DestinationDTO>? Destinations { get; set; } = new List<DestinationDTO>();

    }
}
