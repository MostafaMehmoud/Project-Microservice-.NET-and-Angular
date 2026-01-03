using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using catalog.Application.Queries;
using catalog.Application.Responses;
using catalog.Core.Repositories;
using catalog.Core.Spacs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace catalog.Application.Handlers.Queries
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, Pagination<ProductResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        private readonly ILogger<GetAllProductsQueryHandler> _logger;   
        public GetAllProductsQueryHandler(IMapper mapper,IProductRepository productRepository,
            ILogger<GetAllProductsQueryHandler> logger)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _logger = logger;
        }
        public async Task<Pagination<ProductResponseDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var ProductList =await _productRepository.GetAllProducts(request.CatalogSpacParams);
            var productResponseDtoList = _mapper.Map<Pagination<ProductResponseDto>>(ProductList);
            _logger.LogInformation("GetAllProductsQueryHandler Handled");
            return productResponseDtoList;
        }
    }
}
