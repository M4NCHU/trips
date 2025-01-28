using AutoMapper;
using backend.Domain.DTOs;
using backend.Models;

namespace backend.Domain.Mappings
{
    public class TripDestinationMapping : Profile
    {
        public TripDestinationMapping()
        {
            CreateMap<TripDestinationModel, TripDestinationDTO>()
                .ForMember(dest => dest.DayNumber, opt => opt.MapFrom(src => src.DayNumber));

        }

        
    }
}
