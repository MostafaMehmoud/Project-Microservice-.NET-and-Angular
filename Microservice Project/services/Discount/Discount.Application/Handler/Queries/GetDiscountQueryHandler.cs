using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Discount.Application.Queries;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;

namespace Discount.Application.Handler.Queries
{
    public class GetDiscountQueryHandler : IRequestHandler<GetDiscountQuery, CouponModel>
    {
        private readonly IDiscountRepostory _repository;
        private readonly ILogger<GetDiscountQueryHandler> _logger;
        private readonly IMapper _mapper;
        public GetDiscountQueryHandler(IDiscountRepostory repository, ILogger<GetDiscountQueryHandler> logger,IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<CouponModel> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
        {
           var coupon =await _repository.GetDiscount(request.ProductName);
            if (coupon == null)
            {
                _logger.LogError("Discount not found for ProductName:{ProductName}", request.ProductName);
                throw new RpcException(new Status( StatusCode.NotFound,$"Discount not found for ProductName:{request.ProductName} not found"));
            }
                _logger.LogInformation("Discount is retrived for ProductName:{ProductName}, Amount:{Amount}", coupon.ProductName, coupon.Amount);
          var couponModel=_mapper.Map<CouponModel>(coupon);
            return couponModel;
        }
    }
}
