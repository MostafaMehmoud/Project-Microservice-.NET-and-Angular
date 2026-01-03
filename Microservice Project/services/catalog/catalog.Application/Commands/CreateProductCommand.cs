using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using catalog.Application.Responses;
using catalog.Core.Entities;
using catalog.Core.Entities;
using MediatR;
using MongoDB.Bson.Serialization.Attributes;

namespace catalog.Application.Commands
{
    public class CreateProductCommand:IRequest<ProductResponseDto>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string summary { get; set; }
        public string ImageFile { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.Decimal128)]
        public decimal Price { get; set; }
        public ProductBrand brand { get; set; }
        public ProductType type { get; set; }
    }
}
