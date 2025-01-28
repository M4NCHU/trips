using AutoMapper;
using backend.Domain.DTO;
using backend.Domain.DTOs;
using backend.Domain.Models;
using backend.Models;

namespace backend.Domain.Mappings
{
    public class GeoLocationMapper : Profile
    {
        public GeoLocationMapper()
        {
            CreateMap<GeoLocationDTO, GeoLocationModel>();
        }
    }
}
