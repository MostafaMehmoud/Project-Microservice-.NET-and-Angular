using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discount.Application.Commands;
using Discount.Core.Repositories;
using MediatR;

namespace Discount.Application.Handler.Commands
{
    public class DeleteDiscountCommandHandler : IRequestHandler<DeleteDiscountCommand, bool>
    {
        
        private readonly IDiscountRepostory _repository;
        public DeleteDiscountCommandHandler(IDiscountRepostory repository)
        {
            _repository = repository;
        }
        public async Task<bool> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
        {
            var deleted = await _repository.DeleteDiscount(request.ProductName);
            return deleted;
        }
    }
}
