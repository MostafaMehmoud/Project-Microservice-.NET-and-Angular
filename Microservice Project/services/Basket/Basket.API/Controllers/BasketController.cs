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

namespace Basket.API.Controllers
{
    [ApiVersion("1")]

    
    public class BasketController : BaseApiController
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
        [HttpGet()]
        [ProducesResponseType(typeof(ShoppingCartResponse), StatusCodes.Status200OK)]
        [Route("[action]/{userName}", Name = "GetBasketByUserName")]
        public async Task<ActionResult<ShoppingCartResponse>> GetBasket(string userName)
        {
            var query = new GetBasketByUserNameQuery(userName);   
            var response = await _mediator.Send(query);
            return Ok(response);
        }
        [HttpPost("CreateBasket")]
        [ProducesResponseType(typeof(ShoppingCartResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<ShoppingCartResponse>> CreateBasket([FromBody] CreateShoppingCartCommand command)
        {
            
            var basket = await _mediator.Send(command);
            return Ok(basket);
        }
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("[action]/{userName}", Name = "DeleteBasketByUserName")]
        public async Task<IActionResult> DeleteBasket(string userName)
        {

            var command = new DeleteShoppingCartCommand(userName);
            return Ok(await _mediator.Send(command));
        }
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CheckoutBasket([FromBody] BasketCheckout basketCheckout)
        {
            var query = new GetBasketByUserNameQuery(basketCheckout.UserName);
            ShoppingCartResponse? basket = await _mediator.Send(query);
            if (basket == null)
                return BadRequest();
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;
            _logger.LogInformation("Publishing BasketCheckoutEvent for user: {UserName}", basketCheckout.UserName);
            await _publishEndpoint.Publish(eventMessage);
            _logger.LogInformation("BasketCheckoutEvent published successfully");

            var command = new DeleteShoppingCartCommand(basketCheckout.UserName);
            await _mediator.Send(command);
            return Accepted();

        }
    }
}
