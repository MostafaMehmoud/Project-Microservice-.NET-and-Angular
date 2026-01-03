using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using catalog.Application.Queries;
using catalog.Application.Responses;
using catalog.Core.Repositories;
using catalog.Core.Entities;
using catalog.Core.Repositories;
using MediatR;

namespace catalog.Application.Handlers.Queries
{
    public class GetAllBrandsQueryHandler : IRequestHandler<GetAllBrandsQuery, IList<BrandResponseDto>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;
        public GetAllBrandsQueryHandler(IBrandRepository brandRepository,IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }
        public async Task<IList<BrandResponseDto>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var productBrandsList= await _brandRepository.GetAllBrands();
            var brandResponseDtoList=_mapper.Map<IList<ProductBrand>,IList<BrandResponseDto>>(productBrandsList.ToList());
            return brandResponseDtoList.ToList();
        }
    }
}
