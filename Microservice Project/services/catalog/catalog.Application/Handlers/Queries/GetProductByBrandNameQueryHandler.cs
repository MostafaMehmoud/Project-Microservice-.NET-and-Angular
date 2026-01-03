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

namespace catalog.Application.Handlers.Queries
{
    public class GetProductByBrandNameQueryHandler : IRequestHandler<GetProductByBrandNameQuery, IList<ProductResponseDto>>
    {
        private readonly IProductRepository _productRepository; 
        private readonly IMapper _mapper;
        public GetProductByBrandNameQueryHandler(IMapper mapper,IProductRepository productRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }
        public async Task<IList<ProductResponseDto>> Handle(GetProductByBrandNameQuery request, CancellationToken cancellationToken)
        {
            var productsList = await _productRepository.GetProductsByBrand(request.BrandName);
            var productResponseDtoList = _mapper.Map<IList<ProductResponseDto>>(productsList.ToList());
            return productResponseDtoList.ToList();
        }
    }
}
