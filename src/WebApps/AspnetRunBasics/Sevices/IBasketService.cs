﻿using System.Threading.Tasks;
using AspnetRunBasics.Model;
using AspnetRunBasics.Models;

namespace AspnetRunBasics.Sevices
{
    public interface IBasketService
    {
        Task<BasketModel> GetBasket(string userName);
        Task<BasketModel> UpdateBasket(BasketModel model);
        Task CheckoutBasket(BasketCheckoutModel model);
    }
}