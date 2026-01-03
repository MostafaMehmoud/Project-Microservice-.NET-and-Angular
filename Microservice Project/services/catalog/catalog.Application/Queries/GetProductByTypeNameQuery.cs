using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using catalog.Application.Responses;
using MediatR;

namespace catalog.Application.Queries
{
    public class GetProductByTypeNameQuery : IRequest<IList<ProductResponseDto>>
    {
        public string TypeName { get; set; }
        public GetProductByTypeNameQuery(string typeName)
        {
            TypeName = typeName;
        }
    }
}
