using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using catalog.Application.Queries;
using catalog.Application.Responses;
using catalog.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace catalog.Application.Handlers.Queries
{
    public class GetProductsByProductNameQueryHandler : IRequestHandler<GetProductsByProductNameQuery, IList<ProductResponseDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetProductsByProductNameQueryHandler> _logger; 
        public GetProductsByProductNameQueryHandler(IMapper mapper,IProductRepository productRepository,
            ILogger<GetProductsByProductNameQueryHandler> logger)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _logger = logger;
        }
        public async Task<IList<ProductResponseDto>> Handle(GetProductsByProductNameQuery request, CancellationToken cancellationToken)
        {
            var productsList = await _productRepository.GetProductsByName(request.ProductName);
            var productResponseDtoList = _mapper.Map<IList<ProductResponseDto>>(productsList.ToList());
            _logger.LogInformation("Returned {Count} products for ProductName: {ProductName}", productResponseDtoList.Count, request.ProductName);
            return productResponseDtoList.ToList();
        }
    }
}
