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
            
            // Map CategoryModel to CategoryDTO
            CreateMap<ParticipantModel, ParticipantDTO>();
        }
    }
}
