using backend.Domain.DTO.Cart;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Application.Services.Interfaces
{
    public interface ICartService
    {
        Task<(List<CartItemDTO> Cart, string SerializedCart, CookieOptions CookieOptions)> AddToCartAsync(CartItemDTO model, string cartCookie, string? userId, bool isIncrease = true);
        Task<List<CartItemDTO>> GetCartByUserIdAsync(string userId);
        Task<(List<CartItemDTO> Cart, string SerializedCart, CookieOptions CookieOptions)> RemoveFromCartAsync(Guid itemId, string cartCookie, string? userId);
        Task<List<CartItemDTO>> GetCartAsync(string cartCookie, string? userId);
        Task<(List<CartItemDTO> Cart, string SerializedCart, CookieOptions CookieOptions)> DecreaseQuantityAsync(CartItemDTO model, string cartCookie, string? userId);

    }
}
