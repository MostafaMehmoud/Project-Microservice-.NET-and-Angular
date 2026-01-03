using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using catalog.Core.Entities;
using MongoDB.Driver;

namespace catalog.Infrastructure.Data.Context
{
    public interface ICatalogContext
    {
        public IMongoCollection<Product> Products { get; }
        public IMongoCollection<ProductBrand> ProductBrands { get; }
        public IMongoCollection<ProductType> ProductTypes { get; }
    }
}
