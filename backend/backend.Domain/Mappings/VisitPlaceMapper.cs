using AutoMapper;
using backend.Domain.DTOs;
using backend.Models;
using System.Linq;

namespace backend.Domain.Mappings
{
    public class VisitPlaceMapper : Profile
    {
        public VisitPlaceMapper()
        {

            // Map VisitPlace to VisitPlaceDTO
            CreateMap<VisitPlace, VisitPlaceDTO>();
        }
    }
}
