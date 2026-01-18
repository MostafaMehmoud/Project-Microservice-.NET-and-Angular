using catalog.Core.Entities;
using catalog.Core.Entities;
using catalog.Core.Repositories;
using catalog.Core.Spacs;
using catalog.Infrastructure.Data.Context;
using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace catalog.Infrastructure.Repositories
{
    public class ProductRepositories : IProductRepository, IBrandRepository, ITypeRepository
    {
        public ICatalogContext _context { get; set; }
        private readonly ILogger<ProductRepositories> _logger;  
        public ProductRepositories(ICatalogContext context,ILogger<ProductRepositories> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Product> GetProductById(string id)
        {
            return await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

     

public async Task<Pagination<Product>> GetAllProducts(CatalogSpacParams catalogSpecParams)
    {
        var builder = Builders<Product>.Filter;
        var filter = builder.Empty;

        // Search
        if (!string.IsNullOrEmpty(catalogSpecParams.search))
        {
            filter &= builder.Regex(p => p.Name,
                new MongoDB.Bson.BsonRegularExpression(catalogSpecParams.search, "i"));
        }

        // Brand filter - حول string لـ ObjectId
        if (!string.IsNullOrEmpty(catalogSpecParams.brandId))
        {
            _logger.LogInformation($"Applying brand filter: {catalogSpecParams.brandId}");

            // ✅ الحل هنا
            var brandObjectId = ObjectId.Parse(catalogSpecParams.brandId);
            filter &= builder.Eq("brand._id", brandObjectId);
        }

        // Type filter - نفس الحاجة
        if (!string.IsNullOrEmpty(catalogSpecParams.typeId))
        {
            _logger.LogInformation($"Applying type filter: {catalogSpecParams.typeId}");

            // ✅ الحل هنا
            var typeObjectId = ObjectId.Parse(catalogSpecParams.typeId);
            filter &= builder.Eq("type._id", typeObjectId);
        }

        var totalItems = await _context.Products.CountDocumentsAsync(filter);
        _logger.LogInformation($"Total items found: {totalItems}");

        var data = await DataFilter(catalogSpecParams, filter);
        _logger.LogInformation($"Returned items count: {data.Count}");

        return new Pagination<Product>(
            catalogSpecParams.pageIndex,
            catalogSpecParams.pageSize,
            (int)totalItems,
            data
        );
    }
    public async Task<IEnumerable<Product>> GetProductsByName(string name)
        {
            return await _context.Products.Find(p => p.Name == name).ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProductsByBrand(string name)
        {
            return await _context.Products.Find(p => p.brand.Name == name).ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProductsByType(string typename)
        {
            return await _context.Products.Find(p => p.type.Name == typename).ToListAsync();
        }
        public async Task<Product> CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
            return product;
        }

        public async Task<bool> DeleteProduct(string id)
        {
            var deleteResult = await _context.Products.DeleteOneAsync(p => p.Id == id);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await _context.Products.ReplaceOneAsync(p => p.Id == product.Id, product);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<IEnumerable<ProductBrand>> GetAllBrands()
        {
            return await _context.ProductBrands.Find(b => true).ToListAsync();
        }

       

        public async Task<IEnumerable<ProductType>> GetAllTypes()
        {
            return await _context.ProductTypes.Find(t => true).ToListAsync();  
        }







        private async Task<IReadOnlyList<Product>> DataFilter(CatalogSpacParams catalogSpacParams, FilterDefinition<Product> filter)

        {
            // Default sorting by Name
            var sortDef = Builders<Product>.Sort.Ascending("Name");

            if (!string.IsNullOrEmpty(catalogSpacParams.sort))
            {
                switch (catalogSpacParams.sort.ToLower())
                {
                    case "priceasc":
                        sortDef = Builders<Product>.Sort.Ascending("Price");
                        break;
                    case "pricedesc":
                        sortDef = Builders<Product>.Sort.Descending("Price");
                        break;
                    default:
                        sortDef = Builders<Product>.Sort.Ascending("Name");
                        break;
                }
            }

         
            // Apply filter + sort + pagination
            return await _context.Products
                .Find(filter)
                .Sort(sortDef)
                .Skip((catalogSpacParams.pageIndex - 1) * catalogSpacParams.pageSize)
                .Limit(catalogSpacParams.pageSize)
                .ToListAsync();
        }
    }
}
