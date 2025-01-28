using AutoMapper;
using backend.Domain.DTO;
using backend.Domain.DTOs;
using backend.Domain.Models;
using backend.Models;

namespace backend.Domain.Mappings
{
    public class ReservationMapper : Profile
    {
        public ReservationMapper()
        {
            CreateMap<ReservationModel, ReservationDTO>();
        }
    }
}
