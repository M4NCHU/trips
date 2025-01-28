using AutoMapper;
using backend.Domain.DTOs;
using backend.Models;
using System.Linq;

namespace backend.Domain.Mappings
{
    public class SelectedPlacesMapper : Profile
    {
        public SelectedPlacesMapper()
        {
            CreateMap<SelectedPlaceModel, SelectedPlaceDTO>();
        }
    }
}
