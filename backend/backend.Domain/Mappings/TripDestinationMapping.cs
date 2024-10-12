using AutoMapper;
using backend.Domain.DTOs;
using backend.Models;

namespace backend.Domain.Mappings
{
    public class TripDestinationMapping : Profile
    {
        public TripDestinationMapping()
        {
            // Configure mapping for TripDestinationModel to TripDestinationDTO
            CreateMap<TripDestinationModel, TripDestinationDTO>()
                .ForMember(dest => dest.DayNumber, opt => opt.MapFrom(src => src.DayNumber));

        }

        
    }
}
