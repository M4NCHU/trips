using backend.Domain.enums;
using backend.Infrastructure.Authentication;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public class CartRepository : Repository<CartModel>, ICartRepository
    {
        private readonly TripsDbContext _context;
        private readonly ILogger<CartRepository> _logger;

        public CartRepository(TripsDbContext context, ILogger<CartRepository> logger) : base(context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<CartModel?> GetCartItemAsync(string userId, Guid itemId, CartItemType itemType)
        {
            _logger.LogInformation("Fetching cart item for User ID: {UserId}, Item ID: {ItemId}", userId, itemId);
            return await _context.Cart
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ItemId == itemId && c.ItemType == itemType);
        }

 
        public async Task<List<CartModel>> GetCartByUserIdAsync(string userId)
        {
            _logger.LogInformation("Fetching cart for User ID: {UserId}", userId);
            return await _context.Cart
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task RemoveCartItemAsync(Guid cartItemId)
        {
            _logger.LogInformation("Removing cart item with ID: {CartItemId}", cartItemId);
            var cartItem = await _context.Cart.FindAsync(cartItemId);
            if (cartItem != null)
            {
                _context.Cart.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<dynamic>> GetCartDetailsInOneQueryAsync(
     List<Guid> destinationIds,
     List<Guid> accommodationIds,
     List<Guid> visitPlaceIds,
     List<Guid> tripIds)
        {
            var destinations = _context.Destination
                .Where(d => destinationIds.Contains(d.Id))
                .Select(d => new { ItemId = d.Id, Destination = d });

        /*    var accommodations = _context.Accommodation
                .Where(a => accommodationIds.Contains(a.Id))
                .Select(a => new { ItemId = a.Id, Accommodation = a });

            var visitPlaces = _context.VisitPlace
                .Where(vp => visitPlaceIds.Contains(vp.Id))
                .Select(vp => new { ItemId = vp.Id, VisitPlace = vp });

            var trips = _context.Trip
                .Where(t => tripIds.Contains(t.Id))
                .Select(t => new { ItemId = t.Id, Trip = t });*/

            // Połączenie danych w jedną listę
            var combined = destinations
                .Cast<dynamic>()
              /*  .Union(accommodations)
                .Union(visitPlaces)
                .Union(trips)*/;

            return await combined.ToListAsync();
        }






        public async Task AddToCartAsync(CartModel cartItem)
        {
            _logger.LogInformation("Adding cart item for User ID: {UserId}, Item ID: {ItemId}", cartItem.UserId, cartItem.ItemId);
            await _context.Cart.AddAsync(cartItem);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateCartItemAsync(CartModel cartItem)
        {
            _logger.LogInformation("Updating cart item with ID: {CartItemId}", cartItem.Id);
            _context.Cart.Update(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CartItemExistsAsync(Guid cartItemId)
        {
            _logger.LogInformation("Checking if cart item with ID {CartItemId} exists", cartItemId);
            return await _context.Cart.AnyAsync(c => c.Id == cartItemId);
        }
    }
}
