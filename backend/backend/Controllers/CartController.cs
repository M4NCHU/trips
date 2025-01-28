using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Application.Services.Interfaces;
using backend.Domain.DTO.Cart;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddToCart([FromBody] CartItemDTO model)
        {
            var userId = User.Identity?.Name;
            var cartCookie = Request.Cookies["cart"];

            var result = await _cartService.AddToCartAsync(model, cartCookie, userId);

            Response.Cookies.Append("cart", result.SerializedCart, result.CookieOptions);

            return Ok(new { message = "Produkt dodany do koszyka", cart = result.Cart });
        }

        [HttpGet]
        [Route("cart")]
        public async Task<IActionResult> GetCart()
        {
            var cartCookie = Request.Cookies["cart"];
            var cartFromCookie = _cartService.GetCartAsync(cartCookie, "");

            return Ok(cartFromCookie);
        }


        [HttpPost("decrease")]
        public async Task<IActionResult> DecreaseQuantity([FromBody] CartItemDTO model)
        {
            var (cart, serializedCart, cookieOptions) = await _cartService.DecreaseQuantityAsync(model, Request.Cookies["cart"], User.Identity?.Name);

            Response.Cookies.Append("cart", serializedCart, cookieOptions);
            return Ok(cart);
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveFromCart([FromBody] Guid itemId)
        {
            var (cart, serializedCart, cookieOptions) = await _cartService.RemoveFromCartAsync(itemId, Request.Cookies["cart"], User.Identity?.Name);

            Response.Cookies.Append("cart", serializedCart, cookieOptions);
            return Ok(cart);
        }


       

    }


    public class RemoveFromCartRequest
    {
        public Guid ItemId { get; set; }
    }
}
