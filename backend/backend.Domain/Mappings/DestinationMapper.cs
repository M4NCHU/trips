using AutoMapper;
using backend.Domain.DTOs;
using backend.Models;
using System.Linq;

namespace backend.Domain.Mappings
{
    public class DestinationMapper : Profile
    {
        public DestinationMapper()
        {
            // Map DestinationModel to DestinationDTO
            CreateMap<DestinationModel, DestinationDTO>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom((src, dest, destMember, context) =>
                    $"{context.Items["BaseUrl"]}/Images/Destinations/{src.PhotoUrl}"))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.VisitPlaces, opt => opt.MapFrom(src => src.VisitPlaces));


            CreateMap<DestinationModel, ResponseDestinationDTO>()
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom((src, dest, destMember, context) =>
                $"{context.Items["BaseUrl"]}/Images/Destinations/{src.PhotoUrl}"))  
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));



            // Map VisitPlace to VisitPlaceDTO
            CreateMap<VisitPlace, VisitPlaceDTO>()
                .ForMember(vp => vp.PhotoUrl, opt => opt.MapFrom((src, dest, destMember, context) =>
                    $"{context.Items["BaseUrl"]}/Images/VisitPlace/{src.PhotoUrl}"));

            // Map TripDestinationModel to TripDestinationDTO
            CreateMap<TripDestinationModel, TripDestinationDTO>();

            CreateMap<CreateDestinationDTO, DestinationModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid())) 
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow)) 
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ImageFile, opt => opt.Ignore());  
        }
    }
}
