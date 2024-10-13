using AutoMapper;
using backend.Domain.DTOs;
using backend.Models;
using System.Linq;

namespace backend.Domain.Mappings
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            // Map CategoryModel to CategoryDTO
            CreateMap<CategoryModel, CategoryDTO>();
            CreateMap<CreateCategoryRequestDTO, CategoryModel>()
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ImageFile, opt => opt.Ignore());
        }
    }
}
