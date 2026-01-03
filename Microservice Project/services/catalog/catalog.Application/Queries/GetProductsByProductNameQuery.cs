using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using catalog.Application.Responses;
using MediatR;

namespace catalog.Application.Queries
{
    public class GetProductsByProductNameQuery:IRequest<IList<ProductResponseDto>>
    {
        public string ProductName { get; set; }
        public GetProductsByProductNameQuery(string productName)
        {
            ProductName = productName;
        }
    }
}
