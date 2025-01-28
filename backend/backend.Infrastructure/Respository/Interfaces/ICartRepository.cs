using backend.Domain.enums;
using backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public interface ICartRepository : IRepository<CartModel>
    {
        Task<CartModel?> GetCartItemAsync(string userId, Guid itemId, CartItemType itemType);
        Task<List<CartModel>> GetCartByUserIdAsync(string userId);
        Task RemoveCartItemAsync(Guid cartItemId);
        Task AddToCartAsync(CartModel cartItem);
        Task UpdateCartItemAsync(CartModel cartItem);
        Task<bool> CartItemExistsAsync(Guid cartItemId);
        Task<List<object>> GetCartDetailsInOneQueryAsync(
    List<Guid> destinationIds,
    List<Guid> accommodationIds,
    List<Guid> visitPlaceIds,
    List<Guid> tripIds);
    }
}
