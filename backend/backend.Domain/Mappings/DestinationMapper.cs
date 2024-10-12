﻿using AutoMapper;
using backend.Domain.DTOs;
using backend.Models;

namespace backend.Domain.Mappings
{
    public class DestinationMapper : Profile
    {
        public DestinationMapper()
        {
            // Map DestinationModel to DestinationDTO
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

            // Map CreateDestinationDTO to DestinationModel
            CreateMap<CreateDestinationDTO, DestinationModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ImageFile, opt => opt.Ignore());
        }
    }
}
