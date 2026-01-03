using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using catalog.Core.Entities;
using catalog.Core.Repositories;
using catalog.Infrastructure.Data.Context;
using catalog.Core.Entities;


using MongoDB.Driver;
using catalog.Core.Spacs;

namespace catalog.Infrastructure.Repositories
{
    public class ProductRepositories : IProductRepository, IBrandRepository, ITypeRepository
    {
        public ICatalogContext _context { get; set; }
        public ProductRepositories(ICatalogContext context)
        {
            _context = context;
        }
        public async Task<Product> GetProductById(string id)
        {
            return await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Pagination<Product>> GetAllProducts(CatalogSpacParams catalogSpacParams)
        {
            var filter = Builders<Product>.Filter.Empty;

            if (!string.IsNullOrEmpty(catalogSpacParams.BrandId))
            {
                filter &= Builders<Product>.Filter.Eq(p => p.brand.Id, catalogSpacParams.BrandId);
            }

            if (!string.IsNullOrEmpty(catalogSpacParams.TypeId))
            {
                filter &= Builders<Product>.Filter.Eq(p => p.type.Id, catalogSpacParams.TypeId);
            }

            if (!string.IsNullOrEmpty(catalogSpacParams.Search))
            {
                var searchFilter = Builders<Product>.Filter.Regex(p => p.Name,
                    new MongoDB.Bson.BsonRegularExpression(catalogSpacParams.Search, "i"));
                filter &= searchFilter;
            }

            var totalItems = await _context.Products.CountDocumentsAsync(filter);
            var data = await DataFilter(catalogSpacParams, filter);

            return new Pagination<Product>(
                catalogSpacParams.PageIndex,
                catalogSpacParams.PageSize,
                (int)totalItems,
                data);
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

            if (!string.IsNullOrEmpty(catalogSpacParams.Sort))
            {
                switch (catalogSpacParams.Sort.ToLower())
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
                .Skip((catalogSpacParams.PageIndex - 1) * catalogSpacParams.PageSize)
                .Limit(catalogSpacParams.PageSize)
                .ToListAsync();
        }
    }
}
