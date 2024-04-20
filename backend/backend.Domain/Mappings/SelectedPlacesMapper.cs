using AutoMapper;
using backend.Domain.DTOs;
using backend.Models;
using System.Linq;

namespace backend.Domain.Mappings
{
    public class TripParticipantMapper : Profile
    {
        public TripParticipantMapper()
        {

            // Map TripParticipantModel to TripParticipantDTO
            CreateMap<TripParticipantModel, TripParticipantDTO>();
        }
    }
}
