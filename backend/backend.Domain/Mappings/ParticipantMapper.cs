using AutoMapper;
using backend.Domain.DTOs;
using backend.Models;
using System.Linq;

namespace backend.Domain.Mappings
{
    public class ParticipantMapper : Profile
    {
        public ParticipantMapper()
        { 
            CreateMap<ParticipantModel, ParticipantDTO>();
        }
    }
}
