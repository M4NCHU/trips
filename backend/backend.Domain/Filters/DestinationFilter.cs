using backend.Domain.DTO.GeoLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Domain.Filters
{
    public class DestinationFilter
    {
        public string? CategoryId { get; set; }
        public double? NorthEastLat { get; set; }
        public double? NorthEastLng { get; set; }
        public double? SouthWestLat { get; set; }
        public double? SouthWestLng { get; set; }
    }

}
