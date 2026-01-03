using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using catalog.Application.Responses;
using catalog.Core.Spacs;
using MediatR;

namespace catalog.Application.Queries
{
    public class GetAllProductsQuery: IRequest<Pagination<ProductResponseDto>>
    {
        public CatalogSpacParams CatalogSpacParams { get; set; }    
        public GetAllProductsQuery(CatalogSpacParams catalogSpacParams)
        {
            CatalogSpacParams = catalogSpacParams;
        }    
    }
}
