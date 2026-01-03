using Discount.Application.Commands;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;

namespace Discount.API.Services
{
    public class DiscountService: DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IMediator _mediator;
        public DiscountService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var query = new Application.Queries.GetDiscountQuery(request.ProductName);
            var coupon = await _mediator.Send(query);
            return coupon;
        }
        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var cmd = new CreateDiscountCommand { 
              ProductName=  request.Coupon.ProductName,
              Description=  request.Coupon.Description,
               Amount= request.Coupon.Amount };
            
            var coupon = await _mediator.Send(cmd);
            return coupon;
        }
        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var cmd = new UpdateDiscountCommand
            {
                Id = request.Coupon.Id, 
                ProductName = request.Coupon.ProductName,
                Description = request.Coupon.Description,
                Amount = request.Coupon.Amount
            };
            var coupon = await _mediator.Send(cmd);
            return coupon;
        }
        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var cmd = new DeleteDiscountCommand( request.ProductName);

            var result = await _mediator.Send(cmd);
            // Implement the delete logic here, possibly using a command and mediator pattern
            // For now, we'll return a dummy response
            var response = new DeleteDiscountResponse
            {
                Success = result // Set to true or false based on actual deletion result
            };
            return await Task.FromResult(response);
        }
    }
}
