using Asp.Versioning;
using AutoMapper;
using Basket.Application.Commands;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Enitities;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers.V2
{
    [ApiVersion("2")]
    
    [Route("api/v{version:apiversion}/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<BasketController> _logger;
        private readonly IMapper _mapper;
        public BasketController(IMediator mediator,
            IPublishEndpoint publishEndpoint,
            IMapper mapper,
            ILogger<BasketController> logger)
        {
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
            _logger = logger;
        }
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CheckoutBasket([FromBody] BasketCheckoutV2 basketCheckout)
        {
            var query = new GetBasketByUserNameQuery(basketCheckout.UserName);
            ShoppingCartResponse? basket = await _mediator.Send(query);
            if (basket == null)
                return BadRequest();
            var eventMessage = _mapper.Map<BasketCheckoutEventV2>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;
            _logger.LogInformation("Publishing BasketCheckoutEvent for user: {UserName} With Version 2", basketCheckout.UserName);
            await _publishEndpoint.Publish(eventMessage);
            _logger.LogInformation("BasketCheckoutEvent published successfully");

            var command = new DeleteShoppingCartCommand(basketCheckout.UserName);
            await _mediator.Send(command);
            return Accepted();

        }
    }
}
