using AutoMapper;
using backend.Domain.DTOs;
using backend.Models;
using System.Linq;

namespace backend.Domain.Mappings
{
    public class TripDestinationMapper : Profile
    {
        public TripDestinationMapper()
        {
            
            // Map CategoryModel to CategoryDTO
            CreateMap<TripDestinationModel, TripDestinationDTO>();
        }
    }
}
