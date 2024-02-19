using backend.Domain.DTOs;
using backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Domain.Mappings
{
    public class TripDestinationMapping
    {

        public TripDestinationDTO MapToTripDestinationDTO(TripDestinationModel model)
        {
            return new TripDestinationDTO
            {
                Id = model.Id,
                TripId = model.TripId,
                DestinationId = model.DestinationId,
                /*CreatedBy = model.CreatedBy,*/
               /* Title = model.Title,*/
              /*  Destination = model.Destination != null ? MapToDestinationDTO(model.Destination) : null,
                SelectedPlaces = model.SelectedPlace?.Select(MapToSelectedPlaceDTO).ToList(),*/
                DayNumber = model.DayNumber,
            };
        }
    }
}
