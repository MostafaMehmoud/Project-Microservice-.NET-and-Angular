using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using catalog.Core.Entities;
using MongoDB.Driver;

namespace catalog.Infrastructure.Data.Context
{
    public static class ProductContextSeed
    {
        public static async Task SeedDataAsync(IMongoCollection<Product> productCollection)
        {
            var hasProducts = await productCollection.Find(p => true).AnyAsync();
            if (hasProducts)
            {
                return;
            }
            var path = Path.Combine("Data", "SeedData", "Product.json");
            if (!File.Exists(path))
            {
                return;
            }
            var productData = await File.ReadAllTextAsync(path);
            var products = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Product>>(productData);
            if (products != null && products.Any())
            {
                await productCollection.InsertManyAsync(products);
            }
        }       
    }
}
