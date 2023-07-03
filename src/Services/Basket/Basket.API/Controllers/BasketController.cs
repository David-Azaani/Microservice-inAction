﻿using System.Net;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly DiscountGrpcService _discountGrpcService;

        public BasketController(IBasketRepository basketRepository,DiscountGrpcService discountGrpcService)
        {
            _basketRepository =
                basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
            _discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
        }

        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var userBasket = await _basketRepository.GetBasket(userName);

            return Ok(userBasket ?? new ShoppingCart(userName));


        }

        [HttpPost(Name = "UpdateBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart shoppingCart)
        {
            // TODO : Here we have to Consume Discount.Grpc 
            // TODO : Calculating the new Price
            // In this Scenario basket is client and discount is server !
            foreach (var item in shoppingCart.Items)
            {
                var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                if (item.Price<coupon.Amount)
                {
                    item.Price = 0;
                }
                else
                {
                    item.Price -= coupon.Amount;
                }

            }

            
            var updatedBasket = await _basketRepository.UpdateBasket(shoppingCart);

            return Ok(updatedBasket);

        }

        [HttpDelete("{userName}",Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _basketRepository.DeleteBasket(userName);
            return Ok();
        }

    }
}
