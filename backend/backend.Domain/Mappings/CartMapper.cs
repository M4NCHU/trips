using AutoMapper;
using backend.Domain.DTO.Cart;
using backend.Models;

namespace backend.Domain.Mappings
{
    public class CartMapper : Profile
    {
        public CartMapper()
        {
            CreateMap<CartModel, CartItemDTO>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.ModifiedAt))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Destination, opt => opt.MapFrom(src => src.Destination));
        }
    }
}
