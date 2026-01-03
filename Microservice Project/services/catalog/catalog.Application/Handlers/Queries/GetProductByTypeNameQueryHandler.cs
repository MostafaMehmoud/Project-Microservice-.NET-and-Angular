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
    public class GetProductByTypeNameQueryHandler : IRequestHandler<GetProductByTypeNameQuery, IList<ProductResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        public GetProductByTypeNameQueryHandler(IMapper mapper, IProductRepository productRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }
        public async Task<IList<ProductResponseDto>> Handle(GetProductByTypeNameQuery request, CancellationToken cancellationToken)
        {
            var productsList = await _productRepository.GetProductsByType(request.TypeName);
            var productResponseDtoList = _mapper.Map<IList<ProductResponseDto>>(productsList.ToList());
            return productResponseDtoList.ToList();
        }
    }
}
