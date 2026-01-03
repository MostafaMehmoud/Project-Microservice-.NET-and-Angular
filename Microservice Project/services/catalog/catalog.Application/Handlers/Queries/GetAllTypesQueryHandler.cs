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
    public class GetAllTypesQueryHandler : IRequestHandler<GetAllTypesQuery, IList<TypeResponseDto>>
    {
        private readonly ITypeRepository _typeRepository;
        private readonly IMapper _mapper;
        public GetAllTypesQueryHandler(IMapper mapper,ITypeRepository typeRepository)
        {
            _mapper = mapper;
            _typeRepository = typeRepository;
        }
        public async Task<IList<TypeResponseDto>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
        {
            var productTypesList =await _typeRepository.GetAllTypes();
            var typeResponseDtoList = _mapper.Map<IList<TypeResponseDto>>(productTypesList.ToList());
            return typeResponseDtoList.ToList();
        }
    }
}
