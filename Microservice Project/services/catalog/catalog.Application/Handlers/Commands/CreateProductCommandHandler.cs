using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using catalog.Application.Commands;
using catalog.Application.Responses;
using catalog.Core.Repositories;
using MediatR;

namespace catalog.Application.Handlers.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductResponseDto>
    {
        private readonly IProductRepository _productRepository; 
        private readonly IMapper _mapper;
        public CreateProductCommandHandler(IMapper mapper,IProductRepository productRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }
        public async Task<ProductResponseDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var productEntity = _mapper.Map<catalog.Core.Entities.Product>(request);
            var createdProduct = await _productRepository.CreateProduct(productEntity);
            var productResponseDto = _mapper.Map<ProductResponseDto>(createdProduct);
            return productResponseDto;
        }
    }
}
