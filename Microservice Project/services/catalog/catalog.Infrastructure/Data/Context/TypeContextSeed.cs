using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using catalog.Core.Entities;
using MongoDB.Driver;


namespace catalog.Infrastructure.Data.Context
{
    public static class TypeContextSeed
    {
        public static async Task SeedDataAsync(IMongoCollection<ProductType> typeCollection)
        {
            var hasTypes = await typeCollection.Find(t => true).AnyAsync();
            if (hasTypes)
            {
                return;
            }
            var path = Path.Combine("Data", "SeedData", "ProductType.json");
            if (!File.Exists(path))
            {
                return;
            }
            var typeData = await File.ReadAllTextAsync(path);
            var types = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<ProductType>>(typeData);
            if (types != null && types.Any())
            {
                await typeCollection.InsertManyAsync(types);
            }
        }
    }
}
