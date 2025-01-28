using AutoMapper;
using backend.Application.Services.Interfaces;
using backend.Domain.DTO.Cart;
using backend.Domain.DTOs;
using backend.Domain.enums;
using backend.Domain.Filters;
using backend.Infrastructure.Respository;
using backend.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Application.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CartService> _logger;
        private readonly IMapper _mapper;
        private readonly IDestinationService _destinationService;

        public CartService(
            IUnitOfWork unitOfWork,
            ILogger<CartService> logger,
            IMapper mapper,
            IDestinationService destinationService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _destinationService = destinationService;
        }

        public async Task<(List<CartItemDTO> Cart, string SerializedCart, CookieOptions CookieOptions)> AddToCartAsync(CartItemDTO model, string cartCookie, string? userId, bool isIncrease = true)
        {
            var cart = HandleCartFromCookies(cartCookie, model, isIncrease);

            if (!string.IsNullOrEmpty(userId))
            {
                await UpdateCartInDatabase(model, userId, isIncrease);
            }

            var serializedCart = System.Text.Json.JsonSerializer.Serialize(cart);
            var cookieOptions = GetCookieOptions();

            return (cart, serializedCart, cookieOptions);
        }


        public async Task<(List<CartItemDTO> Cart, string SerializedCart, CookieOptions CookieOptions)> DecreaseQuantityAsync(CartItemDTO model, string cartCookie, string? userId)
        {
            return await AddToCartAsync(model, cartCookie, userId, isIncrease: false);
        }


        public async Task<(List<CartItemDTO> Cart, string SerializedCart, CookieOptions CookieOptions)> RemoveFromCartAsync(Guid itemId, string cartCookie, string? userId)
        {
            List<CartItemDTO> cart = new();

            if (!string.IsNullOrEmpty(cartCookie))
            {
                cart = System.Text.Json.JsonSerializer.Deserialize<List<CartItemDTO>>(cartCookie);
                cart = cart.Where(item => item.Id != itemId).ToList();
            }

            if (!string.IsNullOrEmpty(userId))
            {
                var cartItem = await _unitOfWork.Carts.GetCartItemAsync(userId, itemId, CartItemType.Destination);
                if (cartItem != null)
                {
                    await _unitOfWork.Carts.RemoveCartItemAsync(cartItem.Id);
                    await _unitOfWork.SaveChangesAsync();
                }
            }

            var serializedCart = System.Text.Json.JsonSerializer.Serialize(cart);
            var cookieOptions = GetCookieOptions();

            return (cart, serializedCart, cookieOptions);
        }


        public async Task<List<CartItemDTO>> GetCartAsync(string cartCookie, string? userId)
        {
            var cartItems = new List<CartItemDTO>();

            if (!string.IsNullOrEmpty(cartCookie))
            {
                var cartModels = System.Text.Json.JsonSerializer.Deserialize<List<CartModel>>(cartCookie);

                if (cartModels != null)
                {
                    cartItems.AddRange(_mapper.Map<List<CartItemDTO>>(cartModels));
                }
            }

            if (!string.IsNullOrEmpty(userId))
            {
                var dbCartItems = await _unitOfWork.Carts.GetByConditionAsync(c => c.UserId == userId);
                cartItems.AddRange(_mapper.Map<List<CartItemDTO>>(dbCartItems));
            }

            var destinationIds = cartItems
                .Where(item => item.ItemType == CartItemType.Destination)
                .Select(item => item.ItemId)
                .Distinct()
                .ToList();

            if (destinationIds.Any())
            {
                var destinations = await _destinationService.GetDestinationsByIdsAsync(destinationIds);

                foreach (var item in cartItems.Where(item => item.ItemType == CartItemType.Destination))
                {
                    var destinationDetail = destinations.FirstOrDefault(d => d.Id == item.ItemId);
                    if (destinationDetail != null)
                    {
                        item.Destination = destinationDetail;
                    }
                }
            }

            return cartItems;
        }



        private List<CartItemDTO> HandleCartFromCookies(string cartCookie, CartItemDTO model, bool isIncrease)
        {
            var cart = new List<CartItemDTO>();

            if (!string.IsNullOrEmpty(cartCookie))
            {
                cart = System.Text.Json.JsonSerializer.Deserialize<List<CartItemDTO>>(cartCookie);
            }

            var existingItem = cart.FirstOrDefault(item =>
                item.ItemId == model.ItemId && item.ItemType == model.ItemType);

            if (existingItem != null)
            {
                existingItem.Quantity += isIncrease ? model.Quantity : -model.Quantity;

                if (existingItem.Quantity <= 0)
                {
                    cart.Remove(existingItem);
                }
                else
                {
                    existingItem.ModifiedAt = DateTime.UtcNow;
                }
            }
            else if (isIncrease)
            {
                model.Id = Guid.NewGuid();
                cart.Add(model);
            }

            return cart;
        }



        private async Task UpdateCartInDatabase(CartItemDTO model, string userId, bool isIncrease)
        {
            var existingItem = await _unitOfWork.Carts.GetCartItemAsync(userId, model.ItemId, model.ItemType);

            if (existingItem != null)
            {
                existingItem.Quantity += isIncrease ? model.Quantity : -model.Quantity;

                if (existingItem.Quantity <= 0)
                {
                    await _unitOfWork.Carts.RemoveCartItemAsync(existingItem.Id);
                }
                else
                {
                    existingItem.ModifiedAt = DateTime.UtcNow;
                    await _unitOfWork.Carts.UpdateAsync(existingItem);
                }
            }
            else if (isIncrease)
            {
                var cartItem = _mapper.Map<CartModel>(model);
                cartItem.UserId = userId;
                await _unitOfWork.Carts.AddAsync(cartItem);
            }

            await _unitOfWork.SaveChangesAsync();
        }


        public async Task<List<CartItemDTO>> GetCartByUserIdAsync(string userId)
        {
            var cartItems = await _unitOfWork.Carts.GetByConditionAsync(c => c.UserId == userId);
            return _mapper.Map<List<CartItemDTO>>(cartItems);
        }

      

        private CookieOptions GetCookieOptions()
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            };
        }
    }
    
}
