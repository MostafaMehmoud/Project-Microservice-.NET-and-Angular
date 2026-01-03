using System.Net;
using catalog.Application.Commands;
using catalog.Application.Queries;
using catalog.Application.Responses;
using catalog.Core.Spacs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace catalog.API.Controllers
{
  
    public class CatalogController : BaseApiController
    {
        private readonly IMediator _Mediator;
        public CatalogController(IMediator mediator)
        {
            _Mediator = mediator;
        }
        [HttpGet]
        [Route("[action]/{id}",Name ="GetProductById")]
        [ProducesResponseType(typeof(ProductResponseDto),(int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ProductResponseDto> GetProductById(string id)
        {
            var query=new GetProductByIdQuery(id);
            var product = await _Mediator.Send(query);
            return product;
        }
        [HttpGet]
        [Route("[action]/{ProductName}", Name = "GetProductsByProductName")]
        [ProducesResponseType(typeof(IList<ProductResponseDto>), (int)HttpStatusCode.OK)]
        
        public async Task<IList<ProductResponseDto>> GetProductsByProductName(string ProductName)
        {
            var query = new GetProductsByProductNameQuery(ProductName);
            var product = await _Mediator.Send(query);
            return product;
        }
        [HttpGet]
        [Route("GetAllProducts")]
        [ProducesResponseType(typeof(IList<ProductResponseDto>), (int)HttpStatusCode.OK)]

        public async Task<Pagination<ProductResponseDto>> GetAllProducts([FromQuery] CatalogSpacParams catalogSpacParams)
        {
            var query = new GetAllProductsQuery(catalogSpacParams);
            var product = await _Mediator.Send(query);
            return product;
        }
        [HttpGet]
        [Route("GetAllBrands")]
        [ProducesResponseType(typeof(IList<BrandResponseDto>), (int)HttpStatusCode.OK)]

        public async Task<IList<BrandResponseDto>> GetAllBrands()
        {
            var query = new GetAllBrandsQuery();
            var product = await _Mediator.Send(query);
            return product;
        }
        [HttpGet]
        [Route("GetAllTypes")]
        [ProducesResponseType(typeof(IList<TypeResponseDto>), (int)HttpStatusCode.OK)]

        public async Task<IList<TypeResponseDto>> GetAllTypes()
        {
            var query = new GetAllTypesQuery();
            var product = await _Mediator.Send(query);
            return product;
        }
        [HttpPost]
        [Route("CreateProduct")]
        [ProducesResponseType(typeof(ProductResponseDto), (int)HttpStatusCode.OK)]

        public async Task<ProductResponseDto> CreateProduct(CreateProductCommand productCommand)
        {
            
            var product = await _Mediator.Send<ProductResponseDto>(productCommand);
            return product;
        }
        [HttpPut]
        [Route("UpdateProduct")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]

        public async Task<bool> UpdateProduct(UpdateProductCommand productCommand)
        {

            var result = await _Mediator.Send<bool>(productCommand);
            return result;
        }
        [HttpDelete]
        [Route("{id}",Name ="DeleteProduct")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]

        public async Task<bool> DeleteProduct(string id)
        {
            var command = new DeleteProductCommand(id); 

            var result = await _Mediator.Send(command);
            return result;
        }
    }
}
