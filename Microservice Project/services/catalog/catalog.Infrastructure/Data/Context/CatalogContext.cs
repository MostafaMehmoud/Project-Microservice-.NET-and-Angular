using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using catalog.Core.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace catalog.Infrastructure.Data.Context
{
    public class CatalogContext : ICatalogContext
    {
        public IMongoCollection<Product> Products { get; }

        public IMongoCollection<ProductBrand> ProductBrands { get; }

        public IMongoCollection<ProductType> ProductTypes { get; }
        public CatalogContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["DatabaseSettings:ConnectionString"]);
            var database = client.GetDatabase(configuration["DatabaseSettings:DatabaseName"]);
            Products = database.GetCollection<Product>(configuration["DatabaseSettings:ProductsCollection"]);
            ProductBrands = database.GetCollection<ProductBrand>(configuration["DatabaseSettings:BrandsCollection"]);
            ProductTypes = database.GetCollection<ProductType>(configuration["DatabaseSettings:TypesCollection"]);
            ProductContextSeed.SeedDataAsync(Products).Wait();
            BrandContextSeed.SeedDataAsync(ProductBrands).Wait();
            TypeContextSeed.SeedDataAsync(ProductTypes).Wait();

        }
    }
}
