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
        }
    }
}
