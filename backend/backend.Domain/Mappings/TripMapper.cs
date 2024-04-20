using AutoMapper;
using backend.Domain.DTOs;
using backend.Models;
using System.Linq;

namespace backend.Domain.Mappings
{
    public class TripMapper : Profile
    {
        public TripMapper()
        {

            // Map TripModel to TripDTO
            CreateMap<TripModel, TripDTO>();
        }
    }
}
