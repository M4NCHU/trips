using AutoMapper;
using backend.Domain.DTOs;
using backend.Domain.Models;
using backend.Models;

namespace backend.Domain.Mappings
{
    public class AccommodationMapper : Profile
    {
        public AccommodationMapper()
        {

            CreateMap<AccommodationModel, AccommodationDTO>()
               .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom((src, dest, destMember, context) =>
                   src.PhotoUrl != null ? $"{context.Items["BaseUrl"]}/Images/Accommodations/{src.PhotoUrl}" : null))
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description ?? string.Empty))
               .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location ?? string.Empty))
               .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
               .ForMember(dest => dest.BedAmount, opt => opt.MapFrom(src => src.BedAmmount)) 
               .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
               .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.ModifiedAt))
               .ForMember(dest => dest.GeoLocation, opt => opt.MapFrom(src => src.GeoLocation));

            CreateMap<CreateAccommodationDTO, AccommodationModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ImageFile, opt => opt.Ignore())
                .ForMember(dest => dest.GeoLocation, opt => opt.Ignore());

            CreateMap<GeoLocationModel, GeoLocationDTO>();

            CreateMap<PagedResult<AccommodationModel>, PagedResult<AccommodationDTO>>();
        }
    }
}
