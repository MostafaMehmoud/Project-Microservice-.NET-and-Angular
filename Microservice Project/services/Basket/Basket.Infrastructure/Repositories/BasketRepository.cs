using System.Data;
using Basket.Core.Enitities;
using Basket.Core.Repositories;
using Microsoft.Extensions.Caching.Distributed;
namespace Basket.Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository

    {
        private readonly IDistributedCache _radiusCaching; // Fixed the type name and added a field  

        public BasketRepository(IDistributedCache distributedCache) // Added constructor to inject IDistributedCache  
        {
            _radiusCaching = distributedCache;
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket= _radiusCaching.GetString(userName);
            if (string.IsNullOrEmpty(basket))
            {
                return null;
            }
            return System.Text.Json.JsonSerializer.Deserialize<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart cart)
        {
            var basket=await _radiusCaching.GetStringAsync(cart.UserName);
            if (string.IsNullOrEmpty(basket))
            {
               await _radiusCaching.SetStringAsync(cart.UserName, System.Text.Json.JsonSerializer.Serialize(cart));
                return await GetBasket(cart.UserName);
            }
            else
            {
                new Exception("Basket Name Is Already Exist"); 
                return await GetBasket(cart.UserName);
            }
        }
        public async Task DeleteBasket(string userName)
        {
            var basket=await _radiusCaching.GetStringAsync(userName);
            if (!string.IsNullOrEmpty(basket))
            {
                await _radiusCaching.RemoveAsync(userName);
            }
        }

    }
}
