using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using catalog.Core.Entities;
using MongoDB.Driver;

namespace catalog.Infrastructure.Data.Context
{
    public static class BrandContextSeed
    {
        public static async Task SeedDataAsync(IMongoCollection<ProductBrand> brandCollection)
        {
            var hasBrands = await brandCollection.Find(b => true).AnyAsync();

            // ✅ لو فيه داتا بالفعل، ماتعملش Insert
            if (hasBrands)
            {
                return;
            }

            var path = Path.Combine("Data", "SeedData", "ProductBrand.json");
            if (!File.Exists(path))
            {
                Console.WriteLine($"Seed file not found at: {path}");
                return;
            }

            var brandData = await File.ReadAllTextAsync(path);
            var brands = JsonSerializer.Deserialize<IEnumerable<ProductBrand>>(brandData);

            if (brands != null && brands.Any())
            {
                await brandCollection.InsertManyAsync(brands);
                Console.WriteLine("✅ Brand seed data inserted successfully!");
            }
        }
    }
}
