using AutoMapper;
using backend.Domain.DTOs;
using backend.Domain.Models;
using backend.Models;

namespace backend.Domain.Mappings
{
    public class DestinationMapper : Profile
    {
        public DestinationMapper()
        {
            CreateMap<DestinationModel, DestinationDTO>()
               .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom((src, dest, destMember, context) =>
                   src.PhotoUrl != null ? $"{context.Items["BaseUrl"]}/Images/Destinations/{src.PhotoUrl}" : null)) 
               .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
               .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category ?? null))  
               .ForMember(dest => dest.VisitPlaces, opt => opt.MapFrom(src => src.VisitPlaces ?? new List<VisitPlaceModel>())) 
               .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description ?? string.Empty))  
               .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location ?? string.Empty)) 
               .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
               .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt ?? DateTime.UtcNow))  
               .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.ModifiedAt ?? DateTime.UtcNow));

            CreateMap<CreateDestinationDTO, DestinationModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ImageFile, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.VisitPlaces, opt => opt.Ignore())
                .ForMember(dest => dest.TripDestinations, opt => opt.Ignore())
                .ForMember(dest => dest.GeoLocation, opt => opt.Ignore());


            CreateMap<DestinationModel, ResponseAccomodationDTO>()
               .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom((src, dest, destMember, context) =>
                   src.PhotoUrl != null ? $"{context.Items["BaseUrl"]}/Images/Destinations/{src.PhotoUrl}" : null))
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description ?? string.Empty))
               .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location ?? string.Empty))
               .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
               .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
               .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.ModifiedAt))
               .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
               .ForMember(dest => dest.GeoLocation, opt => opt.MapFrom(src => src.GeoLocation));

            CreateMap<GeoLocationModel, GeoLocationDTO>();

        }
    }
}
